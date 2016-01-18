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

		if (unitNavAgent.enabled)
			 unitNavAgent.SetDestination(transform.position);
	}

	void SetNavDestination()
	{
		if (targetTransform == null)
		{
			if (WaypointController.instance.SecondaryStructures.Count > 0)
			{
				float smallestDistance = 0f;

				foreach (Transform structureTransform in WaypointController.instance.SecondaryStructures)
				{
					if (structureTransform != null)
					{
						float structureDistance = CalculatePathLength(structureTransform.position);

						if (smallestDistance == 0f || structureDistance < smallestDistance)
						{
							smallestDistance = structureDistance;
							targetTransform = structureTransform;
						}
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

	float CalculatePathLength(Vector3 targetPosition)
	{
		NavMeshPath path = new NavMeshPath();

		if (unitNavAgent.enabled)
			NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);

		Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
		allWayPoints[0] = transform.position;
		allWayPoints[allWayPoints.Length - 1] = targetPosition;

		for(int i = 0; i < path.corners.Length; i++)
		{
			allWayPoints[i + 1] = path.corners[i];
		}

		float pathLength = 0;

		for(int i = 0; i < allWayPoints.Length - 1; i++)
		{
			pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
		}

		return pathLength;
	}
}
