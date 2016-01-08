using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour
{
	// Boost Ideas (Allow Boost Upgrades):
	// [DONE] - Precision Strike (High damage guided missiles)
	// - EMP Blast (Temporarily disables enemy units)
	// - Destroyer Support (Temporary High-Altitude Air Support)
	// - Combat Engineers (Repair damaged barriers/structures)
	// - Systems Overdrive (For a limited time, turrets fire faster and ordinance deals increased damage)

	[SerializeField] BoostContainer[] boostContainers;
	int enabledBoost;
	bool canActivate = true;
	float boostCooldown = 4f;
	public bool CanActivate { get { return canActivate; } set { canActivate = value; }}
	public float BoostCooldown { get { return boostCooldown; }}

	public static BoostController instance;

	void Awake()
	{
		instance = this;
		enabledBoost = 0;
		boostCooldown = boostContainers[enabledBoost].BoostCooldown;
	}

	public void ActivateBoost()
	{
		if (canActivate)
			Instantiate(boostContainers[enabledBoost].BoostPrefab, boostContainers[enabledBoost].BoostSpawnTransform.position,
				boostContainers[enabledBoost].BoostSpawnTransform.rotation);
	}
}

[System.Serializable]
public class BoostContainer
{
	[SerializeField] string boostName;
	[SerializeField] float boostCooldown;
	[SerializeField] GameObject boostPrefab;
	[SerializeField] Transform boostSpawnTransform;
	public float BoostCooldown { get { return boostCooldown; }}
	public GameObject BoostPrefab { get { return boostPrefab; }}
	public Transform BoostSpawnTransform { get { return boostSpawnTransform; }}
}
