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
		{
			if (!GameController.instance.GameOver)
				AudioController.instance.PlayOneshot("SFX/Enemy_Teleport", gameObject);

			Instantiate(spawnEffect, transform.position, transform.rotation);
		}

		switch (UnitName)
		{
		case "Enemy Light Tank":
			AudioController.instance.CreateInstance("SFX/Enemy_Unit_Light_Tank", gameObject);
			AudioController.instance.Play("SFX/Enemy_Unit_Light_Tank", gameObject);
			break;
		case "Enemy Heavy Tank":
			AudioController.instance.CreateInstance("SFX/Enemy_Unit_Heavy_Tank", gameObject);
			AudioController.instance.Play("SFX/Enemy_Unit_Heavy_Tank", gameObject);
			break;
		case "Enemy Fighter":
			AudioController.instance.CreateInstance("SFX/Enemy_Unit_Fighter", gameObject);
			AudioController.instance.Play("SFX/Enemy_Unit_Fighter", gameObject);
			break;
		case "Enemy Dropship":
			AudioController.instance.CreateInstance("SFX/Enemy_Unit_Dropship", gameObject);
			AudioController.instance.Play("SFX/Enemy_Unit_Dropship", gameObject);
			break;
		}

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
			{
				EnemySpawnerController.instance.SpawnNextEnemyWave();
			
				if (TutorialController.instance.TutorialActive && TutorialController.instance.CurrentTutorialScreen == 19)
					TutorialController.instance.NextTutorialScreen();
			}

			if (deathExplosion != null)
			{
				AudioController.instance.PlayOneshot("SFX/Explosion_Normal", gameObject);
				deathExplosionObject = (GameObject)Instantiate(deathExplosion, transform.position, transform.rotation);
			}

			StartCoroutine(WaitAndDestroy(0.5f));
		}
	}

	IEnumerator WaitAndDestroy(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		HealthBarController.instance.DisableEnemyHealthBar(gameObject);

		switch (UnitName)
		{
		case "Enemy Light Tank":
			AudioController.instance.Stop("SFX/Enemy_Unit_Light_Tank", gameObject);
			break;
		case "Enemy Heavy Tank":
			AudioController.instance.Stop("SFX/Enemy_Unit_Heavy_Tank", gameObject);
			break;
		case "Enemy Fighter":
			AudioController.instance.Stop("SFX/Enemy_Unit_Fighter", gameObject);
			break;
		case "Enemy Dropship":
			AudioController.instance.Stop("SFX/Enemy_Unit_Dropship", gameObject);
			break;
		}

		Destroy(gameObject);
	}
}
