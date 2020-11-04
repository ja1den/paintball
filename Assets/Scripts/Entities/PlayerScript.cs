using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IPunObservable
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
	private NetworkManager networkManager;
	private GameManager gameManager;

	private Rigidbody2D rb;

	private Vector2 moveDirection;
	private Vector2 lookDirection;

	private int maxHealth;

	private Vector2 zone;
	private float prevTime = 0f;

	private WeaponScript weaponScript;

	private Dictionary<int, GameObject> bullets = new Dictionary<int, GameObject>();
	private int bulletCount = 0;

	void Awake()
	{
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
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
			if (respawn + 5f < Time.time)
			{
				health = maxHealth;
				isAlive = true;
			}

			rb.velocity = Vector3.zero;
		}
		else
		{
			// Client's Player
			if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

			// Playing
			if (gameManager.gameState != GameState.Play)
			{
				rb.velocity = Vector3.zero;
				return;
			}

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
					photonView.RPC("Damage", RpcTarget.All, zoneDamage, -1);
					prevTime = Time.time;
				}
			}
		}
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		// Weapon
		photonView.Owner.CustomProperties.TryGetValue("weapon", out object number);
		GameObject weaponPrefab = gameManager.weapons[(int)number];

		GameObject weapon = Instantiate(weaponPrefab, transform.position + weaponPrefab.transform.position, Quaternion.identity);
		weapon.transform.SetParent(transform.Find("Weapon"));

		weaponScript = weapon.GetComponent<WeaponScript>();

		// Color
		SetColor(networkManager.colors[(photonView.OwnerActorNr - 1) % 8]);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		foreach (KeyValuePair<int, GameObject> entry in bullets)
			Destroy(entry.Value);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(health);
		}
		else
		{
			health = (int)stream.ReceiveNext();
		}

		// Update Sprites
		foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
			spriteRenderer.enabled = health != 0;

		// Update Collider
		GetComponent<CircleCollider2D>().enabled = health != 0;
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
	public void Damage(int damage, int dealer)
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Alive?
		if (!isAlive) return;

		// Apply Damage
		health = Mathf.Max(0, health - damage);

		if (health == 0)
		{
			// Respawn
			respawn = Time.time;
			isAlive = false;

			// Spawnpoint
			transform.position = gameManager.spawns[Random.Range(0, gameManager.spawns.Length)].transform.position;

			// Zone Damage
			if (dealer == -1) return;

			// Update Score
			Player player = PhotonNetwork.CurrentRoom.GetPlayer(dealer);
			player.CustomProperties.TryGetValue("score", out object score);

			Hashtable playerProps = new Hashtable();
			playerProps.Add("score", (int)(score ?? 0) + 1);

			player.SetCustomProperties(playerProps);
		}
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
