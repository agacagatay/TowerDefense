using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllySpawnerController : MonoBehaviour
{
	[SerializeField] UIWidget uiWidget;
	[SerializeField] GameObject turretBranch;
	[SerializeField] GameObject missileBatteryBranch;
	bool spawnerDisplayEnabled = false;
	List<GameObject> spawnBranchOptions = new List<GameObject>();
	List<GameObject> spawnBranchObjects = new List<GameObject>();

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

	public void ShowSpawnerOptions(AllySpawnerPosition allySpawnerPosition)
	{
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
			GameObject branchClone = (GameObject)Instantiate(spawnBranch, transform.position, branchRotation);
			branchClone.transform.parent = transform;
			branchClone.transform.localScale = new Vector3(1f, 1f, 1f);

			Transform branchCloneChild = branchClone.transform.GetChild(0);
			branchCloneChild.localRotation = Quaternion.Euler(0f, 0f, -((-angle * branchNumber) + offset));

			spawnBranchObjects.Add(branchClone);
			branchNumber++;
		}
	}

	public void HideSpawnerOptions()
	{
		foreach (GameObject spawnBranch in spawnBranchObjects)
		{
			Destroy(spawnBranch);
		}

		spawnBranchOptions.Clear();
		spawnBranchObjects.Clear();
		spawnerDisplayEnabled = false;
	}
}
