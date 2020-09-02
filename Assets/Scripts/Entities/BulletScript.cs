using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	[Header("Movement")]
	public float speed = 0.4f;

	[Space(10)]

	public Vector2 direction;

	[Header("Appearance")]
	public Color color;

	void Start()
	{
		transform.SetParent(GameObject.Find("Bullets").transform);
	}

	void Update()
	{
		transform.Translate(direction * speed, Space.World);
	}
}
