using UnityEngine;
using System.Collections;

public class AllySpawnerButton : MonoBehaviour
{
	void OnClick()
	{
		switch(gameObject.tag)
		{
		case "TabTurret":
			if (ResourcesController.instance.Shards >= AllySpawnerController.instance.TurretCost)
			{
				ResourcesController.instance.Shards -= AllySpawnerController.instance.TurretCost;
				ResourcesController.instance.UpdateShards();

				AllySpawnerController.instance.SpawnTurret("Turret");
			}
			else
				Debug.Log("Insufficient Shards");
			
			break;
		case "TabMissileBattery":
			if (ResourcesController.instance.Shards >= AllySpawnerController.instance.MissileBatteryCost)
			{
				ResourcesController.instance.Shards -= AllySpawnerController.instance.MissileBatteryCost;
				ResourcesController.instance.UpdateShards();

				AllySpawnerController.instance.SpawnTurret("Missile Battery");
			}
			else
				Debug.Log("Insufficient Shards");
			
			break;
		}
	}
}
