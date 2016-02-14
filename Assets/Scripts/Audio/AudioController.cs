using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour
{
	[SerializeField] GameObject audioInstancePrefab;
	AudioInstance audioInstance;
	public Dictionary<string, AudioInstance> audioInstances = new Dictionary<string, AudioInstance>();

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
		audioInstance.AssignEventInstance(anchorObject, eventName, "event:/" + eventName);
		audioInstance.Play();

		audioInstances.Add(audioInstanceClone.name, audioInstance);
	}

	public void PlayOneshot(GameObject anchorObject, string eventName)
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/" + eventName, anchorObject.transform.position);
	}

	public void Pause(string eventName)
	{
		AudioController.instance.audioInstances.TryGetValue(eventName, out audioInstance);

		if (audioInstance != null)
			audioInstance.Pause();
	}

	public void Resume(string eventName)
	{
		AudioController.instance.audioInstances.TryGetValue(eventName, out audioInstance);

		if (audioInstance != null)
			audioInstance.Resume();
	}

	public void SetVolume(string eventName, float volume)
	{
		AudioController.instance.audioInstances.TryGetValue(eventName, out audioInstance);

		if (audioInstance != null)
			audioInstance.SetVolume(volume);
	}

	public void SetParameter(string eventName, string paramName, float value)
	{
		AudioController.instance.audioInstances.TryGetValue(eventName, out audioInstance);

		if (audioInstance != null)
			audioInstance.SetParameterValue(paramName, value);
	}

	public void Stop(string eventName)
	{
		AudioController.instance.audioInstances.TryGetValue(eventName, out audioInstance);

		if (audioInstance != null)
			audioInstance.Stop();
	}
}