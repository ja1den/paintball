using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI.Extensions;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
	[Header("General")]
	public Color color;

	[Header("Movement")]
	public float speed = 0.4f;

	[Space(10)]

	public Vector2 mDelta;

	[Header("Debug")]
	new private SpriteRenderer renderer;

	void Awake()
	{
		// Components
		renderer = GetComponent<SpriteRenderer>();
	}

	void Start()
	{
		// Parent
		transform.SetParent(GameObject.Find("Players").transform);
	}

	void FixedUpdate()
	{
		// Check Client
		if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

		// Movement
		transform.Translate(mDelta);
	}

	public void Move(InputAction.CallbackContext context)
	{
		mDelta = context.ReadValue<Vector2>() * speed;
	}

	// Update Color
	public void SetColor(Color _color)
	{
		color = renderer.color = _color;
	}

	// Photon Instantiate
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		SetColor((Color)(info.photonView.InstantiationData[0]));
	}
}
