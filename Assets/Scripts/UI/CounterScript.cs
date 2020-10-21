using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CounterScript : MonoBehaviour
{
	[Header("Debug")]
	private TMP_Text text;

	[Space(10)]

	private GameManager gameManager;

	void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		text = GetComponent<TMP_Text>();
	}

	void Update()
	{
		text.text = Mathf.CeilToInt(gameManager.startLength - (float)gameManager.time).ToString();
		text.enabled = !gameManager.isPlaying;
	}
}
