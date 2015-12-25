using UnityEngine;
using System.Collections;

public class TurretSpawnController : MonoBehaviour
{
	[SerializeField] GameObject turretPrefab;
	[SerializeField] Transform spawnTransform;

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
		if (gesture.pickedObject == gameObject)
		{
			GameObject turretClone =  (GameObject)Instantiate(turretPrefab, spawnTransform.position, spawnTransform.rotation);
			AllyStructureVariables allyStructureVariables = turretClone.GetComponent<AllyStructureVariables>();
			allyStructureVariables.TurretSpawnObject = gameObject;
			gameObject.SetActive(false);
		}
	}
}
