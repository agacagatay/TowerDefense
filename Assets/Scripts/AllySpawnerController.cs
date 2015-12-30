using UnityEngine;
using System.Collections;

public class AllySpawnerController : MonoBehaviour
{
	[SerializeField] GameObject turretPrefab;
	[SerializeField] int turretCost;
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
			AllyStructureController allyStructureVariables = turretClone.GetComponent<AllyStructureController>();
			allyStructureVariables.TurretSpawnObject = gameObject;

			ResourcesController.instance.Shards -= turretCost;
			ResourcesController.instance.UpdateShards();

			gameObject.SetActive(false);
		}
	}
}
