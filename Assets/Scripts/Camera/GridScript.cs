using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
	[Header("GameObjects")]
	public GameObject linePrefab;

	[Header("Debug")]
	private Dictionary<(int, Direction), GameObject> lines =
		new Dictionary<(int, Direction), GameObject>();

	public enum Direction { H, V }

	void Start()
	{
		// Screen to World Space
		RectInt screen = ScreenRect(Camera.main);

		// Horizontal Lines
		for (int i = screen.yMin; i < screen.yMax; i++)
			SpawnLine(i, Direction.H);

		// Vertical Lines
		for (int i = screen.xMin; i < screen.xMax; i++)
			SpawnLine(i, Direction.V);
	}

	void Update()
	{
		// Screen to World Space
		RectInt screen = ScreenRect(Camera.main);

		// Horizontal Lines
		for (int i = screen.yMin - 1; i <= screen.yMax + 1; i++)
		{
			// Spawn a Line
			if (!lines.TryGetValue((i, Direction.H), out GameObject line))
				line = SpawnLine(i, Direction.H);

			// Follow Camera
			line.transform.position = new Vector3(screen.center.x, line.transform.position.y, line.transform.position.z);
			line.transform.localScale = new Vector3(screen.size.x, 0.1f, 1);
		}

		// Vertical Lines
		for (int i = screen.xMin - 1; i <= screen.xMax + 1; i++)
		{
			// Spawn a Line
			if (!lines.TryGetValue((i, Direction.V), out GameObject line))
				line = SpawnLine(i, Direction.V);

			// Follow Camera
			line.transform.position = new Vector3(line.transform.position.x, screen.center.y, line.transform.position.z);
			line.transform.localScale = new Vector3(0.1f, screen.size.y, 1);
		}

		// Lines to Remove
		List<((int, Direction), GameObject)> linesToRemove = new List<((int, Direction), GameObject)>();

		// Find Lines
		foreach (KeyValuePair<(int, Direction), GameObject> entry in lines)
		{
			if (entry.Key.Item2 == Direction.H)
				if (entry.Key.Item1 < screen.yMin - 1 || screen.yMax + 1 < entry.Key.Item1)
					linesToRemove.Add((entry.Key, entry.Value));

			if (entry.Key.Item2 == Direction.V)
				if (entry.Key.Item1 < screen.xMin - 1 || screen.xMax + 1 < entry.Key.Item1)
					linesToRemove.Add((entry.Key, entry.Value));
		}

		// Destroy Lines
		foreach (((int, Direction), GameObject) entry in linesToRemove)
		{
			lines.Remove(entry.Item1);
			Destroy(entry.Item2);
		}
	}

	GameObject SpawnLine(int position, Direction direction)
	{
		Transform gridParent = GameObject.Find("Grid").transform;

		Vector3 spawn = direction == Direction.H ?
			new Vector3(0, position, gridParent.position.z) :
			new Vector3(position, 0, gridParent.position.z);

		GameObject line = Instantiate(linePrefab, spawn, Quaternion.identity);
		line.transform.SetParent(gridParent, true);

		lines.Add((position, direction), line);
		return line;
	}

	RectInt ScreenRect(Camera camera)
	{
		RectInt rect = new RectInt();

		Vector3 min = camera.ScreenToWorldPoint(new Vector3(camera.rect.xMin * camera.pixelWidth, camera.rect.yMin * camera.pixelHeight, 0));
		Vector3 max = camera.ScreenToWorldPoint(new Vector3(camera.rect.xMax * camera.pixelWidth, camera.rect.yMax * camera.pixelHeight, 0));

		rect.SetMinMax(new Vector2Int(Mathf.FloorToInt(min.x), Mathf.FloorToInt(min.y)), new Vector2Int(Mathf.CeilToInt(max.x), Mathf.CeilToInt(max.y)));

		return rect;
	}
}
