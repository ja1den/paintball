using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneScript : MonoBehaviour
{
	void Awake()
	{
		// Load the Main Scene (Debug)
		if (!PhotonNetwork.InRoom)
		{
			SceneManager.LoadScene("LoadScene", LoadSceneMode.Single);
			return;
		}
	}
}
