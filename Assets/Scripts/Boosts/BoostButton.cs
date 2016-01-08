using UnityEngine;
using System.Collections;

public class BoostButton : MonoBehaviour
{
	[SerializeField] string boostName;
	[SerializeField] UITexture boostIcon;
	[SerializeField] UITexture boostDarkIcon;

	void OnClick()
	{
		if (BoostController.instance.CanActivate && SpawnedEnemyDictionary.instance.spawnedEnemyDictionary.Count > 0)
		{
			switch (boostName)
			{
			case "Precision Strike":
				BoostController.instance.EnabledBoost = 0;
				break;
			case "Turret Overdrive":
				BoostController.instance.EnabledBoost = 1;
				break;
			case "Structure Repair":
				BoostController.instance.EnabledBoost = 2;
				break;
			default:
				Debug.LogError("No valid boost specified");
				break;
			}
				
			StartCoroutine(ActivateBoost());
		}
	}

	IEnumerator ActivateBoost()
	{
		BoostController.instance.ActivateBoost();
		BoostController.instance.CanActivate = false;
		boostIcon.enabled = false;
		boostDarkIcon.fillAmount = 0f;
		float cooldownTime = BoostController.instance.BoostCooldown;

		while (boostDarkIcon.fillAmount < 1f)
		{
			boostDarkIcon.fillAmount += Time.deltaTime / cooldownTime;

			if (boostDarkIcon.fillAmount >= 1f)
			{
				boostDarkIcon.fillAmount = 1f;
				boostIcon.enabled = true;
			}

			yield return null;
		}

		BoostController.instance.CanActivate = true;
	}
}
