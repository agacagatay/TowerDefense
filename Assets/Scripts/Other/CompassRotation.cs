using UnityEngine;
using System.Collections;

public class CompassRotation : MonoBehaviour
{
	[SerializeField] Transform cameraRotation;

	void Update()
	{
		transform.rotation = Quaternion.Euler(0f, 0f, cameraRotation.eulerAngles.y);
	}
}
