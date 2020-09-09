using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviour, IPunInstantiateMagicCallback
{
	[Header("Movement")]
	public Vector2 moveDirection;
	public float moveSpeed;

	[Header("Appearance")]
	public Color color;

	[Header("Debug")]
	private Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		transform.SetParent(GameObject.Find("Bullets").transform);
		Destroy(gameObject, 10f);
	}

	void Update()
	{
		rb.velocity = moveDirection * moveSpeed;
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		SetColor((Color)(info.photonView.InstantiationData[0]));
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Destroy(gameObject);
	}

	public void SetColor(Color color)
	{
		GetComponent<Shapes2D.Shape>().settings.fillColor = this.color = color;
	}
}
