using UnityEngine;
using System.Collections;

public class MenuMedalsCounter : MonoBehaviour
{
	[SerializeField] UILabel medalCountLabel;
	public static MenuMedalsCounter instance;

	void Awake()
	{
		instance = this;
		UpdateMedalsCount();
	}

	public void UpdateMedalsCount()
	{
		medalCountLabel.text = EncryptedPlayerPrefs.GetInt("TotalMedals", 0).ToString("N0");
	}
}
