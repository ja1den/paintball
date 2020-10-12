using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameScript : MonoBehaviour
{
	[Header("Control")]
	public float delay = 1f;

	[Header("Debug")]
	private float prevTime;
	private int frames;

	void Awake()
	{
		prevTime = Time.time;
	}

	void Start()
	{
		GetComponent<TMP_Text>().text = Mathf.RoundToInt(1f / Time.deltaTime).ToString();
	}

	void Update()
	{
		if (prevTime + delay < Time.time)
		{
			GetComponent<TMP_Text>().text = frames.ToString();

			prevTime = Time.time;
			frames = 0;
		}
		else frames++;
	}
}
