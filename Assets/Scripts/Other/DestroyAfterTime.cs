using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{
	[SerializeField] float waitTime;

	void Start()
	{
		StartCoroutine(WaitAndDestroy());
	}

	IEnumerator WaitAndDestroy()
	{
		yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);
	}
}
