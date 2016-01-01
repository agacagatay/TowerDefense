using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnedEnemyDictionary : MonoBehaviour
{
	public Dictionary<GameObject, int> spawnedEnemyDictionary = new Dictionary<GameObject, int>();
	public static SpawnedEnemyDictionary instance;

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
			Debug.Log("Total Enemies: " + spawnedEnemyDictionary.Count.ToString());
	}
}
