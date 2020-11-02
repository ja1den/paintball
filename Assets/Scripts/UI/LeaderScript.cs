using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Shapes2D;

public class LeaderScript : MonoBehaviourPunCallbacks
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

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			player.CustomProperties.TryGetValue("score", out object score);
			bestScore = Mathf.Max((int)(score ?? 0), bestScore);
		}

		text.text = bestScore.ToString();
	}
}
