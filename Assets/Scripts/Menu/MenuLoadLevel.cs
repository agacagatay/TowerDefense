using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuLoadLevel : MonoBehaviour
{
	[SerializeField] GameObject[] objectsToActivate;
	[SerializeField] float waitBeforeSceneLoad;

	public void LoadLevel(int levelToLoad)
	{
		AudioController.instance.PlayOneshot("SFX/Menu_Activate", AudioController.instance.gameObject);
		AudioController.instance.PlayOneshot("SFX/Menu_Trigger_Spawn", AudioController.instance.gameObject);
		AudioController.instance.Stop("Music/Music_Main_Menu", AudioController.instance.gameObject);
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
