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
	[SerializeField] GameObject ordinancePrefab;
	[SerializeField] Transform[] ordinanceSpawnTransforms;
	[SerializeField] bool fireSequentially = false;
	[SerializeField] float fireRate;
	[SerializeField] LayerMask turretLayerMask;
	Transform targetTransform;
	bool canFire = true;
	bool turretReset = true;
	int sequenceVal = 0;
	List<GameObject> potentialTargets = new List<GameObject>();
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

			Vector3 targetDir = targetTransform.position - horPivot.position;
			targetDir.y = 0f;
			Vector3 forward = horPivot.forward;
			float angle = Vector3.Angle(targetDir, forward);

			if (angle < 10f && canFire)
			{
				StartCoroutine(FireWeapon());
			}
		}
	}
	
	void FindTargetsInRange()
	{
		targetTransform = null;
		turretReset = true;
		int priorityValue;
		int targetPriorityValue = 0;
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, turretRange);

		foreach (Collider hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.tag == "Ally" || hitCollider.gameObject.tag == "PrimaryStructure")
			{
				if (!(hitCollider.gameObject.tag == "PrimaryStructure" && GameController.instance.SecondaryStructures.Count > 0))
				{
					RaycastHit hit;

					if (!Physics.Linecast(verPivot.position, hitCollider.transform.position, out hit, turretLayerMask))
					{
						turretReset = false;
						
						if (SpawnedAllyDictionary.instance.spawnedAllyDictionary.TryGetValue(hitCollider.gameObject, out priorityValue))
						{
							if (priorityValue >= targetPriorityValue)
							{
								targetPriorityValue = priorityValue;
								potentialTargets.Add(hitCollider.gameObject);
							}
						}
					}
				}
			}
		}

		foreach (GameObject potentialTarget in potentialTargets)
		{
			if (SpawnedAllyDictionary.instance.spawnedAllyDictionary.TryGetValue(potentialTarget, out priorityValue))
			{
				if (priorityValue == targetPriorityValue)
				{
					highestPriorityTargets.Add(potentialTarget);
				}
			}
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

		potentialTargets.Clear();
		highestPriorityTargets.Clear();
	}

	IEnumerator FireWeapon()
	{
		canFire = false;
		GameObject ordinanceClone;

		if (fireSequentially)
		{
			ordinanceClone = (GameObject)Instantiate(ordinancePrefab, ordinanceSpawnTransforms[sequenceVal].position, ordinanceSpawnTransforms[sequenceVal].rotation);
			sequenceVal++;

			if (sequenceVal >= ordinanceSpawnTransforms.Length)
				sequenceVal = 0;
		}
		else
		{
			int randomTransform = Random.Range(0, ordinanceSpawnTransforms.Length);
			ordinanceClone = (GameObject)Instantiate(ordinancePrefab, ordinanceSpawnTransforms[randomTransform].position, ordinanceSpawnTransforms[randomTransform].rotation);
		}

		OrdinanceController ordinanceController = ordinanceClone.GetComponent<OrdinanceController>();
		ordinanceController.TargetTransform = targetTransform;

		yield return new WaitForSeconds(fireRate);

		canFire = true;
	}
}
