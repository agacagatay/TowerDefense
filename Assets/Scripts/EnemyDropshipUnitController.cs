using UnityEngine;
using System.Collections;

public class EnemyDropshipUnitController : MonoBehaviour
{
	[SerializeField] float activationDelayMin;
	[SerializeField] float activationDelayMax;
	[SerializeField] Rigidbody unitRigidbody;
	[SerializeField] NavMeshAgent unitNavAgent;
	[SerializeField] EnemyNavController unitNavController;
	[SerializeField] EnemyTurretController unitTurretController;

	void Start()
	{
		StartCoroutine(InitializeActivation());
	}
	
	IEnumerator InitializeActivation()
	{
		float randomWait = Random.Range(activationDelayMin, activationDelayMax);
		yield return new WaitForSeconds(randomWait);

		Destroy(unitRigidbody);
		unitNavAgent.enabled = true;
		unitNavController.enabled = true;
		unitTurretController.enabled = true;
	}
}
