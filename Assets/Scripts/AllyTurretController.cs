using UnityEngine;
using System.Collections;

public class AllyTurretController : MonoBehaviour
{
	[SerializeField] Transform horPivot;
	[SerializeField] Transform verPivot;
	[SerializeField] float horLookSpeed;
	[SerializeField] float verLookSpeed;
	[SerializeField] float airRange;
	[SerializeField] float groundRange;
	[SerializeField] string priorityTargetTag;
	[SerializeField] GameObject ordinancePrefab;
	[SerializeField] Transform[] ordinanceSpawnTransforms;
	[SerializeField] float fireRate;
	[SerializeField] LayerMask turretLayerMask;
	Transform targetTransform;
	Collider[] airHitColliders;
	Collider[] groundHitColliders;
	bool canFire = true;

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
	
	void FindEnemiesInRange()
	{
		bool targetAcquired = false;
		bool priorityTargetAcquired = false;

		if (airRange > 0f)
		{
			Vector3 airColliderPos = new Vector3(transform.position.x, 30f, transform.position.z);
			airHitColliders = Physics.OverlapSphere(airColliderPos, airRange);
		}

		if (groundRange > 0f)
			groundHitColliders = Physics.OverlapSphere(transform.position, groundRange);

		if (groundHitColliders != null && groundHitColliders.Length > 0)
		{
			foreach (Collider groundHitCollider in groundHitColliders)
			{
				if (groundHitCollider.gameObject.tag == "Enemy")
				{
					string unitType;
					
					if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.TryGetValue(groundHitCollider.gameObject, out unitType))
					{
						if (unitType == priorityTargetTag)
						{
							targetAcquired = true;
							priorityTargetAcquired = true;
							targetTransform = groundHitCollider.transform;
						}
						else if (!priorityTargetAcquired && unitType != "Aircraft" && (targetTransform == null || Vector3.Distance(groundHitCollider.transform.position, transform.position) < 
							Vector3.Distance(targetTransform.position, transform.position)))
						{
							targetAcquired = true;
							targetTransform = groundHitCollider.transform;
						}
					}
				}
			}
		}

		if (!targetAcquired && airHitColliders != null && airHitColliders.Length > 0)
		{
			foreach (Collider airHitCollider in airHitColliders)
			{
				if (airHitCollider.gameObject.tag == "Enemy")
				{
					targetAcquired = true;
					string unitType;

					if (SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.TryGetValue(airHitCollider.gameObject, out unitType))
					{
						if (unitType == "Dropship" && (targetTransform == null || Vector3.Distance(airHitCollider.transform.position, transform.position) < 
							Vector3.Distance(targetTransform.position, transform.position)))
						{
							targetAcquired = true;
							targetTransform = airHitCollider.transform;
						}
						else if (unitType == "Aircraft" && (targetTransform == null || Vector3.Distance(airHitCollider.transform.position, transform.position) < 
							Vector3.Distance(targetTransform.position, transform.position)))
						{
							targetAcquired = true;
							targetTransform = airHitCollider.transform;
						}
					}
				}
			}
		}

		if (!targetAcquired)
			targetTransform = null;
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
