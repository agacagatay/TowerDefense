using UnityEngine;
using System.Collections;

public class AllyDestroyTurret : MonoBehaviour
{
	AllyStructureController structureController;
	public AllyStructureController StructureController { get { return structureController; } set { structureController = value; }}

	void OnClick()
	{
		StructureController.SetSelectTurretAlpha(0f);
		StructureController.DamageStructure(1000);
	}
}
