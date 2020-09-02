using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
	[Header("Movement")]
	public float speed = 0.4f;

	[Space(10)]

	public Vector2 mDelta;

	[Header("Appearance")]
	public Color color;

	[Space(10)]

	private GameObject body;

	[Header("Debug")]
	private RectTransform canvas;

	void Awake()
	{
		canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
		body = transform.Find("Body").gameObject;
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
		transform.Translate(mDelta, Space.World);
	}

	public void Move(InputAction.CallbackContext context)
	{
		mDelta = context.ReadValue<Vector2>() * speed;
	}

	public void Look(InputAction.CallbackContext context)
	{
		// Client's Player
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Look at Cursor
		Vector2 cursor = context.ReadValue<Vector2>();
		Vector2 offset = new Vector2(canvas.rect.width, canvas.rect.height) * 0.5f;

		float angle = Vector2.SignedAngle(Vector2.up, cursor - offset);

		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	// Update Color
	public void SetColor(Color _color)
	{
		body.GetComponent<Shapes2D.Shape>().settings.fillColor = color = _color;
	}

	// Photon Instantiate
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		SetColor((Color)(info.photonView.InstantiationData[0]));
	}
}
