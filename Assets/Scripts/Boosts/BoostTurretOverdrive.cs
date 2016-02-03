using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoostTurretOverdrive : MonoBehaviour
{
	[SerializeField] float activeTime;

	void Start()
	{
		StartCoroutine(TriggerOverdrive());
	}

	IEnumerator TriggerOverdrive()
	{
		HUDController.instance.ToggleTurretTimer(activeTime);
		BoostController.instance.TurretOverdriveEnabled = true;

		foreach(KeyValuePair<GameObject, int> allyKeyValuePair in SpawnedAllyDictionary.instance.spawnedAllyDictionary)
		{
			AllyTurretController allyTurretController = allyKeyValuePair.Key.GetComponent<AllyTurretController>();

			if (allyTurretController != null)
				allyTurretController.OverdriveActive = true;
		}

		yield return new WaitForSeconds(activeTime);
		BoostController.instance.TurretOverdriveEnabled = false;

		foreach(KeyValuePair<GameObject, int> allyKeyValuePair in SpawnedAllyDictionary.instance.spawnedAllyDictionary)
		{
			AllyTurretController allyTurretController = allyKeyValuePair.Key.GetComponent<AllyTurretController>();

			if (allyTurretController != null)
				allyTurretController.OverdriveActive = false;
		}

		Destroy(gameObject);
	}
}
