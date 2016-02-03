using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoadScene : MonoBehaviour
{
	[SerializeField] bool replayLevel;
	[SerializeField] GameObject[] objectsToActivate;
	[SerializeField] float waitBeforeSceneLoad;

	void OnClick()
	{
		Time.timeScale = 1f;
		StartCoroutine(WaitAndLoadMenu());
	}

	IEnumerator WaitAndLoadMenu()
	{
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
