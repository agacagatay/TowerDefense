using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class LevelLoadScene : MonoBehaviour
{
	[SerializeField] bool replayLevel;
	[SerializeField] GameObject[] objectsToActivate;
	[SerializeField] float waitBeforeSceneLoad;

	void OnClick()
	{
		Time.timeScale = 1f;

		if (EncryptedPlayerPrefs.GetInt("AdsDisabled", 0) == 0)
		{
			float totalPlayTime = EncryptedPlayerPrefs.GetFloat("TotalPlayTime", 0f) + GameController.instance.SessionPlayTime;

			if (totalPlayTime >= 300f)
			{
				EncryptedPlayerPrefs.SetFloat("TotalPlayTime", 0f);

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

		foreach(GameObject objectToActivate in objectsToActivate)
		{
			objectToActivate.SetActive(true);
		}

		yield return new WaitForSeconds(waitBeforeSceneLoad);

		if (replayLevel)
			SceneManager.LoadSceneAsync(GameController.instance.LevelNumberInt);
		else
			SceneManager.LoadSceneAsync(0);
	}
}
