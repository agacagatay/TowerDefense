using UnityEngine;
using System.Collections;

public class EnemyNavController : MonoBehaviour
{
	[SerializeField] NavMeshAgent unitNavAgent;
	[SerializeField] bool groundUnit = false;
	[SerializeField] bool followNavSlope = false;
	[SerializeField] float heightOffset;

	void Start()
	{
		if (groundUnit)
			unitNavAgent.SetDestination(WaypointController.instance.GroundWaypoint.transform.position);
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

		if (Input.GetKeyDown(KeyCode.I))
		{
			unitNavAgent.SetDestination(WaypointController.instance.GroundWaypoint.transform.position);
		}
	}

	void SetNewAirWayoint()
	{
		int randomWaypoint = Random.Range(0, WaypointController.instance.AirWaypoints.Length);
		unitNavAgent.SetDestination(WaypointController.instance.AirWaypoints[randomWaypoint].transform.position);
	}

	public void DisableMovement()
	{
		unitNavAgent.SetDestination(transform.position);
	}

	public void EnableMovement()
	{
		unitNavAgent.SetDestination(WaypointController.instance.GroundWaypoint.transform.position);
	}
}
