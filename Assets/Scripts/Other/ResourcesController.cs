using UnityEngine;
using System.Collections;

public class ResourcesController : MonoBehaviour
{
	[SerializeField] int turretQuota;
	[SerializeField] UILabel turretText;
	[SerializeField] int missileBatteryQuota;
	[SerializeField] UILabel missileBatteryText;
	int maxTurrets;
	int maxMissileBatteries;
	public int TurretQuota { get { return turretQuota; } set { turretQuota = value; }}
	public int MissileBatteryQuota { get { return missileBatteryQuota; } set { missileBatteryQuota = value; }}

	public static ResourcesController instance;

	void Awake()
	{
		instance = this;

		maxTurrets = turretQuota;
		maxMissileBatteries = missileBatteryQuota;

		UpdateResources();
	}

	public void UpdateResources()
	{
		if (TurretQuota > maxTurrets)
			TurretQuota = maxTurrets;

		if (MissileBatteryQuota > maxMissileBatteries)
			MissileBatteryQuota = maxMissileBatteries;

		turretText.text = turretQuota.ToString("N0");
		missileBatteryText.text = missileBatteryQuota.ToString("N0");
	}
}
