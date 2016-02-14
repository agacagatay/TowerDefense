﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuLoadLevel : MonoBehaviour
{
	[SerializeField] GameObject[] objectsToActivate;
	[SerializeField] float waitBeforeSceneLoad;

	public void LoadLevel(int levelToLoad)
	{
//		AudioInstance audioInstance = AudioController.instance.GetAudioInstance("Music/Music_Main_Menu");
//
//		if (audioInstance != null)
//			audioInstance.Stop();

		AudioController.instance.Stop("Music/Music_Main_Menu");
		
		StartCoroutine(WaitAndLoadLevel(levelToLoad));
	}

	IEnumerator WaitAndLoadLevel(int levelToLoad)
	{
		foreach(GameObject objectToActivate in objectsToActivate)
		{
			objectToActivate.SetActive(true);
		}

		yield return new WaitForSeconds(waitBeforeSceneLoad);

		SceneManager.LoadSceneAsync(levelToLoad);
	}
}
