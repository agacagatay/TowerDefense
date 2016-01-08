﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] float spawnLoopWait;
	int spawnCount = 1;
	bool spawnComplete = false;
	public bool SpawnComplete { get { return spawnComplete; }}

	public void SpawnEnemies(int spawnEnemyCount, float spawnInitialWait)
	{
		StartCoroutine(SpawnAndWait(spawnEnemyCount, spawnInitialWait));
	}

	IEnumerator SpawnAndWait(int spawnEnemyCount, float spawnInitialWait)
	{
		yield return new WaitForSeconds(spawnInitialWait);

		for (int i = 1; i <= spawnEnemyCount; i++)
		{
			GameObject clone = (GameObject)Instantiate(enemyPrefab, transform.position, transform.rotation);
			clone.name = gameObject.name + spawnCount;
			spawnCount++;

			yield return new WaitForSeconds(spawnLoopWait);
		}

		spawnComplete = true;
	}
}