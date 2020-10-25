using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
	[Header("Debug")]
	private TMP_Text text;

	[HideInInspector]
	public GameManager gameManager;

	void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		text = GetComponent<TMP_Text>();
	}

	void Update()
	{
		text.text = TimeSpan.FromSeconds(gameManager.roundLength - Math.Floor(gameManager.time - gameManager.startLength)).ToString(@"mm\:ss");
		text.enabled = gameManager.isPlaying;
	}
}
