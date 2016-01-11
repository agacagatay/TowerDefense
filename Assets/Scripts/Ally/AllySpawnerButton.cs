using UnityEngine;
using System.Collections;

public class AllySpawnerButton : MonoBehaviour
{
	void OnClick()
	{
		switch(gameObject.tag)
		{
		case "TabTurret":
			if (HUDController.instance.TurretQuota > 0)
			{
				--HUDController.instance.TurretQuota;
				AllySpawnerController.instance.SpawnTurret("Turret");
			}
			else
				Debug.Log("Turret Limit Reached");
			
			break;
		case "TabMissileBattery":
			if (HUDController.instance.MissileBatteryQuota > 0)
			{
				--HUDController.instance.MissileBatteryQuota;
				AllySpawnerController.instance.SpawnTurret("Missile Battery");
			}
			else
				Debug.Log("Missile Battery Limit Reached");
			
			break;
		}

		HUDController.instance.UpdateResources();
	}
}
