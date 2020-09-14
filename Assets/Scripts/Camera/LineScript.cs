using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
	public void Start()
	{
		transform.SetParent(GameObject.Find("Grid").transform);
	}

	public void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}