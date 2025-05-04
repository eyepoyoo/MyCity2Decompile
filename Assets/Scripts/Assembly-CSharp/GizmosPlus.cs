using System;
using UnityEngine;

public static class GizmosPlus
{
	public static void DrawArc(Vector3 center, Vector3 dir, float angle, float distance)
	{
		float num = distance * 2f * (float)Math.PI * (angle / 360f);
		int num2 = Mathf.Max((int)(num / 0.25f), 2);
		Quaternion quaternion = Quaternion.Euler(0f, (0f - angle) / 2f, 0f);
		Quaternion quaternion2 = Quaternion.Euler(0f, angle / (float)(num2 - 1), 0f);
		Vector3 vector = dir * distance;
		vector = quaternion * vector;
		for (int i = 0; i < num2; i++)
		{
			Vector3 vector2 = quaternion2 * vector;
			Gizmos.DrawLine(center + vector, center + vector2);
			vector = vector2;
		}
	}

	public static void DrawArrow(Vector3 position, Vector3 forward, Vector3 right, float shaftWidth, float headWidth, float headLen, float length)
	{
		Vector3 vector = (length - headLen) * forward;
		Vector3 to = position + length * forward;
		Vector3 vector2 = shaftWidth * right;
		Vector3 vector3 = headWidth * right;
		Gizmos.DrawLine(position + vector2, position + vector2 + vector);
		Gizmos.DrawLine(position - vector2, position - vector2 + vector);
		Gizmos.DrawLine(position - vector2, position + vector2);
		Gizmos.DrawLine(position + vector2 + vector, position + vector3 + vector);
		Gizmos.DrawLine(position - vector2 + vector, position - vector3 + vector);
		Gizmos.DrawLine(position + vector3 + vector, to);
		Gizmos.DrawLine(position - vector3 + vector, to);
	}

	public static void DrawBoundingBox(Vector3 position, Quaternion rotation, Bounds _bounds)
	{
		Vector3 vector = rotation * new Vector3(_bounds.min.x, _bounds.max.y, _bounds.min.z) + position;
		Vector3 vector2 = rotation * new Vector3(_bounds.min.x, _bounds.max.y, _bounds.max.z) + position;
		Vector3 vector3 = rotation * new Vector3(_bounds.max.x, _bounds.max.y, _bounds.max.z) + position;
		Vector3 vector4 = rotation * new Vector3(_bounds.max.x, _bounds.max.y, _bounds.min.z) + position;
		Vector3 vector5 = rotation * new Vector3(_bounds.min.x, _bounds.min.y, _bounds.min.z) + position;
		Vector3 vector6 = rotation * new Vector3(_bounds.min.x, _bounds.min.y, _bounds.max.z) + position;
		Vector3 vector7 = rotation * new Vector3(_bounds.max.x, _bounds.min.y, _bounds.max.z) + position;
		Vector3 vector8 = rotation * new Vector3(_bounds.max.x, _bounds.min.y, _bounds.min.z) + position;
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector);
		Gizmos.DrawLine(vector5, vector6);
		Gizmos.DrawLine(vector6, vector7);
		Gizmos.DrawLine(vector7, vector8);
		Gizmos.DrawLine(vector8, vector5);
		Gizmos.DrawLine(vector, vector5);
		Gizmos.DrawLine(vector2, vector6);
		Gizmos.DrawLine(vector3, vector7);
		Gizmos.DrawLine(vector4, vector8);
	}

	public static void drawRect(Vector3 start, Rect rectToDraw)
	{
		Vector3 vector = start;
		Vector3 vector2 = start;
		vector2.x = rectToDraw.x;
		vector2.z = rectToDraw.y;
		vector.x = rectToDraw.x + rectToDraw.width;
		vector.z = rectToDraw.y;
		Gizmos.DrawLine(vector2, vector);
		vector2 = vector;
		vector.z = rectToDraw.y + rectToDraw.height;
		Gizmos.DrawLine(vector2, vector);
		vector2 = vector;
		vector.x = rectToDraw.x;
		Gizmos.DrawLine(vector2, vector);
		vector2 = vector;
		vector.z = rectToDraw.y;
		Gizmos.DrawLine(vector2, vector);
	}

	public static void drawCircle(Vector3 center, float radius, int numSegments = 30)
	{
		Transform transform = new GameObject().transform;
		transform.position = center;
		Vector3 vector = center + transform.forward * radius;
		Vector3 vector2 = vector;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < numSegments; i++)
		{
			zero = transform.eulerAngles;
			zero.y += 360f / (float)numSegments;
			transform.eulerAngles = zero;
			vector2 = center + transform.forward * radius;
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
		UnityEngine.Object.DestroyImmediate(transform.gameObject);
	}

	public static void drawSpawnPoint(Transform trans, float scale = 1f)
	{
		float num = 2f;
		Vector3 vector = trans.position + scale * (trans.forward * num);
		Vector3 vector2 = trans.position + scale * (trans.forward * num / 2f + trans.right * num / 2f);
		Vector3 vector3 = trans.position + scale * (-trans.forward * num / 2f + trans.right * num / 2f);
		Vector3 vector4 = trans.position + scale * (-trans.forward * num / 2f - trans.right * num / 2f);
		Vector3 vector5 = trans.position + scale * (trans.forward * num / 2f - trans.right * num / 2f);
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector5);
		Gizmos.DrawLine(vector5, vector);
	}

	public static void drawAlignedRect(Vector3 centre, Vector3 forward, float width, float depth)
	{
		Vector3 vector = forward * depth / 2f;
		Vector3 vector2 = Vector3.Cross(Vector3.up, forward) * width / 2f;
		Vector3 vector3 = centre + vector - vector2;
		Vector3 vector4 = centre + vector + vector2;
		Vector3 vector5 = centre - vector + vector2;
		Vector3 vector6 = centre - vector - vector2;
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector5);
		Gizmos.DrawLine(vector5, vector6);
		Gizmos.DrawLine(vector6, vector3);
	}

	public static void DrawArrowThin(Vector3 start, Vector3 end, float headDist = 1f, float size = 1f, bool isHeadDistNorm = true)
	{
		Vector3 vector = end - start;
		Vector3 normalized = vector.normalized;
		float t = ((!isHeadDistNorm) ? (headDist / vector.magnitude) : headDist);
		Vector3 vector2 = Vector3.Cross(normalized, Vector3.up);
		Vector3 vector3 = Vector3.Lerp(start, end, t);
		Vector3 to = vector3 + (-normalized + vector2) * size;
		Vector3 to2 = vector3 + (-normalized - vector2) * size;
		Gizmos.DrawLine(start, end);
		Gizmos.DrawLine(vector3, to);
		Gizmos.DrawLine(vector3, to2);
	}

	public static void DrawSquare(Vector3 centre, float size, Vector3 forward, Vector3 right)
	{
		float num = size / 2f;
		Vector3 vector = centre - right * num + forward * num;
		Vector3 vector2 = centre + right * num + forward * num;
		Vector3 vector3 = centre + right * num - forward * num;
		Vector3 vector4 = centre - right * num - forward * num;
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector);
	}
}
