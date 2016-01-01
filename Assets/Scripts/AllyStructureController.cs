using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyStructureController : MonoBehaviour
{
	[SerializeField] int priorityValue;
	[SerializeField] int structureHealth = 100;
	[SerializeField] bool isTurret = false;
	[SerializeField] bool isBarrier = false;
	GameObject turretSpawnObject;
	List<GameObject> stoppedEnemyUnits = new List<GameObject>();
	public GameObject TurretSpawnObject { get { return turretSpawnObject; } set { turretSpawnObject = value; }}

	void Start()
	{
		SpawnedAllyDictionary.instance.spawnedAllyDictionary.Add(gameObject, priorityValue);
	}

	public void DamageStructure(int damageValue)
	{
		structureHealth -= damageValue;

		if (structureHealth <= 0)
		{
			if (isTurret)
				TurretSpawnObject.SetActive(true);

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

	public void BarrierPerimeterTrigger(GameObject enemyUnitObject)
	{
		stoppedEnemyUnits.Add(enemyUnitObject);
	}
}
