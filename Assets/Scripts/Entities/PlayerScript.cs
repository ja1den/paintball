using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
	[Header("Control")]
	public int team;

	[Space(10)]

	public bool isRespawning = false;
	public float respawn = 0;

	[Header("Movement")]
	public float speed;

	[Header("Health")]
	public int health;

	[Space(10)]

	public int zoneDamage = 20;
	public float prevTime = 0f;

	[Header("Appearance")]
	public Color color;

	[Header("Weapons")]
	public GameObject[] weapons;
	public GameObject bulletPrefab;

	[Space(10)]

	public bool isShooting = false;

	[Header("Debug")]
	private GameManager gameManager;
	private Vector2 zone;

	[Space(10)]

	private Rigidbody2D rb;

	[Space(10)]

	private Vector2 moveDirection;
	private Vector2 lookDirection;

	[Space(10)]

	private int maxHealth;

	[Space(10)]

	private WeaponScript weaponScript;

	[HideInInspector]
	public int bulletCount = 0;

	void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		zone = GameObject.Find("Background").transform.localScale;

		rb = GetComponent<Rigidbody2D>();

		transform.Find("Health").GetComponent<HealthScript>().maxHealth = maxHealth = health;
	}

	void Start()
	{
		transform.position += new Vector3(0, 0, -photonView.OwnerActorNr);
		transform.SetParent(GameObject.Find("Players").transform);
	}

	void FixedUpdate()
	{
		// Movement
		if (!isRespawning) rb.velocity = moveDirection * speed;

		// Shoot
		if (weaponScript && isShooting) weaponScript.Shoot(this, lookDirection);

		// Zone Tick
		if (transform.position.x < -zone.x / 2 || zone.x / 2 < transform.position.x || transform.position.y < -zone.y / 2 || zone.y / 2 < transform.position.y)
		{
			if (prevTime + 0.5f < Time.time)
			{
				if (photonView.IsMine) photonView.RPC("Damage", RpcTarget.All, zoneDamage);
				prevTime = Time.time;
			}
		}

		// Respawn
		if (isRespawning && respawn + 5f < Time.time)
		{
			// Show Sprites
			foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
				spriteRenderer.enabled = true;

			// Enable Collider
			GetComponent<CircleCollider2D>().enabled = true;

			// Reset
			isRespawning = false;
		}
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		// Color
		SetTeam((int)info.photonView.InstantiationData[0]);

		// Weapon
		if (weapons.Length > 0)
		{
			GameObject weaponPrefab = weapons[(int)info.photonView.InstantiationData[1]];

			GameObject weapon = Instantiate(weaponPrefab, transform.position + weaponPrefab.transform.position, Quaternion.identity);
			weapon.transform.SetParent(transform.Find("Weapon"));

			weaponScript = weapon.GetComponent<WeaponScript>();
		}
	}

	public void Move(InputAction.CallbackContext context)
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Direction
		moveDirection = context.ReadValue<Vector2>();
	}

	public void Look(InputAction.CallbackContext context)
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Respawning
		if (isRespawning) return;

		// Look at Cursor
		Vector2 cursor = context.ReadValue<Vector2>();
		Vector2 offset = new Vector2(Screen.width, Screen.height) * 0.5f;

		lookDirection = (cursor - offset).normalized;

		float angle = Vector2.SignedAngle(Vector2.up, lookDirection);
		weaponScript.transform.parent.transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	public void Fire(InputAction.CallbackContext context)
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Respawning
		if (isRespawning) return;

		// Toggle Shooting
		switch (context.phase)
		{
			case InputActionPhase.Started:
				isShooting = true;
				break;

			case InputActionPhase.Canceled:
				isShooting = false;
				break;
		}
	}

	public void SetTeam(int team)
	{
		transform.Find("Body").GetComponent<Shapes2D.Shape>().settings.fillColor = gameManager.teams[this.team = team].color;
	}

	[PunRPC]
	public int Damage(int damage)
	{
		health = Mathf.Max(0, health - damage);

		if (health == 0)
		{
			// Reset Player
			transform.position = gameManager.teams[team].spawn.transform.position;
			health = maxHealth;

			// Hide Sprites
			foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
				spriteRenderer.enabled = false;

			// Disable Collider
			GetComponent<CircleCollider2D>().enabled = false;

			// Respawn
			isRespawning = true;
			respawn = Time.time;
		}

		return health;
	}

	[PunRPC]
	public void CreateBullet(int owner, int number, Vector3 position, Vector2 direction, int damage, float size)
	{
		GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
		bullet.transform.localScale = new Vector3(size, size, 1);

		BulletScript bulletScript = bullet.GetComponent<BulletScript>();

		bulletScript.owner = PhotonNetwork.GetPhotonView(owner).GetComponent<PlayerScript>();
		bulletScript.number = number;

		bulletScript.moveDirection = direction;
		bulletScript.damage = damage;
	}

	[PunRPC]
	public void DestroyBullet(int owner, int number)
	{
		foreach (BulletScript bulletScript in FindObjectsOfType<BulletScript>())
			if (bulletScript.owner.photonView.ViewID == owner && bulletScript.number == number)
				Destroy(bulletScript.gameObject);
	}
}
