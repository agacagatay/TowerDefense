using UnityEngine;
using System.Collections;

public class MenuMusicPlay : MonoBehaviour
{
	void Start()
	{
		AudioController.instance.Play(gameObject, "Music/Music_Main_Menu");
	}
}
