using UnityEngine;
using System.Collections;

public class EnemySpawnerController : MonoBehaviour
{
	[SerializeField] EnemySpawnWave[] enemySpawnerWaves;
	int enemyWaveNumber = 0;
	public int EnemyWaveNumber { get { return enemyWaveNumber; }}
	public int TotalEnemyWaves { get { return enemySpawnerWaves.Length; }}

	public static EnemySpawnerController instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		StartCoroutine(SpawnFirstWave());
	}

	IEnumerator SpawnFirstWave()
	{
		HUDController.instance.DisplayTwoString("Level Begin", "Enemies Spawn In " + enemySpawnerWaves[enemyWaveNumber].WaveInitialWait.ToString("N0") + 
			" Seconds", 4f, 2f);
		HUDController.instance.ToggleEnemySpawnTimer(enemySpawnerWaves[enemyWaveNumber].WaveInitialWait);

		yield return new WaitForSeconds(enemySpawnerWaves[enemyWaveNumber].WaveInitialWait);

		HUDController.instance.DisplayTwoString("Enemy Wave Incoming", "Wave " + (1 + enemyWaveNumber).ToString("N0") + 
			" of " + enemySpawnerWaves.Length.ToString("N0"), 2f, 2f);

		HUDController.instance.SetEnemyWaveLabel("Enemy Wave " + (EnemyWaveNumber + 1).ToString("N0") + " of " +
			TotalEnemyWaves.ToString("N0"));

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
		StartCoroutine(SpawnEnemyWave());
	}

	IEnumerator SpawnEnemyWave()
	{
		HUDController.instance.SetEnemyWaveLabel(" ");
		++enemyWaveNumber;

		if (enemyWaveNumber < enemySpawnerWaves.Length)
		{
			HUDController.instance.DisplayTwoString("Enemy Wave Defeated", "Next Wave In " + enemySpawnerWaves[enemyWaveNumber].WaveInitialWait.ToString("N0") + 
				" Seconds", 2f, 2f);
			HUDController.instance.ToggleEnemySpawnTimer(enemySpawnerWaves[enemyWaveNumber].WaveInitialWait);
				
			yield return new WaitForSeconds(enemySpawnerWaves[enemyWaveNumber].WaveInitialWait);

			HUDController.instance.DisplayTwoString("Enemy Wave Incoming", "Wave " + (1 + enemyWaveNumber).ToString("N0") + 
				" of " + enemySpawnerWaves.Length.ToString("N0"), 2f, 2f);

			HUDController.instance.SetEnemyWaveLabel("Enemy Wave " + (EnemyWaveNumber + 1).ToString("N0") + " of " +
				TotalEnemyWaves.ToString("N0"));

			enemySpawnerWaves[enemyWaveNumber].TriggerEnemyWave(enemyWaveNumber);
		}
		else
			GameController.instance.GameWin();
	}
}

[System.Serializable]
public class EnemySpawnWave
{
	[SerializeField] string waveName;
	[SerializeField] float waveInitialWait;
	[SerializeField] EnemySpawnWaveContainer[] enemySpawnWaveContainers;
	public float WaveInitialWait { get { return waveInitialWait; }}

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