using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreScript : MonoBehaviourPunCallbacks
{
	[Header("Debug")]
	private TMP_Text text;

	void Awake()
	{
		text = GetComponent<TMP_Text>();
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		int bestScore = 0;
		int thisScore = 0;

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			player.CustomProperties.TryGetValue("score", out object score);

			if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) thisScore = (int)(score ?? 0);
			bestScore = Mathf.Max((int)(score ?? 0), bestScore);
		}

		text.text = $"L: {bestScore}\nY: {thisScore}";
	}
}
