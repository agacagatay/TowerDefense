using UnityEngine;
using System.Collections;

public class AllySpawnerPosition : MonoBehaviour
{
	[SerializeField] GameObject turretPrefab;
	[SerializeField] int turretCost;
	[SerializeField] Transform spawnTransform;
	[SerializeField] string[] spawnOptions;
	public Transform SpawnTransform { get { return spawnTransform; }}
	public string[] SpawnOptions { get { return spawnOptions; }}

	public void EnableSpawnerPosition()
	{
		Collider spawnerPositionCollider = gameObject.GetComponent<Collider>();
		spawnerPositionCollider.enabled = true;

		Renderer spawnerPositionRenderer = gameObject.GetComponent<Renderer>();
		spawnerPositionRenderer.enabled = true;
	}

	public void DisableSpawnerPosition()
	{
		Collider spawnerPositionCollider = gameObject.GetComponent<Collider>();
		spawnerPositionCollider.enabled = false;

		Renderer spawnerPositionRenderer = gameObject.GetComponent<Renderer>();
		spawnerPositionRenderer.enabled = false;
	}
}
