using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	[Header("General")]
	public string gameVersion = "1.0.0";

	void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	void Start()
	{
		Connect();
	}

	void Connect()
	{
		if (PhotonNetwork.IsConnected)
		{
			// Join a Random Room
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			// Connect to Photon
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
		}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("Random Join Failed");

		// Create a new Room
		PhotonNetwork.CreateRoom(null, new RoomOptions());
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Photon");

		// Connect to a Room
		Connect();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogWarningFormat($"Disconnected from Photon: {cause}");
	}
}
