using UnityEngine;
using System.Collections;

public class TargetAreaSphere : MonoBehaviour
{
	[SerializeField] GameObject targetSpherePrefab;
	GameObject selectedTurret;
	GameObject targetSphere;
	float sphereScale;

	public static TargetAreaSphere instance;

	void Awake()
	{
		instance = this;
	}

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

	void On_SimpleTap(Gesture gesture)
	{
		if (targetSphere != null)
			Destroy(targetSphere);

		if (gesture.pickedObject.tag == "Ally")
		{
			AllyTurretController allyTurretController = gesture.pickedObject.GetComponent<AllyTurretController>();

			if (allyTurretController != null)
			{
				selectedTurret = gesture.pickedObject;
				sphereScale = 0f;

				if (allyTurretController.GroundRange > 0f)
					sphereScale = allyTurretController.GroundRange;
				else if (allyTurretController.AirRange > 0f)
					sphereScale = allyTurretController.AirRange;

				if (sphereScale > 0f)
				{
					targetSphere = (GameObject)Instantiate(targetSpherePrefab, allyTurretController.transform.position, targetSpherePrefab.transform.rotation);
					targetSphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
				}
			}
		}
	}

	public void DestroyAreaSphere(GameObject turretObject)
	{
		if (targetSphere != null && turretObject == selectedTurret)
			Destroy(targetSphere);
	}
}
