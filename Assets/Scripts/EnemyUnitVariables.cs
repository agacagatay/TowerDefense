using UnityEngine;
using System.Collections;

public class EnemyUnitVariables : MonoBehaviour
{
	[SerializeField] string enemyUnitType;
	[SerializeField] int unitHealth = 100;

	void Start()
	{
		SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Add(gameObject, enemyUnitType);
	}

	public void DamageUnit(int damageValue)
	{
		unitHealth -= damageValue;

		if (unitHealth <= 0)
		{
			ResourcesController.instance.Shards += 100;
			ResourcesController.instance.UpdateShards();
			Destroy(gameObject);
		}
	}
}
