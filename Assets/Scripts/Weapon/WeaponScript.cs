using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class WeaponScript : MonoBehaviourPunCallbacks
{
	[Header("GameObjects")]
	public GameObject bulletPrefab;

	[Header("Attributes")]
	public float damage = 100f;
	public float delay = 1f;

	[Header("Debug")]
	private float prevTime = 0f;

	public void Shoot(Vector2 direction, Color color)
	{
		if (prevTime + delay < Time.time)
		{
			photonView.RPC("CreateBullet", RpcTarget.All, direction, color);
			prevTime = Time.time;
		}
	}

	[PunRPC]
	public abstract void CreateBullet(Vector2 direction, Color color);
}
