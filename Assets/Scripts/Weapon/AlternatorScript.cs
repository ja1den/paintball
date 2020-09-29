using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AlternatorScript : WeaponScript
{
	[Header("Attributes")]
	public float damage = 10f;
	public float delay = 0.1f;

	[Header("Debug")]
	private float prevTime = 0f;

	[Space(10)]

	private int spawn = 0;

	public override void Shoot(PhotonView photonView, Vector2 direction, Color color)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, transform.Find($"Spawn {spawn + 1}").position, direction, color, 0.3f);
			prevTime = Time.time;

			spawn = 1 - spawn;
		}
	}
}
