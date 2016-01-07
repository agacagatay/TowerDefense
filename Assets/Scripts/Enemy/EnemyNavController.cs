using UnityEngine;
using System.Collections;

public class EnemyNavController : MonoBehaviour
{
	[SerializeField] NavMeshAgent unitNavAgent;
	[SerializeField] bool groundUnit = false;
	[SerializeField] bool followNavSlope = false;
	[SerializeField] float heightOffset;
	Transform targetTransform;

	void Start()
	{
		if (groundUnit)
			InvokeRepeating("SetNavDestination", 0f, 0.5f);
		else
			SetNewAirWayoint();
	}

	void Update()
	{
		if (groundUnit && followNavSlope)
		{
			NavMeshHit hit;
			
			if (NavMesh.SamplePosition(transform.position + transform.forward, out hit, 1f, 1))
			{
				transform.LookAt(new Vector3(hit.position.x, hit.position.y + heightOffset, hit.position.z));
			}
		}

		if (!groundUnit && Vector3.Distance(unitNavAgent.destination, transform.position) <= unitNavAgent.stoppingDistance)
			SetNewAirWayoint();
	}

	void SetNewAirWayoint()
	{
		int randomWaypoint = Random.Range(0, WaypointController.instance.AirWaypoints.Length);
		unitNavAgent.SetDestination(WaypointController.instance.AirWaypoints[randomWaypoint].transform.position);
	}

	public void EnableMovement()
	{
		InvokeRepeating("SetNavDestination", 0f, 0.5f);
	}

	public void DisableMovement()
	{
		CancelInvoke("SetNavDestination");
		unitNavAgent.SetDestination(transform.position);
	}

	void SetNavDestination()
	{
		if (WaypointController.instance.SecondaryStructures.Count > 0)
		{
			float smallestDistance = 0f;

			foreach (Transform structureTransform in WaypointController.instance.SecondaryStructures)
			{
				if (structureTransform != null)
				{
					float structureDistance = Vector3.Distance(structureTransform.position, transform.position);

					if (smallestDistance == 0f || structureDistance < smallestDistance)
						targetTransform = structureTransform;
				}
			}
		}
		else if (WaypointController.instance.PrimaryStructure != null)
			targetTransform = WaypointController.instance.PrimaryStructure;

		if (targetTransform != null)
			unitNavAgent.SetDestination(targetTransform.position);
		else
			DisableMovement();
	}
}
