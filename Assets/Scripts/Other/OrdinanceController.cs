using UnityEngine;
using System.Collections;

public class OrdinanceController : MonoBehaviour
{
	[SerializeField] bool allyOrdinance;
	[SerializeField] bool isMissile;
	[SerializeField] GameObject missileVFXPrefab;
	[SerializeField] float travelSpeed;
	[SerializeField] float turnSpeed;
	[SerializeField] int damage;
	[SerializeField] bool splashDamage = false;
	[SerializeField] float splashDamageRange;
	[SerializeField] GameObject explosionPrefab;
	int overrideDamage = 0;
	Transform targetTransform;
	bool overdriveActive = false;
	GameObject missileVFXObject;
	public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; }}
	public int OverrideDamage { get { return overrideDamage; } set { overrideDamage = value; }}
	public bool OverdriveActive { get { return overdriveActive; } set { overdriveActive = value; }}

	void Start()
	{
		Destroy(gameObject, 4f);

		if (isMissile && !GameController.instance.GameOver)
		{
			AudioController.instance.CreateInstance("SFX/Ally_Ordinance_Missile_Booster", gameObject);
			AudioController.instance.Play("SFX/Ally_Ordinance_Missile_Booster", gameObject);
			missileVFXObject = (GameObject)Instantiate(missileVFXPrefab, transform.position, transform.rotation);
		}
	}

	void Update()
	{
		if (targetTransform != null)
		{
			Quaternion newRotation = Quaternion.LookRotation(targetTransform.position - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
		}

		transform.Translate(Vector3.forward * Time.deltaTime * travelSpeed);

		if (isMissile)
		{
			missileVFXObject.transform.position = transform.position;
			missileVFXObject.transform.rotation = transform.rotation;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!GameController.instance.GameOver)
		{
			if (allyOrdinance && (other.gameObject.tag == "EnemyGround" || other.gameObject.tag == "EnemyAir"))
			{
				if (OverrideDamage != 0)
					damage = OverrideDamage;

				if (!splashDamage)
				{
					EnemyUnitVariables enemyUnitVariables = other.GetComponent<EnemyUnitVariables>();

					if (OverdriveActive)
						enemyUnitVariables.DamageUnit(damage * 2);
					else
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

							if (enemyUnitVariables != null)
							{
								if (OverdriveActive)
									enemyUnitVariables.DamageUnit(damage * 2);
								else
									enemyUnitVariables.DamageUnit(damage);
							}
						}
					}
				}
			}
			else if (!allyOrdinance && (other.gameObject.tag == "Ally" || other.gameObject.tag == "PrimaryStructure"))
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

							if (allyStructureController != null)
								allyStructureController.DamageStructure(damage);
						}
					}
				}
			}
		}

		if (other.gameObject.tag != "Turret" && other.gameObject.tag != "Ordinance" && other.gameObject.tag != "SpawnPosition" && other.gameObject.tag != "Ignore")
		{
			if (allyOrdinance && other.gameObject.tag != "Ally")
			{
				if (isMissile)
				{
					MissileVFXDestroy missileVFXDestroy = missileVFXObject.GetComponent<MissileVFXDestroy>();
					missileVFXDestroy.DisableEmmision();
				}

				if (explosionPrefab != null)
				{
					if (explosionPrefab.name == "Explosion Missile")
						AudioController.instance.Stop("SFX/Ally_Ordinance_Missile_Booster", gameObject);

					if (!GameController.instance.GameOver)
					{
						if (explosionPrefab.name == "Explosion Bullet Big")
							AudioController.instance.PlayOneshot("SFX/Ally_Ordinance_Big", gameObject);
						if (explosionPrefab.name == "Explosion Missile")
							AudioController.instance.PlayOneshot("SFX/Ally_Ordinance_Missile_Explosion", gameObject);
					}

					Instantiate(explosionPrefab, transform.position, transform.rotation);
				}

				Destroy(gameObject);
			}
			else if (!allyOrdinance && other.gameObject.tag != "EnemyGround" && other.gameObject.tag != "EnemyAir")
			{
				if (explosionPrefab != null)
				{
					if (explosionPrefab.name == "Explosion Bullet Big")
						AudioController.instance.PlayOneshot("SFX/Ally_Ordinance_Big", gameObject);

					Instantiate(explosionPrefab, transform.position, transform.rotation);
				}

				Destroy(gameObject);
			}
		}
	}
}
