using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyTurretController : MonoBehaviour
{
	[SerializeField] Transform horPivot;
	[SerializeField] Transform verPivot;
	[SerializeField] float horLookSpeed;
	[SerializeField] float verLookSpeed;
	[SerializeField] float airRange;
	[SerializeField] float groundRange;
	[SerializeField] GameObject ordinancePrefab;
	[SerializeField] Transform[] ordinanceSpawnTransforms;
	[SerializeField] float fireRate;
	[SerializeField] LayerMask turretLayerMask;
	Transform targetTransform;
	Collider[] airHitColliders;
	Collider[] groundHitColliders;
	bool canFire = true;
	List<GameObject> potentialTargets = new List<GameObject>();
	List<GameObject> highestPriorityTargets = new List<GameObject>();

	void Start()
	{
		InvokeRepeating("FindEnemiesInRange", 0, 0.5f);
	}

	void Update()
	{
		if (targetTransform != null)
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

			if (angle < 10f)
			{
				RaycastHit hit;

				if (!Physics.Linecast(ordinanceSpawnTransforms[0].position, targetTransform.position, out hit, turretLayerMask) && canFire)
					StartCoroutine(FireWeapon());
			}
		}
	}
	
	void FindEnemiesInRange()
	{
		targetTransform = null;
		bool targetAcquired = false;
		int targetPriorityValue = 0;

		if (airRange > 0f)
		{
			Vector3 airColliderPos = new Vector3(transform.position.x, 30f, transform.position.z);
			airHitColliders = Physics.OverlapSphere(airColliderPos, airRange);
		}

		if (groundRange > 0f)
			groundHitColliders = Physics.OverlapSphere(transform.position, groundRange);

		if (groundRange > 0f && groundHitColliders != null && groundHitColliders.Length > 0)
		{
			int priorityValue;

			foreach (Collider groundHitCollider in groundHitColliders)
			{
				if (groundHitCollider.gameObject.tag == "EnemyGround")
				{
					targetAcquired = true;
					
					if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.TryGetValue(groundHitCollider.gameObject, out priorityValue))
					{
						if (priorityValue >= targetPriorityValue)
						{
							targetPriorityValue = priorityValue;
							potentialTargets.Add(groundHitCollider.gameObject);
						}
					}
				}
			}

			foreach (GameObject potentialTarget in potentialTargets)
			{
				if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.TryGetValue(potentialTarget, out priorityValue))
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
		}

		if (!targetAcquired && airRange > 0f && airHitColliders != null && airHitColliders.Length > 0)
		{
			int priorityValue;

			foreach (Collider airHitCollider in airHitColliders)
			{
				if (airHitCollider.gameObject.tag == "EnemyAir")
				{
					if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.TryGetValue(airHitCollider.gameObject, out priorityValue))
					{
						if (priorityValue >= targetPriorityValue)
						{
							targetPriorityValue = priorityValue;
							potentialTargets.Add(airHitCollider.gameObject);
						}
					}
				}
			}

			foreach (GameObject potentialTarget in potentialTargets)
			{
				if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.TryGetValue(potentialTarget, out priorityValue))
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
						targetAcquired = true;
						targetTransform = priorityTarget.transform;
					}
				}
			}
		}

		potentialTargets.Clear();
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
