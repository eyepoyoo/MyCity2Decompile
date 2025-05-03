using UnityEngine;

public class BezierControlPoint : MonoBehaviour, IBezierControlPoint
{
	public BezierControlPointSide side;

	public BezierControlPointSide Side
	{
		get
		{
			return side;
		}
	}

	public Vector3 CurrentPosition
	{
		get
		{
			return base.transform.position;
		}
		set
		{
			base.transform.position = value;
		}
	}

	public Vector3 CurrentPositionLocal
	{
		get
		{
			return base.transform.localPosition + base.transform.parent.localPosition;
		}
	}

	private void OnDrawGizmos()
	{
		if (base.transform.parent.parent != null)
		{
			BezierCurveManager bezierCurveManager = base.transform.parent.parent.GetComponent(typeof(BezierCurveManager)) as BezierCurveManager;
			if (bezierCurveManager.DrawControlPoints)
			{
				Gizmos.DrawIcon(base.transform.position, "/Bezier/BezierControlPoint.png", false);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!base.transform.parent)
		{
			return;
		}
		BezierWaypoint component = base.transform.parent.GetComponent<BezierWaypoint>();
		if ((bool)component)
		{
			Vector3 vectorToWaypoint = component.CurrentPosition - base.transform.position;
			if (vectorToWaypoint.sqrMagnitude > 0f)
			{
				component.SetPositionOfOther(this, vectorToWaypoint);
			}
		}
	}
}
