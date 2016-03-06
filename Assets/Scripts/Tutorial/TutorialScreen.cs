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
		TutorialController.instance.NextTutorialScreen();
	}
}
