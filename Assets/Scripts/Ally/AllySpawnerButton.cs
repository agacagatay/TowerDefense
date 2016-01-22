using UnityEngine;
using System.Collections;

public class AllySpawnerButton : MonoBehaviour
{
	void OnClick()
	{
		switch(gameObject.tag)
		{
		case "TabArtillary":
			if (AllySpawnerController.instance.ArtillaryQuota > 0)
			{
				--AllySpawnerController.instance.ArtillaryQuota;
				AllySpawnerController.instance.SpawnTurret("Artillary");
			}
			else
				Debug.Log("Artillary Limit Reached");

			break;
		case "TabMinigun":
			if (AllySpawnerController.instance.MinigunQuota > 0)
			{
				--AllySpawnerController.instance.MinigunQuota;
				AllySpawnerController.instance.SpawnTurret("Minigun");
			}
			else
				Debug.Log("Minigun Limit Reached");

			break;
		case "TabTurret":
			if (AllySpawnerController.instance.TurretQuota > 0)
			{
				--AllySpawnerController.instance.TurretQuota;
				AllySpawnerController.instance.SpawnTurret("Turret");
			}
			else
				Debug.Log("Turret Limit Reached");
			
			break;
		case "TabMissileBattery":
			if (AllySpawnerController.instance.MissileBatteryQuota > 0)
			{
				--AllySpawnerController.instance.MissileBatteryQuota;
				AllySpawnerController.instance.SpawnTurret("Missile Battery");
			}
			else
				Debug.Log("Missile Battery Limit Reached");
			
			break;
		}

		HUDController.instance.UpdateResources();
	}
}
