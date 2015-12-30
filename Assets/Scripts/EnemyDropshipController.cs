using UnityEngine;
using System.Collections;

public class EnemyDropshipController : MonoBehaviour
{
	[SerializeField] NavMeshAgent unitNavAgent;
	[SerializeField] GameObject dropUnitPrefab;
	[SerializeField] Transform unitSpawnTransform;
	bool bugOutTriggered = false;

	void Start()
	{
		SetNewAirWayoint();
	}

	void Update()
	{
		if (!bugOutTriggered && Vector3.Distance(unitNavAgent.destination, transform.position) <= 
			unitNavAgent.stoppingDistance)
		{
			DeployUnits();
			unitNavAgent.SetDestination(WaypointController.instance.BugOutWaypoint.position);
			bugOutTriggered = true;
		}
		else if (bugOutTriggered && Vector3.Distance(unitNavAgent.destination, transform.position) <= 
			unitNavAgent.stoppingDistance)
		{
			Destroy(gameObject);
		}
	}
	
	void SetNewAirWayoint()
	{
		int randomWaypoint = Random.Range(0, WaypointController.instance.AirWaypoints.Length);
		unitNavAgent.SetDestination(WaypointController.instance.AirWaypoints[randomWaypoint].transform.position);
	}

	void DeployUnits()
	{
		Instantiate(dropUnitPrefab, unitSpawnTransform.position, unitSpawnTransform.rotation);
	}
}
