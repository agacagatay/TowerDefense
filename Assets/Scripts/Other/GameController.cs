﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
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
