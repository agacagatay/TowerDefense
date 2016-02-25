using UnityEngine;
using System.Collections;

public class MenuButtonClose : MonoBehaviour
{
	[SerializeField] GameObject objectToDisable;

	void OnClick()
	{
		if (objectToDisable != null)
		{
			AudioController.instance.PlayOneshot("SFX/Menu_Close", AudioController.instance.gameObject);
			objectToDisable.SetActive(false);
		}
	}
}
