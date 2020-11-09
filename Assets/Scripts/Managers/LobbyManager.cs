using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	[Header("Control")]
	public bool hasWeapon = false;

	[Header("UI")]
	public CanvasGroup weaponPanel;
	public CanvasGroup infoPanel;

	[Space(10)]

	public TMP_Text playerText;
	public Button playButton;

	void Start()
	{
		// Panels
		weaponPanel = GameObject.Find("WeaponPanel").GetComponent<CanvasGroup>();
		infoPanel = GameObject.Find("InfoPanel").GetComponent<CanvasGroup>();

		// InfoPanel
		playerText = GameObject.Find("PlayerText").GetComponent<TMP_Text>();
		playButton = GameObject.Find("PlayButton").GetComponent<Button>();

		// Update
		UpdateUI();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		UpdateUI();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		UpdateUI();
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		UpdateUI();
	}

	void UpdateUI()
	{
		// Panels
		weaponPanel.alpha = hasWeapon ? 0 : 1;
		weaponPanel.interactable = !hasWeapon;

		infoPanel.alpha = hasWeapon ? 1 : 0;
		infoPanel.interactable = hasWeapon;

		// InfoPanel
		bool canStart = PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount != 1;

		if (canStart)
		{
			foreach (Player player in PhotonNetwork.PlayerList)
			{
				player.CustomProperties.TryGetValue("weapon", out object weapon);
				if (weapon == null) canStart = false;
			}
		}

		playButton.gameObject.SetActive(canStart);
		playerText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
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

	public void SelectWeapon(int number)
	{
		Hashtable playerProps = new Hashtable();
		playerProps.Add("weapon", number - 1);

		PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
		hasWeapon = true;

		UpdateUI();
	}
}
