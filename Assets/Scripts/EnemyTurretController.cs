using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTurretController : MonoBehaviour
{
	[SerializeField] Transform horPivot;
	[SerializeField] Transform verPivot;
	[SerializeField] float horLookSpeed;
	[SerializeField] float verLookSpeed;
	[SerializeField] float turretRange;
	[SerializeField] string priorityTargetTag;
	[SerializeField] GameObject ordinancePrefab;
	[SerializeField] Transform[] ordinanceSpawnTransforms;
	[SerializeField] float fireRate;
	[SerializeField] LayerMask turretLayerMask;
	Transform targetTransform;
	bool canFire = true;
	bool turretReset = true;
	List<GameObject> highestPriorityTargets = new List<GameObject>();

	void Start()
	{
		InvokeRepeating("FindTargetsInRange", 0, 0.5f);
	}

	void Update()
	{
		if (turretReset)
		{
			horPivot.localRotation = Quaternion.Lerp(horPivot.localRotation, Quaternion.identity, Time.deltaTime * horLookSpeed);
			verPivot.localRotation = Quaternion.Lerp(verPivot.localRotation, Quaternion.identity, Time.deltaTime * verLookSpeed);
		}
		else if (targetTransform != null)
		{
			Quaternion newHorRotation = Quaternion.LookRotation(targetTransform.position - horPivot.position);
			newHorRotation.x = 0f;
			newHorRotation.z = 0f;

			Quaternion newVerRotation = Quaternion.LookRotation(targetTransform.position - verPivot.position);
			newVerRotation.y = 0f;
			newVerRotation.z = 0f;

			horPivot.rotation = Quaternion.Lerp(horPivot.rotation, newHorRotation, Time.deltaTime * horLookSpeed);
			verPivot.localRotation = Quaternion.Lerp(verPivot.localRotation, newVerRotation, Time.deltaTime * verLookSpeed);

			RaycastHit hit;

			if (Physics.Raycast(ordinanceSpawnTransforms[0].position, verPivot.forward, out hit, 100f, turretLayerMask))
			{
				if (hit.transform.gameObject == targetTransform.gameObject && canFire)
				{
					StartCoroutine(FireWeapon());
				}
			}
		}
	}
	
	void FindTargetsInRange()
	{
		targetTransform = null;
		turretReset = true;
		int targetPriorityValue = 0;
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, turretRange);

		foreach (Collider hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.tag == "Ally")
			{
				turretReset = false;
				int priorityValue;

				if (SpawnedAllyDictionary.instance.spawnedAllyDictionary.TryGetValue(hitCollider.gameObject, out priorityValue))
				{
					if (priorityValue >= targetPriorityValue)
					{
						targetPriorityValue = priorityValue;
					}

					foreach(KeyValuePair<GameObject, int> keyValue in SpawnedAllyDictionary.instance.spawnedAllyDictionary)
					{
						if (keyValue.Value == targetPriorityValue)
							highestPriorityTargets.Add(keyValue.Key);
					}

					foreach (GameObject priorityTarget in highestPriorityTargets)
					{
						if (priorityTarget != null)
						{
							if (targetTransform == null || Vector3.Distance(priorityTarget.transform.position, transform.position) < 
								Vector3.Distance(targetTransform.position, transform.position))
							{
								targetTransform = priorityTarget.transform;
							}
						}
					}
				}
			}
		}

		highestPriorityTargets.Clear();
	}

	IEnumerator FireWeapon()
	{
		canFire = false;

		int randomTransform = Random.Range(0, ordinanceSpawnTransforms.Length);
		GameObject ordinanceClone = (GameObject)Instantiate(ordinancePrefab, ordinanceSpawnTransforms[randomTransform].position, ordinanceSpawnTransforms[randomTransform].rotation);
		OrdinanceController ordinanceController = ordinanceClone.GetComponent<OrdinanceController>();
		ordinanceController.TargetTransform = targetTransform;

		yield return new WaitForSeconds(fireRate);

		canFire = true;
	}
}
