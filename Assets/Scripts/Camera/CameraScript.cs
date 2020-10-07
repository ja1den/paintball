using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[Header("General")]
	public GameObject target;

	void Update()
	{
		if (target) transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
	}
}
