using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
	[Header("Movement")]
	public float moveSpeed;

	[Header("Appearance")]
	public Color color;

	[Header("Weapons")]
	public GameObject[] weapons;

	[Space(10)]

	public GameObject bulletPrefab;

	[Header("Debug")]
	[ReadOnly]
	public Vector2 moveDirection;
	[ReadOnly]
	public Vector2 lookDirection;

	[Space(10)]

	private Rigidbody2D rb;

	[Space(10)]

	private WeaponScript weaponScript;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		// Choose a Random Weapon
		if (weapons.Length > 0)
		{
			GameObject weaponPrefab = weapons[Random.Range(0, weapons.Length)];

			GameObject weapon = Instantiate(weaponPrefab, transform.position + weaponPrefab.transform.position, Quaternion.identity);
			weapon.transform.SetParent(transform.Find("Weapon"));

			weaponScript = weapon.GetComponent<WeaponScript>();
		}
	}

	void Start()
	{
		transform.SetParent(GameObject.Find("Players").transform);
	}

	void FixedUpdate()
	{
		rb.velocity = moveDirection * moveSpeed;
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		SetColor((Color)(info.photonView.InstantiationData[0]));
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
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	public void Fire(InputAction.CallbackContext context)
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Shoot
		if (weaponScript && context.started)
		{
			weaponScript.Shoot(photonView, lookDirection, color);
		}
	}

	public void SetColor(Color color)
	{
		transform.Find("Body").GetComponent<Shapes2D.Shape>().settings.fillColor = this.color = color;
	}

	[PunRPC]
	public void CreateBullet(Vector3 position, Vector2 direction, Color color)
	{
		GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
		BulletScript bulletScript = bullet.GetComponent<BulletScript>();

		bulletScript.moveDirection = direction;
		bulletScript.SetColor(color);
	}
}
