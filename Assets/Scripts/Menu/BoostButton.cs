using UnityEngine;
using System.Collections;

public class BoostButton : MonoBehaviour
{
	[SerializeField] UITexture boostIcon;
	[SerializeField] UITexture boostDarkIcon;

	void OnClick()
	{
		if (BoostController.instance.CanActivate)
			StartCoroutine(ActivateBoost(BoostController.instance.BoostCooldown));
	}

	IEnumerator ActivateBoost(float cooldownTime)
	{
		BoostController.instance.ActivateBoost();
		BoostController.instance.CanActivate = false;
		boostIcon.enabled = false;
		boostDarkIcon.fillAmount = 0f;

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
