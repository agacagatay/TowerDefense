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
			Instantiate(turretPrefab, spawnTransform.position, spawnTransform.rotation);
			gameObject.SetActive(false);
		}
	}
}
