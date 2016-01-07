using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour
{
	bool canActivate = true;
	float boostCooldown = 4f;
	public bool CanActivate { get { return canActivate; } set { canActivate = value; }}
	public  float BoostCooldown { get { return boostCooldown; }}

	public static BoostController instance;

	void Awake()
	{
		instance = this;
	}

	public void ActivateBoost()
	{
		if (canActivate)
			Debug.Log("Boost Activated");
	}
}
