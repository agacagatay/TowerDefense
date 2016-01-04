using UnityEngine;
using System.Collections;

public class EnemyUnitVariables : MonoBehaviour
{
	[SerializeField] int priorityValue = 0;
	[SerializeField] int unitHealth = 100;

	void Start()
	{
		SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Add(gameObject, priorityValue);
	}

	public void DamageUnit(int damageValue)
	{
		unitHealth -= damageValue;

		if (unitHealth <= 0)
			Destroy(gameObject);
	}
}
