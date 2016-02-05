using UnityEngine;
using System.Collections;

public class ResetPlayerPrefs : MonoBehaviour
{

	void Start()
	{
		PlayerPrefs.DeleteAll();
		Debug.Log("Player Prefs Reset");
	}
}
