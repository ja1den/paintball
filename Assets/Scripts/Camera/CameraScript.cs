using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[Header("General")]
	public GameObject target;

	[Space(10)]

	public Vector3 offset = new Vector3(0, 0, -10);
	public float speed = 1f;

	[Header("Debug")]
	private PlayerScript playerScript;

	[Space(10)]

	private Vector3 velocity;

	void Start()
	{
		playerScript = target.GetComponent<PlayerScript>();
	}

	void Update()
	{
		if (target)
		{
			if (playerScript.isRespawning)
			{
				transform.position = Vector3.SmoothDamp(transform.position, target.transform.position + offset, ref velocity, speed);
			}
			else
			{
				transform.position = target.transform.position + offset;
			}
		}
	}
}
