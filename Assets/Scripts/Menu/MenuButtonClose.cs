using UnityEngine;
using System.Collections;

public class MenuButtonClose : MonoBehaviour
{
	[SerializeField] GameObject objectToDisable;

	void OnClick()
	{
		objectToDisable.SetActive(false);
	}
}
