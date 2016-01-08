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

	public bool SpawnWaveComplete()
	{
		return enemySpawnerWaves[enemyWaveNumber].SpawnWaveComplete();
	}

	public void SpawnNextEnemyWave()
	{
		++enemyWaveNumber;

		if (enemyWaveNumber == enemySpawnerWaves.Length)
			GameController.instance.GameWin();
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

	public bool SpawnWaveComplete()
	{
		bool isComplete = true;

		foreach (EnemySpawnWaveContainer enemySpawnWaveContainer in enemySpawnWaveContainers)
		{
			if (!enemySpawnWaveContainer.EnemySpawnerScript.SpawnComplete)
				isComplete = false;
		}

		return isComplete;
	}
}

[System.Serializable]
public class EnemySpawnWaveContainer
{
	[SerializeField] EnemySpawner enemySpawner;
	[SerializeField] int spawnCount;
	[SerializeField] float spawnWait;
	public EnemySpawner EnemySpawnerScript { get { return enemySpawner; }}

	public void TriggerEnemyWave()
	{
		enemySpawner.SpawnEnemies(spawnCount, spawnWait);
	}
}