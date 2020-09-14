using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class WeaponScript : MonoBehaviourPunCallbacks
{
	public abstract void Shoot(PhotonView photonView, Vector2 direction, Color color);
}
