using UnityEngine;
using System.Collections;

public class MenuAudioBusSlider : MonoBehaviour
{
	[SerializeField] AudioBusController audioBusController;

	public void MusicValueChange()
	{
		audioBusController.SetMusicBusVolume(gameObject.GetComponent<UISlider>().value);
	}

	public void SFXValueChange()
	{
		audioBusController.SetSFXBusVolume(gameObject.GetComponent<UISlider>().value);
	}
}
