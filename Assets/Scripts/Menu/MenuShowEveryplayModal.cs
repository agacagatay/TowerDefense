using UnityEngine;
using System.Collections;

public class MenuShowEveryplayModal : MonoBehaviour
{
	void OnClick()
	{
		AudioController.instance.PlayOneshot("SFX/Menu_Open", AudioController.instance.gameObject);
		EveryplayController.instance.ShowSharingModal();
	}
}
