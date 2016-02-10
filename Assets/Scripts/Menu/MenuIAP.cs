using UnityEngine;
using System.Collections;

public class MenuIAP : MonoBehaviour
{
	[SerializeField] string productID;

	void OnClick()
	{
		InAppPurchaseController.instance.BuyItem(productID);
	}
}
