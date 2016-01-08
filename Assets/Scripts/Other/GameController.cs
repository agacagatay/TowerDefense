using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	// Game Score
	// - Mission Success (5,000)
	// - Secondary Structures Intact (1,000 each)
	// - Barrier Structures Intact (500 each)
	// - Enemies Destroyed (200 each)
	// - Turrets Spawned (100 each)

	bool isVictory = false;
	bool isDefeat = false;
	public bool IsVictory { get { return isVictory; }}
	public bool IsDefeat { get { return isDefeat; }}

	public static GameController instance;

	void Awake()
	{
		instance = this;
	}

	public void GameWin()
	{
		if (!isDefeat)
		{
			isVictory = true;
			Debug.Log("VICTORY");
		}
	}

	public void GameLose()
	{
		if (!isVictory)
		{
			isDefeat = true;
			Debug.Log("DEFEAT");
		}
	}
}
