﻿using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour
{
	[SerializeField] BoostContainer[] boostContainers;
	int enabledBoost;
	float boostCooldown;
	bool turretOverdriveEnabled = false;
	public int EnabledBoost { get { return enabledBoost; } set { enabledBoost = value; }}
	public float BoostCooldown { get { return boostCooldown; }}
	public bool TurretOverdriveEnabled { get { return turretOverdriveEnabled; } set { turretOverdriveEnabled = value; }}

	public static BoostController instance;

	void Awake()
	{
		instance = this;
	}

	public void ActivateBoost()
	{
		if (!boostContainers[EnabledBoost].BoostActive)
		{
			boostCooldown = boostContainers[EnabledBoost].BoostCooldown;

			Instantiate(boostContainers[EnabledBoost].BoostPrefab, boostContainers[EnabledBoost].BoostSpawnTransform.position,
				boostContainers[EnabledBoost].BoostSpawnTransform.rotation);

			if (EncryptedPlayerPrefs.GetInt("VibrationMode", 1) == 1)
				Handheld.Vibrate();

			GameCenterManager.SubmitAchievement(100f, "achievement_activate_boost", true);
		}
	}

	public void ToggleBoostActivation(int selectedBoost, bool boostActiveValue)
	{
		boostContainers[selectedBoost].BoostActive = boostActiveValue;
	}

	public bool ReturnBoostActivation(int selectedBoost)
	{
		return boostContainers[selectedBoost].BoostActive;
	}
}

[System.Serializable]
public class BoostContainer
{
	[SerializeField] string boostName;
	[SerializeField] float boostCooldown;
	[SerializeField] GameObject boostPrefab;
	[SerializeField] Transform boostSpawnTransform;
	bool boostActive = false;
	public float BoostCooldown { get { return boostCooldown; }}
	public GameObject BoostPrefab { get { return boostPrefab; }}
	public Transform BoostSpawnTransform { get { return boostSpawnTransform; }}
	public bool BoostActive { get { return boostActive; } set { boostActive = value; }}
}
