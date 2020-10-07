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

	public override void Shoot(PhotonView photonView, Vector2 direction, int team, Color color)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, photonView.ViewID, transform.Find("Spawn 1").position, direction, team, damage, 0.4f, color);
			photonView.RPC("CreateBullet", RpcTarget.All, photonView.ViewID, transform.Find("Spawn 2").position, direction * -1, team, damage, 0.4f, color);

			prevTime = Time.time;
		}
	}
}
