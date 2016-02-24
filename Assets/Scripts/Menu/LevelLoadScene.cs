using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class LevelLoadScene : MonoBehaviour
{
	[SerializeField] bool replayLevel;
	[SerializeField] GameObject[] objectsToActivate;
	[SerializeField] float waitBeforeSceneLoad;
	FMOD.Studio.Bus musicBus;
	FMOD.Studio.Bus sfxBus;

	void Start()
	{
		FMODUnity.RuntimeManager.StudioSystem.getBus("bus:/Music", out musicBus);
		FMODUnity.RuntimeManager.StudioSystem.getBus("bus:/SFX", out sfxBus);
	}

	void OnClick()
	{
		Time.timeScale = 1f;

		if (EncryptedPlayerPrefs.GetInt("AdsDisabled", 0) == 0)
		{
			float totalPlayTime = EncryptedPlayerPrefs.GetFloat("TotalPlayTime", 0f) + GameController.instance.SessionPlayTime;

			if (totalPlayTime >= 300f)
			{
				EncryptedPlayerPrefs.SetFloat("TotalPlayTime", 0f);
				musicBus.setPaused(true);
				AudioController.instance.Pause("SFX/Ambience", AudioController.instance.gameObject);

				if (Advertisement.isSupported && Advertisement.IsReady())
					Advertisement.Show();
			}
			else
			{
				EncryptedPlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);
			}

			PlayerPrefs.Save();
		}

		StartCoroutine(WaitAndLoadMenu());
	}

	IEnumerator WaitAndLoadMenu()
	{
		while (Advertisement.isShowing)
		{
			yield return null;
		}

		musicBus.setPaused(false);
		musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		AudioController.instance.Resume("SFX/Ambience", AudioController.instance.gameObject);
		AudioController.instance.Stop("SFX/Ambience", AudioController.instance.gameObject);

		foreach(GameObject objectToActivate in objectsToActivate)
		{
			objectToActivate.SetActive(true);
		}

		yield return new WaitForSeconds(waitBeforeSceneLoad);
		sfxBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);

		if (replayLevel)
			SceneManager.LoadSceneAsync(GameController.instance.LevelNumberInt);
		else
			SceneManager.LoadSceneAsync(0);
	}
}
