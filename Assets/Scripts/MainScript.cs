using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using TMPro;

public enum GameState
{
	Menu, Play
}

public class MainScript : NetworkedBehaviour
{
	[Header("UI")]
	public TMP_InputField nameInput;
	public Button playButton;

	void Start()
	{
		// NetworkingManager.Singleton.StartClient();
	}

	public void NameInput()
	{
		playButton.interactable = nameInput.text.Length >= 3;
	}

	public void PlayButton()
	{
		Debug.Log("Joining as '" + nameInput.text + "'");
	}
}
