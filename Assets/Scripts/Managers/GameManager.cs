using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	[Header("Control")]
	public bool isPlaying;

	[Header("GameObjects")]
	public GameObject playerPrefab;
	public GameObject[] spawns;

	[Header("Weapons")]
	public GameObject[] weapons;

	[Header("Colors")]
	public Color[] colors;

	void Awake()
	{
		// Spawns
		spawns = GameObject.FindGameObjectsWithTag("Respawn");
	}

	void Start()
	{
		// Spawn a Player
		object[] playerData = new object[] { Random.Range(0, weapons.Length), colors[Random.Range(0, colors.Length)] };
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawns[Random.Range(0, spawns.Length)].transform.position, Quaternion.identity, 0, playerData);

		// Assign the Camera
		Camera.main.GetComponent<CameraScript>().SetTarget(player.GetComponent<PlayerScript>());
	}
}
