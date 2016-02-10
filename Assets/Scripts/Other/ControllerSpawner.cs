using UnityEngine;
using System.Collections;

public class ControllerSpawner : MonoBehaviour
{
	[SerializeField] GameObject gameCenterControllerPrefab;
	[SerializeField] GameObject inAppPurchaseControllerPrefab;

	void Awake()
	{
		GameObject gameCenterControllerObject = GameObject.FindGameObjectWithTag("GameCenterController");
		GameObject inAppPurchaseControllerObject = GameObject.FindGameObjectWithTag("InAppPurchaseController");

		if (gameCenterControllerObject == null)
			Instantiate(gameCenterControllerPrefab, transform.position, transform.rotation);

		if (inAppPurchaseControllerObject == null)
			Instantiate(inAppPurchaseControllerPrefab, transform.position, transform.rotation);
	}
}
