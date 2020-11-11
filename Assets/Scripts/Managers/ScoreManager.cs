using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreManager : MonoBehaviour
{
	[Header("Debug")]
	private TMP_Text text;

	void Awake()
	{
		text = GameObject.Find("MessageText").GetComponent<TMP_Text>();
	}

	void Start()
	{
		int bestScore = 0;
		int thisScore = 0;

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			player.CustomProperties.TryGetValue("score", out object score);

			if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) thisScore = (int)(score ?? 0);
			bestScore = Mathf.Max((int)(score ?? 0), bestScore);
		}

		text.text = thisScore == bestScore ? "You win!" : "You lose!";
	}

	public void Click()
	{
		// Disable Button
		GameObject.Find("PlayButton").GetComponent<Button>().interactable = false;

		// CustomProperties
		Hashtable playerProps = new Hashtable();

		foreach (DictionaryEntry entry in PhotonNetwork.LocalPlayer.CustomProperties)
			playerProps.Add(entry.Key, null);

		PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

		// New Room
		GameObject.Find("NetworkManager").GetComponent<NetworkManager>().NewRoom();
	}
}
