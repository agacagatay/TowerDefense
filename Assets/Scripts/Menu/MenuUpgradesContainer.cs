using UnityEngine;
using System.Collections;

public class MenuUpgradesContainer : MonoBehaviour
{
	[SerializeField] MenuButtonToggle storeMenuToggle;
	[SerializeField] GameObject upgradeButton;
	[SerializeField] UILabel towerNameLabel;
	[SerializeField] UILabel towerTierLabel;
	[SerializeField] UILabel towerDamageLabel;
	[SerializeField] UILabel towerRangeLabel;
	[SerializeField] UILabel towerAttackSpeedLabel;
	[SerializeField] UILabel towerUpgradeCostLabel;
	[SerializeField] UILabel towerDescriptionLabel;
	[SerializeField] GameObject[] towerMeshes;
	[SerializeField] TowerInformation[] towerInformation;
	int currentTowerArrayPos;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			int totalMedals = EncryptedPlayerPrefs.GetInt("TotalMedals", 0);
			EncryptedPlayerPrefs.SetInt("TotalMedals", totalMedals + 1);
			MenuMedalsCounter.instance.UpdateMedalsCount();
			PlayerPrefs.Save();
		}
	}

	void Start()
	{
		ToggleUpgradeMenu();
	}

	public void ToggleUpgradeMenu()
	{
		foreach(TowerInformation towerInfoHolder in towerInformation)
		{
			towerInfoHolder.SetInfo();
		}

		currentTowerArrayPos = 0;
		SetTowerInfo();
	}

	public void IncreaseTowerArrayPos()
	{
		currentTowerArrayPos++;

		if (currentTowerArrayPos >= towerMeshes.Length)
			currentTowerArrayPos = 0;

		SetTowerInfo();
	}

	public void DecreaseTowerArrayPos()
	{
		currentTowerArrayPos--;

		if (currentTowerArrayPos < 0)
			currentTowerArrayPos = towerMeshes.Length - 1;

		SetTowerInfo();
	}

	public void SetTowerInfo()
	{
		towerNameLabel.text = towerInformation[currentTowerArrayPos].TowerName;
		towerDescriptionLabel.text = towerInformation[currentTowerArrayPos].TowerDescription;
		towerTierLabel.text = towerInformation[currentTowerArrayPos].TowerTierString;
		towerDamageLabel.text = towerInformation[currentTowerArrayPos].TowerDamage.ToString();
		towerRangeLabel.text = towerInformation[currentTowerArrayPos].TowerRange.ToString();
		towerAttackSpeedLabel.text = towerInformation[currentTowerArrayPos].TowerAttackSpeed.ToString() + " sec";

		if (towerInformation[currentTowerArrayPos].TowerTier < towerInformation[currentTowerArrayPos].TowerUpgradeLength)
			towerUpgradeCostLabel.text = towerInformation[currentTowerArrayPos].TowerUpgradeCost.ToString();
		else
			towerUpgradeCostLabel.text = "--";

		for (int i = 0; i < towerMeshes.Length; i++)
		{
			if (i == currentTowerArrayPos)
				towerMeshes[i].SetActive(true);
			else
				towerMeshes[i].SetActive(false);
		}

		if (towerInformation[currentTowerArrayPos].TowerTier < towerInformation[currentTowerArrayPos].TowerUpgradeLength)
			upgradeButton.SetActive(true);
		else
			upgradeButton.SetActive(false);
	}

	public void TriggerUpgrade()
	{
		int totalMedals = EncryptedPlayerPrefs.GetInt("TotalMedals", 0);
		string towerName = towerInformation[currentTowerArrayPos].TowerName;
		int currentTowerTier = EncryptedPlayerPrefs.GetInt(towerName + " Tier", 1);

		if (totalMedals >= towerInformation[currentTowerArrayPos].TowerUpgradeCost && currentTowerTier < towerInformation[currentTowerArrayPos].TowerUpgradeLength)
		{
			GameCenterManager.SubmitAchievement(100f, "achievement_tower_upgrade", true);

			totalMedals -= towerInformation[currentTowerArrayPos].TowerUpgradeCost;
			EncryptedPlayerPrefs.SetInt("TotalMedals", totalMedals);
			MenuMedalsCounter.instance.UpdateMedalsCount();

			EncryptedPlayerPrefs.SetInt(towerName + " Tier", currentTowerTier + 1);
			bool allTowersUpgraded = true;
			bool towersFullyUpgraded = true;

			foreach(TowerInformation towerInfoHolder in towerInformation)
			{
				towerInfoHolder.SetInfo();

				if (towerInfoHolder.TowerTier == 1)
				{
					allTowersUpgraded = false;
					towersFullyUpgraded = false;
				}
				else if (towerInfoHolder.TowerTier < towerInformation[currentTowerArrayPos].TowerUpgradeLength)
				{
					towersFullyUpgraded = false;
				}
			}

			if (allTowersUpgraded)
				GameCenterManager.SubmitAchievement(100f, "achievement_upgrade_all_towers", true);
			else if (towersFullyUpgraded)
				GameCenterManager.SubmitAchievement(100f, "achievement_fully_upgrade_towers", true);

			SetTowerInfo();
		}
		else if (currentTowerTier < towerInformation[currentTowerArrayPos].TowerUpgradeLength)
		{
			storeMenuToggle.ToggleMenu();
		}
			
	}
}

[System.Serializable]
public class TowerInformation
{
	[SerializeField] string towerName;
	[SerializeField] string towerDescription;
	[SerializeField] int[] towerDamage;
	[SerializeField] float[] towerRange;
	[SerializeField] float[] towerAttackSpeed;
	[SerializeField] int[] towerUpgradeCost;
	int towerTier;
	int arrayPos;
	public string TowerName { get { return towerName; }}
	public string TowerDescription { get { return towerDescription; }}
	public int TowerTier { get { return towerTier; }}
	public string TowerTierString { get { return TowerName + " Tier " + towerTier + "/" + TowerUpgradeLength.ToString(); }}
	public int TowerDamage { get { return towerDamage[arrayPos]; }}
	public float TowerRange { get { return towerRange[arrayPos]; }}
	public float TowerAttackSpeed { get { return towerAttackSpeed[arrayPos]; }}
	public int TowerUpgradeCost { get { return towerUpgradeCost[arrayPos]; }}
	public int TowerUpgradeLength { get { return towerUpgradeCost.Length; }}

	public void SetInfo()
	{
		towerTier = EncryptedPlayerPrefs.GetInt(towerName + " Tier", 1);
		arrayPos = towerTier - 1;

		EncryptedPlayerPrefs.SetInt(towerName + " Damage", towerDamage[arrayPos]);
		EncryptedPlayerPrefs.SetFloat(towerName + " Range", towerRange[arrayPos]);
		EncryptedPlayerPrefs.SetFloat(towerName + " Attack Speed", towerAttackSpeed[arrayPos]);

		if (towerTier < TowerUpgradeLength)
			EncryptedPlayerPrefs.SetInt(towerName + " Upgrade Cost", towerUpgradeCost[arrayPos]);
		
		PlayerPrefs.Save();
	}
}
