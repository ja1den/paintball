using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

public class CrownScript : MonoBehaviour
{
	[Header("Shape")]
	public bool generate;

	void OnValidate()
	{
		if (generate)
		{
			Shape shape = GetComponent<Shape>();

			shape.settings.polyVertices = new Vector2[7] {
				new Vector2( -0.4f, -0.5f),
				new Vector2( -0.5f,  0.5f),
				new Vector2(-0.25f,  0.1f),
				new Vector2(    0f,  0.5f),
				new Vector2( 0.25f,  0.1f),
				new Vector2(  0.5f,  0.5f),
				new Vector2(  0.4f, -0.5f)
			};

			shape.ComputeAndApply();
		}

		generate = false;
	}
}
