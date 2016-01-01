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
		//Debug.Log("Shards: " + Shards);
	}

	public void UpdateShards()
	{
		//Debug.Log("Shards: " + Shards);
	}
}
