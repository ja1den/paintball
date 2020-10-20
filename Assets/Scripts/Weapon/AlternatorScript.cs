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

	public override void Shoot(PlayerScript playerScript, Vector2 direction)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, playerScript.photonView.ViewID,
				transform.Find($"Spawn {spawn + 1}").position, direction, damage, 0.3f);

			prevTime = Time.time;
			spawn = 1 - spawn;
		}
	}
}
