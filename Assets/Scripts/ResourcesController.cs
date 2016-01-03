using UnityEngine;
using System.Collections;

public class ResourcesController : MonoBehaviour
{
	int shards;
	public int Shards { get { return shards; } set { shards = value; }}

	public static ResourcesController instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		Shards += 1000;
		Debug.Log("Shards: " + Shards);
	}

	public void UpdateShards()
	{
		Debug.Log("Shards: " + Shards);
	}
}
