using UnityEngine;
using System.Collections;

public class AllySpawnerTab : MonoBehaviour
{
	[SerializeField] UILabel textLabelLeft;
	[SerializeField] UILabel textLabelRight;
	string labelText;

	public void ToggleText(int branchNumber)
	{
		switch (gameObject.tag)
		{
		case "TabArtillary":
			labelText = "Artillary";
			break;
		case "TabMinigun":
			labelText = "Minigun";
			break;
		case "TabTurret":
			labelText = "Turret";
			break;
		case "TabMissileBattery":
			labelText = "Missile\nBattery";
			break;
		default:
			Debug.LogError("No valid tab specified");
			break;
		}

		if (branchNumber < 2)
		{
			textLabelRight.text = labelText;
			textLabelRight.gameObject.SetActive(true);
		}
		else
		{
			textLabelLeft.text = labelText;
			textLabelLeft.gameObject.SetActive(true);
		}
	}
}
