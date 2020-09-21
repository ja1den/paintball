using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
	[Header("Grid")]
	public GameObject linePrefab;

	[Header("Debug")]
	private Dictionary<(int, Direction), GameObject> lines =
		new Dictionary<(int, Direction), GameObject>();

	public enum Direction
	{
		Horizontal,
		Vertical
	}

	void Start()
	{
		// Camera Bounds
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

		(int, int) hRound = (Mathf.FloorToInt(bottomLeft.y), Mathf.CeilToInt(topRight.y));
		(int, int) vRound = (Mathf.FloorToInt(bottomLeft.x), Mathf.CeilToInt(topRight.x));

		// Horizontal Lines
		for (int i = hRound.Item1; i < hRound.Item2; i++)
			SpawnLine(i, Direction.Horizontal);

		// Vertical Lines
		for (int i = vRound.Item1; i < vRound.Item2; i++)
			SpawnLine(i, Direction.Vertical);
	}

	void Update()
	{
		// Camera Bounds
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

		(int, int) hRound = (Mathf.FloorToInt(bottomLeft.y), Mathf.CeilToInt(topRight.y));
		(int, int) vRound = (Mathf.FloorToInt(bottomLeft.x), Mathf.CeilToInt(topRight.x));

		// Lines to Remove
		List<((int, Direction), GameObject)> linesToRemove = new List<((int, Direction), GameObject)>();

		// Update Lines
		foreach (KeyValuePair<(int, Direction), GameObject> entry in lines)
		{
			(int, Direction) key = entry.Key;
			GameObject line = entry.Value;

			switch (key.Item2)
			{
				case Direction.Horizontal:

					// Destroy
					if (key.Item1 < hRound.Item1 - 1 || hRound.Item2 + 1 < key.Item1)
					{
						linesToRemove.Add((key, line));
						continue;
					}

					// Follow Camera
					line.transform.position = new Vector3(Camera.main.transform.position.x, line.transform.position.y, 10);
					line.transform.localScale = new Vector3(Camera.main.orthographicSize * 2 * Camera.main.aspect, 0.1f, 1);

					break;

				case Direction.Vertical:

					// Destroy
					if (key.Item1 < vRound.Item1 - 1 || vRound.Item2 + 1 < key.Item1)
					{
						linesToRemove.Add((key, line));
						continue;
					}

					// Follow Camera
					line.transform.position = new Vector3(line.transform.position.x, Camera.main.transform.position.y, 10);
					line.transform.localScale = new Vector3(0.1f, Camera.main.orthographicSize * 2, 1);

					break;
			}
		}

		// Destroy Lines
		foreach (((int, Direction), GameObject) entry in linesToRemove)
		{
			lines.Remove(entry.Item1);
			Destroy(entry.Item2);
		}

		// Spawn Lines
		if (!lines.ContainsKey((hRound.Item1, Direction.Horizontal)))
			SpawnLine(hRound.Item1, Direction.Horizontal);

		if (!lines.ContainsKey((hRound.Item2, Direction.Horizontal)))
			SpawnLine(hRound.Item2, Direction.Horizontal);

		if (!lines.ContainsKey((vRound.Item1, Direction.Vertical)))
			SpawnLine(vRound.Item1, Direction.Vertical);

		if (!lines.ContainsKey((vRound.Item2, Direction.Vertical)))
			SpawnLine(vRound.Item2, Direction.Vertical);
	}

	void SpawnLine(int position, Direction direction)
	{
		Vector3 spawn = direction == Direction.Horizontal ?
			new Vector3(0, position, 10) :
			new Vector3(position, 0, 10);

		GameObject line = Instantiate(linePrefab, spawn, Quaternion.identity);
		line.transform.SetParent(GameObject.Find("Grid").transform);

		lines.Add((position, direction), line);
	}
}
