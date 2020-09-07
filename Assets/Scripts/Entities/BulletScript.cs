using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviour, IPunInstantiateMagicCallback
{
	[Header("Movement")]
	public float speed;

	[Space(10)]

	public Vector2 direction;

	[Header("Appearance")]
	public Color color;

	void Start()
	{
		transform.SetParent(GameObject.Find("Bullets").transform);

		// Lifetime
		Destroy(gameObject, 10f);
	}

	void Update()
	{
		transform.Translate(direction * speed * Time.deltaTime, Space.World);
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
