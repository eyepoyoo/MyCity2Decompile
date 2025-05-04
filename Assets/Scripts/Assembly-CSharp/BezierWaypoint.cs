using UnityEngine;

public class BezierWaypoint : MonoBehaviour, IBezierWaypoint
{
	public float normAngle;

	private Vector3 _localNormal = Vector3.zero;

	private IBezierControlPoint leftPoint;

	private IBezierControlPoint rightPoint;

	public Vector3 localNormal
	{
		get
		{
			if (!Application.isPlaying || _localNormal == Vector3.zero)
			{
				Vector3 vector = LeftPoint.CurrentPositionLocal - RightPoint.CurrentPositionLocal;
				Vector3 lhs = Vector3.Cross(vector, Vector3.up);
				if (lhs.sqrMagnitude < 1f)
				{
					lhs = Vector3.left;
				}
				Vector3 vector2 = Vector3.Cross(lhs, vector);
				Quaternion quaternion = Quaternion.AngleAxis(normAngle, vector);
				_localNormal = (quaternion * vector2).normalized;
			}
			return _localNormal;
		}
	}

	public Vector3 globalNormal
	{
		get
		{
			return base.transform.parent.TransformDirection(localNormal);
		}
	}

	public bool IsValid
	{
		get
		{
			return LeftPoint != null && RightPoint != null;
		}
	}

	public IBezierControlPoint LeftPoint
	{
		get
		{
			return leftPoint;
		}
		set
		{
			leftPoint = value;
		}
	}

	public IBezierControlPoint RightPoint
	{
		get
		{
			return rightPoint;
		}
		set
		{
			rightPoint = value;
		}
	}

	public Vector3 CurrentPosition
	{
		get
		{
			return base.transform.position;
		}
	}

	public Vector3 CurrentPositionLocal
	{
		get
		{
			return base.transform.localPosition;
		}
		set
		{
			base.transform.localPosition = value;
		}
	}

	public Vector3 forward
	{
		get
		{
			return (rightPoint.CurrentPosition - CurrentPosition).normalized;
		}
	}

	private void Awake()
	{
		SetControlPoints();
	}

	public void SetControlPoints()
	{
		Component[] componentsInChildren = GetComponentsInChildren(typeof(IBezierControlPoint));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			IBezierControlPoint bezierControlPoint = (IBezierControlPoint)componentsInChildren[i];
			switch (bezierControlPoint.Side)
			{
			case BezierControlPointSide.Left:
				LeftPoint = bezierControlPoint;
				break;
			case BezierControlPointSide.Right:
				RightPoint = bezierControlPoint;
				break;
			default:
				Debug.LogError("Bezier Curve control points must be set either left or right in the Editor");
				break;
			}
		}
	}

	public void SetPositionOfOther(IBezierControlPoint controlPoint, Vector3 vectorToWaypoint)
	{
		if (RightPoint != null && LeftPoint != null)
		{
			vectorToWaypoint.Normalize();
			if (controlPoint.Side == BezierControlPointSide.Left)
			{
				float magnitude = (CurrentPosition - RightPoint.CurrentPosition).magnitude;
				RightPoint.CurrentPosition = CurrentPosition + vectorToWaypoint * magnitude;
			}
			else
			{
				float magnitude2 = (CurrentPosition - LeftPoint.CurrentPosition).magnitude;
				LeftPoint.CurrentPosition = CurrentPosition + vectorToWaypoint * magnitude2;
			}
		}
	}

	private void OnDrawGizmos()
	{
		BezierCurveManager bezierCurveManager = base.transform.parent.GetComponent(typeof(BezierCurveManager)) as BezierCurveManager;
		if (!IsValid || !bezierCurveManager.DrawGizmos)
		{
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "/Bezier/BezierWaypoint.png", false);
		if (bezierCurveManager.DrawControlPoints)
		{
			SetControlPoints();
			if (RightPoint != null && LeftPoint != null)
			{
				Gizmos.DrawLine(RightPoint.CurrentPosition, LeftPoint.CurrentPosition);
				Gizmos.DrawLine(CurrentPosition, CurrentPosition + base.transform.parent.TransformDirection(localNormal));
			}
		}
	}
}
