using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour
{
	[SerializeField] UISprite primaryStructureHealth;
	[SerializeField] UISprite secondaryStructureHealth;
	[SerializeField] UILabel artillaryText;
	[SerializeField] UILabel minigunText;
	[SerializeField] UILabel turretText;
	[SerializeField] UILabel missileBatteryText;
	[SerializeField] UILabel announcementLabel;
	[SerializeField] UILabel noticePrimaryLabel;
	[SerializeField] UILabel noticeSecondaryLabel;
	[SerializeField] TweenAlpha tweenAlpha;
	[SerializeField] UISprite turretOverdriveTimer;
	[SerializeField] UILabel waveCounterLabel;
	AllyStructureController primaryStructureController;
	float totalSecondaryHealth;
	float remainingTime;
	public UILabel WaveCounterLabel { get { return waveCounterLabel; }}

	public static HUDController instance;

	void Awake()
	{
		instance = this;

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
		artillaryText.text = AllySpawnerController.instance.ArtillaryQuota.ToString("N0");
		minigunText.text = AllySpawnerController.instance.MinigunQuota.ToString("N0");
		turretText.text = AllySpawnerController.instance.TurretQuota.ToString("N0");
		missileBatteryText.text = AllySpawnerController.instance.MissileBatteryQuota.ToString("N0");
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

	public void ToggleEnemySpawnTimer(float timeUntilSpawn)
	{
		remainingTime = timeUntilSpawn;
		waveCounterLabel.text = "Next Wave In " + FormatTime(remainingTime);

		InvokeRepeating("ReduceTime", 1f, 1f);
	}

	void ReduceTime()
	{
		--remainingTime;

		if (remainingTime <= 0f)
		{
			CancelInvoke("ReduceTime");
			waveCounterLabel.text = " ";
		}
		else
			waveCounterLabel.text = "Next Wave In " + FormatTime(remainingTime);
	}

	string FormatTime(float timeToFormat)
	{
		int minutes = Mathf.FloorToInt(timeToFormat / 60f);
		int seconds = Mathf.FloorToInt(timeToFormat - minutes * 60f);
		string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		return formattedTime;
	}

	public void SetEnemyWaveLabel(string waveString)
	{
		waveCounterLabel.text = waveString;
	}
}
