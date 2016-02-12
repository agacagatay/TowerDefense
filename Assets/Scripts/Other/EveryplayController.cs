using UnityEngine;
using System.Collections;

public class EveryplayController : MonoBehaviour
{
	public static EveryplayController instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		if (Everyplay.IsRecordingSupported())
			StartRecording();
	}

	public void StartRecording()
	{
		Everyplay.StartRecording();
	}

	public void PauseRecording()
	{
		if (!Everyplay.IsPaused())
			Everyplay.PauseRecording();
	}

	public void ResumeRecording()
	{
		if (Everyplay.IsPaused())
			Everyplay.ResumeRecording();
	}

	public void StopRecording()
	{
		if (Everyplay.IsRecording())
			Everyplay.StopRecording();
	}

	public void ShowSharingModal()
	{
		if (Everyplay.IsSupported())
			Everyplay.ShowSharingModal();
	}
}
