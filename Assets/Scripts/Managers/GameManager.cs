using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	[Header("GameObjects")]
	public GameObject playerPrefab;

	[Space(10)]

	public GameObject[] spawns;

	[Header("Weapons")]
	public GameObject[] weapons;

	void Awake()
	{
		// Load the Main Scene (Debug)
		if (!PhotonNetwork.InRoom)
		{
			SceneManager.LoadScene("LoadScene", LoadSceneMode.Single);
			return;
		}

		// Spawns
		spawns = GameObject.FindGameObjectsWithTag("Respawn");
	}

	void Start()
	{
		// Player Data
		object[] playerData = new object[] { Random.Range(0, weapons.Length), Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 1f) };

		// Spawn a Player
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawns[Random.Range(0, spawns.Length)].transform.position, Quaternion.identity, 0, playerData);

		// Assign the Camera
		Camera.main.GetComponent<CameraScript>().target = player;
	}
}
