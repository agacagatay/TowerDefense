﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	[SerializeField] Vector3 cameraStartPosition;
	[SerializeField] Vector3 minPos;
	[SerializeField] Vector3 maxPos;
	[SerializeField] float panSpeed = 80f;
	[SerializeField] float zoomSpeed = 2f;
	Vector3 deltaPos;
	Vector3 oldPos;
	Vector3 panOrigin;
	bool canPan = true;

	void Start()
	{
		transform.localPosition = cameraStartPosition;
	}

	void OnEnable()
	{
		EasyTouch.On_DragStart += On_DragStart;
		EasyTouch.On_Drag += On_Drag;
		EasyTouch.On_Pinch += On_Pinch;
		EasyTouch.On_PinchEnd += On_PinchEnd;
	}

	void OnDisable()
	{
		UnsubscribeEvent();
	}

	void OnDestroy()
	{
		UnsubscribeEvent();
	}

	void UnsubscribeEvent()
	{
		EasyTouch.On_DragStart -= On_DragStart;
		EasyTouch.On_Drag -= On_Drag;
		EasyTouch.On_Pinch -= On_Pinch;
		EasyTouch.On_PinchEnd -= On_PinchEnd;
	}

	void On_DragStart(Gesture gesture)
	{
		if (canPan && gesture.touchCount == 1)
		{
			oldPos = transform.localPosition;
			panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		}
	}

	void On_Drag(Gesture gesture)
	{
		if (canPan && gesture.touchCount == 1)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOrigin;
			deltaPos = new Vector3(oldPos.x + -pos.x * panSpeed, transform.localPosition.y, oldPos.z + -pos.y * panSpeed);

			if (deltaPos.x < minPos.x)
				deltaPos.x = minPos.x;
			else if (deltaPos.x > maxPos.x)
				deltaPos.x = maxPos.x;

			if (deltaPos.z < minPos.z)
				deltaPos.z = minPos.z;
			else if (deltaPos.z > maxPos.z)
				deltaPos.z = maxPos.z;

			transform.localPosition = deltaPos;
		}
	}

	void On_Pinch(Gesture gesture)
	{
		StopCoroutine(MovementRelease());
		canPan = false;
		float zoom = (Time.deltaTime * gesture.deltaPinch) * zoomSpeed;
		deltaPos = new Vector3(transform.localPosition.x, transform.localPosition.y - zoom, transform.localPosition.z);

		if (deltaPos.y < minPos.y)
			deltaPos.y = minPos.y;
		else if (deltaPos.y > maxPos.y)
			deltaPos.y = maxPos.y;

		transform.localPosition = deltaPos;
	}

	void On_PinchEnd (Gesture gesture)
	{
		StartCoroutine(MovementRelease());
	}

	IEnumerator MovementRelease()
	{
		yield return new WaitForSeconds(0.1f);
		canPan = true;
	}
}