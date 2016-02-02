using UnityEngine;
using System.Collections;

public class MenuLevelSelectContainer : MonoBehaviour
{
	[SerializeField] string levelNumberString;
	[SerializeField] int levelNumberInt;
	[SerializeField] UISprite levelContainerSprite;
	[SerializeField] UILabel levelNameLabel;
	[SerializeField] UILabel highScoreLabel;
	[SerializeField] UILabel missionStatusLabel;
	[SerializeField] GameObject[] medalIcons;
	[SerializeField] MenuLoadLevel menuLoadLevel;

	void Awake()
	{
		if (EncryptedPlayerPrefs.GetInt("MissionStatus01", 0) == 0)
		{
			EncryptedPlayerPrefs.SetInt("MissionStatus01", 1);
			PlayerPrefs.Save();
		}

		int missionStatus = EncryptedPlayerPrefs.GetInt("MissionStatus" + levelNumberString, 0);

		if (missionStatus == 0)
		{
			levelContainerSprite.alpha = 0.4f;
			missionStatusLabel.text = "Mission Status: [FF0000]Locked";
		}
		else if (missionStatus == 1)
		{
			levelContainerSprite.alpha = 1f;
			missionStatusLabel.text = "Mission Status: [00FF00]Active";
		}
		else if (missionStatus == 2)
		{
			levelContainerSprite.alpha = 1f;
			missionStatusLabel.text = "Mission Status: Completed";
		}

		levelNameLabel.text = "LEVEL " + levelNumberString;

		int highScore = EncryptedPlayerPrefs.GetInt("HighScore" + levelNumberString, 0);
		highScoreLabel.text = "High Score: " + highScore.ToString("N0");

		int awardedStars = EncryptedPlayerPrefs.GetInt("MedalsEarned" + levelNumberString, 0);

		for (int i = 0; i < medalIcons.Length; i++)
		{
			UISprite starSprite = medalIcons[i].GetComponent<UISprite>();

			if (i < awardedStars)
				starSprite.color = Color.white;
			else
				starSprite.color = Color.HSVToRGB(0f, 0f, 0.3f);
		}
	}

	public void LoadLevel()
	{
		menuLoadLevel.LoadLevel(levelNumberInt);
	}
}
