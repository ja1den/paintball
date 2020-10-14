using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RespawnScript : MonoBehaviour
{
	[Header("Debug")]
	private TMP_Text text;

	[Space(10)]

	[HideInInspector]
	public PlayerScript playerScript;

	void Awake()
	{
		text = GetComponent<TMP_Text>();
	}

	void Update()
	{
		text.text = Mathf.CeilToInt(5 - (Time.time - playerScript.respawn)).ToString();
		text.enabled = playerScript.isRespawning;
	}
}
