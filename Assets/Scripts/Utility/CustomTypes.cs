using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Paintball
{
	internal static class CustomTypes
	{
		// Register Types
		internal static void Register()
		{
			PhotonPeer.RegisterType(typeof(Color), (byte)'C', SerializeColor, DeserializeColor);
		}

		public static readonly byte[] memColor = new byte[3 * 4];

		private static short SerializeColor(StreamBuffer outStream, object customobject)
		{
			Color color = (Color)customobject;

			int index = 0;
			lock (memColor)
			{
				byte[] bytes = memColor;

				Protocol.Serialize(color.r, bytes, ref index);
				Protocol.Serialize(color.g, bytes, ref index);
				Protocol.Serialize(color.b, bytes, ref index);

				outStream.Write(bytes, 0, 3 * 4);
			}

			return 3 * 4;
		}

		public static object DeserializeColor(StreamBuffer inStream, short length)
		{
			Color color = new Color();

			int index = 0;
			lock (memColor)
			{
				inStream.Read(memColor, 0, 3 * 4);

				Protocol.Deserialize(out color.r, memColor, ref index);
				Protocol.Deserialize(out color.g, memColor, ref index);
				Protocol.Deserialize(out color.b, memColor, ref index);
			}

			return color;
		}
	}
}
