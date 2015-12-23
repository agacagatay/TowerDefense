using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{
	[SerializeField] Transform targetTransform;
	[SerializeField] Transform horPivot;
	[SerializeField] Transform verPivot;
	[SerializeField] float horLookSpeed;
	[SerializeField] float verLookSpeed;

	void Update()
	{
		Quaternion newHorRotation = Quaternion.LookRotation(targetTransform.position - horPivot.position);
		newHorRotation.x = 0f;
		newHorRotation.z = 0f;

		Quaternion newVerRotation = Quaternion.LookRotation(targetTransform.position - verPivot.position);
		newVerRotation.y = 0f;
		newVerRotation.z = 0f;

		horPivot.localRotation = Quaternion.Lerp(horPivot.localRotation, newHorRotation, Time.deltaTime * horLookSpeed);
		verPivot.localRotation = Quaternion.Lerp(verPivot.localRotation, newVerRotation, Time.deltaTime * verLookSpeed);
	}
}
