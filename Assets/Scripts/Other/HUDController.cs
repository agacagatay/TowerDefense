using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour
{
	[SerializeField] UISprite primaryStructureHealth;
	[SerializeField] UISprite secondaryStructureHealth;
	AllyStructureController primaryStructureController;
	float totalSecondaryHealth;

	public static HUDController instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		primaryStructureController = WaypointController.instance.PrimaryStructure.GetComponent<AllyStructureController>();

		foreach(Transform secondaryStructureTransform in WaypointController.instance.SecondaryStructures)
		{
			AllyStructureController secondaryStructureController = secondaryStructureTransform.GetComponent<AllyStructureController>();
			totalSecondaryHealth += secondaryStructureController.InitialStructureHealth * 1f;
		}
	}

	public void UpdateBaseDisplay()
	{
		StartCoroutine(WaitAndUpdateHUD());
	}

	IEnumerator WaitAndUpdateHUD()
	{
		yield return null;

		if (primaryStructureController != null)
			primaryStructureHealth.fillAmount = (primaryStructureController.StructureHealth * 1f) / primaryStructureController.InitialStructureHealth;
		else
			primaryStructureHealth.fillAmount = 0f;

		if (WaypointController.instance.SecondaryStructures.Count > 0)
		{
			float currentSecondaryHealth = 0f;

			foreach(Transform secondaryStructureTransform in WaypointController.instance.SecondaryStructures)
			{
				AllyStructureController secondaryStructureController = secondaryStructureTransform.GetComponent<AllyStructureController>();
				currentSecondaryHealth += secondaryStructureController.StructureHealth * 1f;
			}
				
			secondaryStructureHealth.fillAmount = currentSecondaryHealth / totalSecondaryHealth;
		}
		else
			secondaryStructureHealth.fillAmount = 0f;
	}
}
