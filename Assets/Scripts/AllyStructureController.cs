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
	GameObject turretSelectObject;
	AllyDestroyTurret destroyTurretTab;
	UIWidget turretSelectWidget;
	List<GameObject> stoppedEnemyUnits = new List<GameObject>();
	public string TurretType { get { return turretType; }}
	public GameObject TurretSpawnObject { get { return turretSpawnObject; } set { turretSpawnObject = value; }}

	void OnEnable()
	{
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable()
	{
		UnsubscribeEvent();
	}

	void OnDestroy()
	{
		UnsubscribeEvent();
	}

	void UnsubscribeEvent()
	{
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}

	private void On_SimpleTap(Gesture gesture)
	{
		if (isTurret)
		{
			SetSelectTurretAlpha(0f);

			if (gesture.pickedObject == gameObject)
			{
				turretSelectWidget.SetAnchor(transform);
				SetSelectTurretAlpha(1f);
			}
		}
	}

	void Start()
	{
		SpawnedAllyDictionary.instance.spawnedAllyDictionary.Add(gameObject, priorityValue);
		structureHealth = initialStructureHealth;

		if (isTurret)
		{
			turretSelectObject = GameObject.FindGameObjectWithTag("SelectTurret");

			destroyTurretTab = turretSelectObject.GetComponentInChildren<AllyDestroyTurret>();
			destroyTurretTab.StructureController = this;

			turretSelectWidget = turretSelectObject.GetComponent<UIWidget>();
			SetSelectTurretAlpha(0f);
		}
	}

	public void SetSelectTurretAlpha(float alphaValue)
	{
		turretSelectWidget.alpha = alphaValue;
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
