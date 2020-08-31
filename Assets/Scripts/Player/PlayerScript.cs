using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI.Extensions;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
	[Header("Movement")]
	public float speed = 0.4f;

	[Space(10)]

	public Vector2 mDelta;

	[Header("Appearance")]
	public Color color;
	public UIPrimitiveBase[] colorables;

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

	public void Look(InputAction.CallbackContext context)
	{
		// Look at Cursor
		Vector3 cursorPos = context.ReadValue<Vector2>();
		Debug.Log(cursorPos);
	}

	// Update Color
	public void SetColor(Color _color)
	{
		color = _color;

		for (int i = 0; i < colorables.Length; i++)
		{
			colorables[i].color = color;
		}
	}

	// Photon Instantiate
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		SetColor((Color)(info.photonView.InstantiationData[0]));
	}
}
