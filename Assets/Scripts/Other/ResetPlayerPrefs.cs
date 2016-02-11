using UnityEngine;
using System.Collections;

public class ResetPlayerPrefs : MonoBehaviour
{
	void Start()
	{
		PlayerPrefs.DeleteAll();
		GameCenterManager.ResetAchievements();
		Debug.Log("Player Prefs Reset");
	}
}
