using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameScript : MonoBehaviour
{
	void FixedUpdate()
	{
		GetComponent<TMP_Text>().text = (1 / Time.deltaTime).ToString();
	}
}
