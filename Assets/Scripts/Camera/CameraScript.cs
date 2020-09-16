using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[Header("General")]
	public GameObject target;

	[Header("Grid")]
	public GameObject linePrefab;
	public int spacing = 1;

	[Header("Debug")]

	public List<LineScript> hLines = new List<LineScript>();
	public List<LineScript> vLines = new List<LineScript>();

	void Start()
	{
		// Bounds
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

		(int, int) hRound = (Mathf.FloorToInt(bottomLeft.y), Mathf.CeilToInt(topRight.y));
		(int, int) vRound = (Mathf.FloorToInt(bottomLeft.x), Mathf.CeilToInt(topRight.x));

		// Lines
		for (int i = hRound.Item1; i < hRound.Item2; i += spacing)
			SpawnLine(new Vector3(0, i, 10), Direction.Horizontal);

		for (int i = vRound.Item1; i < vRound.Item2; i += spacing)
			SpawnLine(new Vector3(i, 0, 10), Direction.Vertical);
	}

	void Update()
	{
		// Target
		transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);

		// Bounds
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

		(int, int) hRound = (Mathf.FloorToInt(bottomLeft.y), Mathf.CeilToInt(topRight.y));
		(int, int) vRound = (Mathf.FloorToInt(bottomLeft.x), Mathf.CeilToInt(topRight.x));

		// Horizontal
		foreach (LineScript lineScript in hLines.ToArray())
		{
			if (lineScript.position.y < hRound.Item1 - 1 || hRound.Item2 + 1 < lineScript.position.y)
			{
				hLines.Remove(lineScript);
				Destroy(lineScript.gameObject);
			}
		}

		if (!hLines.Exists(lineScript => lineScript.position.y == hRound.Item1))
			SpawnLine(new Vector3(0, hRound.Item1, 10), Direction.Horizontal);

		if (!hLines.Exists(lineScript => lineScript.position.y == hRound.Item2))
			SpawnLine(new Vector3(0, hRound.Item2, 10), Direction.Horizontal);

		// Vertical
		foreach (LineScript lineScript in vLines.ToArray())
		{
			if (lineScript.position.x - 1 < vRound.Item1 || vRound.Item2 + 1 < lineScript.position.x)
			{
				vLines.Remove(lineScript);
				Destroy(lineScript.gameObject);
			}
		}

		if (!vLines.Exists(lineScript => lineScript.position.y == vRound.Item1))
			SpawnLine(new Vector3(vRound.Item1, 0, 10), Direction.Vertical);

		if (!vLines.Exists(lineScript => lineScript.position.y == vRound.Item2))
			SpawnLine(new Vector3(vRound.Item2, 0, 10), Direction.Vertical);
	}

	void SpawnLine(Vector3 position, Direction direction)
	{
		GameObject line = Instantiate(linePrefab, position, Quaternion.identity);
		LineScript lineScript = line.GetComponent<LineScript>();

		lineScript.direction = direction;
		lineScript.position = position;

		if (direction == Direction.Horizontal) hLines.Add(lineScript);
		else vLines.Add(lineScript);
	}
}
