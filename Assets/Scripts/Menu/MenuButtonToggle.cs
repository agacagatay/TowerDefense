using UnityEngine;
using System.Collections;

public class MenuButtonToggle : MonoBehaviour
{
	[SerializeField] GameObject objectToEnable;
	[SerializeField] GameObject[] objectsToDisable;

	void OnClick()
	{
		ToggleMenu();
	}

	public void ToggleMenu()
	{
		foreach (GameObject objectToDisable in objectsToDisable)
		{
			objectToDisable.SetActive(false);
		}

		if (objectToEnable != null)
		{
			AudioController.instance.PlayOneshot("SFX/Menu_Open", AudioController.instance.gameObject);
			objectToEnable.SetActive(true);
		}
	}
}
