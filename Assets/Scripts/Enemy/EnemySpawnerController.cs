using UnityEngine;
using System.Collections;

public class EnemySpawnerController : MonoBehaviour
{
	[SerializeField] EnemySpawnWave[] enemySpawnerWaves;
	int enemyWaveNumber = 0;

	public static EnemySpawnerController instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		enemySpawnerWaves[enemyWaveNumber].TriggerEnemyWave(enemyWaveNumber);
	}

	public void SpawnNextEnemyWave()
	{
		++enemyWaveNumber;

		if (enemyWaveNumber == enemySpawnerWaves.Length)
			Debug.Log("VICTORY!");
		else
			enemySpawnerWaves[enemyWaveNumber].TriggerEnemyWave(enemyWaveNumber);
	}
}

[System.Serializable]
public class EnemySpawnWave
{
	[SerializeField] EnemySpawnWaveContainer[] enemySpawnWaveContainers;

	public void TriggerEnemyWave(int enemyWaveNumber)
	{
		foreach (EnemySpawnWaveContainer enemySpawnWaveContainer in enemySpawnWaveContainers)
		{
			enemySpawnWaveContainer.TriggerEnemyWave();
		}
	}
}

[System.Serializable]
public class EnemySpawnWaveContainer
{
	[SerializeField] EnemySpawner enemySpawner;
	[SerializeField] int spawnCount;
	[SerializeField] float spawnWait;

	public void TriggerEnemyWave()
	{
		enemySpawner.SpawnEnemies(spawnCount, spawnWait);
	}
}