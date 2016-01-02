using UnityEngine;
using System.Collections;

public class AllySpawnerPosition : MonoBehaviour
{
	[SerializeField] GameObject turretPrefab;
	[SerializeField] int turretCost;
	[SerializeField] Transform spawnTransform;
	[SerializeField] string[] spawnOptions;
	public string[] SpawnOptions { get { return spawnOptions; }}

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

			DisableSpawnerPosition();
		}
	}

	public void EnableSpawnerPosition()
	{
		Collider spawnerPositionCollider = gameObject.GetComponent<Collider>();
		spawnerPositionCollider.enabled = true;

		Renderer spawnerPositionRenderer = gameObject.GetComponent<Renderer>();
		spawnerPositionRenderer.enabled = true;
	}

	public void DisableSpawnerPosition()
	{
		Collider spawnerPositionCollider = gameObject.GetComponent<Collider>();
		spawnerPositionCollider.enabled = false;

		Renderer spawnerPositionRenderer = gameObject.GetComponent<Renderer>();
		spawnerPositionRenderer.enabled = false;
	}
}
