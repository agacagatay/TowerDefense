using UnityEngine;
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
	Transform deltaPos;
	Vector2 cameraVelocity;
	float deltaValue;
	bool canPan = true;

	void Start()
	{
		transform.localPosition = cameraStartPosition;
	}

	void OnEnable()
	{
		EasyTouch.On_Drag += On_Drag;
		EasyTouch.On_Pinch += On_Pinch;
		EasyTouch.On_Twist += On_Twist;
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
		EasyTouch.On_Drag -= On_Drag;
		EasyTouch.On_Pinch -= On_Pinch;
		EasyTouch.On_Twist -= On_Twist;
	}

	void On_Drag(Gesture gesture)
	{
		if (canPan)
		{
			transform.Translate(new Vector3(-gesture.deltaPosition.x * panSpeed, 0f, -gesture.deltaPosition.y * panSpeed));

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

			cameraVelocity = (gesture.deltaPosition / Time.deltaTime);
			deltaValue = 0f;
		}
	}

	void On_Pinch(Gesture gesture)
	{
		float zoom = gesture.deltaPinch * zoomSpeed;
		transform.Translate(new Vector3(0f, -zoom, 0f));

		Vector3 cameraPosition = transform.localPosition;

		if (cameraPosition.y < minPos.y)
			cameraPosition.y = minPos.y;
		else if (cameraPosition.y > maxPos.y)
			cameraPosition.y = maxPos.y;

		transform.localPosition = cameraPosition;
	}

	void On_Twist(Gesture gesture)
	{
		transform.eulerAngles += new Vector3(0f, gesture.twistAngle, 0f);
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
		transform.Translate(new Vector3(-lerpCameraVelocity.x, 0f, -lerpCameraVelocity.y));

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