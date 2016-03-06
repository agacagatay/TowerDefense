using UnityEngine;
using System.Collections;

public class HighlightRotate : MonoBehaviour
{
	[SerializeField] float rotationSpeed;

	void Update()
	{
		transform.Rotate(Vector3.back * (rotationSpeed * Time.deltaTime));
	}
}
