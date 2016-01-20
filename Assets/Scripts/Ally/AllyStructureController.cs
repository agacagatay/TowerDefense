using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyStructureController : MonoBehaviour
{
	[SerializeField] int priorityValue;
	[SerializeField] int initialStructureHealth = 100;
	[SerializeField] bool isTurret = false;
	[SerializeField] string turretType;
	[SerializeField] GameObject turretSelectTab;
	[SerializeField] bool hasPerimeter = false;
	[SerializeField] bool isPrimaryStructure = false;
	[SerializeField] bool isSecondaryStructure = false;
	[SerializeField] string structureName;
	int structureHealth;
	GameObject turretSpawnObject;
	List<GameObject> stoppedEnemyUnits = new List<GameObject>();
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

		if (IsSecondaryStructure)
			GameController.instance.secondaryStructures.Add(gameObject);
		else if (IsTurret)
			GameController.instance.TurretsSpawned++;
		else if (!IsPrimaryStructure)
			GameController.instance.barrierStructures.Add(gameObject);
	}

	public void DamageStructure(int damageValue)
	{
		structureHealth -= damageValue;
		HealthBarController.instance.UpdateHealthBar(this);

		if (structureHealth <= 0)
		{
			if (isTurret)
			{
				if (turretType == "Artillary")
					++HUDController.instance.ArtillaryQuota;
				else if (turretType == "Minigun")
					++HUDController.instance.MinigunQuota;
				else if (turretType == "Turret")
					++HUDController.instance.TurretQuota;
				else if (turretType == "Missile Battery")
					++HUDController.instance.MissileBatteryQuota;
				else
					Debug.LogError("Invalid turret type specified");

				HUDController.instance.UpdateResources();
				AllySpawnerPosition allySpawnerPosition = turretSpawnObject.GetComponent<AllySpawnerPosition>();
				allySpawnerPosition.EnableSpawnerPosition();
			}

			if (hasPerimeter)
			{
				foreach (GameObject stoppedEnemyUnit in stoppedEnemyUnits)
				{
					if (stoppedEnemyUnit != null)
					{
						EnemyNavController enemyNavController = stoppedEnemyUnit.GetComponent<EnemyNavController>();
						enemyNavController.EnableMovement();
					}
				}

				stoppedEnemyUnits.Clear();
			}

			if (isPrimaryStructure)
				GameController.instance.GameLose();
			else if (isSecondaryStructure)
			{
				WaypointController.instance.SecondaryStructures.Remove(transform);
				GameController.instance.secondaryStructures.Remove(gameObject);
			}
			else if (IsTurret)
			{
				if (AllySpawnerController.instance.StructureController == this)
					AllySpawnerController.instance.HideTurretSelectTab();
					
				TargetAreaSphere.instance.DestroyAreaSphere(gameObject);
			}
			else if (!IsTurret)
				GameController.instance.barrierStructures.Remove(gameObject);

			if (isPrimaryStructure || isSecondaryStructure)
				HUDController.instance.UpdateBaseDisplay();

			if (HealthBarController.instance.StructureController == this)
				HealthBarController.instance.DisableAllHealthBars();

			SpawnedAllyDictionary.instance.spawnedAllyDictionary.Remove(gameObject);
			Destroy(gameObject);
		}
		else if (isPrimaryStructure || isSecondaryStructure)
			HUDController.instance.UpdateBaseDisplay();
	}

	public void RepairStructure(int repairValue)
	{
		structureHealth += repairValue;

		if (structureHealth > initialStructureHealth)
			structureHealth = initialStructureHealth;
	}

	public void BarrierPerimeterTrigger(GameObject enemyUnitObject)
	{
		stoppedEnemyUnits.Add(enemyUnitObject);
	}
}
