﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyStructureController : MonoBehaviour
{
	[SerializeField] int priorityValue;
	[SerializeField] int initialStructureHealth = 100;
	[SerializeField] bool isTurret = false;
	[SerializeField] string turretType;
	//[SerializeField] GameObject turretSelectTab;
	[SerializeField] bool hasPerimeter = false;
	[SerializeField] bool isPrimaryStructure = false;
	[SerializeField] bool isSecondaryStructure = false;
	[SerializeField] string structureName;
	[SerializeField] GameObject deathExplosion;
	int structureHealth;
	bool structureDead = false;
	GameObject turretSpawnObject;
	public int InitialStructureHealth { get { return initialStructureHealth; }}
	public int StructureHealth { get { return structureHealth; }}
	public bool IsTurret { get { return isTurret; }}
	public string TurretType { get { return turretType; }}
	public GameObject TurretSpawnObject { get { return turretSpawnObject; } set { turretSpawnObject = value; }}
	public bool IsPrimaryStructure { get { return isPrimaryStructure; }}
	public bool IsSecondaryStructure { get { return isSecondaryStructure; }}
	public string StructureName { get { return structureName; }}

	void Start()
	{
		SpawnedAllyDictionary.instance.spawnedAllyDictionary.Add(gameObject, priorityValue);
		structureHealth = initialStructureHealth;

		if (IsPrimaryStructure)
			GameController.instance.PrimaryStructure = gameObject;
		else if (IsSecondaryStructure)
			GameController.instance.SecondaryStructures.Add(gameObject);
		else if (IsTurret)
			GameController.instance.TurretsSpawned++;
		else if (!IsPrimaryStructure)
			GameController.instance.BarrierStructures.Add(gameObject);
	}

	public void DamageStructure(int damageValue)
	{
		if (!structureDead)
		{
			if (isPrimaryStructure && GameController.instance.SecondaryStructures.Count > 0f)
				return;
			
			structureHealth -= damageValue;
			HealthBarController.instance.UpdateHealthBar(this);

			if (structureHealth <= 0)
			{
				structureDead = true;

				if (isTurret)
				{
					if (turretType == "Artillary")
						++AllySpawnerController.instance.ArtillaryQuota;
					else if (turretType == "Minigun")
						++AllySpawnerController.instance.MinigunQuota;
					else if (turretType == "Turret")
						++AllySpawnerController.instance.TurretQuota;
					else if (turretType == "Missile Battery")
						++AllySpawnerController.instance.MissileBatteryQuota;
					else
						Debug.LogError("Invalid turret type specified");

					HUDController.instance.UpdateResources();
				}

				if (hasPerimeter)
				{
					foreach (KeyValuePair<GameObject, int> enemyUnit in SpawnedEnemyDictionary.instance.spawnedEnemyDictionary)
					{
						EnemyNavController enemyNavController = enemyUnit.Key.GetComponent<EnemyNavController>();

						if (enemyNavController != null)
							enemyNavController.EnableMovement();
					}
				}

				if (isPrimaryStructure)
					GameController.instance.GameLose();
				else if (isSecondaryStructure)
				{
					GameController.instance.SecondaryStructures.Remove(gameObject);
				}
				else if (IsTurret)
				{
					if (AllySpawnerController.instance.StructureController == this)
						AllySpawnerController.instance.HideTurretSelectTab();
						
					TargetAreaSphere.instance.DestroyAreaSphere(gameObject);
				}
				else if (!IsTurret)
					GameController.instance.BarrierStructures.Remove(gameObject);

				if (isPrimaryStructure || isSecondaryStructure)
					HUDController.instance.UpdateBaseDisplay();

				if (HealthBarController.instance.StructureController == this)
					HealthBarController.instance.DisableAllHealthBars();

				SpawnedAllyDictionary.instance.spawnedAllyDictionary.Remove(gameObject);

				if (deathExplosion != null)
					Instantiate(deathExplosion, transform.position, transform.rotation);

				StartCoroutine(WaitAndDestroy(0.5f));
			}
			else if (isPrimaryStructure || isSecondaryStructure)
				HUDController.instance.UpdateBaseDisplay();
		}
	}

	IEnumerator WaitAndDestroy(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		if (TurretSpawnObject != null)
		{
			AllySpawnerPosition allySpawnerPosition = turretSpawnObject.GetComponent<AllySpawnerPosition>();
			allySpawnerPosition.EnableSpawnerPosition();
		}

		Destroy(gameObject);
	}

	public void RepairStructure(int repairValue)
	{
		structureHealth += repairValue;

		if (structureHealth > initialStructureHealth)
			structureHealth = initialStructureHealth;
	}
}
