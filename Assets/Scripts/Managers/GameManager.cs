using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

[System.Serializable]
public struct Team
{
	public GameObject spawn;
	public Color color;
}

public class GameManager : MonoBehaviour
{
	[Header("GameObjects")]
	public GameObject playerPrefab;

	[Header("Teams")]
	public Team[] teams;

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
		// Player Data
		int team = Random.Range(0, teams.Length);

		// Spawn a Player
		object[] playerData = new object[] { team, Random.Range(0, weapons.Length) };
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, teams[team].spawn.transform.position, Quaternion.identity, 0, playerData);

		// Assign the Camera
		Camera.main.GetComponent<CameraScript>().target = player;
	}
}
