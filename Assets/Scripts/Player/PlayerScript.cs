using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using CustomExtensions;

public class PlayerScript : MonoBehaviourPunCallbacks
{
	[Header("General")]
	public Color color;

	[Header("Movement")]
	public float speed = 0.4f;

	[Space(10)]

	public Vector3 mDelta;

	[Header("Debug")]
	new private Renderer renderer;

	void Start()
	{
		// Components
		renderer = GetComponent<Renderer>();

		// Parent
		transform.SetParent(GameObject.Find("Players").transform);

		// Random Color
		if (photonView.IsMine)
		{
			color = Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 1f);

			Hashtable hashtable = new Hashtable();
			hashtable.Add("color", color.ToVector3());

			PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
		}
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

	public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
	{
		// Read Network Color
		object obj;
		if (photonView.Owner.CustomProperties.TryGetValue("color", out obj))
		{
			Vector3 vector = (Vector3)obj;
			renderer.material.color = vector.ToColor();
		}
	}
}
