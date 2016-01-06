using UnityEngine;
using System.Collections;

public class AllyDestroyTurret : MonoBehaviour
{
	AllyStructureController structureController;
	public AllyStructureController StructureController { get { return structureController; } set { structureController = value; }}

	void OnClick()
	{
		AllySpawnerController.instance.HideTurretSelectTab();
		StructureController.DamageStructure(1000);
	}
}
