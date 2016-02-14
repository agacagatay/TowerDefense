using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioInstance : MonoBehaviour
{
	GameObject positionAnchor;
	FMOD.Studio.EventInstance eventInstance;
	FMOD.Studio.EventDescription eventDescription;
	FMOD.Studio.ParameterInstance parameterInstance;
	bool instanceAssigned = false;
	bool is3D;

//	void Start()
//	{
//		DontDestroyOnLoad(gameObject);
//	}

	public void AssignEventInstance(GameObject anchorObject, string eventPath)
	{
		positionAnchor = anchorObject;
		eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventPath);
		eventInstance.getDescription(out eventDescription);
		eventDescription.is3D(out is3D);
		instanceAssigned = true;
	}

	public void SetParameterValue(string paramName, float value)
	{
		eventInstance.getParameter(paramName, out parameterInstance);
		parameterInstance.setValue(value);
	}

	public void Play()
	{
		if (instanceAssigned)
			eventInstance.start();
		else
			Debug.LogError("Event Instance Never Assigned");
	}

	public void Pause()
	{
		if (instanceAssigned)
		{
			bool isPaused;
			eventInstance.getPaused(out isPaused);

			if (!isPaused)
				eventInstance.setPaused(false);
		}
		else
			Debug.LogError("Event Instance Never Assigned");
	}

	public void Resume()
	{
		if (instanceAssigned)
		{
			bool isPaused;
			eventInstance.getPaused(out isPaused);

			if (isPaused)
				eventInstance.setPaused(false);
		}
		else
			Debug.LogError("Event Instance Never Assigned");
	}

	public void SetVolume(float volume)
	{
		if (instanceAssigned)
		{
			eventInstance.setVolume(volume);
		}
		else
			Debug.LogError("Event Instance Never Assigned");
	}

	public void Stop(bool stopImmediately)
	{
		if (instanceAssigned)
		{
			if (stopImmediately)
				eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			else
				eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

			eventInstance.release();
			AudioController.instance.audioInstances.Remove(this);
			Destroy(gameObject);
		}
		else
			Debug.LogError("Event Instance Never Assigned");
	}

	void Update()
	{
		if (instanceAssigned && is3D)
			eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(positionAnchor));
	}
}