using System;
using System.Collections.Generic;
using AmuzoPhysics;
using UnityEngine;

[ExecuteInEditMode]
public class LinePath : MonoBehaviour
{
	[Serializable]
	public class Vertex
	{
		public Vector3 _localPos;

		private Vector3 _scenePos;

		public Vector3 LocalPos
		{
			get
			{
				return _localPos;
			}
		}

		public Vector3 ScenePos
		{
			get
			{
				return _scenePos;
			}
		}

		public Vertex(Vector3 pos, bool isScenePos, LinePath linePath)
		{
			_localPos = Vector3.zero;
			_scenePos = Vector3.zero;
			if (isScenePos)
			{
				SetScenePos(pos, linePath);
			}
			else
			{
				SetLocalPos(pos, linePath);
			}
		}

		public void SetLocalPos(Vector3 pos, LinePath linePath)
		{
			_localPos = pos;
			_scenePos = linePath.transform.TransformPoint(pos);
		}

		public void SetScenePos(Vector3 pos, LinePath linePath)
		{
			_scenePos = pos;
			_localPos = linePath.transform.InverseTransformPoint(pos);
		}

		public void RefreshScenePos(LinePath linePath)
		{
			_scenePos = linePath.transform.TransformPoint(_localPos);
		}
	}

	[Flags]
	private enum ERefresh
	{
		BOUNDS = 1,
		PROPERTIES = 2,
		VERTSSCENEPOS = 4,
		VERTS2D = 8
	}

	public const float GIZMO_UNSELECTED_ALPHA = 0.25f;

	public static Color GIZMO_COLOUR = Color.blue;

	public List<Vertex> _vertices;

	public float _radius;

	public bool _isClosed;

	public bool _is2d;

	private bool _isSimplePolygon;

	private bool _isConvexPolygon;

	private float _polygonDirection;

	private Bounds _localBounds;

	private Bounds _sceneBounds;

	public List<Vertex> Vertices
	{
		get
		{
			return _vertices;
		}
	}

	private int _numVertices
	{
		get
		{
			return (_vertices != null) ? _vertices.Count : 0;
		}
	}

	public int NumVertices
	{
		get
		{
			return _numVertices;
		}
	}

	private int _numEdges
	{
		get
		{
			return (!_isClosed) ? (_numVertices - 1) : _numVertices;
		}
	}

	public int NumEdges
	{
		get
		{
			return _numEdges;
		}
	}

	private int _minVertices
	{
		get
		{
			return (!_isClosed) ? 2 : 3;
		}
	}

	public float Radius
	{
		get
		{
			return _radius;
		}
		set
		{
			SetRadius(value);
		}
	}

	public bool IsClosed
	{
		get
		{
			return _isClosed;
		}
		set
		{
			_isClosed = value;
		}
	}

	public bool _isOpen
	{
		get
		{
			return !_isClosed;
		}
		set
		{
			_isClosed = !value;
		}
	}

	public bool IsOpen
	{
		get
		{
			return _isOpen;
		}
		set
		{
			_isOpen = value;
		}
	}

	private bool _set2d
	{
		set
		{
			if (_is2d != value)
			{
				_is2d = value;
				Refresh2dVertices();
			}
		}
	}

	public bool Is2d
	{
		get
		{
			return _is2d;
		}
		set
		{
			_set2d = value;
		}
	}

	public bool _pIsSimplePolygon
	{
		get
		{
			return _isSimplePolygon;
		}
	}

	public bool _pIsConvexPolygon
	{
		get
		{
			return _isConvexPolygon;
		}
	}

	public bool _pIsClockwisePolygon
	{
		get
		{
			return _polygonDirection > 0f;
		}
	}

	public bool _pIsAnticlockwisePolygon
	{
		get
		{
			return _polygonDirection < 0f;
		}
	}

	public Bounds LocalBounds
	{
		get
		{
			return _localBounds;
		}
	}

	public Bounds SceneBounds
	{
		get
		{
			return _sceneBounds;
		}
	}

	protected virtual void Awake()
	{
		Refresh(ERefresh.PROPERTIES | ERefresh.VERTSSCENEPOS);
	}

	private void Update()
	{
		if (!Application.isPlaying)
		{
			EditorRefresh();
		}
	}

	private void EnsureVertices()
	{
		if (_vertices == null)
		{
			_vertices = new List<Vertex>();
		}
	}

	public void AddVertex()
	{
		Vector3 pos = GenerateNewVertexPos(-1);
		AddVertex(pos, false);
	}

	public void AddVertex(Vector3 pos, bool isScenePos)
	{
		EnsureVertices();
		if (_vertices != null)
		{
			_vertices.Add(new Vertex(pos, isScenePos, this));
			Refresh(ERefresh.BOUNDS | ERefresh.PROPERTIES);
		}
	}

	public void InsertVertex(int beforeVertIndex)
	{
		Vector3 pos = GenerateNewVertexPos(beforeVertIndex);
		InsertVertex(pos, false, beforeVertIndex);
	}

	public void InsertVertex(Vector3 pos, bool isScenePos, int beforeVertIndex)
	{
		EnsureVertices();
		if (_vertices != null)
		{
			if (0 <= beforeVertIndex && beforeVertIndex < _numVertices)
			{
				_vertices.Insert(beforeVertIndex, new Vertex(pos, isScenePos, this));
			}
			else
			{
				_vertices.Add(new Vertex(pos, isScenePos, this));
			}
			Refresh(ERefresh.BOUNDS | ERefresh.PROPERTIES);
		}
	}

	public void RemoveVertex(int vertIndex)
	{
		if (0 <= vertIndex && vertIndex < _numVertices)
		{
			_vertices.RemoveAt(vertIndex);
			Refresh(ERefresh.BOUNDS | ERefresh.PROPERTIES);
		}
	}

	public void RemoveAllVertices()
	{
		if (_vertices != null)
		{
			_vertices.Clear();
			Refresh(ERefresh.BOUNDS | ERefresh.PROPERTIES);
		}
	}

	public void SetVertexLocalPos(int vertIndex, Vector3 pos)
	{
		if (0 <= vertIndex && vertIndex < _numVertices)
		{
			_vertices[vertIndex].SetLocalPos(pos, this);
			Refresh(ERefresh.BOUNDS | ERefresh.PROPERTIES);
			OnVertexPositionChanged(vertIndex);
		}
	}

	public void SetVertexScenePos(int vertIndex, Vector3 pos)
	{
		if (0 <= vertIndex && vertIndex < _numVertices)
		{
			_vertices[vertIndex].SetScenePos(pos, this);
			Refresh(ERefresh.BOUNDS | ERefresh.PROPERTIES);
			OnVertexPositionChanged(vertIndex);
		}
	}

	protected virtual void OnVertexPositionChanged(int vertIndex)
	{
	}

	public void ReverseVertexOrder()
	{
		if (_vertices != null)
		{
			_vertices.Reverse();
		}
		Refresh(ERefresh.PROPERTIES);
	}

	public int[] TriangulatePolygon(bool isRandomize)
	{
		if (!_isSimplePolygon)
		{
			return null;
		}
		int count = _vertices.Count;
		int num = (isRandomize ? UnityEngine.Random.Range(0, count) : 0);
		Vector2[] array = new Vector2[_vertices.Count];
		List<int> list = new List<int>(_vertices.Count);
		for (int i = 0; i < _vertices.Count; i++)
		{
			array[i].x = _vertices[i].LocalPos.x;
			array[i].y = _vertices[i].LocalPos.y;
			list.Add((num + i) % count);
		}
		List<int> list2 = new List<int>(_vertices.Count - 2);
		int[] array2 = new int[3];
		Vector2[] array3 = new Vector2[3];
		while (list.Count > 3)
		{
			bool flag = false;
			for (int j = 0; j < list.Count; j++)
			{
				array2[0] = (list.Count + j - 1) % list.Count;
				array2[1] = j;
				array2[2] = (j + 1) % list.Count;
				array3[0] = array[list[array2[0]]];
				array3[1] = array[list[array2[1]]];
				array3[2] = array[list[array2[2]]];
				float vertexAngleSign = GetVertexAngleSign(array3[1] - array3[0], array3[2] - array3[1], true);
				if (vertexAngleSign == 0f)
				{
					list.RemoveAt(j--);
				}
				else
				{
					if (vertexAngleSign < 0f)
					{
						continue;
					}
					bool flag2 = false;
					for (int k = 0; k < list.Count; k++)
					{
						if (k != array2[0] && k != array2[1] && k != array2[2] && Collision2d.TriangleContainsPoint(array3[0], array3[1], array3[2], array[list[k]], false))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				Debug.LogWarning("[LinePath.Triangulate] Failed to find ear");
				break;
			}
			list2.Add(list[array2[0]], list[array2[1]], list[array2[2]]);
			list.RemoveAt(array2[1]);
		}
		list2.Add(list[0], list[1], list[2]);
		return list2.ToArray();
	}

	private void EditorRefresh()
	{
		if (!Application.isPlaying)
		{
			for (int i = _numVertices; i < _minVertices; i++)
			{
				AddVertex();
			}
			Refresh(ERefresh.VERTSSCENEPOS);
		}
	}

	private Vector3 GenerateNewVertexPos(int beforeIndex)
	{
		Vector3 result = Vector3.zero;
		int numVertices = _numVertices;
		if (numVertices == 1)
		{
			result = _vertices[0].LocalPos + Vector3.right;
		}
		else if (numVertices > 1)
		{
			int num;
			int num2;
			int relPos;
			if (beforeIndex == 0 && _isOpen)
			{
				num = 0;
				num2 = 1;
				relPos = -1;
			}
			else if (0 <= beforeIndex && beforeIndex < numVertices)
			{
				num = beforeIndex - 1;
				num2 = beforeIndex;
				relPos = 0;
			}
			else
			{
				num = numVertices - 2;
				num2 = numVertices - 1;
				relPos = 1;
			}
			if (_isClosed)
			{
				num = ((num >= 0) ? (num % numVertices) : (num % numVertices + numVertices));
				num2 = ((num2 >= 0) ? (num2 % numVertices) : (num2 % numVertices + numVertices));
			}
			if (0 <= num && num < numVertices && 0 <= num2 && num2 < numVertices && num != num2)
			{
				result = GenerateNewVertexPos(_vertices[num], _vertices[num2], relPos);
			}
		}
		return result;
	}

	private Vector3 GenerateNewVertexPos(Vertex cv1, Vertex cv2, int relPos)
	{
		Vector3 zero = Vector3.zero;
		if (relPos > 0)
		{
			return MathHelper.LerpVector3(0f, cv1.LocalPos, 1f, cv2.LocalPos, 2f);
		}
		if (relPos < 0)
		{
			return MathHelper.LerpVector3(0f, cv1.LocalPos, 1f, cv2.LocalPos, -1f);
		}
		return MathHelper.LerpVector3(0f, cv1.LocalPos, 1f, cv2.LocalPos, 0.5f);
	}

	private void RefreshVerticesScenePos()
	{
		for (int i = 0; i < _numVertices; i++)
		{
			_vertices[i].RefreshScenePos(this);
		}
		Refresh(ERefresh.BOUNDS);
	}

	private void Refresh2dVertices()
	{
		if (_is2d)
		{
			FlattenVertices();
		}
		Refresh(ERefresh.PROPERTIES);
	}

	private void FlattenVertices()
	{
		for (int i = 0; i < _numVertices; i++)
		{
			Vector3 localPos = _vertices[i].LocalPos;
			localPos.z = 0f;
			_vertices[i].SetLocalPos(localPos, this);
		}
		Refresh(ERefresh.BOUNDS);
	}

	private void SetRadius(float newRadius)
	{
		if (_radius != newRadius)
		{
			_radius = newRadius;
			Refresh(ERefresh.BOUNDS);
		}
	}

	public void RefreshProperties()
	{
		Refresh(ERefresh.PROPERTIES);
	}

	private void Refresh(ERefresh flags)
	{
		if ((flags & ERefresh.VERTSSCENEPOS) == ERefresh.VERTSSCENEPOS)
		{
			RefreshVerticesScenePos();
		}
		if ((flags & ERefresh.VERTS2D) == ERefresh.VERTS2D)
		{
			Refresh2dVertices();
		}
		if ((flags & ERefresh.BOUNDS) == ERefresh.BOUNDS)
		{
			RefreshBounds();
		}
		if ((flags & ERefresh.PROPERTIES) == ERefresh.PROPERTIES)
		{
			CheckProperties();
		}
	}

	private void RefreshBounds()
	{
		_localBounds.SetMinMax(Vector3.zero, Vector3.zero);
		_sceneBounds.SetMinMax(Vector3.zero, Vector3.zero);
		if (_numVertices > 0)
		{
			_localBounds.SetMinMax(_vertices[0].LocalPos, _vertices[0].LocalPos);
			_sceneBounds.SetMinMax(_vertices[0].ScenePos, _vertices[0].ScenePos);
			for (int i = 1; i < _numVertices; i++)
			{
				_localBounds.Encapsulate(_vertices[i].LocalPos);
				_sceneBounds.Encapsulate(_vertices[i].ScenePos);
			}
		}
	}

	private void CheckProperties()
	{
		CheckSimplePolygon();
		CheckConvexPolygon();
	}

	private void CheckSimplePolygon()
	{
		_isSimplePolygon = false;
		if (!_is2d || !_isClosed || _vertices == null || _vertices.Count < 3)
		{
			return;
		}
		int count = _vertices.Count;
		Vector2[] array = new Vector2[count];
		for (int i = 0; i < count - 1; i++)
		{
			array[i].x = _vertices[i + 1].LocalPos.x - _vertices[i].LocalPos.x;
			array[i].y = _vertices[i + 1].LocalPos.y - _vertices[i].LocalPos.y;
		}
		array[count - 1].x = _vertices[0].LocalPos.x - _vertices[count - 1].LocalPos.x;
		array[count - 1].y = _vertices[0].LocalPos.y - _vertices[count - 1].LocalPos.y;
		MathHelper.SimEqSolver simEqSolver = new MathHelper.SimEqSolver();
		float[,] array2 = new float[2, 3];
		for (int j = 0; j < count - 2; j++)
		{
			array2[0, 0] = array[j].x;
			array2[1, 0] = array[j].y;
			int num = ((j <= 0) ? (count - 1) : count);
			for (int k = j + 2; k < num; k++)
			{
				array2[0, 1] = 0f - array[k].x;
				array2[1, 1] = 0f - array[k].y;
				array2[0, 2] = _vertices[k].LocalPos.x - _vertices[j].LocalPos.x;
				array2[1, 2] = _vertices[k].LocalPos.y - _vertices[j].LocalPos.y;
				float[] array3 = simEqSolver.Solve(array2);
				if (array3 != null && !(array3[0] < 0f) && !(array3[0] > 1f) && !(array3[1] < 0f) && !(array3[1] > 1f))
				{
					return;
				}
			}
		}
		_isSimplePolygon = true;
	}

	private static void GetPerpVector(float vx, float vy, out float perpx, out float perpy)
	{
		perpx = vy;
		perpy = 0f - vx;
	}

	private static void GetPerpVector(float vx, float vy, out Vector2 perp)
	{
		perp.x = vy;
		perp.y = 0f - vx;
	}

	private static void GetPerpVector(Vector2 v, out Vector2 perp)
	{
		perp.x = v.y;
		perp.y = 0f - v.x;
	}

	private void CheckConvexPolygon()
	{
		_isConvexPolygon = false;
		_polygonDirection = 0f;
		if (!_isSimplePolygon)
		{
			return;
		}
		_isConvexPolygon = true;
		Vector2 vector = default(Vector2);
		vector.x = _vertices[_vertices.Count - 1].LocalPos.x - _vertices[_vertices.Count - 2].LocalPos.x;
		vector.y = _vertices[_vertices.Count - 1].LocalPos.y - _vertices[_vertices.Count - 2].LocalPos.y;
		vector.Normalize();
		Vector2 vector2 = default(Vector2);
		vector2.x = _vertices[0].LocalPos.x - _vertices[_vertices.Count - 1].LocalPos.x;
		vector2.y = _vertices[0].LocalPos.y - _vertices[_vertices.Count - 1].LocalPos.y;
		vector2.Normalize();
		Vector2 perp;
		GetPerpVector(vector, out perp);
		float num = Mathf.Sign(Vector2.Dot(vector2, perp)) * Mathf.Acos(Vector2.Dot(vector, vector2));
		float num2 = num;
		for (int i = 0; i < _vertices.Count - 1; i++)
		{
			vector = vector2;
			vector2.x = _vertices[i + 1].LocalPos.x - _vertices[i].LocalPos.x;
			vector2.y = _vertices[i + 1].LocalPos.y - _vertices[i].LocalPos.y;
			vector2.Normalize();
			GetPerpVector(vector, out perp);
			float num3 = Mathf.Sign(Vector2.Dot(vector2, perp)) * Mathf.Acos(Vector2.Dot(vector, vector2));
			num2 += num3;
			if (_isConvexPolygon && num3 != 0f)
			{
				if (num == 0f)
				{
					num = num3;
				}
				else if (Mathf.Sign(num3) == 0f - Mathf.Sign(num))
				{
					_isConvexPolygon = false;
				}
			}
		}
		_polygonDirection = Mathf.Sign(num2);
	}

	private float GetVertexAngleSign(Vector2 u, Vector2 v, bool isRelative)
	{
		Vector2 perp;
		GetPerpVector(u, out perp);
		float num = Mathf.Sign(Vector2.Dot(v, perp));
		if (isRelative && _polygonDirection != 0f)
		{
			num *= _polygonDirection;
		}
		return num;
	}

	private void OnDrawGizmos()
	{
		DrawGizmos(false);
	}

	private void OnDrawGizmosSelected()
	{
		DrawGizmos(true);
	}

	public virtual void DrawGizmos(bool isSelected)
	{
		Color gIZMO_COLOUR = GIZMO_COLOUR;
		float a = (gIZMO_COLOUR.a = ((!isSelected) ? 0.25f : 1f));
		Color color = gIZMO_COLOUR;
		int numVertices = _numVertices;
		int numEdges = _numEdges;
		for (int i = 0; i < numEdges; i++)
		{
			Vector3 scenePos = _vertices[i].ScenePos;
			Vector3 scenePos2 = _vertices[(i + 1) % numVertices].ScenePos;
			Gizmos.color = gIZMO_COLOUR;
			Gizmos.DrawLine(scenePos, scenePos2);
			if (!isSelected)
			{
				continue;
			}
			Vector3 normalized = (scenePos2 - scenePos).normalized;
			Vector3 vector;
			Vector3 vector2;
			if (_is2d)
			{
				vector = base.transform.forward;
				vector2 = Vector3.Cross(vector, normalized);
			}
			else
			{
				vector2 = MathHelper.GeneratePerpVector3(normalized);
				vector = Vector3.Cross(normalized, vector2);
			}
			if (_is2d)
			{
				color = 0.75f * gIZMO_COLOUR;
				color.a = a;
				Gizmos.color = color;
				if (_radius > 0f)
				{
					Gizmos.DrawLine(scenePos + _radius * vector2, scenePos2 + _radius * vector2);
					Gizmos.DrawLine(scenePos - _radius * vector2, scenePos2 - _radius * vector2);
				}
			}
			float magnitude = (scenePos2 - scenePos).magnitude;
			Vector3 vector3 = 0.5f * (scenePos + scenePos2);
			float num = magnitude * 0.1f;
			float num2 = num * 0.5f;
			scenePos = vector3 + 0.5f * num * normalized;
			scenePos2 = num2 * vector - num * normalized;
			Gizmos.color = gIZMO_COLOUR;
			for (int j = 0; j < 6; j++)
			{
				Gizmos.DrawRay(scenePos, scenePos2);
				scenePos2 = MathHelper.RotateVector3(scenePos2, normalized, (float)Math.PI / 3f);
			}
		}
	}
}
