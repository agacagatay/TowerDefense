using UnityEngine;
using System.Collections;

public class LevelPause : MonoBehaviour
{
	[SerializeField] GameObject pauseMenu;

	void OnClick()
	{
		EveryplayController.instance.PauseRecording();
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}
}
