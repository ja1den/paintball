using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
	[Header("Control")]
	public int team;

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

	private WeaponScript weaponScript;
	private GameObject weaponParent;

	void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		zone = GameObject.Find("Background").transform.localScale;

		rb = GetComponent<Rigidbody2D>();

		weaponParent = transform.Find("Weapon").gameObject;
	}

	void Start()
	{
		transform.position = new Vector3(0, 0, -photonView.OwnerActorNr);
		transform.SetParent(GameObject.Find("Players").transform);
	}

	void FixedUpdate()
	{
		// Movement
		rb.velocity = moveDirection * speed;

		// Shoot
		if (weaponScript && isShooting)
		{
			weaponScript.Shoot(photonView, lookDirection, team, color);
		}

		// Zone Tick
		if (transform.position.x < -zone.x / 2 || zone.x / 2 < transform.position.x || transform.position.y < -zone.y / 2 || zone.y / 2 < transform.position.y)
		{
			if (prevTime + 0.5f < Time.time)
			{
				if (photonView.IsMine) photonView.RPC("Damage", RpcTarget.All, zoneDamage);
				prevTime = Time.time;
			}
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
			weapon.transform.SetParent(weaponParent.transform);

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

		// Look at Cursor
		Vector2 cursor = context.ReadValue<Vector2>();
		Vector2 offset = new Vector2(Screen.width, Screen.height) * 0.5f;

		lookDirection = (cursor - offset).normalized;

		float angle = Vector2.SignedAngle(Vector2.up, lookDirection);
		weaponParent.transform.rotation = Quaternion.Euler(0, 0, angle);
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

	public void SetTeam(int team)
	{
		transform.Find("Body").GetComponent<Shapes2D.Shape>().settings.fillColor = gameManager.teams[this.team = team];
	}

	[PunRPC]
	public int Damage(int damage)
	{
		return health = Mathf.Max(0, health - damage);
	}

	[PunRPC]
	public void CreateBullet(int owner, Vector3 position, Vector2 direction, int team, int damage, float size, Color color)
	{
		GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
		bullet.transform.localScale = new Vector3(size, size, 1);

		BulletScript bulletScript = bullet.GetComponent<BulletScript>();

		bulletScript.SetTeam(team);
		bulletScript.owner = owner;

		bulletScript.moveDirection = direction;
		bulletScript.damage = damage;
	}
}
