using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuLoadLevel : MonoBehaviour
{
	[SerializeField] GameObject[] objectsToActivate;
	[SerializeField] float waitBeforeSceneLoad;
	[SerializeField] int levelToLoad;

	void OnClick()
	{
		StartCoroutine(WaitAndLoadScene());
	}

	IEnumerator WaitAndLoadScene()
	{
		foreach(GameObject objectToActivate in objectsToActivate)
		{
			objectToActivate.SetActive(true);
		}

		yield return new WaitForSeconds(waitBeforeSceneLoad);

		SceneManager.LoadSceneAsync(levelToLoad);
	}
}
