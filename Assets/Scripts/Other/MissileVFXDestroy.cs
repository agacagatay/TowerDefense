using UnityEngine;
using System.Collections;

public class MissileVFXDestroy : MonoBehaviour
{
	[SerializeField] ParticleSystem[] particles;

	void Start()
	{
		StartCoroutine(WaitAndDestroy());
	}

	public void DisableEmmision()
	{
		foreach (ParticleSystem particleSystem in particles)
		{
			particleSystem.enableEmission = false;
		}
	}

	IEnumerator WaitAndDestroy()
	{
		yield return new WaitForSeconds(4f);

		foreach (ParticleSystem particleSystem in particles)
		{
			particleSystem.enableEmission = false;
		}

		yield return new WaitForSeconds(5f);
		Destroy(gameObject);
	}
}
