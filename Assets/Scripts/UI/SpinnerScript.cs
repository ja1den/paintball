using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerScript : MonoBehaviour
{
	[Header("General")]
	public float speed = 540f;

	void Update()
	{
		transform.Rotate(new Vector3(0, 0, Time.deltaTime * speed));
	}
}
