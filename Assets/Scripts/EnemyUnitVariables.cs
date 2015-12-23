using UnityEngine;
using System.Collections;

public class EnemyUnitVariables : MonoBehaviour
{
	[SerializeField] string enemyUnitType;

	void Start()
	{
		SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Add(gameObject, enemyUnitType);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			string unitType;

			if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.TryGetValue(gameObject, out unitType))
				Debug.Log(unitType);
		}
		else if (Input.GetKeyDown(KeyCode.K))
		{
			SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Remove(gameObject);
			Destroy(gameObject);
		}
	}
}
