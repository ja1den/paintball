using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	[Header("UI")]
	public TMP_Text playerText;
	public Button playButton;

	void Start()
	{
		// Get Components
		playerText = GameObject.Find("PlayerText").GetComponent<TMP_Text>();
		playButton = GameObject.Find("PlayButton").GetComponent<Button>();

		UpdateUI();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
		{
			LoadGame();
		}

		UpdateUI();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		UpdateUI();
	}

	void UpdateUI()
	{
		playerText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
		playButton.gameObject.SetActive(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount != 1);
	}

	public void LoadGame()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel("GameScene");

			PhotonNetwork.CurrentRoom.IsVisible = false;
			PhotonNetwork.CurrentRoom.IsOpen = false;
		}
	}
}
