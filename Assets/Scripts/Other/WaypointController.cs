using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointController : MonoBehaviour
{
	[SerializeField] Transform primaryStructure;
	[SerializeField] List<Transform> secondaryStructures = new List<Transform>();
	[SerializeField] Transform[] airWaypoints;
	[SerializeField] Transform bugOutWaypoint;
	public Transform PrimaryStructure { get { return primaryStructure; }}
	public List<Transform> SecondaryStructures { get { return secondaryStructures; } set { secondaryStructures = value; }}
	public Transform[] AirWaypoints { get { return airWaypoints; }}
	public Transform BugOutWaypoint { get { return bugOutWaypoint; }}

	public static WaypointController instance;

	void Awake()
	{
		instance = this;
	}
}
