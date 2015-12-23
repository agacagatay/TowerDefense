using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
	[SerializeField] Transform horPivot;
	[SerializeField] Transform verPivot;
	[SerializeField] float horLookSpeed;
	[SerializeField] float verLookSpeed;
	[SerializeField] float turretRange;
	[SerializeField] string priorityTargetTag;
	Transform targetTransform;

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

			horPivot.localRotation = Quaternion.Lerp(horPivot.localRotation, newHorRotation, Time.deltaTime * horLookSpeed);
			verPivot.localRotation = Quaternion.Lerp(verPivot.localRotation, newVerRotation, Time.deltaTime * verLookSpeed);
		}
	}
	
	void FindEnemiesInRange()
	{
		bool targetAcquired = false;
		bool priorityTargetAcquired = false;
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, turretRange);

		foreach (Collider hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.tag == "Enemy")
			{
				targetAcquired = true;
				string unitType;

				if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.TryGetValue(hitCollider.gameObject, out unitType))
				{
					if (unitType == priorityTargetTag)
					{
						targetAcquired = true;
						priorityTargetAcquired = true;
						targetTransform = hitCollider.transform;
					}
					else if (!priorityTargetAcquired && (targetTransform == null || Vector3.Distance(hitCollider.transform.position, transform.position) < 
						Vector3.Distance(targetTransform.position, transform.position)))
					{
						targetAcquired = true;
						targetTransform = hitCollider.transform;
					}
				}
			}

			if (!targetAcquired)
				targetTransform = null;
		}
	}
}
