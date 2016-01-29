using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
	[SerializeField] float rotationSpeed;

	void Update()
	{
		transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
	}
}
