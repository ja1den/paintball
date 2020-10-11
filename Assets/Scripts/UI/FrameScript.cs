using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameScript : MonoBehaviour
{
	[Header("Control")]
	public float delay = 1f;

	[Header("Debug")]
	private float prevTime = 0f;

	void Update()
	{
		if (prevTime + delay < Time.time)
		{
			GetComponent<TMP_Text>().text = Mathf.RoundToInt(1f / Time.deltaTime).ToString();
			prevTime = Time.time;
		}
	}
}
