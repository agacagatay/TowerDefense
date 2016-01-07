using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour
{
	bool canActivate = true;
	float boostCooldown = 4f;
	public bool CanActivate { get { return canActivate; } set { canActivate = value; }}
	public  float BoostCooldown { get { return boostCooldown; }}

	public static BoostController instance;

	// Boost Ideas (Allow Boost Upgrades):
	// - Percision Strike (High damage guided missiles)
	// - EMP Detonation (Temporarily disables enemy units)
	// - Destroyer Support (Temporary High-Altitude Air Support)
	// - Repair Kit (Repair damaged barriers/structures)
	// - Overdrive (For a limited time, turrets fire faster and ordinance deals increased damage)

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
