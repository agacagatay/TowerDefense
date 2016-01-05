using UnityEngine;
using System.Collections;

public class AllySpawnerPosition : MonoBehaviour
{
	[SerializeField] GameObject turretPrefab;
	[SerializeField] GameObject mapIconPrefab;
	[SerializeField] int turretCost;
	[SerializeField] Transform spawnTransform;
	[SerializeField] string[] spawnOptions;
	GameObject mapIcon;
	public Transform SpawnTransform { get { return spawnTransform; }}
	public string[] SpawnOptions { get { return spawnOptions; }}

	void Start()
	{
		mapIcon = (GameObject)Instantiate(mapIconPrefab, transform.position, transform.rotation);
	}

	public void EnableSpawnerPosition()
	{
		Collider spawnerPositionCollider = gameObject.GetComponent<Collider>();
		spawnerPositionCollider.enabled = true;

		Renderer spawnerPositionRenderer = gameObject.GetComponent<Renderer>();
		spawnerPositionRenderer.enabled = true;

		if (mapIcon == null)
			mapIcon = (GameObject)Instantiate(mapIconPrefab, transform.position, transform.rotation);
	}

	public void DisableSpawnerPosition()
	{
		Collider spawnerPositionCollider = gameObject.GetComponent<Collider>();
		spawnerPositionCollider.enabled = false;

		Renderer spawnerPositionRenderer = gameObject.GetComponent<Renderer>();
		spawnerPositionRenderer.enabled = false;

		if (mapIcon != null)
			Destroy(mapIcon);
	}
}
