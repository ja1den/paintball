using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistScript : MonoBehaviour
{
	void Start()
	{
		DontDestroyOnLoad(this);
	}
}
