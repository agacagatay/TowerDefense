using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour
{
	[SerializeField] GameObject audioInstancePrefab;
	public List<AudioInstance> audioInstances = new List<AudioInstance>();

	public static AudioController instance;

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void Play(GameObject anchorObject, string eventName)
	{
		Vector3 vectorZero = new Vector3(0f, 0f, 0f);
		Quaternion quaternionZero = Quaternion.Euler(0f, 0f, 0f);

		GameObject audioInstanceClone = (GameObject)Instantiate(audioInstancePrefab, vectorZero, quaternionZero);
		audioInstanceClone.transform.parent = transform;
		audioInstanceClone.name = eventName;

		AudioInstance audioInstance = audioInstanceClone.GetComponent<AudioInstance>();
		audioInstance.AssignEventInstance(anchorObject, "event:/" + eventName);
		audioInstance.Play();

		audioInstances.Add(audioInstance);
	}

	public void PlayOneshot(GameObject anchorObject, string eventName)
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/" + eventName, anchorObject.transform.position);
	}
}