using UnityEngine;

public interface IBezierControlPoint
{
	BezierControlPointSide Side { get; }

	Vector3 CurrentPosition { get; set; }

	Vector3 CurrentPositionLocal { get; }
}
