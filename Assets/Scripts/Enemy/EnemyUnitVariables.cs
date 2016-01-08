using UnityEngine;
using System.Collections;

public class EnemyUnitVariables : MonoBehaviour
{
	[SerializeField] int priorityValue = 0;
	[SerializeField] int unitHealth = 100;

	void Start()
	{
		SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Add(gameObject, priorityValue);
	}

	public void DamageUnit(int damageValue)
	{
		unitHealth -= damageValue;

		if (unitHealth <= 0)
		{
			SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Remove(gameObject);

			if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Count == 0 && EnemySpawnerController.instance.SpawnWaveComplete())
				EnemySpawnerController.instance.SpawnNextEnemyWave();

			Destroy(gameObject);
		}
	}
}
