using UnityEngine;

public interface IBezierWaypoint
{
	IBezierControlPoint LeftPoint { get; }

	IBezierControlPoint RightPoint { get; }

	Vector3 CurrentPosition { get; }

	Vector3 CurrentPositionLocal { get; }

	Vector3 localNormal { get; }

	Vector3 globalNormal { get; }

	Vector3 forward { get; }
}
