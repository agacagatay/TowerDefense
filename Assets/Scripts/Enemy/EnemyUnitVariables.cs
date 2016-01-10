using UnityEngine;
using System.Collections;

public class EnemyUnitVariables : MonoBehaviour
{
	[SerializeField] int priorityValue = 0;
	[SerializeField] int unitHealth = 100;
	[SerializeField] string unitName;
	int initialHealth;
	public int UnitHealth { get { return unitHealth; }}
	public int InitialHealth { get { return initialHealth; }}
	public string UnitName { get { return unitName; }}

	void Start()
	{
		SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Add(gameObject, priorityValue);
		initialHealth = UnitHealth;
	}

	public void DamageUnit(int damageValue)
	{
		unitHealth -= damageValue;
		HealthBarController.instance.UpdateEnemyHealthBar(this);

		if (unitHealth <= 0)
		{
			GameController.instance.EnemiesKilled++;
			SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Remove(gameObject);

			if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Count == 0 && EnemySpawnerController.instance.SpawnWaveComplete())
				EnemySpawnerController.instance.SpawnNextEnemyWave();

			HealthBarController.instance.DisableEnemyHealthBar(gameObject);
			Destroy(gameObject);
		}
	}
}
