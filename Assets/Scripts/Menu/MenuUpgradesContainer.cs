using UnityEngine;
using System.Collections;

public class MenuUpgradesContainer : MonoBehaviour
{
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

	void OnEnable()
	{
		foreach(TowerInformation towerInfoHolder in towerInformation)
		{
			towerInfoHolder.SetInfo();
		}

		currentTowerArrayPos = 0;
		SetTowerInfo(0);
	}

	public void IncreaseTowerArrayPos()
	{
		currentTowerArrayPos++;

		if (currentTowerArrayPos >= towerMeshes.Length)
			currentTowerArrayPos = 0;

		SetTowerInfo(currentTowerArrayPos);
	}

	public void DecreaseTowerArrayPos()
	{
		currentTowerArrayPos--;

		if (currentTowerArrayPos < 0)
			currentTowerArrayPos = towerMeshes.Length - 1;

		SetTowerInfo(currentTowerArrayPos);
	}

	public void SetTowerInfo(int towerArrayPos)
	{
		towerNameLabel.text = towerInformation[towerArrayPos].TowerName;
		towerDescriptionLabel.text = towerInformation[towerArrayPos].TowerDescription;
		towerTierLabel.text = towerInformation[towerArrayPos].TowerTierString;
		towerDamageLabel.text = towerInformation[towerArrayPos].TowerDamage.ToString();
		towerRangeLabel.text = towerInformation[towerArrayPos].TowerRange.ToString();
		towerAttackSpeedLabel.text = towerInformation[towerArrayPos].TowerAttackSpeed.ToString() + " sec";
		towerUpgradeCostLabel.text = towerInformation[towerArrayPos].TowerUpgradeCost.ToString();

		for (int i = 0; i < towerMeshes.Length; i++)
		{
			if (i == towerArrayPos)
				towerMeshes[i].SetActive(true);
			else
				towerMeshes[i].SetActive(false);
		}
	}

	public void TriggerUpgrade()
	{
		Debug.Log("Upgrade Triggered");
	}
}

[System.Serializable]
public class TowerInformation
{
	[SerializeField] string towerName;
	[SerializeField] string towerDescription;
	[SerializeField] int[] towerDamage;
	[SerializeField] int[] towerRange;
	[SerializeField] float[] towerAttackSpeed;
	[SerializeField] int[] towerUpgradeCost;
	int towerTier;
	int arrayPos;
	public string TowerName { get { return towerName; }}
	public string TowerDescription { get { return towerDescription; }}
	public string TowerTierString { get { return TowerName + " Tier " + towerTier; }}
	public int TowerDamage { get { return towerDamage[arrayPos]; }}
	public int TowerRange { get { return towerRange[arrayPos]; }}
	public float TowerAttackSpeed { get { return towerAttackSpeed[arrayPos]; }}
	public int TowerUpgradeCost { get { return towerUpgradeCost[arrayPos]; }}

	public void SetInfo()
	{
		towerTier = EncryptedPlayerPrefs.GetInt(towerName + " Tier", 1);
		arrayPos = towerTier - 1;

		EncryptedPlayerPrefs.SetInt(towerName + " Damage", towerDamage[arrayPos]);
		EncryptedPlayerPrefs.SetInt(towerName + " Range", towerDamage[arrayPos]);
		EncryptedPlayerPrefs.SetInt(towerName + " Attack Speed", towerDamage[arrayPos]);
		EncryptedPlayerPrefs.SetInt(towerName + " Upgrade Cost", towerDamage[arrayPos]);
		PlayerPrefs.Save();
	}
}
