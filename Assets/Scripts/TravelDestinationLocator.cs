using UnityEngine;
using System.Collections;

public class TravelDestinationLocator : MonoBehaviour
{
	public static TravelDestinationLocator instance;

	void Awake()
	{
		instance = this;
	}
}
