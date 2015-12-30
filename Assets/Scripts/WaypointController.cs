using UnityEngine;
using System.Collections;

public class WaypointController : MonoBehaviour
{
	[SerializeField] Transform groundWaypoint;
	[SerializeField] Transform[] airWaypoints;
	[SerializeField] Transform bugOutWaypoint;
	public Transform GroundWaypoint { get { return groundWaypoint; }}
	public Transform[] AirWaypoints { get { return airWaypoints; }}
	public Transform BugOutWaypoint { get { return bugOutWaypoint; }}

	public static WaypointController instance;

	void Awake()
	{
		instance = this;
	}
}
