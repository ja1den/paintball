using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Shapes2D;

public class ShotgunScript : WeaponScript
{
	[Header("Attributes")]
	public float delay = 0.5f;

	[Space(10)]

	public int damage = 10;
	public float size = 0.3f;

	[Header("Shape")]
	public bool generate;

	[Header("Debug")]
	private float prevTime = 0f;

	void OnValidate()
	{
		if (generate)
		{
			Shape shape = transform.Find("Barrel").GetComponent<Shape>();

			shape.settings.polyVertices = new Vector2[4] {
				new Vector2(-0.4f, -0.5f),
				new Vector2(-0.5f,  0.5f),
				new Vector2( 0.5f,  0.5f),
				new Vector2( 0.4f, -0.5f)
			};

			shape.ComputeAndApply();
		}

		generate = false;
	}

	public override void Shoot(PlayerScript playerScript, Vector2 direction)
	{
		if (prevTime + delay < Time.time)
		{
			for (int i = 1; i <= 5; i++) CreateBullet(playerScript, transform.Find($"Spawn {i}"), damage, size);

			prevTime = Time.time;
		}
	}
}
