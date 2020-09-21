using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DoubleScript : WeaponScript
{
	[Header("Attributes")]
	public float damage = 25f;
	public float delay = 0.25f;

	[Header("Debug")]
	private float prevTime = 0f;

	public override void Shoot(PhotonView photonView, Vector2 direction, Color color)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, transform.Find("Spawn 1").position, direction, color, 0.4f);
			photonView.RPC("CreateBullet", RpcTarget.All, transform.Find("Spawn 2").position, direction * -1, color, 0.4f);

			prevTime = Time.time;
		}
	}
}
