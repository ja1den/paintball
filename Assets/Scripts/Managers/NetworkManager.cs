using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;

		// Custom Types
		Paintball.CustomTypes.Register();
	}

	void Start()
	{
		// Connect to Photon
		PhotonNetwork.ConnectUsingSettings();
		PhotonNetwork.GameVersion = Application.version;
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Photon");

		// Join a Random Room
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined a Room");

		// Load the Game Scene
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel("GameScene");
		}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("Random Join Failed");

		// Create a new Room
		PhotonNetwork.CreateRoom(null, new RoomOptions());
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogWarningFormat($"Disconnected from Photon: {cause}");
	}


}
