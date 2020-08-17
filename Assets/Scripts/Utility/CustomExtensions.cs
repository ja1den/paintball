using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomExtensions
{
	public static class ColorExtension
	{
		public static Vector3 ToVector3(this Color color)
		{
			return new Vector3(color.r, color.g, color.b);
		}
	}

	public static class Vector3Extension
	{
		public static Color ToColor(this Vector3 vector)
		{
			return new Color(vector.x, vector.y, vector.z);
		}
	}
}