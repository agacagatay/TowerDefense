using UnityEngine;
using System.Collections;

public class MenuButtonOpen : MonoBehaviour
{
	[SerializeField] GameObject objectToDisable;

	void OnClick()
	{
		objectToDisable.SetActive(true);
	}
}
