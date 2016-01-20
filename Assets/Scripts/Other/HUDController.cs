using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour
{
	[SerializeField] UISprite primaryStructureHealth;
	[SerializeField] UISprite secondaryStructureHealth;
	[SerializeField] int artillaryQuota;
	[SerializeField] UILabel artillaryText;
	[SerializeField] int minigunQuota;
	[SerializeField] UILabel minigunText;
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
	int maxArtillary;
	int maxMinigun;
	int maxTurrets;
	int maxMissileBatteries;
	public int ArtillaryQuota { get { return artillaryQuota; } set { artillaryQuota = value; }}
	public int MinigunQuota { get { return minigunQuota; } set { minigunQuota = value; }}
	public int TurretQuota { get { return turretQuota; } set { turretQuota = value; }}
	public int MissileBatteryQuota { get { return missileBatteryQuota; } set { missileBatteryQuota = value; }}

	public static HUDController instance;

	void Awake()
	{
		instance = this;

		maxArtillary = ArtillaryQuota;
		maxMinigun = MinigunQuota;
		maxTurrets = TurretQuota;
		maxMissileBatteries = MissileBatteryQuota;

		UpdateResources();
	}

	void Start()
	{
		primaryStructureController = GameController.instance.PrimaryStructure.GetComponent<AllyStructureController>();

		foreach(GameObject secondaryStructureObject in GameController.instance.SecondaryStructures)
		{
			AllyStructureController secondaryStructureController = secondaryStructureObject.GetComponent<AllyStructureController>();
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

		if (GameController.instance.SecondaryStructures.Count > 0)
		{
			float currentSecondaryHealth = 0f;

			foreach(GameObject secondaryStructureObject in GameController.instance.SecondaryStructures)
			{
				AllyStructureController secondaryStructureController = secondaryStructureObject.GetComponent<AllyStructureController>();
				currentSecondaryHealth += secondaryStructureController.StructureHealth * 1f;
			}
				
			secondaryStructureHealth.fillAmount = currentSecondaryHealth / totalSecondaryHealth;
		}
		else
			secondaryStructureHealth.fillAmount = 0f;
	}

	public void UpdateResources()
	{
		if (ArtillaryQuota > maxArtillary)
			ArtillaryQuota = maxArtillary;

		if (MinigunQuota > maxMinigun)
			MinigunQuota = maxMinigun;

		if (TurretQuota > maxTurrets)
			TurretQuota = maxTurrets;

		if (MissileBatteryQuota > maxMissileBatteries)
			MissileBatteryQuota = maxMissileBatteries;

		artillaryText.text = ArtillaryQuota.ToString("N0");
		minigunText.text = MinigunQuota.ToString("N0");
		turretText.text = TurretQuota.ToString("N0");
		missileBatteryText.text = MissileBatteryQuota.ToString("N0");
	}

	public void DisplayOneString(string noticeMessage, float holdTime, float fadeTime)
	{
		tweenAlpha.ResetToBeginning();
		announcementLabel.text = noticeMessage;
		tweenAlpha.delay = holdTime;
		tweenAlpha.duration = fadeTime;
		announcementLabel.gameObject.SetActive(true);
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
		noticePrimaryLabel.gameObject.SetActive(true);
		noticeSecondaryLabel.gameObject.SetActive(true);
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
