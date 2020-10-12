using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DoubleScript : WeaponScript
{
	[Header("Attributes")]
	public int damage = 25;
	public float delay = 0.25f;

	[Header("Debug")]
	private float prevTime = 0f;

	public override void Shoot(PlayerScript playerScript, Vector2 direction)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, playerScript.photonView.ViewID, playerScript.bulletCount++,
				transform.Find("Spawn 1").position, direction, damage, 0.4f);

			photonView.RPC("CreateBullet", RpcTarget.All, playerScript.photonView.ViewID, playerScript.bulletCount++,
				transform.Find("Spawn 2").position, direction * -1, damage, 0.4f);

			prevTime = Time.time;
		}
	}
}
