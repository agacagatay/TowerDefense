using UnityEngine;
using System.Collections;

public class EnemyNavController : MonoBehaviour
{
	[SerializeField] NavMeshAgent unitNavAgent;
	[SerializeField] bool followNavSlope = false;
	[SerializeField] float heightOffset;

	void Start()
	{
		unitNavAgent.destination = TravelDestinationLocator.instance.transform.position;
	}

	void Update()
	{
		if (followNavSlope)
		{
			NavMeshHit hit;
			
			if (NavMesh.SamplePosition(transform.position + transform.forward, out hit, 1f, 1))
			{
				transform.LookAt(new Vector3(hit.position.x, hit.position.y + heightOffset, hit.position.z));
			}
		}
	}
}
