using UnityEngine;
using System.Collections;

public class EnemyUnitVariables : MonoBehaviour
{
	[SerializeField] string enemyUnitType;
	int unitHealth = 100;

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

	public void DamageUnit(int damageValue)
	{
		unitHealth -= damageValue;

		if (unitHealth <= 0)
			Destroy(gameObject);
	}
}
