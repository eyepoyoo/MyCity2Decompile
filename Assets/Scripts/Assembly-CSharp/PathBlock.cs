using System;
using UnityEngine;

[RequireComponent(typeof(BezierCurveManager))]
public class PathBlock : PoolableObject
{
	[NonSerialized]
	public GeneratedPath _path;

	[NonSerialized]
	public int _index = -999;

	[NonSerialized]
	public string prefabName;

	private BezierCurveManager _bezierCurve;

	private float _simpleLength;

	public IBezierWaypoint _startWaypoint
	{
		get
		{
			return _bezierCurve.waypointList[0];
		}
	}

	public IBezierWaypoint _endWaypoint
	{
		get
		{
			return _bezierCurve.waypointList[_bezierCurve.waypointList.Count - 1];
		}
	}

	public Quaternion _pLocalStartRotation
	{
		get
		{
			return Quaternion.LookRotation(_startWaypoint.RightPoint.CurrentPositionLocal - _startWaypoint.CurrentPositionLocal, _startWaypoint.localNormal);
		}
	}

	public Quaternion _pGlobalEndRotation
	{
		get
		{
			return Quaternion.LookRotation(_endWaypoint.CurrentPosition - _endWaypoint.LeftPoint.CurrentPosition, base.transform.TransformDirection(_endWaypoint.localNormal));
		}
	}

	public float _pLength
	{
		get
		{
			return _bezierCurve.length;
		}
	}

	public int _pIndexFromEnd
	{
		get
		{
			return _path._pNumBlocks - _index - 1;
		}
	}

	protected virtual void Awake()
	{
		_bezierCurve = GetComponent<BezierCurveManager>();
		_simpleLength = Vector3.Distance(_endWaypoint.CurrentPosition, _startWaypoint.CurrentPosition);
	}

	public float GetBlockProgress(Vector3 pos)
	{
		return Vector3.Dot((_endWaypoint.CurrentPosition - _startWaypoint.CurrentPosition).normalized, pos - _startWaypoint.CurrentPosition) / _simpleLength;
	}

	public Vector3 GetPos(float normProgress, bool ignoreY = false)
	{
		return _bezierCurve.GetPositionAtDistance(normProgress * _pLength, ignoreY);
	}

	public Vector3 GetAngles(float normProgress, bool ignoreY = false)
	{
		return _bezierCurve.GetAnglesAtDistance(normProgress * _pLength, ignoreY);
	}

	public virtual void OnEntered()
	{
	}
}
