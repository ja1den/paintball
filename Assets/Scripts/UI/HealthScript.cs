using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
	[Header("Control")]
	public int maxHealth;

	[Header("Debug")]
	public PlayerScript playerScript;

	[Space(10)]

	public GameObject fill;

	void Start()
	{
		playerScript = transform.parent.GetComponent<PlayerScript>();
		fill = transform.Find("Fill").gameObject;
	}

	void Update()
	{
		fill.transform.localPosition = new Vector3(-0.45f + 0.9f * (playerScript.health / (float)maxHealth) / 2f, 0f, -0.1f);
		fill.transform.localScale = new Vector3(0.9f * playerScript.health / (float)maxHealth, 0.20f, 1f);
	}
}
