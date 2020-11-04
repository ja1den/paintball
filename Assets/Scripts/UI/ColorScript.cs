using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Shapes2D;

public class ColorScript : MonoBehaviour
{
	void Start()
	{
		NetworkManager networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		Color color = networkManager.colors[(PhotonNetwork.LocalPlayer.ActorNumber - 1) % 8];

		GetComponent<Shape>().settings.fillColor = color;
	}
}
