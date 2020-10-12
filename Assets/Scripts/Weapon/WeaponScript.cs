using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class WeaponScript : MonoBehaviourPunCallbacks
{
	public abstract void Shoot(PlayerScript playerScript, Vector2 direction);
}
