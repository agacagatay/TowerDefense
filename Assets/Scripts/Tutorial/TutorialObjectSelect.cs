using UnityEngine;
using System.Collections;

public class TutorialObjectSelect : MonoBehaviour
{
	[SerializeField] int tutorialScreenNumber;

	void OnEnable()
	{
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable()
	{
		UnsubscribeEvent();
	}

	void OnDestroy()
	{
		UnsubscribeEvent();
	}

	void UnsubscribeEvent()
	{
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}

	void On_SimpleTap(Gesture gesture)
	{
		if (gesture.pickedObject == gameObject && tutorialScreenNumber == TutorialController.instance.CurrentTutorialScreen)
		{
			TutorialController.instance.NextTutorialScreen();
		}
	}
}
