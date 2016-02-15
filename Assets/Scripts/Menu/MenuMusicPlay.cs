using UnityEngine;
using System.Collections;

public class MenuMusicPlay : MonoBehaviour
{
	void Start()
	{
		AudioController.instance.CreateInstance("Music/Music_Main_Menu", AudioController.instance.gameObject);
		AudioController.instance.Play("Music/Music_Main_Menu", AudioController.instance.gameObject);
	}
}
