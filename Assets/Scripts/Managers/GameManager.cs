using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	[Header("GameObjects")]
	public GameObject playerPrefab;

	void Start()
	{
		// Spawn a Player
		Color color = Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 1f);
		GameObject playerInstance = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0, new object[] { color });
	}
}
