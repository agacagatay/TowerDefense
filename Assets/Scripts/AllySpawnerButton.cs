using UnityEngine;
using System.Collections;

public class AllySpawnerButton : MonoBehaviour
{
	void OnClick()
	{
		switch(gameObject.tag)
		{
		case "TabTurret":
			if (ResourcesController.instance.TurretQuota > 0)
			{
				--ResourcesController.instance.TurretQuota;
				AllySpawnerController.instance.SpawnTurret("Turret");
			}
			else
				Debug.Log("Turret Limit Reached");
			
			break;
		case "TabMissileBattery":
			if (ResourcesController.instance.MissileBatteryQuota > 0)
			{
				--ResourcesController.instance.MissileBatteryQuota;
				AllySpawnerController.instance.SpawnTurret("Missile Battery");
			}
			else
				Debug.Log("Missile Battery Limit Reached");
			
			break;
		}

		ResourcesController.instance.UpdateResources();
	}
}
