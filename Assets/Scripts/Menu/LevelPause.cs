using UnityEngine;
using System.Collections;

public class LevelPause : MonoBehaviour
{
	[SerializeField] GameObject pauseMenu;

	void OnClick()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0f;
	}
}
