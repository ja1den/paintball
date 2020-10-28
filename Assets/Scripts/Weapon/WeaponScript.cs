using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class WeaponScript : MonoBehaviourPunCallbacks
{
	protected void CreateBullet(PlayerScript playerScript, Transform spawn, int damage, float size)
	{
		photonView.RPC("CreateBullet", RpcTarget.All, playerScript.photonView.ViewID, spawn.position, (Vector2)spawn.up, damage, size);
	}

	public abstract void Shoot(PlayerScript playerScript, Vector2 direction);
}
