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

		/*
		public static byte[] Serialize(this Color color)
		{
			Color c = (Color)color;
		}

		public static Color Deserialize(byte[] data)
		{
			return new Color(data[0], data[1], data[2]);
		}
		*/

		/*
		public static object Deserialize(byte[] data)
		{
			var result = new MyCustomType();
			result.Id = data[0];
			return result;
		}

		public static byte[] Serialize(object customType)
		{
			var c = (MyCustomType)customType;
			return new byte[] { c.Id };
		}
		*/
	}

	public static class Vector3Extension
	{
		public static Color ToColor(this Vector3 vector)
		{
			return new Color(vector.x, vector.y, vector.z);
		}
	}
}