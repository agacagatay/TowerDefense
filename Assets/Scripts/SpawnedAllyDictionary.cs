using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnedAllyDictionary : MonoBehaviour
{
	public Dictionary<GameObject, string> spawnedAllyDictionary = new Dictionary<GameObject, string>();
	public static SpawnedAllyDictionary instance;

	void Awake()
	{
		instance = this;
	}
}
