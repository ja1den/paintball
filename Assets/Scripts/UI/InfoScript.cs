using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class InfoScript : MonoBehaviour
{
	[Header("Debug")]
	private TMP_Text text;

	void Awake()
	{
		text = GetComponent<TMP_Text>();
	}

	void Update()
	{
		int players = PhotonNetwork.CountOfPlayers;
		text.text = $"{PhotonNetwork.CloudRegion}";
	}
}
