using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoostTurretOverdrive : MonoBehaviour
{
	[SerializeField] GameObject timerPrefab;
	[SerializeField] float activeTime;

	void Start()
	{
		StartCoroutine(TriggerOverdrive());
	}

	IEnumerator TriggerOverdrive()
	{
		GameObject timerPrefabClone = GameObject.FindGameObjectWithTag("TurretOverdriveTimer");
		UISprite timerPrefabSprite = timerPrefabClone.GetComponentInChildren<UISprite>();
		timerPrefabSprite.fillAmount = 1f;
		float initialTime = activeTime;

		UILabel timerLabel = timerPrefabClone.GetComponent<UILabel>();
		timerLabel.alpha = 1f;

		foreach(KeyValuePair<GameObject, int> allyKeyValuePair in SpawnedAllyDictionary.instance.spawnedAllyDictionary)
		{
			AllyTurretController allyTurretController = allyKeyValuePair.Key.GetComponent<AllyTurretController>();

			if (allyTurretController != null)
				allyTurretController.OverdriveActive = true;
		}

		while (activeTime > 0f)
		{
			activeTime -= Time.deltaTime;
			timerPrefabSprite.fillAmount = activeTime / initialTime;
			yield return null;
		}

		foreach(KeyValuePair<GameObject, int> allyKeyValuePair in SpawnedAllyDictionary.instance.spawnedAllyDictionary)
		{
			AllyTurretController allyTurretController = allyKeyValuePair.Key.GetComponent<AllyTurretController>();

			if (allyTurretController != null)
				allyTurretController.OverdriveActive = false;
		}

		timerLabel.alpha = 0f;
		Destroy(gameObject);
	}
}
