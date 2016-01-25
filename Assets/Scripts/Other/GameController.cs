using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	// Game Score
	// - Mission Success (5,000)
	// - Secondary Structures Intact (1,000 each)
	// - Barrier Structures Intact (500 each)
	// - Enemies Killed (200 each)
	// - Turrets Spawned (100 each)

	GameObject primaryStructure;
	bool isVictory = false;
	bool isDefeat = false;
	bool gameOver = false;
	int enemiesKilled;
	int turretsSpawned;
	int initialSecondary;
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
			primaryStructureScore = 5000;

		int secondaryStructureScore = SecondaryStructures.Count * 1000;
		int barrierStructureScore = BarrierStructures.Count * 500;
		int enemiesKilledScore = EnemiesKilled * 200;
		int turretsSpawnedScore = TurretsSpawned * 100;

		int finalScore = primaryStructureScore + secondaryStructureScore + barrierStructureScore + enemiesKilledScore + turretsSpawnedScore;
		Debug.Log("Score: " + finalScore);

		if (SecondaryStructures.Count >= (initialSecondary * 0.75f))
			Debug.Log("Score Rating: 3 Stars");
		else if (SecondaryStructures.Count >= (initialSecondary * 0.5f))
			Debug.Log("Score Rating: 2 Stars");
		else if (IsVictory)
			Debug.Log("Score Rating: 1 Star");
		else
			Debug.Log("Score Rating: 0 Stars");
	}
}
