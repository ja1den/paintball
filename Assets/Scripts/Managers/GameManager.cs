using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	[Header("GameObjects")]
	public GameObject playerPrefab;

	[Header("Teams")]
	public Color[] teams;

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

		// Validate
		if (teams.Length == 0) throw new System.ArgumentException("Array cannot be empty", "teams");
	}

	void Start()
	{
		// Spawn a Player
		object[] playerData = new object[] { Random.Range(0, teams.Length), Random.Range(0, weapons.Length) };
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0, playerData);

		// Assign the Camera
		Camera.main.GetComponent<CameraScript>().target = player;
	}
}
