using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
	[Header("Control")]
	public bool isAlive = false;
	public float respawn = 3;

	[Space(10)]

	public Color color;

	[Header("Movement")]
	public float speed;

	[Header("Health")]
	public int health;
	public int zoneDamage = 20;

	[Header("Weapons")]
	public GameObject bulletPrefab;
	public bool isShooting = false;

	[Header("Debug")]
	private GameManager gameManager;

	private Rigidbody2D rb;

	private Vector2 moveDirection;
	private Vector2 lookDirection;

	private int maxHealth;

	private Vector2 zone;
	private float prevTime = 0f;

	private WeaponScript weaponScript;

	public Dictionary<int, GameObject> bullets = new Dictionary<int, GameObject>();
	public int bulletCount = 0;

	void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		zone = GameObject.Find("Background").transform.localScale;

		transform.Find("Health").GetComponent<HealthScript>().maxHealth = maxHealth = health;
		rb = GetComponent<Rigidbody2D>();

		if (photonView.IsMine)
			GameObject.Find("RespawnText").GetComponent<RespawnScript>().playerScript = this;
	}

	void Start()
	{
		transform.position += new Vector3(0, 0, -photonView.OwnerActorNr);
		transform.SetParent(GameObject.Find("Players").transform);
	}

	void FixedUpdate()
	{
		// Alive
		if (!isAlive)
		{
			// Respawn
			if (respawn + 5f < Time.time)
			{
				// Show Sprites
				foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
					spriteRenderer.enabled = true;

				// Enable Collider
				GetComponent<CircleCollider2D>().enabled = true;

				// Reset
				isAlive = true;
			}

		}
		else
		{
			// Client's Player
			if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

			// Playing
			if (!gameManager.isPlaying) return;

			// Move
			rb.velocity = moveDirection * speed;

			// Look
			float angle = Vector2.SignedAngle(Vector2.up, lookDirection);
			weaponScript.transform.parent.transform.rotation = Quaternion.Euler(0, 0, angle);

			// Shoot
			if (weaponScript && isShooting)
				weaponScript.Shoot(this, lookDirection);

			// Zone Tick
			if (transform.position.x < -zone.x / 2 || zone.x / 2 < transform.position.x || transform.position.y < -zone.y / 2 || zone.y / 2 < transform.position.y)
			{
				if (prevTime + 0.5f < Time.time)
				{
					photonView.RPC("Damage", RpcTarget.All, zoneDamage);
					prevTime = Time.time;
				}
			}
		}


	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		// Weapon
		GameObject weaponPrefab = gameManager.weapons[(int)info.photonView.InstantiationData[0]];

		GameObject weapon = Instantiate(weaponPrefab, transform.position + weaponPrefab.transform.position, Quaternion.identity);
		weapon.transform.SetParent(transform.Find("Weapon"));

		weaponScript = weapon.GetComponent<WeaponScript>();

		// Color
		SetColor((Color)info.photonView.InstantiationData[1]);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		foreach (KeyValuePair<int, GameObject> entry in bullets)
			Destroy(entry.Value);
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

		// Look at Cursor
		Vector2 cursor = context.ReadValue<Vector2>();
		Vector2 offset = new Vector2(Screen.width, Screen.height) * 0.5f;

		lookDirection = (cursor - offset).normalized;
	}

	public void Fire(InputAction.CallbackContext context)
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

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

	public void SetColor(Color color)
	{
		transform.Find("Body").GetComponent<Shapes2D.Shape>().settings.fillColor = this.color = color;
	}

	[PunRPC]
	public int Damage(int damage)
	{
		health = Mathf.Max(0, health - damage);

		if (health == 0)
		{
			// Respawn
			respawn = Time.time;
			isAlive = false;

			// Reset
			health = maxHealth;

			// Hide Sprites
			foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
				spriteRenderer.enabled = false;

			// Disable Collider
			GetComponent<CircleCollider2D>().enabled = false;

			// Client's Player
			if (!photonView.IsMine && PhotonNetwork.IsConnected) return health;

			// Spawnpoint
			transform.position = gameManager.spawns[Random.Range(0, gameManager.spawns.Length)].transform.position;
		}

		return health;
	}

	[PunRPC]
	public void CreateBullet(int owner, Vector3 position, Vector2 direction, int damage, float size)
	{
		GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
		bullet.transform.localScale = new Vector3(size, size, 1);

		BulletScript bulletScript = bullet.GetComponent<BulletScript>();
		bulletScript.playerScript = PhotonNetwork.GetPhotonView(owner).GetComponent<PlayerScript>();

		bulletScript.moveDirection = direction;
		bulletScript.damage = damage;

		bulletScript.SetColor(color);

		bulletScript.number = bulletCount;
		bullets.Add(bulletCount++, bullet);
	}

	[PunRPC]
	public void DestroyBullet(int number)
	{
		GameObject bullet;
		if (bullets.TryGetValue(number, out bullet))
		{
			bullets.Remove(number);
			Destroy(bullet);
		}
	}
}
