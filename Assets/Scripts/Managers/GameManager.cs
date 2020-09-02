using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("GameObjects")]
	public GameObject playerPrefab;

	[Header("Appearance")]
	public Color[] colors;

	void Awake()
	{
		// Load the Main Scene (Debug)
		if (!PhotonNetwork.InRoom)
		{
			SceneManager.LoadScene("LoadScene", LoadSceneMode.Single);
			return;
		}

		// Validate
		if (colors.Length == 0) throw new System.ArgumentException("Array cannot be empty", "colors");
	}

	void Start()
	{
		// Spawn a Player
		object[] playerData = new object[] { colors[Random.Range(0, colors.Length)] };
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0, playerData);

		// Assign the Camera
		Camera.main.GetComponent<CameraScript>().target = player;
	}
}
