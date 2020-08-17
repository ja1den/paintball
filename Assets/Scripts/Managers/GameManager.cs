using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using CustomExtensions;

public class GameManager : MonoBehaviourPunCallbacks
{
	[Header("GameObjects")]
	public GameObject playerPrefab;

	public override void OnJoinedRoom()
	{
		Color color = Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 1f);

		object[] customData = new object[1];
		customData[0] = color.ToVector3();

		GameObject playerInstance = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0, customData);
	}
}
