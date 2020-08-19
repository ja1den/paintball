using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
	[Header("General")]
	public Color color;

	[Header("Movement")]
	public float speed = 0.4f;

	[Space(10)]

	public Vector3 mDelta;

	[Header("Debug")]
	new private Renderer renderer;

	void Awake()
	{
		// Components
		renderer = GetComponent<Renderer>();
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
		// Apply Input
		Vector2 input = context.ReadValue<Vector2>();
		mDelta = new Vector3(input.x, input.y, 0f) * speed;
	}

	// Update Color
	public void SetColor(Color _color)
	{
		color = _color;
		renderer.material.color = _color;
	}

	// Photon Instantiate
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		SetColor((Color)(info.photonView.InstantiationData[0]));
	}
}
