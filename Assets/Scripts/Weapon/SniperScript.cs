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

	public override void Shoot(PhotonView photonView, Vector2 direction, int team, Color color)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, photonView.ViewID, transform.Find("Spawn").position, direction, team, damage, 0.45f, color);
			prevTime = Time.time;
		}
	}
}
