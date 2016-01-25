using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoostStructureRepair : MonoBehaviour
{
	[SerializeField] int repairValue;

	void Start()
	{
		foreach(KeyValuePair<GameObject, int> allyKeyValuePair in SpawnedAllyDictionary.instance.spawnedAllyDictionary)
		{
			AllyStructureController allyStructureController = allyKeyValuePair.Key.GetComponent<AllyStructureController>();

			if (allyStructureController != null)
			{
				allyStructureController.RepairStructure(repairValue);
				HealthBarController.instance.UpdateHealthBar(allyStructureController);
			}
		}

		HUDController.instance.UpdateBaseDisplay();
		Destroy(gameObject);
	}
}
