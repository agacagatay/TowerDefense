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
	[SerializeField] bool isBarrier = false;
	int structureHealth;
	GameObject turretSpawnObject;
	List<GameObject> stoppedEnemyUnits = new List<GameObject>();
	public bool IsTurret { get { return isTurret; }}
	public string TurretType { get { return turretType; }}
	public GameObject TurretSpawnObject { get { return turretSpawnObject; } set { turretSpawnObject = value; }}

	void Start()
	{
		SpawnedAllyDictionary.instance.spawnedAllyDictionary.Add(gameObject, priorityValue);
		structureHealth = initialStructureHealth;
	}

	public void DamageStructure(int damageValue)
	{
		structureHealth -= damageValue;

		if (structureHealth <= 0)
		{
			if (isTurret)
			{
				if (turretType == "Turret")
					++ResourcesController.instance.TurretQuota;
				else if (turretType == "Missile Battery")
					++ResourcesController.instance.MissileBatteryQuota;
				else
					Debug.LogError("Invalid turret type specified");

				ResourcesController.instance.UpdateResources();
				AllySpawnerPosition allySpawnerPosition = turretSpawnObject.GetComponent<AllySpawnerPosition>();
				allySpawnerPosition.EnableSpawnerPosition();
			}

			if (isBarrier)
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

			Destroy(gameObject);
		}
	}

	public void FortifyStructure(int fortifyValue)
	{
		structureHealth += fortifyValue;
	}

	public void RepairStructure()
	{
		structureHealth = initialStructureHealth;
	}

	public void BarrierPerimeterTrigger(GameObject enemyUnitObject)
	{
		stoppedEnemyUnits.Add(enemyUnitObject);
	}
}
