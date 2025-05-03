using System;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

namespace AmuzoPhysics
{
	public class CollisionTreeNode2d
	{
		private bool _isVerticalSplit;

		private float _position;

		private Bounds2d _bounds;

		private List<Collider2d> _colliders;

		private CollisionTreeNode2d _infront;

		private CollisionTreeNode2d _behind;

		private HashSet<Collider2d> _possibleOverlaps;

		public bool IsVerticalSplit
		{
			get
			{
				return _isVerticalSplit;
			}
			set
			{
				_isVerticalSplit = value;
			}
		}

		public float SplitPosition
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		public Bounds2d Bounds
		{
			get
			{
				return _bounds;
			}
			set
			{
				_bounds = value;
			}
		}

		public List<Collider2d> Colliders
		{
			get
			{
				return _colliders;
			}
			set
			{
				_colliders = value;
			}
		}

		private int _numColliders
		{
			get
			{
				return (_colliders != null) ? _colliders.Count : 0;
			}
		}

		public int NumColliders
		{
			get
			{
				return _numColliders;
			}
		}

		public CollisionTreeNode2d Infront
		{
			get
			{
				return _infront;
			}
			set
			{
				_infront = value;
			}
		}

		public CollisionTreeNode2d Behind
		{
			get
			{
				return _behind;
			}
			set
			{
				_behind = value;
			}
		}

		private bool _isPartitioned
		{
			get
			{
				return _infront != null && _behind != null;
			}
		}

		public CollisionTreeNode2d()
		{
			_colliders = new List<Collider2d>();
			_possibleOverlaps = new HashSet<Collider2d>();
			_bounds.Reset();
		}

		private CollisionTreeNode2d(Bounds2d parentBounds, bool isParentVerticalSplit, float parentSplitPosition, bool isInfront)
			: this()
		{
			_bounds = parentBounds;
			if (isParentVerticalSplit)
			{
				if (isInfront)
				{
					_bounds._min.y = parentSplitPosition;
				}
				else
				{
					_bounds._max.y = parentSplitPosition;
				}
			}
			else if (isInfront)
			{
				_bounds._min.x = parentSplitPosition;
			}
			else
			{
				_bounds._max.x = parentSplitPosition;
			}
		}

		private void PreDestroy()
		{
			if (!_isPartitioned)
			{
				if (_colliders != null)
				{
					_colliders.Clear();
					_colliders = null;
				}
				if (_possibleOverlaps != null)
				{
					_possibleOverlaps.Clear();
					_possibleOverlaps = null;
				}
			}
		}

		private int _TestBounds(ref Bounds2d bounds)
		{
			int result = 0;
			if (_isVerticalSplit)
			{
				if (bounds._max.y <= _position)
				{
					result = -1;
				}
				else if (bounds._min.y >= _position)
				{
					result = 1;
				}
			}
			else if (bounds._max.x <= _position)
			{
				result = -1;
			}
			else if (bounds._min.x >= _position)
			{
				result = 1;
			}
			return result;
		}

		public IEnumerable<Collider2d> PossibleOverlaps(Bounds2d bounds)
		{
			_possibleOverlaps.Clear();
			_FindPossibleOverlaps(ref bounds, _possibleOverlaps);
			return _possibleOverlaps;
		}

		private void _FindPossibleOverlaps(ref Bounds2d bounds, HashSet<Collider2d> overlapContainer)
		{
			if (!bounds.Intersects(ref _bounds))
			{
				return;
			}
			if (_isPartitioned)
			{
				int num = _TestBounds(ref bounds);
				if (num <= 0)
				{
					_behind._FindPossibleOverlaps(ref bounds, overlapContainer);
				}
				if (num >= 0)
				{
					_infront._FindPossibleOverlaps(ref bounds, overlapContainer);
				}
				return;
			}
			foreach (Collider2d collider in _colliders)
			{
				overlapContainer.Add(collider);
			}
		}

		public void AddCollider(Collider2d collider)
		{
			if (_colliders != null && !_colliders.Contains(collider))
			{
				_colliders.Add(collider);
			}
			if (_isPartitioned)
			{
				PlaceCollider(collider);
			}
		}

		public void RemoveCollider(Collider2d collider)
		{
			if (_colliders != null && _colliders.Contains(collider))
			{
				_colliders.Remove(collider);
			}
			if (_isPartitioned)
			{
				_infront.RemoveCollider(collider);
				_behind.RemoveCollider(collider);
			}
		}

		public bool ContainsCollider(Collider2d collider)
		{
			return _colliders != null && _colliders.Contains(collider);
		}

		private void InitBounds()
		{
			_bounds.Reset();
			if (_colliders == null)
			{
				return;
			}
			foreach (Collider2d collider in _colliders)
			{
				_bounds.AddBounds(collider._bounds);
			}
		}

		private void Unpartition()
		{
			if (_isPartitioned)
			{
				_infront.Unpartition();
				_behind.Unpartition();
				if (_colliders == null)
				{
					MergeColliders(_infront._colliders);
					MergeColliders(_behind._colliders);
				}
				_infront.PreDestroy();
				_infront = null;
				_behind.PreDestroy();
				_behind = null;
			}
		}

		private void AutoPartition(int maxDepth, int maxNodeSize, bool isRootNode)
		{
			if (!_isPartitioned && _numColliders > 1)
			{
				_ChooseSplit(out _isVerticalSplit, out _position);
				PlaceColliders(isRootNode);
				if (maxDepth > 0 && _infront._numColliders > maxNodeSize)
				{
					_infront.AutoPartition(maxDepth - 1, maxNodeSize, false);
				}
				if (maxDepth > 0 && _behind._numColliders > maxNodeSize)
				{
					_behind.AutoPartition(maxDepth - 1, maxNodeSize, false);
				}
			}
		}

		private void ManualPartition(bool isVerticalSplit, float splitPosition, bool isKeepColliderList)
		{
			if (!_isPartitioned)
			{
				_isVerticalSplit = isVerticalSplit;
				_position = splitPosition;
				PlaceColliders(isKeepColliderList);
			}
		}

		private void _ChooseSplit(out bool isVerticalSplit, out float splitPosition)
		{
			float x = _bounds._min.x;
			float y = _bounds._min.y;
			float x2 = _bounds._max.x;
			float y2 = _bounds._max.y;
			int num = _numColliders << 1;
			List<float> list = new List<float>(num);
			List<float> list2 = new List<float>(num);
			foreach (Collider2d collider in _colliders)
			{
				list.Add(Mathf.Max(collider._bounds._min.x, x));
				list.Add(Mathf.Min(collider._bounds._max.x, x2));
				list2.Add(Mathf.Max(collider._bounds._min.y, y));
				list2.Add(Mathf.Min(collider._bounds._max.y, y2));
			}
			list.Sort();
			list2.Sort();
			float num2 = list[num - 1] - list[0];
			float num3 = list2[num - 1] - list2[0];
			if (num3 > num2)
			{
				isVerticalSplit = true;
				splitPosition = list2[_numColliders];
			}
			else
			{
				isVerticalSplit = false;
				splitPosition = list[_numColliders];
			}
		}

		private void PlaceColliders(bool isKeepColliderList)
		{
			_infront = new CollisionTreeNode2d(_bounds, _isVerticalSplit, _position, true);
			_behind = new CollisionTreeNode2d(_bounds, _isVerticalSplit, _position, false);
			if (_colliders != null)
			{
				int count = _colliders.Count;
				for (int i = 0; i < count; i++)
				{
					PlaceCollider(_colliders[i]);
				}
				if (!isKeepColliderList)
				{
					_colliders.Clear();
					_colliders = null;
				}
			}
		}

		private void PlaceCollider(Collider2d collider)
		{
			int num = _TestBounds(ref collider._bounds);
			if (num < 0)
			{
				_behind.AddCollider(collider);
				return;
			}
			if (num > 0)
			{
				_infront.AddCollider(collider);
				return;
			}
			_behind.AddCollider(collider);
			_infront.AddCollider(collider);
		}

		private void MergeColliders(IList<Collider2d> collidersSrc)
		{
			if (collidersSrc == null)
			{
				return;
			}
			if (_colliders == null)
			{
				_colliders = new List<Collider2d>(collidersSrc.Count);
			}
			for (int i = 0; i < collidersSrc.Count; i++)
			{
				if (collidersSrc[i] != null && !_colliders.Contains(collidersSrc[i]))
				{
					_colliders.Add(collidersSrc[i]);
				}
			}
		}

		public void Refresh(int maxDepth, int maxNodeSize, bool isRootNode)
		{
			Unpartition();
			InitBounds();
			AutoPartition(maxDepth, maxNodeSize, isRootNode);
		}

		public void DebugRenderColliders(Func<Vector3, Vector3> transformPointFn, bool isRenderShapes)
		{
			RecursiveDebugRenderColliders(transformPointFn, isRenderShapes);
		}

		private void RecursiveDebugRenderColliders(Func<Vector3, Vector3> transformPointFn, bool isRenderShapes)
		{
			if (_colliders != null)
			{
				if (isRenderShapes)
				{
					foreach (Collider2d collider in _colliders)
					{
						collider.DebugRenderShape(transformPointFn, (!collider.IsStatic) ? Color.green : Color.cyan);
					}
				}
				else
				{
					foreach (Collider2d collider2 in _colliders)
					{
						collider2.DebugRenderBounds(transformPointFn, (!collider2.IsStatic) ? Color.green : Color.cyan);
					}
				}
			}
			if (_infront != null)
			{
				_infront.RecursiveDebugRenderColliders(transformPointFn, isRenderShapes);
			}
			if (_behind != null)
			{
				_behind.RecursiveDebugRenderColliders(transformPointFn, isRenderShapes);
			}
		}

		public void DebugRenderPartitions(Func<Vector3, Vector3> transformPointFn, bool isRenderColliders, CollisionTreeNode2d focusNode = null)
		{
			RenderBounds(transformPointFn, _bounds);
			RecursiveDebugRenderPartitions(transformPointFn, _bounds.Min.x, _bounds.Max.x, _bounds.Min.y, _bounds.Max.y, isRenderColliders, focusNode);
		}

		private void RenderBounds(Func<Vector3, Vector3> transformPointFn, Bounds2d bounds)
		{
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector3 arg = bounds.Min;
			Vector3 arg2 = bounds.Max;
			zero.Set(arg.x, arg2.y, 0f);
			zero2.Set(arg2.x, arg.y, 0f);
			arg = transformPointFn(arg);
			zero = transformPointFn(zero);
			arg2 = transformPointFn(arg2);
			zero2 = transformPointFn(zero2);
			Debug.DrawLine(arg, zero, Color.red);
			Debug.DrawLine(zero, arg2, Color.red);
			Debug.DrawLine(arg2, zero2, Color.red);
			Debug.DrawLine(zero2, arg, Color.red);
		}

		private void RecursiveDebugRenderPartitions(Func<Vector3, Vector3> transformPointFn, float minX, float maxX, float minY, float maxY, bool isRenderColliders, CollisionTreeNode2d focusNode)
		{
			Color blue = Color.blue;
			Color magenta = Color.magenta;
			if (focusNode != null && focusNode != this)
			{
				blue.a = 0.2f;
				magenta.a = 0.2f;
			}
			if (_isPartitioned)
			{
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				if (_isVerticalSplit)
				{
					zero.Set(minX, _position, 0f);
					zero2.Set(maxX, _position, 0f);
				}
				else
				{
					zero.Set(_position, minY, 0f);
					zero2.Set(_position, maxY, 0f);
				}
				zero = transformPointFn(zero);
				zero2 = transformPointFn(zero2);
				Debug.DrawLine(zero, zero2, blue);
				if (_infront != null)
				{
					if (_isVerticalSplit)
					{
						_infront.RecursiveDebugRenderPartitions(transformPointFn, minX, maxX, _position, maxY, isRenderColliders, focusNode);
					}
					else
					{
						_infront.RecursiveDebugRenderPartitions(transformPointFn, _position, maxX, minY, maxY, isRenderColliders, focusNode);
					}
				}
				if (_behind != null)
				{
					if (_isVerticalSplit)
					{
						_behind.RecursiveDebugRenderPartitions(transformPointFn, minX, maxX, minY, _position, isRenderColliders, focusNode);
					}
					else
					{
						_behind.RecursiveDebugRenderPartitions(transformPointFn, minX, _position, minY, maxY, isRenderColliders, focusNode);
					}
				}
			}
			else
			{
				if (_colliders == null || !isRenderColliders)
				{
					return;
				}
				foreach (Collider2d collider in _colliders)
				{
					collider.DebugRenderBounds(transformPointFn, magenta);
				}
			}
		}

		public void DebugDump()
		{
			DebugDumpRecursive(0);
		}

		private void DebugDumpRecursive(int depth)
		{
			string text = "[CollisionTreeNode2d]\t";
			for (int i = 0; i < depth; i++)
			{
				text += '\t';
			}
			text += _numColliders;
			if (_isPartitioned)
			{
				text = ((!_isVerticalSplit) ? (text + ", P:x=" + _position.ToString("F2")) : (text + ", P:y=" + _position.ToString("F2")));
			}
			else
			{
				foreach (Collider2d collider in _colliders)
				{
					if (collider != null)
					{
						string text2 = text;
						text = text2 + ", " + collider.Owner.ToString() + " (" + collider.Owner.GetHashCode().ToString("X8") + ")";
					}
					else
					{
						text += "(null)";
					}
				}
			}
			Debug.Log(text);
			if (_infront != null)
			{
				_infront.DebugDumpRecursive(depth + 1);
			}
			if (_behind != null)
			{
				_behind.DebugDumpRecursive(depth + 1);
			}
		}

		public void ReadFromFile(TextAsset jsonFile, Func<string, Collider2d> fnGetCollider)
		{
			if (jsonFile != null)
			{
				JsonData data = Extensions.LoadJson(jsonFile.text);
				_ReadJson(data, fnGetCollider);
			}
		}

		public void WriteToFile(string filePath, Func<Collider2d, string> fnGetColliderId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			JsonWriter jsonWriter = new JsonWriter(stringBuilder);
			jsonWriter.PrettyPrint = true;
			JsonData jsonData = new JsonData();
			_WriteJson(jsonData, fnGetColliderId);
			jsonData.ToJson(jsonWriter);
			FileHelper.WriteTextFile(filePath, stringBuilder.ToString());
		}

		private void _ReadJson(JsonData data, Func<string, Collider2d> fnGetCollider)
		{
			JsonData jsonData = data.TryGet("boundsMinX");
			if (jsonData != null)
			{
				_bounds._min.x = (float)(double)jsonData;
			}
			jsonData = data.TryGet("boundsMinY");
			if (jsonData != null)
			{
				_bounds._min.y = (float)(double)jsonData;
			}
			jsonData = data.TryGet("boundsMaxX");
			if (jsonData != null)
			{
				_bounds._max.x = (float)(double)jsonData;
			}
			jsonData = data.TryGet("boundsMaxY");
			if (jsonData != null)
			{
				_bounds._max.y = (float)(double)jsonData;
			}
			jsonData = data.TryGet("isVerticalSplit");
			if (jsonData != null)
			{
				_isVerticalSplit = (bool)jsonData;
			}
			jsonData = data.TryGet("splitPosition");
			if (jsonData != null)
			{
				_position = (float)(double)jsonData;
			}
			int num = 0;
			jsonData = data.TryGet("behind");
			if (jsonData != null)
			{
				_behind = new CollisionTreeNode2d();
				_behind._ReadJson(jsonData, fnGetCollider);
				num += _behind._numColliders;
			}
			jsonData = data.TryGet("infront");
			if (jsonData != null)
			{
				_infront = new CollisionTreeNode2d();
				_infront._ReadJson(jsonData, fnGetCollider);
				num += _infront._numColliders;
			}
			jsonData = data.TryGet("colliders");
			if (jsonData != null)
			{
				num = jsonData.Count;
				_colliders = new List<Collider2d>(num);
				for (int i = 0; i < num; i++)
				{
					string text = (string)jsonData[i];
					Collider2d collider2d = fnGetCollider(text);
					if (collider2d == null)
					{
						Debug.LogWarning("Failed to get collider from id:" + text);
					}
					_colliders.Add(collider2d);
				}
				return;
			}
			_colliders = new List<Collider2d>(num);
			if (_behind != null)
			{
				foreach (Collider2d collider in _behind._colliders)
				{
					if (!_colliders.Contains(collider))
					{
						_colliders.Add(collider);
					}
				}
			}
			if (_infront == null)
			{
				return;
			}
			foreach (Collider2d collider2 in _infront._colliders)
			{
				if (!_colliders.Contains(collider2))
				{
					_colliders.Add(collider2);
				}
			}
		}

		private void _WriteJson(JsonData data, Func<Collider2d, string> fnGetColliderId)
		{
			data["boundsMinX"] = new JsonData(_bounds.Min.x);
			data["boundsMinY"] = new JsonData(_bounds.Min.y);
			data["boundsMaxX"] = new JsonData(_bounds.Max.x);
			data["boundsMaxY"] = new JsonData(_bounds.Max.y);
			if (_isPartitioned)
			{
				data["isVerticalSplit"] = new JsonData(_isVerticalSplit);
				data["splitPosition"] = new JsonData(_position);
				if (_behind != null)
				{
					JsonData jsonData = new JsonData();
					_behind._WriteJson(jsonData, fnGetColliderId);
					data["behind"] = jsonData;
				}
				if (_infront != null)
				{
					JsonData jsonData2 = new JsonData();
					_infront._WriteJson(jsonData2, fnGetColliderId);
					data["infront"] = jsonData2;
				}
				return;
			}
			JsonData jsonData3 = new JsonData();
			jsonData3.SetJsonType(JsonType.Array);
			for (int i = 0; i < _numColliders; i++)
			{
				string text = fnGetColliderId(_colliders[i]);
				if (text != null && text.Length > 0)
				{
					jsonData3.Add(new JsonData(text));
				}
			}
			data["colliders"] = jsonData3;
		}
	}
}
