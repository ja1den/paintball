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

	void Update()
	{
		if (target)
		{
			transform.position = target.transform.position + offset;
		}
	}

	public void SetTarget(PlayerScript playerScript)
	{
		transform.position = (target = (this.playerScript = playerScript).gameObject).transform.position + offset;
	}
}
