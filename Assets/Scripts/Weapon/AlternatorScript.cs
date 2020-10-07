using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AlternatorScript : WeaponScript
{
	[Header("Attributes")]
	public int damage = 10;
	public float delay = 0.1f;

	[Header("Debug")]
	private float prevTime = 0f;

	[Space(10)]

	private int spawn = 0;

	public override void Shoot(PhotonView photonView, Vector2 direction, int team, Color color)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, photonView.ViewID, transform.Find($"Spawn {spawn + 1}").position, direction, team, damage, 0.3f, color);
			prevTime = Time.time;

			spawn = 1 - spawn;
		}
	}
}
