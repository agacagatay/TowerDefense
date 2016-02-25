using UnityEngine;
using System.Collections;

public class LevelPause : MonoBehaviour
{
	[SerializeField] GameObject pauseMenu;

	void OnClick()
	{
		AudioController.instance.PlayOneshot("SFX/Menu_Open", AudioController.instance.gameObject);
		EveryplayController.instance.PauseRecording();
		GameController.instance.GamePaused = true;
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}
}
