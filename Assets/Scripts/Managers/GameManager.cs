using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
	[Header("Control")]
	public bool isPlaying = false;

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
		// Spawn a Player
		object[] playerData = new object[] { Random.Range(0, weapons.Length), colors[Random.Range(0, colors.Length)] };
		GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawns[Random.Range(0, spawns.Length)].transform.position, Quaternion.identity, 0, playerData);

		// Assign the Camera
		Camera.main.GetComponent<CameraScript>().SetTarget(player.GetComponent<PlayerScript>());
	}

	void Update()
	{
		if ((time = PhotonNetwork.Time - startTime) > startLength) isPlaying = true;
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
