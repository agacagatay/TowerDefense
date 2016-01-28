using UnityEngine;
using System.Collections;

public class BoostButton : MonoBehaviour
{
	[SerializeField] string boostName;
	[SerializeField] UISprite boostIcon;
	[SerializeField] UISprite boostDarkIcon;
	[SerializeField] UISprite overlaySprite;

	void OnClick()
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

		if (!BoostController.instance.ReturnBoostActivation(BoostController.instance.EnabledBoost) && !GameController.instance.GameOver)
		{
			StartCoroutine(ActivateBoost());
		}
	}

	IEnumerator ActivateBoost()
	{
		int toggledBoost = BoostController.instance.EnabledBoost;

		BoostController.instance.ActivateBoost();
		BoostController.instance.ToggleBoostActivation(toggledBoost, true);

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

			overlaySprite.fillAmount = boostDarkIcon.fillAmount;
			yield return null;
		}

		BoostController.instance.ToggleBoostActivation(toggledBoost, false);
	}
}
