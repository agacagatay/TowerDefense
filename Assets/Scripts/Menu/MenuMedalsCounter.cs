using UnityEngine;
using System.Collections;

public class MenuMedalsCounter : MonoBehaviour
{
	[SerializeField] UILabel medalCountLabel;

	void Start()
	{
		UpdateMedalsCount();
	}

	public void UpdateMedalsCount()
	{
		medalCountLabel.text = EncryptedPlayerPrefs.GetInt("TotalMedals", 0).ToString("N0");
	}
}
