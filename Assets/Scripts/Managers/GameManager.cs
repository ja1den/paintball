using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
	[Header("GameObjects")]
	public GameObject playerPrefab;

	public override void OnJoinedRoom()
	{
		GameObject playerInstance = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
	}
}
