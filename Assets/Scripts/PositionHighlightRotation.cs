using UnityEngine;
using System.Collections;

public class PositionHighlightRotation : MonoBehaviour
{
	[SerializeField] float rotationSpeed;

	void Update()
	{
		transform.Rotate (0f, 0f, -rotationSpeed * Time.deltaTime);
	} 
}
