using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("GameObjects")]
	public Canvas canvas;

	[Space(10)]

	public GameObject playerPrefab;

	[Header("Debug")]
	public RectTransform cTrans;

	void Awake()
	{
		// Load the Main Scene
		if (!PhotonNetwork.InRoom)
		{
			SceneManager.LoadScene("LoadScene", LoadSceneMode.Single);
			return;
		}

		// Components
		cTrans = canvas.GetComponent<RectTransform>();
	}

	void Start()
	{
		// Spawn a Player
		Color color = Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 1f);
		Vector3 playerPos = new Vector3(cTrans.rect.width / 2, cTrans.rect.height / 2, 0f);

		GameObject playerInstance = PhotonNetwork.Instantiate(playerPrefab.name, playerPos, Quaternion.identity, 0, new object[] { color });
	}
}
