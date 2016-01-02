using UnityEngine;
using System.Collections;

public class OrdinanceController : MonoBehaviour
{
	[SerializeField] bool allyOrdinance;
	[SerializeField] float travelSpeed;
	[SerializeField] float turnSpeed;
	[SerializeField] int damage;
	[SerializeField] bool splashDamage = false;
	[SerializeField] float splashDamageRange;
	Transform targetTransform;
	public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; }}

	void Start()
	{
		Destroy(gameObject, 4f);
	}

	void Update()
	{
		if (targetTransform != null)
		{
			Quaternion newRotation = Quaternion.LookRotation(targetTransform.position - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
		}

		transform.Translate(Vector3.forward * Time.deltaTime * travelSpeed);
	}

	void OnTriggerEnter(Collider other)
	{
		if (allyOrdinance && (other.gameObject.tag == "EnemyGround" || other.gameObject.tag == "EnemyAir"))
		{
			if (!splashDamage)
			{
				EnemyUnitVariables enemyUnitVariables = other.GetComponent<EnemyUnitVariables>();
				enemyUnitVariables.DamageUnit(damage);
			}
			else
			{
				Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashDamageRange);

				foreach (Collider hitCollider in hitColliders)
				{
					if ((other.gameObject.tag == "EnemyGround" || other.gameObject.tag == "EnemyAir"))
					{
						EnemyUnitVariables enemyUnitVariables = hitCollider.GetComponent<EnemyUnitVariables>();
						enemyUnitVariables.DamageUnit(damage);
					}
				}
			}
		}
		else if (!allyOrdinance && other.gameObject.tag == "Ally")
		{
			if (!splashDamage)
			{
				AllyStructureController allyStructureController = other.GetComponent<AllyStructureController>();
				allyStructureController.DamageStructure(damage);
			}
			else
			{
				Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashDamageRange);

				foreach (Collider hitCollider in hitColliders)
				{
					if (hitCollider.gameObject.tag == "Ally")
					{
						AllyStructureController allyStructureController = hitCollider.GetComponent<AllyStructureController>();
						allyStructureController.DamageStructure(damage);
					}
				}
			}
		}

		if (other.gameObject.tag != "Turret" && other.gameObject.tag != "Ordinance" && other.gameObject.tag != "Ignore")
			Destroy(gameObject);
	}
}
