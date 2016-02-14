using UnityEngine;
using System.Collections;

public class AudioControllerSpawner : MonoBehaviour
{
	[SerializeField] GameObject musicControllerPrefab;

	void Awake()
	{
		GameObject musicControllerObject = GameObject.FindGameObjectWithTag("MusicController");

		if (musicControllerObject == null)
		{
			GameObject musicControllerClone = (GameObject)Instantiate(musicControllerPrefab, transform.position, transform.rotation);
			musicControllerClone.name = "Audio Controller";
		}
	}
}
