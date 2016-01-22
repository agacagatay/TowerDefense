using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllySpawnerController : MonoBehaviour
{
	[SerializeField] UIWidget uiWidget;
	[SerializeField] GameObject spawnTimer;
	[SerializeField] int artillaryQuota;
	[SerializeField] GameObject artillaryPrefab;
	[SerializeField] GameObject artillaryBranch;
	[SerializeField] float artillarySpawnTime;
	[SerializeField] int minigunQuota;
	[SerializeField] GameObject minigunPrefab;
	[SerializeField] GameObject minigunBranch;
	[SerializeField] float minigunSpawnTime;
	[SerializeField] int turretQuota;
	[SerializeField] GameObject turretPrefab;
	[SerializeField] GameObject turretBranch;
	[SerializeField] float turretSpawnTime;
	[SerializeField] int missileBatteryQuota;
	[SerializeField] GameObject missileBatteryPrefab;
	[SerializeField] GameObject missileBatteryBranch;
	[SerializeField] float missileBatterySpawnTime;
	[SerializeField] GameObject turretSelectMenu;
	[SerializeField] GameObject spawnEffect2;
	[SerializeField] GameObject spawnEffect3;
	[SerializeField] GameObject spawnEffect4;
	bool spawnerDisplayEnabled = false;
	bool turretSelectDisplayEnabled = false;
	AllySpawnerPosition selectedSpawnerPosition;
	AllyStructureController structureController;
	UIWidget turretSelectWidget;
	AllyDestroyTurret destroyTurretTab;
	List<GameObject> spawnBranchOptions = new List<GameObject>();
	List<GameObject> spawnBranchObjects = new List<GameObject>();
	public AllySpawnerPosition SelectedSpawnerPosition { get { return selectedSpawnerPosition; }}
	public AllyStructureController StructureController { get { return structureController; }}
	public int ArtillaryQuota { get { return artillaryQuota; } set { artillaryQuota = value; }}
	public int MinigunQuota { get { return minigunQuota; } set { minigunQuota = value; }}
	public int TurretQuota { get { return turretQuota; } set { turretQuota = value; }}
	public int MissileBatteryQuota { get { return missileBatteryQuota; } set { missileBatteryQuota = value; }}

	public static AllySpawnerController instance;

	void Awake()
	{
		instance = this;

		turretSelectWidget = turretSelectMenu.GetComponent<UIWidget>();
		destroyTurretTab = turretSelectMenu.GetComponentInChildren<AllyDestroyTurret>();
		HideTurretSelectTab();
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

	private void On_SimpleTap(Gesture gesture)
	{
		if (gesture.pickedObject != null)
		{
			if (spawnerDisplayEnabled)
				HideSpawnerOptions();

			if (turretSelectDisplayEnabled)
				HideTurretSelectTab();

			if (gesture.pickedObject.tag == "SpawnPosition")
			{
				AllySpawnerPosition allySpawnerPosition = gesture.pickedObject.GetComponent<AllySpawnerPosition>();
				ShowSpawnerOptions(allySpawnerPosition);
			}
			if (gesture.pickedObject.tag == "Ally")
			{
				structureController = gesture.pickedObject.GetComponent<AllyStructureController>();

				if (structureController.IsTurret)
				{
					destroyTurretTab.StructureController = gesture.pickedObject.GetComponentInChildren<AllyStructureController>();
					turretSelectWidget.SetAnchor(gesture.pickedObject.transform);
					ShowTurretSelectTab();
				}
			}
		}
	}

	void ShowSpawnerOptions(AllySpawnerPosition allySpawnerPosition)
	{
		spawnerDisplayEnabled = true;

		selectedSpawnerPosition = allySpawnerPosition;
		uiWidget.SetAnchor(allySpawnerPosition.transform);

		int spawnBranches = allySpawnerPosition.SpawnOptions.Length;
		float angle = 360f/spawnBranches;
		float offset = angle/2f;

		foreach(string spawnOption in allySpawnerPosition.SpawnOptions)
		{
			switch(spawnOption)
			{
			case "Artillary":
				GameObject artillaryBranchClone = artillaryBranch as GameObject;
				UITexture artillarySprite = artillaryBranchClone.GetComponentInChildren<UITexture>();

				if (AllySpawnerController.instance.ArtillaryQuota > 0)
					artillarySprite.alpha = 1f;
				else
					artillarySprite.alpha = 0.3f;

				spawnBranchOptions.Add(artillaryBranchClone);
				break;
			case "Minigun":
				GameObject minigunBranchClone = minigunBranch as GameObject;
				UITexture minigunSprite = minigunBranchClone.GetComponentInChildren<UITexture>();

				if (AllySpawnerController.instance.MinigunQuota > 0)
					minigunSprite.alpha = 1f;
				else
					minigunSprite.alpha = 0.3f;

				spawnBranchOptions.Add(minigunBranchClone);
				break;
			case "Turret":
				GameObject turretBranchClone = turretBranch as GameObject;
				UITexture turretSprite = turretBranchClone.GetComponentInChildren<UITexture>();

				if (AllySpawnerController.instance.TurretQuota > 0)
					turretSprite.alpha = 1f;
				else
					turretSprite.alpha = 0.3f;

				spawnBranchOptions.Add(turretBranchClone);
				break;
			case "Missile Battery":
				GameObject missileBatteryBranchClone = missileBatteryBranch as GameObject;
				UITexture missileBatterySprite = missileBatteryBranchClone.GetComponentInChildren<UITexture>();

				if (AllySpawnerController.instance.MissileBatteryQuota > 0)
					missileBatterySprite.alpha = 1f;
				else
					missileBatterySprite.alpha = 0.3f;

				spawnBranchOptions.Add(missileBatteryBranchClone);
				break;
			default:
				Debug.LogError("Invalid branch specified from spawner position");
				break;
			}
		}

		int branchNumber = 0;

		foreach (GameObject spawnBranch in spawnBranchOptions)
		{
			Quaternion branchRotation = Quaternion.Euler(0f, 0f, (-angle * branchNumber) + offset);
			GameObject branchClone = (GameObject)Instantiate(spawnBranch, uiWidget.transform.position, branchRotation);
			branchClone.transform.parent = uiWidget.transform;
			branchClone.transform.localScale = new Vector3(1f, 1f, 1f);

			AllySpawnerTab allySpawnerTab = branchClone.GetComponent<AllySpawnerTab>();
			allySpawnerTab.ToggleText(branchNumber);

			Transform branchCloneChild = branchClone.transform.GetChild(0);
			branchCloneChild.localRotation = Quaternion.Euler(0f, 0f, -((-angle * branchNumber) + offset));

			spawnBranchObjects.Add(branchClone);
			branchNumber++;
		}
	}

	void HideSpawnerOptions()
	{
		foreach (GameObject spawnBranch in spawnBranchObjects)
		{
			Destroy(spawnBranch);
		}

		spawnBranchOptions.Clear();
		spawnBranchObjects.Clear();
		spawnerDisplayEnabled = false;
	}

	void ShowTurretSelectTab()
	{
		turretSelectDisplayEnabled = true;
		turretSelectMenu.SetActive(true);
	}

	public void HideTurretSelectTab()
	{
		turretSelectMenu.SetActive(false);
		turretSelectDisplayEnabled = false;
	}

	public void SpawnTurret(string prefabName)
	{
		switch(prefabName)
		{
		case "Artillary":
			ToggleSpawnEffect(4);
			StartCoroutine(ToggleTurretSpawn("Artillary", artillarySpawnTime));
			break;
		case "Minigun":
			ToggleSpawnEffect(2);
			StartCoroutine(ToggleTurretSpawn("Minigun", minigunSpawnTime));
			break;
		case "Turret":
			ToggleSpawnEffect(3);
			StartCoroutine(ToggleTurretSpawn("Turret", turretSpawnTime));
			break;
		case "Missile Battery":
			ToggleSpawnEffect(4);
			StartCoroutine(ToggleTurretSpawn("Missile Battery", missileBatterySpawnTime));
			break;
		default:
			Debug.LogError("No proper turret prefab type specified");
			break;
		}

		selectedSpawnerPosition.DisableSpawnerPosition();
		HideSpawnerOptions();
	}

	void ToggleSpawnEffect(int effectLength)
	{
		AllySpawnerPosition spawnPosition = selectedSpawnerPosition;
		Vector3 modifiedPos = new Vector3(spawnPosition.SpawnTransform.position.x, spawnPosition.SpawnTransform.position.y + 15f, spawnPosition.SpawnTransform.position.z);

		switch (effectLength)
		{
		case 2:
			Instantiate(spawnEffect2, modifiedPos, spawnEffect2.transform.rotation);
			break;
		case 3:
			Instantiate(spawnEffect3, modifiedPos, spawnEffect3.transform.rotation);
			break;
		case 4:
			Instantiate(spawnEffect4, modifiedPos, spawnEffect4.transform.rotation);
			break;
		default:
			Debug.LogError("Invalid length specified");
			break;
		}
	}

	IEnumerator ToggleTurretSpawn(string prefabName, float spawnTime)
	{
		AllySpawnerPosition spawnPosition = selectedSpawnerPosition;

		GameObject timerClone = (GameObject)Instantiate(spawnTimer, transform.position, transform.rotation);
		timerClone.transform.parent = uiWidget.transform;
		timerClone.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);

		UIWidget timerWidget = timerClone.GetComponent<UIWidget>();
		timerWidget.SetAnchor(spawnPosition.transform);

		UISprite timerSprite = timerClone.GetComponentInChildren<UISprite>();
		float fillValue = 0f;

		while (fillValue < 1f)
		{
			timerSprite.fillAmount = fillValue;
			fillValue += Time.deltaTime/spawnTime;
			yield return null;
		}

		Destroy(timerClone);

		GameObject turretClone;
		AllyStructureController allyStructureVariables;

		switch(prefabName)
		{
		case "Artillary":
			turretClone = (GameObject)Instantiate(artillaryPrefab, 
				spawnPosition.SpawnTransform.position, spawnPosition.SpawnTransform.rotation);

			allyStructureVariables = turretClone.GetComponent<AllyStructureController>();
			allyStructureVariables.TurretSpawnObject = spawnPosition.gameObject;
			break;
		case "Minigun":
			turretClone = (GameObject)Instantiate(minigunPrefab, 
				spawnPosition.SpawnTransform.position, spawnPosition.SpawnTransform.rotation);

			allyStructureVariables = turretClone.GetComponent<AllyStructureController>();
			allyStructureVariables.TurretSpawnObject = spawnPosition.gameObject;
			break;
		case "Turret":
			turretClone = (GameObject)Instantiate(turretPrefab, 
				spawnPosition.SpawnTransform.position, spawnPosition.SpawnTransform.rotation);

			allyStructureVariables = turretClone.GetComponent<AllyStructureController>();
			allyStructureVariables.TurretSpawnObject = spawnPosition.gameObject;
			break;
		case "Missile Battery":
			turretClone = (GameObject)Instantiate(missileBatteryPrefab, 
				spawnPosition.SpawnTransform.position, spawnPosition.SpawnTransform.rotation);

			allyStructureVariables = turretClone.GetComponent<AllyStructureController>();
			allyStructureVariables.TurretSpawnObject = spawnPosition.gameObject;
			break;
		default:
			Debug.LogError("Invalid prefab name");
			break;
		}
	}
}
