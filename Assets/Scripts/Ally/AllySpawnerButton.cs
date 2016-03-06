using UnityEngine;
using System.Collections;

public class AllySpawnerButton : MonoBehaviour
{
	[SerializeField] GameObject buttonGrowPrefab;

	void OnClick()
	{
		switch(gameObject.tag)
		{
		case "TabArtillery":
			if (AllySpawnerController.instance.ArtilleryQuota > 0)
			{
				SpawnButtonGrow();
				--AllySpawnerController.instance.ArtilleryQuota;
				AllySpawnerController.instance.SpawnTurret("Artillery");
			}

			break;
		case "TabMinigun":
			if (AllySpawnerController.instance.MinigunQuota > 0)
			{
				SpawnButtonGrow();
				--AllySpawnerController.instance.MinigunQuota;
				AllySpawnerController.instance.SpawnTurret("Minigun");
			}

			break;
		case "TabTurret":
			if (AllySpawnerController.instance.TurretQuota > 0)
			{
				SpawnButtonGrow();
				--AllySpawnerController.instance.TurretQuota;
				AllySpawnerController.instance.SpawnTurret("Turret");
			}
			
			break;
		case "TabMissileBattery":
			if (AllySpawnerController.instance.MissileBatteryQuota > 0)
			{
				SpawnButtonGrow();
				--AllySpawnerController.instance.MissileBatteryQuota;
				AllySpawnerController.instance.SpawnTurret("Missile Battery");
			}
			
			break;
		}

		HUDController.instance.UpdateResources();
	}

	void SpawnButtonGrow()
	{
		if (buttonGrowPrefab != null)
		{
			GameObject branchGrowClone = (GameObject)Instantiate(buttonGrowPrefab, transform.parent.transform.position, transform.parent.transform.rotation);
			branchGrowClone.transform.parent = AllySpawnerController.instance.TurretSpawnMenu.transform;
			branchGrowClone.transform.localScale = new Vector3(1f, 1f, 1f);
			AllySpawnerController.instance.GrowIconObject = branchGrowClone;
		}
	}
}
