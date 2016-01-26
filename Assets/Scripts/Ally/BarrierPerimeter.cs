using UnityEngine;
using System.Collections;

public class BarrierPerimeter : MonoBehaviour
{
	[SerializeField] AllyStructureController allyStructureController;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "EnemyGround")
		{
			EnemyNavController enemyNavController = other.GetComponent<EnemyNavController>();
			enemyNavController.DisableMovement();
		}
	}
}
