using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SniperScript : WeaponScript
{
	[Header("Attributes")]
	public int damage = 125;
	public float delay = 1.25f;

	[Header("Debug")]
	private float prevTime = 0f;

	public override void Shoot(PlayerScript playerScript, Vector2 direction)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, playerScript.photonView.ViewID, playerScript.bulletCount++,
				transform.Find("Spawn").position, direction, damage, 0.45f);

			prevTime = Time.time;
		}
	}
}
