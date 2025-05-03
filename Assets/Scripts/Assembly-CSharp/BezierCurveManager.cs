using System;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveManager : MonoBehaviour
{
	public delegate void InitReturnFunction();

	public List<IBezierWaypoint> waypointList = new List<IBezierWaypoint>();

	private MyBezier bezier;

	public bool EnableDistanceCalculations;

	public bool DrawGizmos = true;

	public bool DrawControlPoints = true;

	public bool IsFullLoop;

	public float SecondsForFullLoop = 1f;

	public InitReturnFunction OnInitCallback;

	public Color lineColour = Color.yellow;

	public bool _editorOnly;

	public float length
	{
		get
		{
			return bezier.MaxDistance;
		}
	}

	private void Awake()
	{
		if (!_editorOnly || !Application.isPlaying)
		{
			Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(BezierWaypoint));
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				BezierWaypoint bezierWaypoint = (BezierWaypoint)componentsInChildren[i];
				bezierWaypoint.SetControlPoints();
				waypointList.Add(bezierWaypoint);
			}
			bezier = new MyBezier(waypointList.ToArray());
			if (EnableDistanceCalculations)
			{
				bezier.SetUpDistanceLists(IsFullLoop);
			}
			if (OnInitCallback != null)
			{
				OnInitCallback();
			}
		}
	}

	public Vector3 GetPositionAtDistance(float distance, bool ignoreY = false)
	{
		if (EnableDistanceCalculations)
		{
			float num = bezier.LookupDistanceOfExistingTime(0f);
			float t = bezier.FindTimePointAlongeSplineAtDistance(distance + num);
			return bezier.GetPointAtTime(t, IsFullLoop, ignoreY);
		}
		throw new Exception("In order to use GetPositionAtDistance the EnableDistanceCalculation variable must be set to true at start up so the distance points can be pre-calculated");
	}

	public Vector3 GetAnglesAtDistance(float distance, bool ignoreY = false)
	{
		if (EnableDistanceCalculations)
		{
			float num = bezier.LookupDistanceOfExistingTime(0f);
			float t = bezier.FindTimePointAlongeSplineAtDistance(distance + num);
			return bezier.GetAnglesAtTime(t, IsFullLoop, base.transform, ignoreY);
		}
		throw new Exception("In order to use GetAnglesAtDistance the EnableDistanceCalculation variable must be set to true at start up so the distance points can be pre-calculated");
	}

	public Vector3 GetRotationAtDistance(float distance, bool ignoreY = false)
	{
		return Quaternion.LookRotation(GetPositionAtDistance(distance, ignoreY) - GetPositionAtDistance(distance - 10f, ignoreY)).eulerAngles;
	}

	public float GetAngleAtDistance(float distance, bool ignoreY = false)
	{
		return GetRotationAtDistance(distance + 10f).y - GetRotationAtDistance(distance).y;
	}

	public void OnDrawGizmos()
	{
		if (!DrawGizmos)
		{
			return;
		}
		List<BezierWaypoint> list = new List<BezierWaypoint>();
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(BezierWaypoint));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			BezierWaypoint bezierWaypoint = (BezierWaypoint)componentsInChildren[i];
			bezierWaypoint.SetControlPoints();
			if (bezierWaypoint.IsValid)
			{
				list.Add(bezierWaypoint);
			}
		}
		if (list.Count != 0)
		{
			MyBezier myBezier = new MyBezier(list.ToArray());
			Vector3 vector = myBezier.GetPointAtTime(0f, IsFullLoop);
			for (float num = 0f; num < 1f; num += 0.01f)
			{
				Vector3 pointAtTime = myBezier.GetPointAtTime(num, IsFullLoop);
				Gizmos.color = lineColour;
				Gizmos.DrawLine(vector, pointAtTime);
				vector = pointAtTime;
			}
		}
	}

	public MyBezier CreateGizmoBezier(bool distanceCalculations = false)
	{
		List<BezierWaypoint> list = new List<BezierWaypoint>();
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(BezierWaypoint));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			BezierWaypoint bezierWaypoint = (BezierWaypoint)componentsInChildren[i];
			if (!(bezierWaypoint.transform.parent != base.transform))
			{
				bezierWaypoint.SetControlPoints();
				if (bezierWaypoint.IsValid)
				{
					list.Add(bezierWaypoint);
				}
			}
		}
		if (list.Count != 0)
		{
			MyBezier myBezier = new MyBezier(list.ToArray());
			if (distanceCalculations)
			{
				myBezier.SetUpDistanceLists(IsFullLoop);
			}
			return myBezier;
		}
		return null;
	}

	public Vector3 GetPointAtTime(float time)
	{
		return bezier.GetPointAtTime(time, false);
	}

	public Vector3 GetAnglesAtTime(float time, bool ignoreY = false)
	{
		return bezier.GetAnglesAtTime(time, false, null, ignoreY);
	}
}
