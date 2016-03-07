using UnityEngine;
using System.Collections;

public class TutorialScreen : MonoBehaviour
{
	[SerializeField] bool spawnEnemyWave = false;

	void Start()
	{
		if (spawnEnemyWave)
			EnemySpawnerController.instance.TutorialSpawn();
	}

	public void TutorialScreenComplete()
	{
		AudioController.instance.PlayOneshot("SFX/Menu_Open", AudioController.instance.gameObject);
		TutorialController.instance.NextTutorialScreen();
	}
}
