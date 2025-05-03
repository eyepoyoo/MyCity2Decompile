using System;
using System.Collections.Generic;
using UnityEngine;

public class MyBezier
{
	private const int NumberOfArray = 512;

	private const float OneOverNum = 0.001953125f;

	private bool _hasSetUpDistanceLists;

	private List<IBezierWaypoint> waypointList;

	private Vector3[] vectorArray;

	private float[] distanceArray = new float[512];

	private float[] timeArray = new float[512];

	private Vector3[] pointArray;

	private Vector3[] tangentArray;

	public float MaxDistance;

	public MyBezier(IBezierWaypoint[] waypointsArray)
	{
		waypointList = new List<IBezierWaypoint>();
		foreach (IBezierWaypoint item in waypointsArray)
		{
			waypointList.Add(item);
		}
	}

	public void SetUpDistanceLists(bool fullLoop, bool ignoreY = false)
	{
		_hasSetUpDistanceLists = true;
		distanceArray = new float[512];
		timeArray = new float[512];
		pointArray = new Vector3[512];
		tangentArray = new Vector3[512];
		distanceArray[0] = 0f;
		timeArray[0] = 0f;
		pointArray[0] = GetPointAtTime(0f, fullLoop, ignoreY);
		tangentArray[0] = GetTangentAtTime(0f, fullLoop, ignoreY);
		Vector3 vector = GetPointAtTime(0f, fullLoop);
		Vector3 zero = Vector3.zero;
		float num = 0f;
		for (int i = 1; i < 512; i++)
		{
			float num2 = (float)i * 0.001953125f;
			zero = GetPointAtTime(num2, fullLoop, ignoreY);
			pointArray[i] = zero;
			num += (vector - zero).magnitude;
			distanceArray[i] = num;
			timeArray[i] = num2;
			vector = zero;
			tangentArray[i] = GetTangentAtTime(num2, fullLoop, ignoreY);
		}
		MaxDistance = num;
	}

	public Vector3 GetPointAtTime(float t, bool fullLoop, bool ignoreY = false)
	{
		t %= 1f;
		if (t < 0f)
		{
			t = 1f + t;
		}
		int num = ((!fullLoop) ? (waypointList.Count - 1) : waypointList.Count);
		int num2 = Mathf.FloorToInt(t * (float)num);
		float t2 = t * (float)num - (float)num2;
		Vector3 currentPosition = waypointList[CheckWithinArray(num2, waypointList.Count)].CurrentPosition;
		Vector3 currentPosition2 = waypointList[CheckWithinArray(num2, waypointList.Count)].RightPoint.CurrentPosition;
		Vector3 currentPosition3 = waypointList[CheckWithinArray(num2 + 1, waypointList.Count)].LeftPoint.CurrentPosition;
		Vector3 currentPosition4 = waypointList[CheckWithinArray(num2 + 1, waypointList.Count)].CurrentPosition;
		return DoBezierFor4Points(t2, currentPosition, currentPosition2, currentPosition3, currentPosition4, ignoreY);
	}

	public Vector3 GetTangentAtTime(float t, bool fullLoop, bool ignoreY = false)
	{
		Vector3 pointAtTime = GetPointAtTime(t - 0.01f, fullLoop, ignoreY);
		Vector3 pointAtTime2 = GetPointAtTime(t + 0.01f, fullLoop, ignoreY);
		return Vector3.Normalize(pointAtTime2 - pointAtTime);
	}

	public Vector3 FindClosestPointOnCurve(Vector3 p, float lookAhead, out Vector3 tangent, out float distanceAlongTrack)
	{
		if (!_hasSetUpDistanceLists)
		{
			throw new Exception("In order to use FindClosestPointOnCurve, SetUpDistanceLists must first be called");
		}
		int num = 0;
		float num2 = float.PositiveInfinity;
		for (int i = 0; i < 512; i++)
		{
			Vector3 a = p - pointArray[i];
			float num3 = Vector3.SqrMagnitude(a);
			if (!(num3 >= num2))
			{
				num2 = num3;
				num = i;
			}
		}
		tangent = tangentArray[num];
		distanceAlongTrack = distanceArray[num] + lookAhead;
		return (lookAhead != 0f) ? GetPositionAtDistance(distanceAlongTrack, true) : pointArray[num];
	}

	public Vector3 GetAnglesAtTime(float t, bool fullLoop, Transform trans = null, bool ignoreY = false)
	{
		t %= 1f;
		if (t < 0f)
		{
			t = 1f + t;
		}
		int num = ((!fullLoop) ? (waypointList.Count - 1) : waypointList.Count);
		int num2 = Mathf.FloorToInt(t * (float)num);
		float t2 = t * (float)num - (float)num2;
		IBezierWaypoint bezierWaypoint = waypointList[CheckWithinArray(num2, waypointList.Count)];
		IBezierControlPoint rightPoint = waypointList[CheckWithinArray(num2, waypointList.Count)].RightPoint;
		IBezierControlPoint leftPoint = waypointList[CheckWithinArray(num2 + 1, waypointList.Count)].LeftPoint;
		IBezierWaypoint bezierWaypoint2 = waypointList[CheckWithinArray(num2 + 1, waypointList.Count)];
		Vector3 up = Vector3.up;
		if (trans != null)
		{
			up = trans.TransformDirection(Vector3.Slerp(bezierWaypoint.localNormal, bezierWaypoint2.localNormal, EaseInOut(t2)));
		}
		return DoBezierAnglesFor4Points(t2, bezierWaypoint.CurrentPosition, rightPoint.CurrentPosition, leftPoint.CurrentPosition, bezierWaypoint2.CurrentPosition, up, ignoreY);
	}

	private float EaseInOut(float t)
	{
		return 0.5f * (1f - Mathf.Cos(t * (float)Math.PI));
	}

	private int CheckWithinArray(int x, int c)
	{
		if (x >= c)
		{
			return x % c;
		}
		return x;
	}

	private Vector3 DoBezierFor4Points(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, bool ignoreY = false)
	{
		Vector3 zero = Vector3.zero;
		float num = 1f - t;
		float num2 = num * num;
		float num3 = t * t;
		zero = p0 * num * num2 + p1 * 3f * t * num2 + p2 * 3f * num3 * num + p3 * t * num3;
		if (ignoreY)
		{
			zero.y = 0f;
		}
		return zero;
	}

	private Vector3 DoBezierAnglesFor4Points(float t, Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 up, bool ignoreY = false)
	{
		if (ignoreY)
		{
			a.y = (b.y = (c.y = (d.y = 0f)));
		}
		Vector3 vector = 3f * a;
		Vector3 vector2 = 3f * b;
		Vector3 vector3 = 3f * c;
		Vector3 vector4 = d - vector3 + vector2 - a;
		Vector3 vector5 = vector3 - 2f * vector2 + vector;
		Vector3 vector6 = vector2 - vector;
		return Quaternion.LookRotation(3f * vector4 * t * t + 2f * vector5 * t + vector6, up).eulerAngles;
	}

	public float FindTimePointAlongeSplineAtDistance(float distance)
	{
		distance = Mathf.Min(distance, MaxDistance);
		if (distance < 0f)
		{
			distance = MaxDistance + distance;
		}
		float result = 0f;
		for (int i = 0; i < 512; i++)
		{
			result = timeArray[i];
			if (distance < distanceArray[i])
			{
				break;
			}
		}
		return result;
	}

	public float LookupDistanceOfExistingTime(float time)
	{
		time %= 1f;
		if (time < 0f)
		{
			time += 1f;
		}
		float result = 0f;
		for (int i = 0; i < 512; i++)
		{
			result = distanceArray[i];
			if (time < timeArray[i])
			{
				break;
			}
		}
		return result;
	}

	public Vector3 GetPositionAtDistance(float distance, bool isFullLoop, bool ignoreY = false)
	{
		if (!_hasSetUpDistanceLists)
		{
			throw new Exception("In order to use GetPositionAtDistance, SetUpDistanceLists must first be called");
		}
		float num = LookupDistanceOfExistingTime(0f);
		float t = FindTimePointAlongeSplineAtDistance(distance + num);
		return GetPointAtTime(t, isFullLoop, ignoreY);
	}

	public Vector3 GetAnglesAtDistance(float distance, bool isFullLoop, Transform transform, bool ignoreY = false)
	{
		if (!_hasSetUpDistanceLists)
		{
			throw new Exception("In order to use GetAnglesAtDistance, SetUpDistanceLists must first be called");
		}
		float num = LookupDistanceOfExistingTime(0f);
		float t = FindTimePointAlongeSplineAtDistance(distance + num);
		return GetAnglesAtTime(t, isFullLoop, transform, ignoreY);
	}
}
