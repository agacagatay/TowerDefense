using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour
{
	[SerializeField] BoostContainer[] boostContainers;
	int enabledBoost;
	bool canActivate = true;
	float boostCooldown;
	public int EnabledBoost { get { return enabledBoost; } set { enabledBoost = value; }}
	public bool CanActivate { get { return canActivate; } set { canActivate = value; }}
	public float BoostCooldown { get { return boostCooldown; }}

	public static BoostController instance;

	void Awake()
	{
		instance = this;
	}

	public void ActivateBoost()
	{
		boostCooldown = boostContainers[enabledBoost].BoostCooldown;

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
