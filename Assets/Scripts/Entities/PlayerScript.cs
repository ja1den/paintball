using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
	[Header("Movement")]
	public float speed;

	[Space(10)]

	public Vector2 direction;

	[Header("Shooting")]
	public Vector2 lookDir;

	[Space(10)]

	public GameObject bulletPrefab;
	private GameObject bulletSpawn;

	[Space(10)]

	private bool isShooting = false;
	private float shootTime = 0f;

	[Header("Appearance")]
	public Color color;

	[Header("Debug")]
	private RectTransform canvas;

	void Awake()
	{
		// World
		canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

		// Child
		bulletSpawn = transform.Find("Gun").gameObject;
	}

	void Start()
	{
		transform.SetParent(GameObject.Find("Players").transform);
	}

	void FixedUpdate()
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Movement
		transform.Translate(direction * speed * Time.deltaTime, Space.World);

		// Shooting
		if (isShooting && Time.time - 0.25f >= shootTime)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, bulletSpawn.transform.position, lookDir, color);
			shootTime = Time.time;
		}
	}

	public void Move(InputAction.CallbackContext context)
	{
		direction = context.ReadValue<Vector2>();
	}

	public void Look(InputAction.CallbackContext context)
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Look at Cursor
		Vector2 cursor = context.ReadValue<Vector2>();
		Vector2 offset = new Vector2(canvas.rect.width, canvas.rect.height) * 0.5f;

		lookDir = (cursor - offset).normalized;

		float angle = Vector2.SignedAngle(Vector2.up, lookDir);
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	public void Fire(InputAction.CallbackContext context)
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Button Down
		if (context.started)
			isShooting = true;

		// Button Up
		if (context.canceled)
			isShooting = false;
	}

	[PunRPC]
	public void CreateBullet(Vector3 spawnPos, Vector2 direction, Color color)
	{
		GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
		BulletScript bulletScript = bullet.GetComponent<BulletScript>();

		bulletScript.direction = direction;
		bulletScript.SetColor(color);
	}

	// Update Color
	public void SetColor(Color _color)
	{
		GetComponent<Shapes2D.Shape>().settings.fillColor = color = _color;
	}

	// Photon Instantiate
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		SetColor((Color)(info.photonView.InstantiationData[0]));
	}
}
