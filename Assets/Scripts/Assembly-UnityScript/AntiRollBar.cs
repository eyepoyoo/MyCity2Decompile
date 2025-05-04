using System;
using UnityEngine;

[Serializable]
public class AntiRollBar : MonoBehaviour
{
	public WheelCollider WheelL;

	public WheelCollider WheelR;

	public float AntiRoll;

	public AntiRollBar()
	{
		AntiRoll = 5000f;
	}

	public virtual void FixedUpdate()
	{
		WheelHit hit = default(WheelHit);
		float num = 1f;
		float num2 = 1f;
		bool groundHit = WheelL.GetGroundHit(out hit);
		if (groundHit)
		{
			num = (0f - WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;
		}
		bool groundHit2 = WheelR.GetGroundHit(out hit);
		if (groundHit2)
		{
			num2 = (0f - WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;
		}
		float num3 = (num - num2) * AntiRoll;
		if (groundHit)
		{
			GetComponent<Rigidbody>().AddForceAtPosition(WheelL.transform.up * (0f - num3), WheelL.transform.position);
		}
		if (groundHit2)
		{
			GetComponent<Rigidbody>().AddForceAtPosition(WheelR.transform.up * num3, WheelR.transform.position);
		}
	}

	public virtual void Main()
	{
	}
}
