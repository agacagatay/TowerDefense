using UnityEngine;
using System.Collections;

public class MenuShowEveryplayModal : MonoBehaviour
{
	void OnClick()
	{
		EveryplayController.instance.ShowSharingModal();
	}
}
