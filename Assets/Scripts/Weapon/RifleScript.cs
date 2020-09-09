using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RifleScript : WeaponScript
{
	public GameObject bulletSpawn;

	public override void CreateBullet(Vector2 direction, Color color)
	{
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, Quaternion.identity);
		BulletScript bulletScript = bullet.GetComponent<BulletScript>();

		bulletScript.moveDirection = direction;
		bulletScript.SetColor(color);
	}
}
