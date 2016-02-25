using UnityEngine;
using System.Collections;

public class LevelResume : MonoBehaviour
{
	[SerializeField] GameObject pauseMenu;

	void OnClick()
	{
		Time.timeScale = 1f;
		pauseMenu.SetActive(false);
		GameController.instance.GamePaused = false;
		EveryplayController.instance.ResumeRecording();
		AudioController.instance.PlayOneshot("SFX/Menu_Close", AudioController.instance.gameObject);
	}
}
