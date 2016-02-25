using UnityEngine;
using System.Collections;

public class MenuVibrationToggle : MonoBehaviour
{
	[SerializeField] UILabel buttonText;
	[SerializeField] GameObject enabledCheckbox;
	[SerializeField] GameObject disabledCheckbox;

	void Start()
	{
		int vibrationMode = EncryptedPlayerPrefs.GetInt("VibrationMode", 1);

		if (vibrationMode == 1)
		{
			SetStateEnabled(false);
		}
		else
		{
			SetStateDisabled();
		}
	}

	public void ToggleVibration()
	{
		int vibrationMode = EncryptedPlayerPrefs.GetInt("VibrationMode", 1);

		if (vibrationMode == 1)
		{
			AudioController.instance.PlayOneshot("SFX/Menu_Close", AudioController.instance.gameObject);
			SetStateDisabled();
		}
		else
		{
			AudioController.instance.PlayOneshot("SFX/Menu_Activate", AudioController.instance.gameObject);
			SetStateEnabled(true);
		}
	}

	void SetStateEnabled(bool vibrate)
	{
		EncryptedPlayerPrefs.SetInt("VibrationMode", 1);
		buttonText.text = "Enabled";
		enabledCheckbox.SetActive(true);
		disabledCheckbox.SetActive(false);
		PlayerPrefs.Save();

		if (vibrate)
			Handheld.Vibrate();
	}

	void SetStateDisabled()
	{
		EncryptedPlayerPrefs.SetInt("VibrationMode", 0);
		buttonText.text = "Disabled";
		enabledCheckbox.SetActive(false);
		disabledCheckbox.SetActive(true);
		PlayerPrefs.Save();
	}
}
