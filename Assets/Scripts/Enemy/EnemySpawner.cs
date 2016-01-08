using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] float spawnLoopWait;
	[SerializeField] float spawnRandomRadius;
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
			float randomX = transform.position.x + Random.Range(-spawnRandomRadius, spawnRandomRadius);
			float randomZ = transform.position.z + Random.Range(-spawnRandomRadius, spawnRandomRadius);
			Vector3 randomSpawnPosition = new Vector3(randomX, transform.position.y, randomZ);

			GameObject clone = (GameObject)Instantiate(enemyPrefab, randomSpawnPosition, transform.rotation);
			clone.name = gameObject.name + spawnCount;
			spawnCount++;

			yield return new WaitForSeconds(spawnLoopWait);
		}

		spawnComplete = true;
	}
}