using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	Horizontal,
	Vertical
}

public class LineScript : MonoBehaviour
{
	[Header("Line")]
	public Direction direction;

	[Space(10)]

	public Vector2 position;

	void Start()
	{
		transform.SetParent(GameObject.Find("Grid").transform);
	}

	void Update()
	{
		if (direction == Direction.Horizontal)
		{
			transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, 10);
			transform.localScale = new Vector3(Camera.main.orthographicSize * 2 * Camera.main.aspect, 0.1f, 1);
		}
		else
		{
			transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, 10);
			transform.localScale = new Vector3(0.1f, Camera.main.orthographicSize * 2, 1);
		}
	}
}
