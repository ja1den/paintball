using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[Header("General")]
	public GameObject target;

	[Header("Grid")]
	public float gridSpacing = 5f;

	[Space(10)]

	public GameObject linePrefab;

	void Update()
	{
		transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
	}

	void SpawnLine(Vector2 position, bool isFlat)
	{
		GameObject line = Instantiate(linePrefab, position, Quaternion.identity);
	}
}
