using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	[SerializeField] int levelNumberInt;
	[SerializeField] string levelNumberString;
	[SerializeField] GameObject levelResultsObject;
	[SerializeField] UILabel levelVictoryScoreLabel;
	[SerializeField] UILabel secondaryStructuresScoreLabel;
	[SerializeField] UILabel barrierStructuresScoreLabel;
	[SerializeField] UILabel enemiesDestroyedScoreLabel;
	[SerializeField] UILabel towersSpawnedScoreLabel;
	[SerializeField] UILabel totalScoreLabel;
	[SerializeField] GameObject newHighScoreObject;
	[SerializeField] UISprite[] medalsObjects;
	[SerializeField] UILabel medalsEarnedLabel;
	GameObject primaryStructure;
	bool isVictory = false;
	bool isDefeat = false;
	bool gameOver = false;
	int enemiesKilled;
	int turretsSpawned;
	int initialSecondary;
	public int LevelNumberInt { get { return levelNumberInt; }}
	public bool IsVictory { get { return isVictory; }}
	public bool IsDefeat { get { return isDefeat; }}
	public bool GameOver { get { return gameOver; }}
	public int EnemiesKilled { get { return enemiesKilled; } set { enemiesKilled = value; }}
	public int TurretsSpawned { get { return turretsSpawned; } set { turretsSpawned = value; }}
	public GameObject PrimaryStructure { get { return primaryStructure; } set { primaryStructure = value; }}
	public List<GameObject> SecondaryStructures = new List<GameObject>();
	public List<GameObject> BarrierStructures = new List<GameObject>();

	public static GameController instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		initialSecondary = SecondaryStructures.Count;
	}

	public void GameWin()
	{
		if (!IsVictory && !IsDefeat)
		{
			HUDController.instance.DisplayOneString("VICTORY!", 3f, 2f);
			gameOver = true;
			isVictory = true;
			CalculateScore();
		}
	}

	public void GameLose()
	{
		if (!IsVictory && !IsDefeat)
		{
			HUDController.instance.DisplayOneString("DEFEAT", 3f, 2f);
			gameOver = true;
			isDefeat = true;
			CalculateScore();
		}

	}

	void CalculateScore()
	{
		int primaryStructureScore = 0;

		if (IsVictory)
		{
			primaryStructureScore = 20000;
			EncryptedPlayerPrefs.SetInt("MissionStatus" + levelNumberString, 2);

			if (levelNumberInt < 9)
				EncryptedPlayerPrefs.SetInt("MissionStatus0" + (levelNumberInt + 1).ToString(), 1);
			else
				EncryptedPlayerPrefs.SetInt("MissionStatus" + (levelNumberInt + 1).ToString(), 1);
		}

		levelVictoryScoreLabel.text = primaryStructureScore.ToString("N0");

		int secondaryStructureScore = SecondaryStructures.Count * 1000;
		secondaryStructuresScoreLabel.text = secondaryStructureScore.ToString("N0");

		int barrierStructureScore = BarrierStructures.Count * 500;
		barrierStructuresScoreLabel.text = barrierStructureScore.ToString("N0");

		int enemiesKilledScore = EnemiesKilled * 100;
		enemiesDestroyedScoreLabel.text = enemiesKilledScore.ToString("N0");

		int towersSpawnedScore = TurretsSpawned * 200;
		towersSpawnedScoreLabel.text = towersSpawnedScore.ToString("N0");

		int finalScore = primaryStructureScore + secondaryStructureScore + barrierStructureScore + enemiesKilledScore + towersSpawnedScore;
		totalScoreLabel.text = finalScore.ToString("N0");

		if (finalScore > EncryptedPlayerPrefs.GetInt("HighScore" + levelNumberString, 0))
		{
			EncryptedPlayerPrefs.SetInt("HighScore" + levelNumberString, finalScore);
			newHighScoreObject.SetActive(true);
		}

		int earnedMedals;

		if (SecondaryStructures.Count >= (initialSecondary * 0.75f))
		{
			earnedMedals = 3;
			medalsEarnedLabel.text = "Medals Earned - 3";
			medalsObjects[0].color = Color.HSVToRGB(0f, 0f, 1f);
			medalsObjects[1].color = Color.HSVToRGB(0f, 0f, 1f);
			medalsObjects[2].color = Color.HSVToRGB(0f, 0f, 1f);
		}
		else if (SecondaryStructures.Count >= (initialSecondary * 0.5f))
		{
			earnedMedals = 2;
			medalsEarnedLabel.text = "Medals Earned - 2";
			medalsObjects[0].color = Color.HSVToRGB(0f, 0f, 1f);
			medalsObjects[1].color = Color.HSVToRGB(0f, 0f, 1f);
			medalsObjects[2].color = Color.HSVToRGB(0f, 0f, 0.25f);
		}
		else if (IsVictory)
		{
			earnedMedals = 1;
			medalsEarnedLabel.text = "Medals Earned - 1";
			medalsObjects[0].color = Color.HSVToRGB(0f, 0f, 1f);
			medalsObjects[1].color = Color.HSVToRGB(0f, 0f, 0.25f);
			medalsObjects[2].color = Color.HSVToRGB(0f, 0f, 0.25f);
		}
		else
		{
			earnedMedals = 0;
			medalsEarnedLabel.text = "Medals Earned - 0";
			medalsObjects[0].color = Color.HSVToRGB(0f, 0f, 0.25f);
			medalsObjects[1].color = Color.HSVToRGB(0f, 0f, 0.25f);
			medalsObjects[2].color = Color.HSVToRGB(0f, 0f, 0.25f);
		}

		int previousEarnedMedals = EncryptedPlayerPrefs.GetInt("MedalsEarned" + levelNumberString, 0);

		if (earnedMedals > previousEarnedMedals)
		{
			EncryptedPlayerPrefs.SetInt("MedalsEarned" + levelNumberString, earnedMedals);
			EncryptedPlayerPrefs.SetInt("TotalMedals", EncryptedPlayerPrefs.GetInt("TotalMedals", 0) + (earnedMedals - previousEarnedMedals));
		}

		PlayerPrefs.Save();
		levelResultsObject.SetActive(true);
	}
}
