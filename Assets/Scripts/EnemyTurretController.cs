using UnityEngine;
using System.Collections;

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
	Transform targetTransform;
	bool canFire = true;
	bool turretReset = true;

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

			if (angle < 10f)
			{
				if (canFire)
					StartCoroutine(FireWeapon());
			}
		}
	}
	
	void FindTargetsInRange()
	{
		turretReset = true;
		bool priorityTargetAcquired = false;
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, turretRange);

		foreach (Collider hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.tag == "Ally")
			{
				turretReset = false;
				string structureType;

				if (SpawnedAllyDictionary.instance.spawnedAllyDictionary.TryGetValue(hitCollider.gameObject, out structureType))
				{
					if (structureType == priorityTargetTag)
					{
						priorityTargetAcquired = true;
						targetTransform = hitCollider.transform;
					}
					else if (!priorityTargetAcquired && (targetTransform == null || Vector3.Distance(hitCollider.transform.position, transform.position) < 
						Vector3.Distance(targetTransform.position, transform.position)))
					{
						targetTransform = hitCollider.transform;
					}
				}
			}
		}
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
