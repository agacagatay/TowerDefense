using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnedAllyDictionary : MonoBehaviour
{
	public Dictionary<GameObject, int> spawnedAllyDictionary = new Dictionary<GameObject, int>();
	public static SpawnedAllyDictionary instance;

	void Awake()
	{
		instance = this;
	}
}
