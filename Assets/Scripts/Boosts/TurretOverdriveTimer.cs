using UnityEngine;
using System.Collections;

public class TurretOverdriveTimer : MonoBehaviour
{
	[SerializeField] UISprite timerSprite;

	public void ToggleTurretTimer(float activationTime)
	{
		timerSprite.fillAmount = 1f;
		timerSprite.gameObject.SetActive(true);
		StartCoroutine(StartTimer(activationTime));
	}

	IEnumerator StartTimer(float activationTime)
	{
		float initialTime = activationTime;
		float activeTime = activationTime;

		while (activeTime > 0f)
		{
			activeTime -= Time.deltaTime;
			timerSprite.fillAmount = activeTime / initialTime;
			yield return null;
		}

		timerSprite.gameObject.SetActive(false);
	}
}
