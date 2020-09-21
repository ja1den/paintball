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
		// Photon Config
		PhotonNetwork.AutomaticallySyncScene = true;
		Paintball.CustomTypes.Register();
	}

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
		PhotonNetwork.GameVersion = Application.version;
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel("GameScene");
		}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		PhotonNetwork.CreateRoom(null, new RoomOptions());
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		SceneManager.LoadScene("LoadScene", LoadSceneMode.Single);
		DestroyImmediate(gameObject);
	}
}
