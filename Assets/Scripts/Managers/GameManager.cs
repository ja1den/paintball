using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum GameState
{
	Start, Play, End
}


public class GameManager : MonoBehaviourPunCallbacks
{
	[Header("Control")]
	public GameState gameState = GameState.Start;

	[Space(10)]

	public float startLength = 3f;
	public float roundLength = 120f;

	[Space(10)]

	public double startTime;
	public double time;

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

		// Timer
		startTime = PhotonNetwork.Time;

		// Room Properties
		if (PhotonNetwork.IsMasterClient)
		{
			Hashtable roomProps = new Hashtable();
			roomProps.Add("startTime", startTime);

			PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
		}
	}

	void Start()
	{
		// Player Data
		object[] playerData = new object[] { Random.Range(0, weapons.Length), colors[Random.Range(0, colors.Length)] };

		// Spawn a Player
		Vector3 spawnPos = spawns[Random.Range(0, spawns.Length)].transform.position;
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.identity, 0, playerData);

		// Assign the Camera
		Camera.main.GetComponent<CameraScript>().SetTarget(player.GetComponent<PlayerScript>());
	}

	void Update()
	{
		// Update Time
		time = PhotonNetwork.Time - startTime;

		// Game State
		switch (gameState)
		{
			case GameState.Start:
				if (time > startLength) gameState = GameState.Play;
				break;

			case GameState.Play:
				if (time > startLength + roundLength)
				{
					if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel("ScoreScene");
					gameState = GameState.End;
				}
				break;
		}
	}

	public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
		object startTime;
		if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("startTime", out startTime))
		{
			this.startTime = (double)startTime;
		}
	}
}
