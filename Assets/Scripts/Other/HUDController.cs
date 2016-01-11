using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour
{
	[SerializeField] UISprite primaryStructureHealth;
	[SerializeField] UISprite secondaryStructureHealth;
	[SerializeField] int turretQuota;
	[SerializeField] UILabel turretText;
	[SerializeField] int missileBatteryQuota;
	[SerializeField] UILabel missileBatteryText;
	[SerializeField] UILabel announcementLabel;
	[SerializeField] UILabel noticePrimaryLabel;
	[SerializeField] UILabel noticeSecondaryLabel;
	[SerializeField] TweenAlpha tweenAlpha;
	[SerializeField] UISprite turretOverdriveTimer;
	AllyStructureController primaryStructureController;
	float totalSecondaryHealth;
	int maxTurrets;
	int maxMissileBatteries;
	public int TurretQuota { get { return turretQuota; } set { turretQuota = value; }}
	public int MissileBatteryQuota { get { return missileBatteryQuota; } set { missileBatteryQuota = value; }}

	public static HUDController instance;

	void Awake()
	{
		instance = this;

		maxTurrets = turretQuota;
		maxMissileBatteries = missileBatteryQuota;

		UpdateResources();
	}

	void Start()
	{
		primaryStructureController = WaypointController.instance.PrimaryStructure.GetComponent<AllyStructureController>();

		foreach(Transform secondaryStructureTransform in WaypointController.instance.SecondaryStructures)
		{
			AllyStructureController secondaryStructureController = secondaryStructureTransform.GetComponent<AllyStructureController>();
			totalSecondaryHealth += secondaryStructureController.InitialStructureHealth * 1f;
		}
	}

	public void UpdateBaseDisplay()
	{
		StartCoroutine(WaitAndUpdateHUD());
	}

	IEnumerator WaitAndUpdateHUD()
	{
		yield return null;

		if (primaryStructureController != null)
			primaryStructureHealth.fillAmount = (primaryStructureController.StructureHealth * 1f) / primaryStructureController.InitialStructureHealth;
		else
			primaryStructureHealth.fillAmount = 0f;

		if (WaypointController.instance.SecondaryStructures.Count > 0)
		{
			float currentSecondaryHealth = 0f;

			foreach(Transform secondaryStructureTransform in WaypointController.instance.SecondaryStructures)
			{
				AllyStructureController secondaryStructureController = secondaryStructureTransform.GetComponent<AllyStructureController>();
				currentSecondaryHealth += secondaryStructureController.StructureHealth * 1f;
			}
				
			secondaryStructureHealth.fillAmount = currentSecondaryHealth / totalSecondaryHealth;
		}
		else
			secondaryStructureHealth.fillAmount = 0f;
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

	public void DisplayOneString(string noticeMessage, float holdTime, float fadeTime)
	{
		tweenAlpha.ResetToBeginning();
		announcementLabel.text = noticeMessage;
		tweenAlpha.delay = holdTime;
		tweenAlpha.duration = fadeTime;
		noticePrimaryLabel.gameObject.SetActive(false);
		noticeSecondaryLabel.gameObject.SetActive(false);
		tweenAlpha.gameObject.SetActive(true);
		tweenAlpha.PlayForward();
		StartCoroutine(WaitAndReset(holdTime + fadeTime));
	}

	public void DisplayTwoString(string primaryMessage, string secondaryMessage, float holdTime, float fadeTime)
	{
		tweenAlpha.ResetToBeginning();
		noticePrimaryLabel.text = primaryMessage;
		noticeSecondaryLabel.text = secondaryMessage;
		tweenAlpha.delay = holdTime;
		tweenAlpha.duration = fadeTime;
		announcementLabel.gameObject.SetActive(false);
		tweenAlpha.gameObject.SetActive(true);
		tweenAlpha.PlayForward();
		StartCoroutine(WaitAndReset(holdTime + fadeTime));
	}

	IEnumerator WaitAndReset(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		tweenAlpha.gameObject.SetActive(false);
	}

	public void ToggleTurretTimer(float activationTime)
	{
		turretOverdriveTimer.fillAmount = 1f;
		turretOverdriveTimer.gameObject.SetActive(true);
		StartCoroutine(StartTimer(activationTime));
	}

	IEnumerator StartTimer(float activationTime)
	{
		float initialTime = activationTime;
		float activeTime = activationTime;

		while (activeTime > 0f)
		{
			activeTime -= Time.deltaTime;
			turretOverdriveTimer.fillAmount = activeTime / initialTime;
			yield return null;
		}

		turretOverdriveTimer.gameObject.SetActive(false);
	}
}
