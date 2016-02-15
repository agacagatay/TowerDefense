using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
	[SerializeField] GameObject audioInstancePrefab;
	AudioInstance[] objectAudioInstances;

	public static AudioController instance;

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void Play(string eventName, GameObject anchorObject)
	{
		GameObject audioInstanceClone = (GameObject)Instantiate(audioInstancePrefab, anchorObject.transform.position, 
			anchorObject.transform.rotation);

		audioInstanceClone.transform.parent = anchorObject.transform;
		audioInstanceClone.name = eventName;

		AudioInstance audioInstance = audioInstanceClone.GetComponent<AudioInstance>();
		audioInstance.AssignEventInstance("event:/" + eventName);
		audioInstance.Play();
	}

	public void PlayOneshot(GameObject anchorObject, string eventName)
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/" + eventName, anchorObject.transform.position);
	}

	public void Pause(string eventName, GameObject anchorObject)
	{
		PopulateObjectAudioInstances(anchorObject);

		foreach(AudioInstance objectAudioInstance in objectAudioInstances)
		{
			if (objectAudioInstance.gameObject.name == eventName)
				objectAudioInstance.Pause();
		}
	}

	public void Resume(string eventName, GameObject anchorObject)
	{
		PopulateObjectAudioInstances(anchorObject);

		foreach(AudioInstance objectAudioInstance in objectAudioInstances)
		{
			if (objectAudioInstance.gameObject.name == eventName)
				objectAudioInstance.Resume();
		}
	}

	public void SetVolume(string eventName, GameObject anchorObject, float volume)
	{
		PopulateObjectAudioInstances(anchorObject);

		foreach(AudioInstance objectAudioInstance in objectAudioInstances)
		{
			if (objectAudioInstance.gameObject.name == eventName)
				objectAudioInstance.SetVolume(volume);
		}
	}

	public void SetParameter(string eventName, GameObject anchorObject, string paramName, float value)
	{
		PopulateObjectAudioInstances(anchorObject);

		foreach(AudioInstance objectAudioInstance in objectAudioInstances)
		{
			if (objectAudioInstance.gameObject.name == eventName)
				objectAudioInstance.SetParameter(paramName, value);
		}
	}

	public void Stop(string eventName, GameObject anchorObject)
	{
		PopulateObjectAudioInstances(anchorObject);

		foreach(AudioInstance objectAudioInstance in objectAudioInstances)
		{
			if (objectAudioInstance.gameObject.name == eventName)
				objectAudioInstance.Stop();
		}
	}

	public void StopAllMusic()
	{
		FMOD.Studio.Bus musicBus;
		FMODUnity.RuntimeManager.StudioSystem.getBus("bus:/Music", out musicBus);
		musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	public void StopAllSFX()
	{
		FMOD.Studio.Bus sfxBus;
		FMODUnity.RuntimeManager.StudioSystem.getBus("bus:/SFX", out sfxBus);
		sfxBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	void PopulateObjectAudioInstances(GameObject anchorObject)
	{
		objectAudioInstances = anchorObject.GetComponentsInChildren<AudioInstance>();
	}
}