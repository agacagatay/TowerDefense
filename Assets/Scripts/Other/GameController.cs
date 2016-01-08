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

	bool isVictory = false;
	bool isDefeat = false;
	int enemiesKilled;
	int turretsSpawned;
	public bool IsVictory { get { return isVictory; }}
	public bool IsDefeat { get { return isDefeat; }}
	public int EnemiesKilled { get { return enemiesKilled; } set { enemiesKilled = value; }}
	public int TurretsSpawned { get { return turretsSpawned; } set { turretsSpawned = value; }}
	public List<GameObject> secondaryStructures = new List<GameObject>();
	public List<GameObject> barrierStructures = new List<GameObject>();

	public static GameController instance;

	void Awake()
	{
		instance = this;
	}

	public void GameWin()
	{
		if (!isDefeat)
		{
			Debug.Log("VICTORY");
			isVictory = true;
			CalculateScore();
		}
	}

	public void GameLose()
	{
		if (!isVictory)
		{
			Debug.Log("DEFEAT");
			isDefeat = true;
			CalculateScore();
		}

	}

	void CalculateScore()
	{
		int primaryStructureScore = 0;

		if (IsVictory)
			primaryStructureScore = 5000;

		int secondaryStructureScore = secondaryStructures.Count * 1000;
		int barrierStructureScore = barrierStructures.Count * 500;
		int enemiesKilledScore = EnemiesKilled * 200;
		int turretsSpawnedScore = TurretsSpawned * 100;

		int finalScore = primaryStructureScore + secondaryStructureScore + barrierStructureScore + enemiesKilledScore + turretsSpawnedScore;
		Debug.Log("Final Score: " + finalScore.ToString("N0"));
	}
}
