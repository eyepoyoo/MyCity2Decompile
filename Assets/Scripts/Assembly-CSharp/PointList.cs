using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PointList : MonoBehaviour
{
	[Serializable]
	public class Vertex
	{
		public Vector3 _localPos;

		private Vector3 _scenePos;

		public Vector3 _pLocalPos
		{
			get
			{
				return _localPos;
			}
		}

		public Vector3 _pScenePos
		{
			get
			{
				return _scenePos;
			}
		}

		public Vertex(Vector3 pos, bool isScenePos, Transform transform)
		{
			_localPos = Vector3.zero;
			_scenePos = Vector3.zero;
			if (isScenePos)
			{
				SetScenePos(pos, transform);
			}
			else
			{
				SetLocalPos(pos, transform);
			}
		}

		public void SetLocalPos(Vector3 pos, Transform transform)
		{
			_localPos = pos;
			_scenePos = transform.TransformPoint(pos);
		}

		public void SetScenePos(Vector3 pos, Transform transform)
		{
			_scenePos = pos;
			_localPos = transform.InverseTransformPoint(pos);
		}

		public void RefreshScenePos(Transform transform)
		{
			_scenePos = transform.TransformPoint(_localPos);
		}
	}

	[Flags]
	protected enum ERefresh
	{
		BOUNDS = 1,
		PROPERTIES = 2,
		VERTSSCENEPOS = 4,
		VERTS2D = 8
	}

	public List<Vertex> _vertices;

	public bool _is2d;

	protected Bounds _localBounds;

	protected Bounds _sceneBounds;

	public List<Vertex> _pVertices
	{
		get
		{
			return _vertices;
		}
	}

	public int _pVertexCount
	{
		get
		{
			return (_vertices != null) ? _vertices.Count : 0;
		}
	}

	protected bool _pSet2d
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

	public bool _pIs2d
	{
		get
		{
			return _is2d;
		}
		set
		{
			_pSet2d = value;
		}
	}

	public Bounds _pLocalBounds
	{
		get
		{
			return _localBounds;
		}
	}

	public Bounds _pSceneBounds
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
			_vertices.Add(new Vertex(pos, isScenePos, base.transform));
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
			if (0 <= beforeVertIndex && beforeVertIndex < _pVertexCount)
			{
				_vertices.Insert(beforeVertIndex, new Vertex(pos, isScenePos, base.transform));
			}
			else
			{
				_vertices.Add(new Vertex(pos, isScenePos, base.transform));
			}
			Refresh(ERefresh.BOUNDS | ERefresh.PROPERTIES);
		}
	}

	public void RemoveVertex(int vertIndex)
	{
		if (0 <= vertIndex && vertIndex < _pVertexCount)
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
		if (0 <= vertIndex && vertIndex < _pVertexCount)
		{
			_vertices[vertIndex].SetLocalPos(pos, base.transform);
			Refresh(ERefresh.BOUNDS | ERefresh.PROPERTIES);
			OnVertexPositionChanged(vertIndex);
		}
	}

	public void SetVertexScenePos(int vertIndex, Vector3 pos)
	{
		if (0 <= vertIndex && vertIndex < _pVertexCount)
		{
			_vertices[vertIndex].SetScenePos(pos, base.transform);
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

	public virtual Vector3 ChooseRandomPoint()
	{
		if (_pVertexCount == 0)
		{
			return base.transform.position;
		}
		int index = UnityEngine.Random.Range(0, _pVertexCount);
		return _vertices[index]._pScenePos;
	}

	private void EditorRefresh()
	{
		if (!Application.isPlaying)
		{
			OnEditorRefresh();
			Refresh(ERefresh.VERTSSCENEPOS);
		}
	}

	protected virtual void OnEditorRefresh()
	{
	}

	protected virtual Vector3 GenerateNewVertexPos(int beforeIndex)
	{
		int pVertexCount = _pVertexCount;
		if (pVertexCount == 0)
		{
			return base.transform.localPosition;
		}
		return _vertices[pVertexCount - 1]._pLocalPos;
	}

	private void RefreshVerticesScenePos()
	{
		for (int i = 0; i < _pVertexCount; i++)
		{
			_vertices[i].RefreshScenePos(base.transform);
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
		for (int i = 0; i < _pVertexCount; i++)
		{
			Vector3 pLocalPos = _vertices[i]._pLocalPos;
			pLocalPos.z = 0f;
			_vertices[i].SetLocalPos(pLocalPos, base.transform);
		}
		Refresh(ERefresh.BOUNDS);
	}

	public void RefreshProperties()
	{
		Refresh(ERefresh.PROPERTIES);
	}

	protected virtual void Refresh(ERefresh flags)
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
	}

	private void RefreshBounds()
	{
		_localBounds.SetMinMax(Vector3.zero, Vector3.zero);
		_sceneBounds.SetMinMax(Vector3.zero, Vector3.zero);
		if (_pVertexCount > 0)
		{
			_localBounds.SetMinMax(_vertices[0]._pLocalPos, _vertices[0]._pLocalPos);
			_sceneBounds.SetMinMax(_vertices[0]._pScenePos, _vertices[0]._pScenePos);
			for (int i = 1; i < _pVertexCount; i++)
			{
				_localBounds.Encapsulate(_vertices[i]._pLocalPos);
				_sceneBounds.Encapsulate(_vertices[i]._pScenePos);
			}
		}
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
	}
}
