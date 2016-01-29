using UnityEngine;
using System.Collections;

public class RandomProp : MonoBehaviour
{
	[SerializeField] GameObject[] propObjets;

	void Awake()
	{
		int randomProp = Random.Range(0, propObjets.Length);
		propObjets[randomProp].SetActive(true);
	}
}
