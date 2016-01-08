using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoostPrecisionStrike : MonoBehaviour
{
	[SerializeField] GameObject ordinancePrefab;
	[SerializeField] float initialWait;
	[SerializeField] float minShootWait;
	[SerializeField] float maxShootWait;
	[SerializeField] float spawnRandomRadius;
	[SerializeField] int totalOrdinancePackage;
	int ordinanceNumber = 1;
	List<GameObject> enemyUnits = new List<GameObject>();

	void Start()
	{
		StartCoroutine(FireOrdinance());
	}

	IEnumerator FireOrdinance()
	{
		if (ordinanceNumber == 0)
			yield return new WaitForSeconds(initialWait);
		else if (ordinanceNumber >= totalOrdinancePackage)
			Destroy(gameObject);

		enemyUnits.Clear();

		foreach(KeyValuePair<GameObject, int> enemyKeyValuePair in SpawnedEnemyDictionary.instance.spawnedEnemyDictionary)
		{
			enemyUnits.Add(enemyKeyValuePair.Key);
		}

		if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Count > 0)
		{
			float randomX = transform.position.x + Random.Range(-spawnRandomRadius, spawnRandomRadius);
			float randomZ = transform.position.z + Random.Range(-spawnRandomRadius, spawnRandomRadius);
			Vector3 randomSpawnPosition = new Vector3(randomX, transform.position.y, randomZ);

			int randomInt = Random.Range(0, enemyUnits.Count);

			GameObject ordinanceClone = (GameObject)Instantiate(ordinancePrefab, randomSpawnPosition, transform.rotation);
			OrdinanceController ordinanceController = ordinanceClone.GetComponent<OrdinanceController>();
			ordinanceController.TargetTransform = enemyUnits[randomInt].transform;
			ordinanceNumber++;

			float randomShootWait = Random.Range(minShootWait, maxShootWait);
			yield return new WaitForSeconds(randomShootWait);

			StartCoroutine(FireOrdinance());
		}
		else
			Destroy(gameObject);
	}
}
