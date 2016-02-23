using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioInstance : MonoBehaviour
{
	FMOD.Studio.EventInstance eventInstance;
	FMOD.Studio.EventDescription eventDescription;
	FMOD.Studio.ParameterInstance parameterInstance;
	FMOD.Studio.CueInstance cueInstance;
	bool instanceAssigned = false;
	bool is3D;

	public void AssignEventInstance(string eventPath)
	{
		eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventPath);
		eventInstance.getDescription(out eventDescription);
		eventDescription.is3D(out is3D);
		instanceAssigned = true;
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

	public void SetParameter(string paramName, float value)
	{
		eventInstance.getParameter(paramName, out parameterInstance);
		parameterInstance.setValue(value);
	}

	public void TriggerCue()
	{
		eventInstance.getCue("KeyOFF", out cueInstance);
		cueInstance.trigger();
	}

	public void Release()
	{
		if (instanceAssigned)
		{
			eventInstance.release();
		}
		else
			Debug.LogError("Event Instance Never Assigned");
	}

	public void Stop()
	{
		if (instanceAssigned)
		{
			eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			eventInstance.release();
			Destroy(gameObject);
		}
		else
			Debug.LogError("Event Instance Never Assigned");
	}

	void Update()
	{
		if (instanceAssigned && is3D)
			eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
	}

	void OnDestroy()
	{
		Stop();
	}
}