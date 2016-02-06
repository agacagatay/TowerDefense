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

		objectToEnable.SetActive(true);
	}
}
