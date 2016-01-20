using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointController : MonoBehaviour
{
	[SerializeField] Transform[] airWaypoints;
	[SerializeField] Transform[] airdropWaypoints;
	[SerializeField] Transform bugOutWaypoint;
	public Transform[] AirWaypoints { get { return airWaypoints; }}
	public Transform[] AirdropWaypoints { get { return airdropWaypoints; }}
	public Transform BugOutWaypoint { get { return bugOutWaypoint; }}

	public static WaypointController instance;

	void Awake()
	{
		instance = this;
	}
}
