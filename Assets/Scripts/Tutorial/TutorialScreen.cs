using UnityEngine;
using System.Collections;

public class TutorialScreen : MonoBehaviour
{
	public void TutorialScreenComplete()
	{
		TutorialController.instance.NextTutorialScreen();
	}
}
