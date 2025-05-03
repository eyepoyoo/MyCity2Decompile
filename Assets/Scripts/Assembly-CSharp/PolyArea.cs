using System;
using AmuzoPhysics;
using UnityEngine;

public class PolyArea : LinePath
{
	private const string LOG_TAG = "[PolyArea] ";

	[SerializeField]
	private int[] _triangles;

	[SerializeField]
	private float[] _triAreas;

	[SerializeField]
	private float _totalArea;

	private bool _isAreaCalcActive;

	private bool _isAreaCalcDirty;

	private bool _isFlipDebugRender;

	private string _pLogTag
	{
		get
		{
			return "[PolyArea:" + base.name + "] ";
		}
	}

	public bool _pIsValid
	{
		get
		{
			return base._pIsSimplePolygon;
		}
	}

	public bool _pIsTriangulated
	{
		get
		{
			return _triangles != null && _triangles.Length > 0;
		}
	}

	public bool _pIsAreaCalculated
	{
		get
		{
			return _triAreas != null && _triAreas.Length > 0;
		}
	}

	public float _pTotalArea
	{
		get
		{
			return Mathf.Abs(_totalArea);
		}
	}

	public bool _pIsActiveAreaCalculation
	{
		get
		{
			return _isAreaCalcActive;
		}
		set
		{
			_isAreaCalcActive = value;
		}
	}

	public bool _pIsAreaCalculationDirty
	{
		get
		{
			return _isAreaCalcDirty;
		}
	}

	public bool _pIsFlipDebugRender
	{
		get
		{
			return _isFlipDebugRender;
		}
		set
		{
			_isFlipDebugRender = value;
		}
	}

	protected override void Awake()
	{
		base.Is2d = true;
		base.IsClosed = true;
		base.Awake();
	}

	protected virtual void Start()
	{
		if (!_pIsTriangulated)
		{
			Debug.LogWarning(_pLogTag + "Triangulating at run-time", base.gameObject);
			Triangulate(false);
		}
	}

	public override void DrawGizmos(bool isSelected)
	{
		if (!_pIsTriangulated || !isSelected)
		{
			base.DrawGizmos(isSelected);
			return;
		}
		Color faceColour = LinePath.GIZMO_COLOUR;
		Color edgeColour = LinePath.GIZMO_COLOUR;
		faceColour *= 0.25f;
		edgeColour *= 0.75f;
		edgeColour.a = 1f;
		Utils.ProcessTriangles(_triangles, delegate(int i, int a, int b, int c)
		{
			if (_isFlipDebugRender)
			{
				int num = a;
				a = b;
				b = num;
			}
			DebugDraw.DrawTriImmediate(_vertices[a].ScenePos, _vertices[b].ScenePos, _vertices[c].ScenePos, faceColour, edgeColour);
			return true;
		});
	}

	public void ProcessTriangles(Action<int, int, int> action)
	{
		if (_pIsTriangulated && action != null)
		{
			Utils.ProcessTriangles(_triangles, delegate(int i, int a, int b, int c)
			{
				action(a, b, c);
				return true;
			});
		}
	}

	public virtual bool DoesContainScenePoint(Vector3 point)
	{
		Vector2 localPoint = base.transform.InverseTransformPoint(point);
		bool doesContain = false;
		Utils.ProcessTriangles(_triangles, delegate(int i, int a, int b, int c)
		{
			if (Collision2d.TriangleContainsPoint(_vertices[a].LocalPos, _vertices[b].LocalPos, _vertices[c].LocalPos, localPoint, false))
			{
				doesContain = true;
				return false;
			}
			return true;
		});
		return doesContain;
	}

	public bool DoesOverlapSceneRect(Rect rect, Func<Vector3, Vector2> transformTo2D)
	{
		if (transformTo2D == null)
		{
			return false;
		}
		bool doesOverlap = false;
		Utils.ProcessTriangles(_triangles, delegate(int i, int a, int b, int c)
		{
			if (Collision2d.TriangleOverlapsRect(transformTo2D(_vertices[a].ScenePos), transformTo2D(_vertices[b].ScenePos), transformTo2D(_vertices[c].ScenePos), rect))
			{
				doesOverlap = true;
				return false;
			}
			return true;
		});
		return doesOverlap;
	}

	public Vector3 ChooseRandomPoint()
	{
		Vector3 position = base.transform.position;
		int num = ChooseRandomTriangle();
		if (num < 0)
		{
			return position;
		}
		num *= 3;
		int index = _triangles[num];
		int index2 = _triangles[num + 1];
		int index3 = _triangles[num + 2];
		float num2 = UnityEngine.Random.Range(0f, 1f);
		float num3 = UnityEngine.Random.Range(0f, 1f);
		float num4 = UnityEngine.Random.Range(0f, 1f);
		num4 += num2 + num3;
		num2 /= num4;
		num3 /= num4;
		num4 = 1f - num2 - num3;
		return num2 * _vertices[index].ScenePos + num3 * _vertices[index2].ScenePos + num4 * _vertices[index3].ScenePos;
	}

	public void Triangulate(bool isRandomize)
	{
		_triangles = TriangulatePolygon(isRandomize);
		CalculateArea();
	}

	public void CalculateArea(Func<int, int, int, int, bool> triFilter = null)
	{
		_totalArea = 0f;
		if (_triangles == null)
		{
			return;
		}
		int num = _triangles.Length / 3;
		bool isNewTriAreas = false;
		if (_triAreas == null || _triAreas.Length != num)
		{
			_triAreas = new float[num];
			isNewTriAreas = true;
		}
		float thisArea;
		Utils.ProcessTriangles(_triangles, delegate(int i, int a, int b, int c)
		{
			if (isNewTriAreas || triFilter == null || triFilter(i, a, b, c))
			{
				thisArea = 0.5f * Vector3.Dot(Vector3.Cross(_vertices[a].ScenePos - _vertices[b].ScenePos, _vertices[c].ScenePos - _vertices[b].ScenePos), base.transform.forward);
			}
			else
			{
				thisArea = _triAreas[i];
			}
			if (_totalArea == 0f)
			{
				_totalArea = thisArea;
			}
			else if (Mathf.Sign(thisArea) == Mathf.Sign(_totalArea))
			{
				_totalArea += thisArea;
			}
			else
			{
				Debug.LogWarning("[PolyArea] Inconsistent vertex order in triangle " + i, base.gameObject);
				_totalArea -= thisArea;
			}
			_triAreas[i] = thisArea;
			return true;
		});
		_isAreaCalcDirty = false;
	}

	protected override void OnVertexPositionChanged(int vertIndex)
	{
		base.OnVertexPositionChanged(vertIndex);
		if (_isAreaCalcActive)
		{
			CalculateArea((int i, int a, int b, int c) => vertIndex == a || vertIndex == b || vertIndex == c);
		}
		else
		{
			_isAreaCalcDirty = true;
		}
	}

	private int ChooseRandomTriangle()
	{
		float num = Mathf.Abs(_totalArea);
		if (num == 0f)
		{
			return -1;
		}
		float num2 = UnityEngine.Random.Range(0f, num);
		float num3 = 0f;
		for (int i = 0; i < _triAreas.Length; i++)
		{
			num3 += Mathf.Abs(_triAreas[i]);
			if (num2 <= num3)
			{
				return i;
			}
		}
		return -1;
	}
}
