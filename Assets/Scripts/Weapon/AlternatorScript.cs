using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AlternatorScript : WeaponScript
{
	[Header("Attributes")]
	public float delay = 0.1f;

	[Space(10)]

	public int damage = 10;
	public float size = 0.3f;

	[Header("Debug")]
	private float prevTime = 0f;

	[Space(10)]

	private int spawn = 0;

	public override void Shoot(PlayerScript playerScript, Vector2 direction)
	{
		if (prevTime + delay < Time.time)
		{
			CreateBullet(playerScript, transform.Find($"Spawn {spawn + 1}"), damage, size);

			prevTime = Time.time;
			spawn = 1 - spawn;
		}
	}
}
