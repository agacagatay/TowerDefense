using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class AudioBusController : MonoBehaviour
{
	[SerializeField] UISlider musicSlider;
	[SerializeField] UISlider sfxSlider;
	FMOD.Studio.Bus musicBus;
	FMOD.Studio.Bus sfxBus;

	void Awake()
	{
		FMODUnity.RuntimeManager.StudioSystem.getBus("bus:/Music", out musicBus);
		musicBus.setFaderLevel(EncryptedPlayerPrefs.GetFloat("MusicVolume", 1f));
		musicSlider.value = EncryptedPlayerPrefs.GetFloat("MusicVolume", 1f);

		FMODUnity.RuntimeManager.StudioSystem.getBus("bus:/SFX", out sfxBus);
		sfxBus.setFaderLevel(EncryptedPlayerPrefs.GetFloat("SFXVolume", 1f));
		sfxSlider.value = EncryptedPlayerPrefs.GetFloat("SFXVolume", 1f);
	}

	public void SetMusicBusVolume(float busVolume)
	{
		musicBus.setFaderLevel(busVolume);
		EncryptedPlayerPrefs.SetFloat("MusicVolume", busVolume);
		PlayerPrefs.Save();
	}

	public void SetSFXBusVolume(float busVolume)
	{
		sfxBus.setFaderLevel(busVolume);
		EncryptedPlayerPrefs.SetFloat("SFXVolume", busVolume);
		PlayerPrefs.Save();
	}
}
