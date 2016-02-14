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

	public void Pause(string eventName)
	{
		AudioInstance audioInstance = AudioController.instance.audioInstances.Find(item => item.name == eventName);
		audioInstance.Pause();
	}

	public void Resume(string eventName)
	{
		AudioInstance audioInstance = AudioController.instance.audioInstances.Find(item => item.name == eventName);
		audioInstance.Resume();
	}

	public void SetVolume(string eventName, float volume)
	{
		AudioInstance audioInstance = AudioController.instance.audioInstances.Find(item => item.name == eventName);
		audioInstance.SetVolume(volume);
	}

	public void SetVolume(string eventName, string paramName, float value)
	{
		AudioInstance audioInstance = AudioController.instance.audioInstances.Find(item => item.name == eventName);
		audioInstance.SetParameterValue(paramName, value);
	}

	public void Stop(string eventName)
	{
		AudioInstance audioInstance = AudioController.instance.audioInstances.Find(item => item.name == eventName);
		audioInstance.Stop();
	}

	public AudioInstance GetAudioInstance(string instanceName)
	{
		AudioInstance audioInstance = AudioController.instance.audioInstances.Find(item => item.name == instanceName);
		return audioInstance;
	}
}