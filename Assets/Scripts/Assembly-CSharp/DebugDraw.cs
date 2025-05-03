using System;
using UnityEngine;

public static class DebugDraw
{
	public enum EDrawLineFunc
	{
		Null = 0,
		Debug = 1,
		Gizmos = 2
	}

	private delegate void DDrawLineFunc(Vector3 a, Vector3 b, Color c);

	public const float DefaultPlaneRes = 1f;

	private const int _vertexBufferSize = 64;

	private static DDrawLineFunc[] _drawLineFuncs = new DDrawLineFunc[3] { null, DrawLine_Debug, DrawLine_Gizmos };

	private static EDrawLineFunc _defaultDrawLineFunc = EDrawLineFunc.Debug;

	private static EDrawLineFunc _overrideDrawLineFunc = EDrawLineFunc.Null;

	private static int _drawLineOverrideLock = 0;

	private static Vector3[] _vertexBuffer = new Vector3[64];

	private static Shader _shader;

	private static Material _material;

	private static EDrawLineFunc _setOverrideDrawLineFunc
	{
		set
		{
			if (_overrideDrawLineFunc == EDrawLineFunc.Null || value == EDrawLineFunc.Null)
			{
				_overrideDrawLineFunc = value;
			}
			else
			{
				Debug.LogWarning("Override draw-line function already set");
			}
		}
	}

	public static EDrawLineFunc DrawLineFunc
	{
		set
		{
			_setOverrideDrawLineFunc = value;
		}
	}

	private static DDrawLineFunc _drawLineFunc
	{
		get
		{
			return (_overrideDrawLineFunc == EDrawLineFunc.Null) ? _drawLineFuncs[(int)_defaultDrawLineFunc] : _drawLineFuncs[(int)_overrideDrawLineFunc];
		}
	}

	private static void DrawLine_Debug(Vector3 a, Vector3 b, Color c)
	{
		Debug.DrawLine(a, b, c);
	}

	private static void DrawLine_Gizmos(Vector3 a, Vector3 b, Color c)
	{
		Gizmos.color = c;
		Gizmos.DrawLine(a, b);
	}

	private static void _OnBegin()
	{
		_drawLineOverrideLock++;
	}

	private static void _OnEnd()
	{
		_drawLineOverrideLock--;
		if (_drawLineOverrideLock <= 0)
		{
			_setOverrideDrawLineFunc = EDrawLineFunc.Null;
			_drawLineOverrideLock = 0;
		}
	}

	public static void DrawLine(Vector3 start, Vector3 end, Color colour)
	{
		_OnBegin();
		_drawLineFunc(start, end, colour);
		_OnEnd();
	}

	public static void DrawPlane(Vector3 pos, Vector3 normal, float size, Color colour, Vector3? fwdHint = null, float res = 1f)
	{
		_OnBegin();
		Vector3 rhs = ((!fwdHint.HasValue) ? Vector3.forward : fwdHint.Value);
		Vector3 normalized = Vector3.Cross(normal, rhs).normalized;
		rhs = Vector3.Cross(normalized, normal).normalized;
		if (normal.magnitude > 0f && normalized.magnitude > 0f && rhs.magnitude > 0f)
		{
			int num = -(int)(0.5f * size / res);
			float num2 = (float)num * res;
			float num3 = (float)num * res;
			for (float num4 = num2; num4 <= num2 + size; num4 += res)
			{
				Vector3 a = pos + num4 * normalized + num3 * rhs;
				Vector3 b = pos + num4 * normalized + (num3 + size) * rhs;
				_drawLineFunc(a, b, colour);
			}
			for (float num5 = num3; num5 <= num3 + size; num5 += res)
			{
				Vector3 a = pos + num2 * normalized + num5 * rhs;
				Vector3 b = pos + (num2 + size) * normalized + num5 * rhs;
				_drawLineFunc(a, b, colour);
			}
		}
		_OnEnd();
	}

	public static void DrawPlane(Vector3 pos, Vector3 normal, float distance, float size, Color colour, Vector3? fwdHint = null, float res = 1f)
	{
		_OnBegin();
		pos -= (Vector3.Dot(pos, normal) - distance) * normal;
		DrawPlane(pos, normal, size, colour, fwdHint, res);
		_OnEnd();
	}

	public static void DrawPlane4(Vector3 pos, Vector4 plane, float size, Color colour, Vector3? fwdHint = null, float res = 1f)
	{
		_OnBegin();
		DrawPlane(pos, plane, 0f - plane.w, size, colour, fwdHint, res);
		_OnEnd();
	}

	private static bool CheckMaterial()
	{
		if (_shader == null)
		{
			_shader = Shader.Find("DebugDrawShader");
		}
		if (_material == null && _shader != null)
		{
			_material = new Material(_shader);
		}
		return _shader != null && _material != null;
	}

	public static void DrawBox(Vector3 centre, Vector3 extents, Color colour, Transform tm = null)
	{
		_OnBegin();
		Vector3 right = Vector3.right;
		Vector3 up = Vector3.up;
		Vector3 forward = Vector3.forward;
		if (tm != null)
		{
			centre = tm.TransformPoint(centre);
			extents = Vector3.Scale(extents, tm.lossyScale);
			right = tm.right;
			up = tm.up;
			forward = tm.forward;
		}
		_vertexBuffer[0] = centre - extents.x * right - extents.y * up - extents.z * forward;
		_vertexBuffer[1] = centre - extents.x * right - extents.y * up + extents.z * forward;
		_vertexBuffer[2] = centre - extents.x * right + extents.y * up - extents.z * forward;
		_vertexBuffer[3] = centre - extents.x * right + extents.y * up + extents.z * forward;
		_vertexBuffer[4] = centre + extents.x * right - extents.y * up - extents.z * forward;
		_vertexBuffer[5] = centre + extents.x * right - extents.y * up + extents.z * forward;
		_vertexBuffer[6] = centre + extents.x * right + extents.y * up - extents.z * forward;
		_vertexBuffer[7] = centre + extents.x * right + extents.y * up + extents.z * forward;
		_drawLineFunc(_vertexBuffer[0], _vertexBuffer[1], colour);
		_drawLineFunc(_vertexBuffer[2], _vertexBuffer[3], colour);
		_drawLineFunc(_vertexBuffer[4], _vertexBuffer[5], colour);
		_drawLineFunc(_vertexBuffer[6], _vertexBuffer[7], colour);
		_drawLineFunc(_vertexBuffer[0], _vertexBuffer[2], colour);
		_drawLineFunc(_vertexBuffer[1], _vertexBuffer[3], colour);
		_drawLineFunc(_vertexBuffer[4], _vertexBuffer[6], colour);
		_drawLineFunc(_vertexBuffer[5], _vertexBuffer[7], colour);
		_drawLineFunc(_vertexBuffer[0], _vertexBuffer[4], colour);
		_drawLineFunc(_vertexBuffer[1], _vertexBuffer[5], colour);
		_drawLineFunc(_vertexBuffer[2], _vertexBuffer[6], colour);
		_drawLineFunc(_vertexBuffer[3], _vertexBuffer[7], colour);
		_OnEnd();
	}

	public static void DrawCylinder(Vector3 centre, float radius, float height, float arcLength, Color colour, Func<Vector3, Vector3> transformPointFn)
	{
		_OnBegin();
		int a = Mathf.CeilToInt((float)Math.PI * 2f * radius / arcLength);
		a = Mathf.Max(a, 4);
		float num = (float)Math.PI * 2f / (float)a;
		float num2 = 0f;
		float num3 = 0.5f * height;
		Vector3 vector = centre;
		vector.y -= num3;
		vector.z += radius;
		Vector3 vector2 = vector;
		vector2.y += height;
		if (transformPointFn != null)
		{
			vector = transformPointFn(vector);
			vector2 = transformPointFn(vector2);
		}
		for (int i = 0; i < a; i++)
		{
			num2 += num;
			Vector3 vector3 = centre;
			vector3.x += radius * Mathf.Sin(num2);
			vector3.y -= num3;
			vector3.z += radius * Mathf.Cos(num2);
			Vector3 vector4 = vector3;
			vector4.y += height;
			if (transformPointFn != null)
			{
				vector3 = transformPointFn(vector3);
				vector4 = transformPointFn(vector4);
			}
			_drawLineFunc(vector, vector3, colour);
			_drawLineFunc(vector2, vector4, colour);
			_drawLineFunc(vector, vector2, colour);
			vector = vector3;
			vector2 = vector4;
		}
		_OnEnd();
	}

	public static void DrawStar(Vector3 pos, float innerRadius, float outerRadius, Color colour, Transform tm = null)
	{
		_OnBegin();
		Vector3 vector = Vector3.one;
		Vector3 right = Vector3.right;
		Vector3 up = Vector3.up;
		Vector3 forward = Vector3.forward;
		if (tm != null)
		{
			pos = tm.TransformPoint(pos);
			vector = tm.lossyScale;
			right = tm.right;
			up = tm.up;
			forward = tm.forward;
		}
		Vector3 vector2 = innerRadius * vector;
		_vertexBuffer[0] = pos - vector2.x * right - vector2.y * up - vector2.z * forward;
		_vertexBuffer[1] = pos - vector2.x * right - vector2.y * up + vector2.z * forward;
		_vertexBuffer[2] = pos - vector2.x * right + vector2.y * up - vector2.z * forward;
		_vertexBuffer[3] = pos - vector2.x * right + vector2.y * up + vector2.z * forward;
		_vertexBuffer[4] = pos + vector2.x * right - vector2.y * up - vector2.z * forward;
		_vertexBuffer[5] = pos + vector2.x * right - vector2.y * up + vector2.z * forward;
		_vertexBuffer[6] = pos + vector2.x * right + vector2.y * up - vector2.z * forward;
		_vertexBuffer[7] = pos + vector2.x * right + vector2.y * up + vector2.z * forward;
		Vector3 vector3 = outerRadius * vector;
		_vertexBuffer[8] = pos - vector3.x * right;
		_vertexBuffer[9] = pos + vector3.x * right;
		_vertexBuffer[10] = pos - vector3.y * up;
		_vertexBuffer[11] = pos + vector3.y * up;
		_vertexBuffer[12] = pos - vector3.z * forward;
		_vertexBuffer[13] = pos + vector3.z * forward;
		_drawLineFunc(_vertexBuffer[0], _vertexBuffer[8], colour);
		_drawLineFunc(_vertexBuffer[1], _vertexBuffer[8], colour);
		_drawLineFunc(_vertexBuffer[2], _vertexBuffer[8], colour);
		_drawLineFunc(_vertexBuffer[3], _vertexBuffer[8], colour);
		_drawLineFunc(_vertexBuffer[4], _vertexBuffer[9], colour);
		_drawLineFunc(_vertexBuffer[5], _vertexBuffer[9], colour);
		_drawLineFunc(_vertexBuffer[6], _vertexBuffer[9], colour);
		_drawLineFunc(_vertexBuffer[7], _vertexBuffer[9], colour);
		_drawLineFunc(_vertexBuffer[0], _vertexBuffer[10], colour);
		_drawLineFunc(_vertexBuffer[1], _vertexBuffer[10], colour);
		_drawLineFunc(_vertexBuffer[4], _vertexBuffer[10], colour);
		_drawLineFunc(_vertexBuffer[5], _vertexBuffer[10], colour);
		_drawLineFunc(_vertexBuffer[2], _vertexBuffer[11], colour);
		_drawLineFunc(_vertexBuffer[3], _vertexBuffer[11], colour);
		_drawLineFunc(_vertexBuffer[6], _vertexBuffer[11], colour);
		_drawLineFunc(_vertexBuffer[7], _vertexBuffer[11], colour);
		_drawLineFunc(_vertexBuffer[0], _vertexBuffer[12], colour);
		_drawLineFunc(_vertexBuffer[2], _vertexBuffer[12], colour);
		_drawLineFunc(_vertexBuffer[4], _vertexBuffer[12], colour);
		_drawLineFunc(_vertexBuffer[6], _vertexBuffer[12], colour);
		_drawLineFunc(_vertexBuffer[1], _vertexBuffer[13], colour);
		_drawLineFunc(_vertexBuffer[3], _vertexBuffer[13], colour);
		_drawLineFunc(_vertexBuffer[5], _vertexBuffer[13], colour);
		_drawLineFunc(_vertexBuffer[7], _vertexBuffer[13], colour);
		_OnEnd();
	}

	public static void DrawRectangleXZ(Vector3 centre, float width, float height, Color colour, Func<Vector3, Vector3> transformPointFn)
	{
		_OnBegin();
		float num = 0.5f * width;
		float num2 = 0.5f * height;
		Vector3 vector = centre;
		vector.x -= num;
		vector.z -= num2;
		Vector3 vector2 = centre;
		vector2.x -= num;
		vector2.z += num2;
		Vector3 vector3 = centre;
		vector3.x += num;
		vector3.z += num2;
		Vector3 vector4 = centre;
		vector4.x += num;
		vector4.z -= num2;
		if (transformPointFn != null)
		{
			vector = transformPointFn(vector);
			vector2 = transformPointFn(vector2);
			vector3 = transformPointFn(vector3);
			vector4 = transformPointFn(vector4);
		}
		_drawLineFunc(vector, vector2, colour);
		_drawLineFunc(vector2, vector3, colour);
		_drawLineFunc(vector3, vector4, colour);
		_drawLineFunc(vector4, vector, colour);
		_OnEnd();
	}

	public static void DrawCircleXZ(Vector3 centre, float radius, float arcLength, Color colour, Func<Vector3, Vector3> transformPointFn)
	{
		_OnBegin();
		int a = Mathf.CeilToInt((float)Math.PI * 2f * radius / arcLength);
		a = Mathf.Max(a, 4);
		float num = (float)Math.PI * 2f / (float)a;
		float num2 = 0f;
		Vector3 vector = centre;
		vector.z += radius;
		if (transformPointFn != null)
		{
			vector = transformPointFn(vector);
		}
		for (int i = 0; i < a; i++)
		{
			num2 += num;
			Vector3 vector2 = centre;
			vector2.x += radius * Mathf.Sin(num2);
			vector2.z += radius * Mathf.Cos(num2);
			if (transformPointFn != null)
			{
				vector2 = transformPointFn(vector2);
			}
			_drawLineFunc(vector, vector2, colour);
			vector = vector2;
		}
		_OnEnd();
	}

	public static void DrawTriImmediate(Vector3 v0, Vector3 v1, Vector3 v2, Color faceColour, Color edgeColour, Func<Vector3, Vector3> transformPointFn = null)
	{
		_OnBegin();
		if (!CheckMaterial())
		{
			_OnEnd();
			return;
		}
		if (transformPointFn != null)
		{
			v0 = transformPointFn(v0);
			v1 = transformPointFn(v1);
			v2 = transformPointFn(v2);
		}
		GL.PushMatrix();
		if (faceColour.a > 0f)
		{
			_material.color = faceColour;
			_material.SetPass(0);
			GL.Begin(4);
			GL.Vertex(v0);
			GL.Vertex(v1);
			GL.Vertex(v2);
			GL.End();
		}
		if (edgeColour.a > 0f)
		{
			_material.color = edgeColour;
			_material.SetPass(0);
			GL.Begin(1);
			GL.Vertex(v0);
			GL.Vertex(v1);
			GL.Vertex(v1);
			GL.Vertex(v2);
			GL.Vertex(v2);
			GL.Vertex(v0);
			GL.End();
		}
		GL.PopMatrix();
		_OnEnd();
	}

	public static void DrawQuadImmediate(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Color faceColour, Color edgeColour, Func<Vector3, Vector3> transformPointFn = null)
	{
		_OnBegin();
		if (!CheckMaterial())
		{
			_OnEnd();
			return;
		}
		if (transformPointFn != null)
		{
			v0 = transformPointFn(v0);
			v1 = transformPointFn(v1);
			v2 = transformPointFn(v2);
			v3 = transformPointFn(v3);
		}
		GL.PushMatrix();
		if (faceColour.a > 0f)
		{
			_material.color = faceColour;
			_material.SetPass(0);
			GL.Begin(7);
			GL.Vertex(v0);
			GL.Vertex(v1);
			GL.Vertex(v2);
			GL.Vertex(v3);
			GL.End();
		}
		if (edgeColour.a > 0f)
		{
			_material.color = edgeColour;
			_material.SetPass(0);
			GL.Begin(1);
			GL.Vertex(v0);
			GL.Vertex(v1);
			GL.Vertex(v1);
			GL.Vertex(v2);
			GL.Vertex(v2);
			GL.Vertex(v3);
			GL.Vertex(v3);
			GL.Vertex(v0);
			GL.End();
		}
		GL.PopMatrix();
		_OnEnd();
	}

	public static void DrawCircleSectorImmediate(Vector3 position, Quaternion rotation, float innerRadius, float outerRadius, float startAngle, float endAngle, float maxEdgeLength, Color fillColour, Color edgeColour, Func<Vector3, Vector3> transformPointFn = null)
	{
		_OnBegin();
		if (!CheckMaterial())
		{
			_OnEnd();
			return;
		}
		float num = endAngle - startAngle;
		float num2 = Mathf.Abs(num);
		float num3 = innerRadius * num2;
		float num4 = outerRadius * num2;
		int num5 = Mathf.CeilToInt(num3 / maxEdgeLength);
		int num6 = Mathf.CeilToInt(num4 / maxEdgeLength);
		int num7 = num5 + 1;
		int num8 = num6 + 1;
		float num9 = (float)num7 / (float)num8;
		int num10 = num7 + num8;
		if (num10 > 64)
		{
			num8 = Mathf.FloorToInt(64f / (1f + num9));
			num7 = Mathf.FloorToInt(num9 * (float)num8);
			num7 = Mathf.Max(num7, 1);
			num10 = num7 + num8;
			if (num10 > 64)
			{
				Debug.LogWarning("[DebugDraw.DrawCircleSectorImmediate] Vertex buffer still too small after attempted fix");
			}
			num6 = num8 - 1;
			num5 = num7 - 1;
		}
		float num11 = num / (float)num5;
		float num12 = num / (float)num6;
		float num13 = startAngle;
		for (int i = 0; i < num7; i++)
		{
			_vertexBuffer[i].x = innerRadius * Mathf.Cos(num13);
			_vertexBuffer[i].y = innerRadius * Mathf.Sin(num13);
			_vertexBuffer[i].z = 0f;
			num13 += num11;
		}
		num13 = startAngle;
		for (int j = 0; j < num8; j++)
		{
			_vertexBuffer[num7 + j].x = outerRadius * Mathf.Cos(num13);
			_vertexBuffer[num7 + j].y = outerRadius * Mathf.Sin(num13);
			_vertexBuffer[num7 + j].z = 0f;
			num13 += num12;
		}
		for (int k = 0; k < num10; k++)
		{
			_vertexBuffer[k] = rotation * _vertexBuffer[k];
			_vertexBuffer[k] += position;
		}
		if (transformPointFn != null)
		{
			for (int l = 0; l < num10; l++)
			{
				_vertexBuffer[l] = transformPointFn(_vertexBuffer[l]);
			}
		}
		GL.PushMatrix();
		if (fillColour.a > 0f)
		{
			_material.color = fillColour;
			_material.SetPass(0);
			GL.Begin(4);
			int num14 = 0;
			for (int m = 0; m < num6; m++)
			{
				int num15 = Mathf.CeilToInt(num9 * (float)m);
				if (num15 >= num7)
				{
					num15 = Mathf.FloorToInt(num9 * (float)m);
					if (num15 >= num7)
					{
						Debug.LogWarning("[DebugDraw.DrawCircleSectorImmediate] Inner vertex overflow for outer vertex " + m);
						break;
					}
				}
				if (num14 != num15)
				{
					GL.Vertex(_vertexBuffer[num7 + m]);
					GL.Vertex(_vertexBuffer[num14]);
					GL.Vertex(_vertexBuffer[num15]);
					num14 = num15;
				}
				GL.Vertex(_vertexBuffer[num7 + m]);
				GL.Vertex(_vertexBuffer[num14]);
				GL.Vertex(_vertexBuffer[num7 + m + 1]);
			}
			GL.End();
		}
		if (edgeColour.a > 0f)
		{
			_material.color = edgeColour;
			_material.SetPass(0);
			GL.Begin(1);
			for (int n = 0; n < num5; n++)
			{
				GL.Vertex(_vertexBuffer[n]);
				GL.Vertex(_vertexBuffer[n + 1]);
			}
			for (int num16 = 0; num16 < num6; num16++)
			{
				GL.Vertex(_vertexBuffer[num7 + num16]);
				GL.Vertex(_vertexBuffer[num7 + num16 + 1]);
			}
			GL.Vertex(_vertexBuffer[0]);
			GL.Vertex(_vertexBuffer[num7]);
			GL.Vertex(_vertexBuffer[num7 - 1]);
			GL.Vertex(_vertexBuffer[num10 - 1]);
			GL.End();
		}
		GL.PopMatrix();
		_OnEnd();
	}
}
