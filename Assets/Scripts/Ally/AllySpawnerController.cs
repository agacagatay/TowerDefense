using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllySpawnerController : MonoBehaviour
{
	[SerializeField] UIWidget turretSpawnMenu;
	[SerializeField] GameObject spawnTimer;
	[SerializeField] int artilleryQuota;
	[SerializeField] float artillerySpawnTime;
	[SerializeField] GameObject artilleryPrefab;
	[SerializeField] GameObject artilleryBranch;
	[SerializeField] int minigunQuota;
	[SerializeField] float minigunSpawnTime;
	[SerializeField] GameObject minigunPrefab;
	[SerializeField] GameObject minigunBranch;
	[SerializeField] int turretQuota;
	[SerializeField] float turretSpawnTime;
	[SerializeField] GameObject turretPrefab;
	[SerializeField] GameObject turretBranch;
	[SerializeField] int missileBatteryQuota;
	[SerializeField] float missileBatterySpawnTime;
	[SerializeField] GameObject missileBatteryPrefab;
	[SerializeField] GameObject missileBatteryBranch;
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
	GameObject growIconObject;
	List<GameObject> spawnBranchOptions = new List<GameObject>();
	List<GameObject> spawnBranchObjects = new List<GameObject>();
	public UIWidget TurretSpawnMenu { get { return turretSpawnMenu; }}
	public GameObject GrowIconObject { get { return growIconObject; } set { growIconObject = value; }}
	public AllySpawnerPosition SelectedSpawnerPosition { get { return selectedSpawnerPosition; }}
	public AllyStructureController StructureController { get { return structureController; }}
	public bool SpawnerDisplayEnabled { get { return spawnerDisplayEnabled; }}
	public int ArtilleryQuota { get { return artilleryQuota; } set { artilleryQuota = value; }}
	public int MinigunQuota { get { return minigunQuota; } set { minigunQuota = value; }}
	public int TurretQuota { get { return turretQuota; } set { turretQuota = value; }}
	public int MissileBatteryQuota { get { return missileBatteryQuota; } set { missileBatteryQuota = value; }}

	public static AllySpawnerController instance;

	void Awake()
	{
		instance = this;

		turretSelectWidget = turretSelectMenu.GetComponent<UIWidget>();
		destroyTurretTab = turretSelectMenu.GetComponentInChildren<AllyDestroyTurret>();
	}

	void Start()
	{
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

	void On_SimpleTap(Gesture gesture)
	{
		if (gesture.pickedObject != null)
		{
			if (spawnerDisplayEnabled)
				HideSpawnerOptions();

			if (turretSelectDisplayEnabled)
				HideTurretSelectTab();

			if (gesture.pickedObject.tag == "SpawnPosition")
			{
				selectedSpawnerPosition = gesture.pickedObject.GetComponent<AllySpawnerPosition>();
				ShowSpawnerOptions(selectedSpawnerPosition);
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

	public void ShowSpawnerOptions(AllySpawnerPosition allySpawnerPosition)
	{
		AudioController.instance.PlayOneshot("SFX/Menu_Spawn_Menu", AudioController.instance.gameObject);

		spawnerDisplayEnabled = true;

		if (GrowIconObject != null)
			Destroy(GrowIconObject);

		turretSpawnMenu.SetAnchor(selectedSpawnerPosition.transform);

		int spawnBranches = selectedSpawnerPosition.SpawnOptions.Length;
		float angle = 360f/spawnBranches;
		float offset = angle/2f;

		foreach(string spawnOption in selectedSpawnerPosition.SpawnOptions)
		{
			switch(spawnOption)
			{
			case "Artillery":
				GameObject artilleryBranchClone = artilleryBranch as GameObject;
				UITexture artillerySprite = artilleryBranchClone.GetComponentInChildren<UITexture>();

				if (AllySpawnerController.instance.ArtilleryQuota > 0)
					artillerySprite.alpha = 1f;
				else
					artillerySprite.alpha = 0.3f;

				spawnBranchOptions.Add(artilleryBranchClone);
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
			GameObject branchClone = (GameObject)Instantiate(spawnBranch, turretSpawnMenu.transform.position, branchRotation);
			branchClone.transform.parent = turretSpawnMenu.transform;
			branchClone.transform.localScale = new Vector3(1f, 1f, 1f);

			AllySpawnerTab allySpawnerTab = branchClone.GetComponent<AllySpawnerTab>();
			allySpawnerTab.ToggleText(branchNumber);

			Transform branchCloneChild = branchClone.transform.GetChild(0);
			branchCloneChild.localRotation = Quaternion.Euler(0f, 0f, -((-angle * branchNumber) + offset));

			spawnBranchObjects.Add(branchClone);
			branchNumber++;
		}
	}

	public void HideSpawnerOptions()
	{
		AudioController.instance.PlayOneshot("SFX/Menu_Close", AudioController.instance.gameObject);

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
		AudioController.instance.PlayOneshot("SFX/Menu_Open", AudioController.instance.gameObject);
		turretSelectDisplayEnabled = true;
		turretSelectMenu.SetActive(true);
	}

	public void HideTurretSelectTab()
	{
		AudioController.instance.PlayOneshot("SFX/Menu_Close", AudioController.instance.gameObject);
		turretSelectMenu.SetActive(false);
		turretSelectDisplayEnabled = false;
	}

	public void SpawnTurret(string prefabName)
	{
		AudioController.instance.PlayOneshot("SFX/Menu_Trigger_Spawn", AudioController.instance.gameObject);

		switch(prefabName)
		{
		case "Artillery":
			ToggleSpawnEffect(4);
			StartCoroutine(ToggleTurretSpawn("Artillery", artillerySpawnTime));
			break;
		case "Minigun":
			ToggleSpawnEffect(3);
			StartCoroutine(ToggleTurretSpawn("Minigun", minigunSpawnTime));

			if (TutorialController.instance.TutorialActive && TutorialController.instance.CurrentTutorialScreen == 11)
			{
				TutorialController.instance.NextTutorialScreen();
			}

			break;
		case "Turret":
			ToggleSpawnEffect(2);
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

		switch (effectLength)
		{
		case 2:
			Instantiate(spawnEffect2, spawnPosition.SpawnTransform.position, spawnEffect2.transform.rotation);
			break;
		case 3:
			Instantiate(spawnEffect3, spawnPosition.SpawnTransform.position, spawnEffect3.transform.rotation);
			break;
		case 4:
			Instantiate(spawnEffect4, spawnPosition.SpawnTransform.position, spawnEffect4.transform.rotation);
			break;
		default:
			Debug.LogError("Invalid length specified");
			break;
		}
	}

	IEnumerator ToggleTurretSpawn(string prefabName, float spawnTime)
	{
		AllySpawnerPosition spawnPosition = selectedSpawnerPosition;
		AudioController.instance.CreateInstance("SFX/Tower_Teleport", spawnPosition.gameObject);
		AudioController.instance.Play("SFX/Tower_Teleport", spawnPosition.gameObject);

		GameObject timerClone = (GameObject)Instantiate(spawnTimer, transform.position, transform.rotation);
		timerClone.transform.parent = turretSpawnMenu.transform;
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
		AudioController.instance.Stop("SFX/Tower_Teleport", spawnPosition.gameObject);
		AudioController.instance.PlayOneshot("SFX/Tower_Teleport_End", spawnPosition.gameObject);

		GameObject turretClone;
		AllyStructureController allyStructureVariables;

		switch(prefabName)
		{
		case "Artillery":
			turretClone = (GameObject)Instantiate(artilleryPrefab, 
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
