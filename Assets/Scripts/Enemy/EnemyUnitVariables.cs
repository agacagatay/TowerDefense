using UnityEngine;
using System.Collections;

public class EnemyUnitVariables : MonoBehaviour
{
	[SerializeField] GameObject spawnEffect;
	[SerializeField] int priorityValue = 0;
	[SerializeField] int unitHealth = 100;
	[SerializeField] string unitName;
	[SerializeField] GameObject deathExplosion;
	int initialHealth;
	GameObject deathExplosionObject;
	public int UnitHealth { get { return unitHealth; }}
	public int InitialHealth { get { return initialHealth; }}
	public string UnitName { get { return unitName; }}

	void Start()
	{
		if (spawnEffect != null)
			Instantiate(spawnEffect, transform.position, transform.rotation);

		SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Add(gameObject, priorityValue);
		initialHealth = UnitHealth;
	}

	void Update()
	{
		if (deathExplosionObject != null)
			deathExplosionObject.transform.position = transform.position;
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

			if (deathExplosion != null)
				deathExplosionObject = (GameObject)Instantiate(deathExplosion, transform.position, transform.rotation);

			StartCoroutine(WaitAndDestroy(0.5f));
		}
	}

	IEnumerator WaitAndDestroy(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		HealthBarController.instance.DisableEnemyHealthBar(gameObject);
		Destroy(gameObject);
	}
}
