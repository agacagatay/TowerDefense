using UnityEngine;
using System.Collections;

public class AllyStructureVariables : MonoBehaviour
{
	[SerializeField] string allyStructureType;
	[SerializeField] int structureHealth = 100;
	[SerializeField] bool isTurret = false;
	GameObject turretSpawnObject;
	public GameObject TurretSpawnObject { get { return turretSpawnObject; } set { turretSpawnObject = value; }}

	void Start()
	{
		SpawnedAllyDictionary.instance.spawnedAllyDictionary.Add(gameObject, allyStructureType);
	}

	public void DamageStructure(int damageValue)
	{
		structureHealth -= damageValue;

		if (structureHealth <= 0)
		{
			if (isTurret)
				TurretSpawnObject.SetActive(true);

			Destroy(gameObject);
		}
	}
}
