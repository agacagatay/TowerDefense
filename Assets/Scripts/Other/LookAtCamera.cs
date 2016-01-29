using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
	Transform lookAtTransorm;

	void Start()
	{
		lookAtTransorm = Camera.main.transform;
	}

	void Update()
	{
		transform.LookAt(lookAtTransorm);
	}
}
