﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	[SerializeField] Vector3 cameraStartPosition;
	[SerializeField] Vector3 minPos;
	[SerializeField] Vector3 maxPos;
	[SerializeField] float panSpeed = 80f;
	[SerializeField] float zoomSpeed = 2f;
	[SerializeField] float momentumMultiplier = 1f;
	[SerializeField] float momentumSpeed = 1f;
	Vector3 deltaPos;
	Vector3 oldPos;
	Vector3 panOrigin;
	Vector2 cameraVelocity;
	float deltaValue;
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
	}

	void On_DragStart(Gesture gesture)
	{
		if (canPan)
		{
			oldPos = transform.localPosition;
			panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		}
	}

	void On_Drag(Gesture gesture)
	{
		if (canPan)
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

			cameraVelocity = (gesture.deltaPosition / Time.deltaTime);
			deltaValue = 0f;
		}
	}

	void On_Pinch(Gesture gesture)
	{
		float zoom = (Time.deltaTime * gesture.deltaPinch) * zoomSpeed;
		deltaPos = new Vector3(transform.localPosition.x, transform.localPosition.y - zoom, transform.localPosition.z);

		if (deltaPos.y < minPos.y)
			deltaPos.y = minPos.y;
		else if (deltaPos.y > maxPos.y)
			deltaPos.y = maxPos.y;

		transform.localPosition = deltaPos;
	}

	void Update()
	{
		if (canPan && EasyTouch.GetTouchCount() == 2)
			canPan = false;
		else if (!canPan && EasyTouch.GetTouchCount() == 0)
			canPan = true;
	}

	void FixedUpdate()
	{
		if (deltaValue < 1f)
			deltaValue += Time.deltaTime * momentumSpeed;
		
		Vector2 lerpCameraVelocity = Vector2.Lerp(cameraVelocity * momentumMultiplier, new Vector2(0f, 0f), deltaValue);
		transform.localPosition -= new Vector3(lerpCameraVelocity.x, 0f, lerpCameraVelocity.y);

		Vector3 cameraPosition = transform.localPosition;

		if (cameraPosition.x < minPos.x)
			cameraPosition.x = minPos.x;
		else if (cameraPosition.x > maxPos.x)
			cameraPosition.x = maxPos.x;

		if (cameraPosition.z < minPos.z)
			cameraPosition.z = minPos.z;
		else if (cameraPosition.z > maxPos.z)
			cameraPosition.z = maxPos.z;

		transform.localPosition = cameraPosition;
	}
}