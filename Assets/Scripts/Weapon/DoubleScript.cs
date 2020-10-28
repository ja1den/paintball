using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DoubleScript : WeaponScript
{
	[Header("Attributes")]
	public float delay = 0.25f;

	[Space(10)]

	public int damage = 25;
	public float size = 0.4f;

	[Header("Debug")]
	private float prevTime = 0f;

	public override void Shoot(PlayerScript playerScript, Vector2 direction)
	{
		if (prevTime + delay < Time.time)
		{
			CreateBullet(playerScript, transform.Find("Spawn 1"), damage, size);
			CreateBullet(playerScript, transform.Find("Spawn 2"), damage, size);

			prevTime = Time.time;
		}
	}
}
