using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviour
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

		rb.velocity = moveDirection * moveSpeed;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (LayerMask.Equals(col.gameObject.layer, LayerMask.NameToLayer("Player")))
		{
			Debug.Log("Bullet hit Player!");
		}

		Destroy(gameObject);
	}

	public void SetColor(Color color)
	{
		GetComponent<Shapes2D.Shape>().settings.fillColor = this.color = color;
	}
}
