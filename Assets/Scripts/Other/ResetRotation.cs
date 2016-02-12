using UnityEngine;
using System.Collections;

public class ResetRotation : MonoBehaviour
{
	void Awake()
	{
		transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}
}
