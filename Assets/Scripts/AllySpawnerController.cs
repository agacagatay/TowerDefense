using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllySpawnerController : MonoBehaviour
{
	[SerializeField] UIWidget uiWidget;
	[SerializeField] GameObject turretPrefab;
	[SerializeField] GameObject turretBranch;
	[SerializeField] int turretCost;
	[SerializeField] GameObject missileBatteryPrefab;
	[SerializeField] GameObject missileBatteryBranch;
	[SerializeField] int missileBatteryCost;
	bool spawnerDisplayEnabled = false;
	AllySpawnerPosition selectedSpawnerPosition;
	List<GameObject> spawnBranchOptions = new List<GameObject>();
	List<GameObject> spawnBranchObjects = new List<GameObject>();
	public AllySpawnerPosition SelectedSpawnerPosition { get { return selectedSpawnerPosition; }}
	public int TurretCost { get { return turretCost; }}
	public int MissileBatteryCost { get { return missileBatteryCost; }}

	public static AllySpawnerController instance;

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

	private void On_SimpleTap(Gesture gesture)
	{
		if (spawnerDisplayEnabled)
			HideSpawnerOptions();

		if (gesture.pickedObject.tag == "SpawnPosition")
		{
			AllySpawnerPosition allySpawnerPosition = gesture.pickedObject.GetComponent<AllySpawnerPosition>();
			ShowSpawnerOptions(allySpawnerPosition);
		}
	}

	void ShowSpawnerOptions(AllySpawnerPosition allySpawnerPosition)
	{
		selectedSpawnerPosition = allySpawnerPosition;
		spawnerDisplayEnabled = true;
		uiWidget.SetAnchor(allySpawnerPosition.transform);

		int spawnBranches = allySpawnerPosition.SpawnOptions.Length;
		float angle = 360f/spawnBranches;
		float offset = angle/2f;

		foreach(string spawnOption in allySpawnerPosition.SpawnOptions)
		{
			switch(spawnOption)
			{
			case "Turret":
				spawnBranchOptions.Add(turretBranch);
				break;
			case "Missile Battery":
				spawnBranchOptions.Add(missileBatteryBranch);
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

	public void SpawnTurret(string prefabName)
	{
		GameObject turretClone;
		AllyStructureController allyStructureVariables;

		switch(prefabName)
		{
		case "Turret":
			turretClone = (GameObject)Instantiate(turretPrefab, 
				selectedSpawnerPosition.SpawnTransform.position, selectedSpawnerPosition.SpawnTransform.rotation);

			allyStructureVariables = turretClone.GetComponent<AllyStructureController>();
			allyStructureVariables.TurretSpawnObject = selectedSpawnerPosition.gameObject;

			break;
		case "Missile Battery":
			turretClone = (GameObject)Instantiate(missileBatteryPrefab, 
				selectedSpawnerPosition.SpawnTransform.position, selectedSpawnerPosition.SpawnTransform.rotation);

			allyStructureVariables = turretClone.GetComponent<AllyStructureController>();
			allyStructureVariables.TurretSpawnObject = selectedSpawnerPosition.gameObject;

			break;
		}

		ResourcesController.instance.Shards -= turretCost;
		ResourcesController.instance.UpdateShards();

		selectedSpawnerPosition.DisableSpawnerPosition();
		HideSpawnerOptions();
	}
}
