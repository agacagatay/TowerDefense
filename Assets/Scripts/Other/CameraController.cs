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
		EasyTouch.On_PinchEnd += On_PinchEnd;
		EasyTouch.On_Twist += On_Twist;
		EasyTouch.On_TwistEnd += On_TwistEnd;
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
		EasyTouch.On_PinchEnd -= On_PinchEnd;
		EasyTouch.On_Twist -= On_Twist;
		EasyTouch.On_TwistEnd -= On_TwistEnd;
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
		canPan = false;

		float zoom = gesture.deltaPinch * zoomSpeed;
		transform.Translate(Vector3.up * -zoom);

		Vector3 cameraPosition = transform.localPosition;

		if (cameraPosition.y < minPos.y)
			cameraPosition.y = minPos.y;
		else if (cameraPosition.y > maxPos.y)
			cameraPosition.y = maxPos.y;

		transform.localPosition = cameraPosition;
	}

	void On_PinchEnd (Gesture gesture)
	{
		canPan = true;
	}

	void On_Twist(Gesture gesture)
	{
		canPan = false;
		transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + gesture.twistAngle, 0f);
	}

	void On_TwistEnd (Gesture gesture)
	{
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