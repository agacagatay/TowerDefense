using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour
{
	[SerializeField] GameObject[] tutorialScreens;
	int currentTutorialScreen = 0;
	public GameObject[] TutorialScreens { get { return tutorialScreens; }}
	public int CurrentTutorialScreen { get { return currentTutorialScreen; }}

	public static TutorialController instance;

	void Awake()
	{
		instance = this;
		tutorialScreens[currentTutorialScreen].SetActive(true);
	}

	public void NextTutorialScreen()
	{
		tutorialScreens[currentTutorialScreen].SetActive(false);

		if ((currentTutorialScreen + 1) < tutorialScreens.Length)
			tutorialScreens[currentTutorialScreen + 1].SetActive(true);

		currentTutorialScreen++;
	}
}
