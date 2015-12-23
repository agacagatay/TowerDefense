using UnityEngine;
using System.Collections;

public class EnemyNavController : MonoBehaviour
{
	[SerializeField] NavMeshAgent unitNavAgent;

	void Start()
	{
		unitNavAgent.destination = TravelDestinationLocator.instance.transform.position;
	}
}
