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
	int attemptNumber = 0;
	List<GameObject> enemyUnits = new List<GameObject>();

	void Start()
	{
		foreach(KeyValuePair<GameObject, int> enemyKeyValuePair in SpawnedEnemyDictionary.instance.spawnedEnemyDictionary)
		{
			enemyUnits.Add(enemyKeyValuePair.Key);
		}

		StartCoroutine(FireOrdinance());
	}

	IEnumerator FireOrdinance()
	{
		if (ordinanceNumber == 0 && attemptNumber == 0)
			yield return new WaitForSeconds(initialWait);

		if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Count > 0)
		{
			int randomInt = Random.Range(0, enemyUnits.Count);

			if (enemyUnits[randomInt] != null && ordinanceNumber <= totalOrdinancePackage)
			{
				attemptNumber = 0;

				float randomX = transform.position.x + Random.Range(-spawnRandomRadius, spawnRandomRadius);
				float randomZ = transform.position.z + Random.Range(-spawnRandomRadius, spawnRandomRadius);
				Vector3 randomSpawnPosition = new Vector3(randomX, transform.position.y, randomZ);

				GameObject ordinanceClone = (GameObject)Instantiate(ordinancePrefab, randomSpawnPosition, transform.rotation);
				OrdinanceController ordinanceController = ordinanceClone.GetComponent<OrdinanceController>();
				ordinanceController.TargetTransform = enemyUnits[randomInt].transform;
				ordinanceNumber++;

				float randomShootWait = Random.Range(minShootWait, maxShootWait);
				yield return new WaitForSeconds(randomShootWait);
				StartCoroutine(FireOrdinance());
			}
			else if (enemyUnits[randomInt] == null || (ordinanceNumber <= totalOrdinancePackage && attemptNumber < 1))
			{
				enemyUnits.Remove(enemyUnits[randomInt]);
				attemptNumber++;
				StartCoroutine(FireOrdinance());
			}
			else
				Destroy(gameObject);
		}
		else
			Destroy(gameObject);
	}
}
