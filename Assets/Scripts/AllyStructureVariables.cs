using UnityEngine;
using System.Collections;

public class AllyStructureVariables : MonoBehaviour
{
	[SerializeField] string allyStructureType;
	[SerializeField] int structureHealth = 100;

	void Start()
	{
		SpawnedAllyDictionary.instance.spawnedAllyDictionary.Add(gameObject, allyStructureType);
	}

	public void DamageStructure(int damageValue)
	{
		structureHealth -= damageValue;

		if (structureHealth <= 0)
			Destroy(gameObject);
	}
}
