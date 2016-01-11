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
		HUDController.instance.DisplayTwoString("Enemy Wave Incoming", "Wave " + (1 + enemyWaveNumber).ToString("N0") + 
			" of " + enemySpawnerWaves.Length.ToString("N0"), 2f, 2f);

		enemySpawnerWaves[enemyWaveNumber].TriggerEnemyWave(enemyWaveNumber);
	}

	public bool SpawnWaveComplete()
	{
		if (enemyWaveNumber < enemySpawnerWaves.Length)
			return enemySpawnerWaves[enemyWaveNumber].SpawnWaveComplete();
		else
			return false;
	}

	public void SpawnNextEnemyWave()
	{
		++enemyWaveNumber;

		if (enemyWaveNumber < enemySpawnerWaves.Length)
		{
			HUDController.instance.DisplayTwoString("Enemy Wave Incoming", "Wave " + (1 + enemyWaveNumber).ToString("N0") + 
				" of " + enemySpawnerWaves.Length.ToString("N0"), 2f, 2f);
		}

		if (enemyWaveNumber == enemySpawnerWaves.Length)
			GameController.instance.GameWin();
		else
			enemySpawnerWaves[enemyWaveNumber].TriggerEnemyWave(enemyWaveNumber);
	}
}

[System.Serializable]
public class EnemySpawnWave
{
	[SerializeField] string waveName;
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
	[SerializeField] string spawnerName;
	[SerializeField] EnemySpawner enemySpawner;
	[SerializeField] int spawnCount;
	[SerializeField] float spawnInitialWait;
	public EnemySpawner EnemySpawnerScript { get { return enemySpawner; }}

	public void TriggerEnemyWave()
	{
		enemySpawner.SpawnEnemies(spawnCount, spawnInitialWait);
	}
}