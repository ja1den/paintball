using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SniperScript : WeaponScript
{
	[Header("Attributes")]
	public float damage = 125f;
	public float delay = 1.25f;

	[Header("Debug")]
	private float prevTime = 0f;

	public override void Shoot(PhotonView photonView, Vector2 direction, Color color)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, transform.Find("Spawn").position, direction, color, 0.5f);
			prevTime = Time.time;
		}
	}
}
