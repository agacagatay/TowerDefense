using UnityEngine;
using System.Collections;

public class AllySpawnerPosition : MonoBehaviour
{
	[SerializeField] GameObject mapIconPrefab;
	[SerializeField] int turretCost;
	[SerializeField] Transform spawnTransform;
	[SerializeField] string[] spawnOptions;
	[SerializeField] GameObject[] particleObjects;
	GameObject mapIcon;
	public Transform SpawnTransform { get { return spawnTransform; }}
	public string[] SpawnOptions { get { return spawnOptions; }}

	void Start()
	{
		mapIcon = (GameObject)Instantiate(mapIconPrefab, transform.position, transform.rotation);
	}

	public void EnableSpawnerPosition()
	{
		foreach (GameObject particleObject in particleObjects)
			particleObject.SetActive(true);

		Collider spawnerPositionCollider = gameObject.GetComponent<Collider>();
		spawnerPositionCollider.enabled = true;

		Renderer spawnerPositionRenderer = gameObject.GetComponent<Renderer>();
		spawnerPositionRenderer.enabled = true;

		if (mapIcon == null)
			mapIcon = (GameObject)Instantiate(mapIconPrefab, transform.position, transform.rotation);
	}

	public void DisableSpawnerPosition()
	{
		foreach (GameObject particleObject in particleObjects)
			particleObject.SetActive(false);

		Collider spawnerPositionCollider = gameObject.GetComponent<Collider>();
		spawnerPositionCollider.enabled = false;

		Renderer spawnerPositionRenderer = gameObject.GetComponent<Renderer>();
		spawnerPositionRenderer.enabled = false;

		if (mapIcon != null)
			Destroy(mapIcon);
	}
}
