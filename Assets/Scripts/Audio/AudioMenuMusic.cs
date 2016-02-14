using UnityEngine;
using System.Collections;

public class AudioMenuMusic : MonoBehaviour
{
	void Start()
	{
		AudioController.instance.Play(gameObject, "Music/Music_Main_Menu");
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
			StopMenuMusic();
	}

	public void StopMenuMusic()
	{
		AudioInstance audioInstance = AudioController.instance.audioInstances.Find(item => item.name == "Music/Music_Main_Menu");
		audioInstance.Stop(false);
	}
}
