using UnityEngine;
using System.Collections;

public class TurretPingPong : MonoBehaviour
{
	[SerializeField] float rotationSpeed;
	[SerializeField] float startRotationAngle;
	[SerializeField] float minRotationAngle;
	[SerializeField] float maxRotationAngle;
	float currentRotationAngle;
	bool increaseAngle = true;

	void Start()
	{
		transform.localRotation = Quaternion.Euler(0f, startRotationAngle, 0f);
		currentRotationAngle = startRotationAngle;
	}

	void Update()
	{
		if (increaseAngle)
		{
			currentRotationAngle += Time.deltaTime * rotationSpeed;

			if (currentRotationAngle >= maxRotationAngle)
				increaseAngle = false;
		}
		else
		{
			currentRotationAngle -= Time.deltaTime * rotationSpeed;

			if (currentRotationAngle <= minRotationAngle)
				increaseAngle = true;
		}

		transform.localRotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
	}
}
