using UnityEngine;
using System.Collections;

public class MenuIAP : MonoBehaviour
{
	[SerializeField] string productID;

	void OnClick()
	{
		AudioController.instance.PlayOneshot("SFX/Menu_Activate", AudioController.instance.gameObject);
		InAppPurchaseController.instance.BuyItem(productID);
	}
}
