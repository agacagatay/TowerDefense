using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InAppPurchaseController : MonoBehaviour
{
	// "mini_medal_pack";
	// "minor_medal_pack";
	// "major_medal_pack";
	// "grand_medal_pack";

	private static bool IsInitialized = false;
	public static InAppPurchaseController instance;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(this);

		if (!IsInitialized)
		{
			//You do not have to add products by code if you already did it in settings guide
			//Windows -> IOS Native -> Edit Settings
			//Billing tab.
			IOSInAppPurchaseManager.Instance.AddProductId("mini_medal_pack");
			IOSInAppPurchaseManager.Instance.AddProductId("minor_medal_pack");
			IOSInAppPurchaseManager.Instance.AddProductId("major_medal_pack");
			IOSInAppPurchaseManager.Instance.AddProductId("grand_medal_pack");

			//Event Use Examples
			IOSInAppPurchaseManager.OnVerificationComplete += HandleOnVerificationComplete;
			IOSInAppPurchaseManager.OnStoreKitInitComplete += OnStoreKitInitComplete;

			IOSInAppPurchaseManager.OnTransactionComplete += OnTransactionComplete;
			IOSInAppPurchaseManager.OnRestoreComplete += OnRestoreComplete;

			IsInitialized = true;
		} 

		IOSInAppPurchaseManager.Instance.LoadStore();
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public void BuyItem(string productId)
	{
		IOSInAppPurchaseManager.Instance.BuyProduct(productId);
	}

	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	static void UnlockProducts(string productIdentifier)
	{
		int totalMedals = EncryptedPlayerPrefs.GetInt("TotalMedals", 0);

		switch(productIdentifier)
		{
		case "mini_medal_pack":
			totalMedals += 3;
			break;
		case "minor_medal_pack":
			totalMedals += 8;
			break;
		case "major_medal_pack":
			totalMedals += 20;
			break;
		case "grand_medal_pack":
			totalMedals += 50;
			break;
		}

		EncryptedPlayerPrefs.SetInt("TotalMedals", totalMedals);
		EncryptedPlayerPrefs.SetInt("AdsDisabled", 1);
		PlayerPrefs.Save();

		MenuMedalsCounter.instance.UpdateMedalsCount();
	}

	static void OnTransactionComplete(IOSStoreKitResult result)
	{
		//Debug.Log("OnTransactionComplete: " + result.ProductIdentifier);
		//Debug.Log("OnTransactionComplete: state: " + result.State);

		switch(result.State)
		{
		case InAppPurchaseState.Purchased:
		case InAppPurchaseState.Restored:
			//Our product been succsesly purchased or restored
			//So we need to provide content to our user depends on productIdentifier
			UnlockProducts(result.ProductIdentifier);
			//IOSNativePopUpManager.showMessage("Transaction Complete", "Thank you for your purchase!");
			break;
		case InAppPurchaseState.Deferred:
			//iOS 8 introduces Ask to Buy, which lets parents approve any purchases initiated by children
			//You should update your UI to reflect this deferred state, and expect another Transaction Complete to be called again with a new transaction state 
			//reflecting the parent’s decision or after the transaction times out. Avoid blocking your UI or gameplay while waiting for the transaction to be updated.
			IOSNativePopUpManager.showMessage("Transaction Pending", "Awaiting authorization for transaction");
			break;
		case InAppPurchaseState.Failed:
			//Our purchase flow is failed.
			//We can unlock interface and repor user that the purchase is failed.
			//Debug.Log("Transaction failed with error, code: " + result.Error.Code);
			//Debug.Log("Transaction failed with error, description: " + result.Error.Description);
			IOSNativePopUpManager.showMessage("Transaction Failed", result.Error.Description);
			break;
		}

//		if (result.State == InAppPurchaseState.Failed)
//		{
//			IOSNativePopUpManager.showMessage("Transaction Failed", "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Description);
//		}
//		else
//		{
//			IOSNativePopUpManager.showMessage("Store Kit Response", "product " + result.ProductIdentifier + " state: " + result.State.ToString());
//		}
	}

	static void OnRestoreComplete(IOSStoreKitRestoreResult res)
	{
		if (res.IsSucceeded)
		{
			IOSNativePopUpManager.showMessage("Success", "Restore Completed");
		}
		else
		{
			IOSNativePopUpManager.showMessage("Error: " + res.Error.Code, res.Error.Description);
		}
	}

	static void HandleOnVerificationComplete(IOSStoreKitVerificationResponse response)
	{
		//IOSNativePopUpManager.showMessage("Verification", "Transaction verification status: " + response.status.ToString());
		//Debug.Log("ORIGINAL JSON: " + response.originalJSON);
	}

	static void OnStoreKitInitComplete(ISN_Result result)
	{
		if(result.IsSucceeded)
		{
			int avaliableProductsCount = 0;

			foreach(IOSProductTemplate tpl in IOSInAppPurchaseManager.instance.Products)
			{
				if(tpl.IsAvaliable)
				{
					avaliableProductsCount++;
				}
			}

			//IOSNativePopUpManager.showMessage("StoreKit Init Succeeded", "Available products count: " + avaliableProductsCount);
			//Debug.Log("StoreKit Init Succeeded Available products count: " + avaliableProductsCount);
		}
		else
		{
			//IOSNativePopUpManager.showMessage("StoreKit Init Failed",  "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Description);
		}
	}
}
