using UnityEngine;
using System.Collections;

public class AudioControllerSpawner : MonoBehaviour
{
	[SerializeField] GameObject audioControllerPrefab;

	void Awake()
	{
		GameObject audioControllerObject = GameObject.FindGameObjectWithTag("AudioController");

		if (audioControllerObject == null)
		{
			GameObject musicControllerClone = (GameObject)Instantiate(audioControllerPrefab, transform.position, transform.rotation);
			musicControllerClone.name = "Audio Controller";
		}
	}
}
