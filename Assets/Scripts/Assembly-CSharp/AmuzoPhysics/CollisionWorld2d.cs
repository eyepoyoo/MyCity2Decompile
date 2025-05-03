using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmuzoPhysics
{
	public class CollisionWorld2d
	{
		public struct RaycastHit
		{
			public Collider2d _collider;

			public Vector2 _normal;

			public Vector2 _position;

			public float _length;
		}

		private List<Collider2d> _staticColliders;

		private List<Collider2d> _largeDynamicColliders;

		private List<Collider2d> _smallDynamicColliders;

		private Dictionary<Collider2d, int> _collidersPendingAddRemove;

		private bool _isSafeToAddRemoveColliders = true;

		private bool _isProcessingCollidersPendingAddRemove;

		private CollisionTreeNode2d _staticCollisionTree;

		private int _maxStaticCollisionTreeDepth;

		private int _maxStaticCollisionTreeNodeSize;

		private bool _isStaticCollisionTreeDirty;

		private Stack<CollisionTreeNode2d> _debugStaticCollisionTreeFocusNodes;

		private ContactManager2d _contactManager;

		private Collision2d.Result _collisionResult;

		private Func<Collider2d, Collider2d, bool> _collisionFilter;

		private bool _isStaticCollisionEnabled = true;

		private bool _isSmallDynamicCollisionEnabled = true;

		private bool _isLargeDynamicCollisionEnabled = true;

		private bool _isStaticDynamicChangesAllowed;

		private Action _onBeginCollisionDetection;

		private Action<float> _onEndCollisionDetection;

		public bool _isTestEnabled;

		private bool _setSafeToAddRemoveColliders
		{
			set
			{
				bool isSafeToAddRemoveColliders = _isSafeToAddRemoveColliders;
				_isSafeToAddRemoveColliders = value;
				if (_isSafeToAddRemoveColliders && !isSafeToAddRemoveColliders)
				{
					ProcessCollidersPendingAddRemove();
				}
			}
		}

		public CollisionTreeNode2d StaticCollisionTree
		{
			get
			{
				return _staticCollisionTree;
			}
		}

		public int MaxStaticCollisionTreeDepth
		{
			get
			{
				return _maxStaticCollisionTreeDepth;
			}
		}

		public int MaxStaticCollisionTreeNodeSize
		{
			get
			{
				return _maxStaticCollisionTreeNodeSize;
			}
		}

		public bool _pIsStaticCollisionTreeDirty
		{
			get
			{
				return _isStaticCollisionTreeDirty;
			}
		}

		private CollisionTreeNode2d _debugStaticCollisionTreeFocus
		{
			get
			{
				return (_debugStaticCollisionTreeFocusNodes == null) ? null : _debugStaticCollisionTreeFocusNodes.Peek();
			}
		}

		public IEnumerable<Contact2d> Contacts
		{
			get
			{
				return _contactManager.ActiveContacts;
			}
		}

		public Func<Collider2d, Collider2d, bool> _pCollisionFilter
		{
			set
			{
				_collisionFilter = value;
			}
		}

		public bool IsStaticCollisionEnabled
		{
			get
			{
				return _isStaticCollisionEnabled;
			}
			set
			{
				_isStaticCollisionEnabled = value;
			}
		}

		public bool IsSmallDynamicCollisionEnabled
		{
			get
			{
				return _isSmallDynamicCollisionEnabled;
			}
			set
			{
				_isSmallDynamicCollisionEnabled = value;
			}
		}

		public bool IsLargeDynamicCollisionEnabled
		{
			get
			{
				return _isLargeDynamicCollisionEnabled;
			}
			set
			{
				_isLargeDynamicCollisionEnabled = value;
			}
		}

		public bool IsStaticDynamicChangesAllowed
		{
			get
			{
				return _isStaticDynamicChangesAllowed;
			}
			set
			{
				_isStaticDynamicChangesAllowed = value;
			}
		}

		public bool IsOcclusionCullingEnabled
		{
			get
			{
				return _contactManager != null && _contactManager.IsOcclusionCullingEnabled;
			}
			set
			{
				if (_contactManager != null)
				{
					_contactManager.IsOcclusionCullingEnabled = value;
				}
			}
		}

		public event Action _pOnBeginCollisionDetection
		{
			add
			{
				_onBeginCollisionDetection = (Action)Delegate.Combine(_onBeginCollisionDetection, value);
			}
			remove
			{
				_onBeginCollisionDetection = (Action)Delegate.Remove(_onBeginCollisionDetection, value);
			}
		}

		public event Action<float> _pOnEndCollisionDetection
		{
			add
			{
				_onEndCollisionDetection = (Action<float>)Delegate.Combine(_onEndCollisionDetection, value);
			}
			remove
			{
				_onEndCollisionDetection = (Action<float>)Delegate.Remove(_onEndCollisionDetection, value);
			}
		}

		public CollisionWorld2d(int maxStaticCollisionTreeDepth, int maxStaticCollisionTreeNodeSize, int maxContactManagerContacts)
		{
			_staticColliders = new List<Collider2d>();
			_largeDynamicColliders = new List<Collider2d>();
			_smallDynamicColliders = new List<Collider2d>();
			_collidersPendingAddRemove = new Dictionary<Collider2d, int>();
			_staticCollisionTree = new CollisionTreeNode2d();
			_collisionResult = new Collision2d.Result();
			_contactManager = new ContactManager2d(maxContactManagerContacts);
			_maxStaticCollisionTreeDepth = maxStaticCollisionTreeDepth;
			_maxStaticCollisionTreeNodeSize = maxStaticCollisionTreeNodeSize;
		}

		public void OnUpdate(float dt)
		{
			CollisionProfiler.BeginSection("collision_world_update");
			ProcessStaticDynamicChanges();
			DoCollisionDetection(dt);
			CollisionProfiler.EndSection();
		}

		private void DoCollisionDetection(float dt)
		{
			CollisionProfiler.BeginSection("collision_detection");
			_setSafeToAddRemoveColliders = false;
			CollisionProfiler.BeginSection("on_begin");
			if (_onBeginCollisionDetection != null)
			{
				_onBeginCollisionDetection();
			}
			CollisionProfiler.EndSection();
			if (_contactManager != null)
			{
				_contactManager.OnBeginCollisionDetection();
			}
			if (_isSmallDynamicCollisionEnabled)
			{
				CollisionProfiler.BeginSection("small_dynamic_collision");
				int count = _smallDynamicColliders.Count;
				int count2 = _largeDynamicColliders.Count;
				CollisionProfiler.SetInt("num_small_dynamic_pairs", count * count2);
				for (int i = 0; i < count; i++)
				{
					Collider2d colliderA = _smallDynamicColliders[i];
					for (int j = 0; j < count2; j++)
					{
						Collider2d colliderB = _largeDynamicColliders[j];
						if (IsCollisionEnabled(colliderA, colliderB))
						{
							CollisionProfiler.Increment("num_small_dynamic_tests");
							DoCollisionDetection(colliderA, colliderB, dt);
						}
					}
				}
				CollisionProfiler.EndSection();
			}
			if (_isLargeDynamicCollisionEnabled)
			{
				CollisionProfiler.BeginSection("large_dynamic_collision");
				int count3 = _largeDynamicColliders.Count;
				CollisionProfiler.SetInt("num_large_dynamic_pairs", count3 * (count3 - 1) >> 1);
				for (int k = 0; k < count3; k++)
				{
					Collider2d colliderA2 = _largeDynamicColliders[k];
					for (int l = k + 1; l < count3; l++)
					{
						Collider2d colliderB2 = _largeDynamicColliders[l];
						if (IsCollisionEnabled(colliderA2, colliderB2))
						{
							CollisionProfiler.Increment("num_large_dynamic_tests");
							DoCollisionDetection(colliderA2, colliderB2, dt);
						}
					}
				}
				CollisionProfiler.EndSection();
			}
			if (_staticCollisionTree != null && _isStaticCollisionEnabled)
			{
				CollisionProfiler.BeginSection("static_collision");
				CollisionProfiler.SetInt("num_static_pairs", (_smallDynamicColliders.Count + _largeDynamicColliders.Count) * _staticCollisionTree.NumColliders);
				foreach (Collider2d smallDynamicCollider in _smallDynamicColliders)
				{
					foreach (Collider2d item in _staticCollisionTree.PossibleOverlaps(smallDynamicCollider._bounds))
					{
						if (IsCollisionEnabled(smallDynamicCollider, item))
						{
							CollisionProfiler.Increment("num_static_tests");
							DoCollisionDetection(smallDynamicCollider, item, dt);
						}
					}
				}
				foreach (Collider2d largeDynamicCollider in _largeDynamicColliders)
				{
					foreach (Collider2d item2 in _staticCollisionTree.PossibleOverlaps(largeDynamicCollider._bounds))
					{
						if (IsCollisionEnabled(largeDynamicCollider, item2))
						{
							CollisionProfiler.Increment("num_static_tests");
							DoCollisionDetection(largeDynamicCollider, item2, dt);
						}
					}
				}
				CollisionProfiler.EndSection();
			}
			foreach (Collider2d smallDynamicCollider2 in _smallDynamicColliders)
			{
				smallDynamicCollider2.OnDynamicEndCollisionDetection();
			}
			foreach (Collider2d largeDynamicCollider2 in _largeDynamicColliders)
			{
				largeDynamicCollider2.OnDynamicEndCollisionDetection();
			}
			if (_contactManager != null)
			{
				_contactManager.OnEndCollisionDetection();
			}
			CollisionProfiler.BeginSection("on_end");
			if (_onEndCollisionDetection != null)
			{
				_onEndCollisionDetection(dt);
			}
			CollisionProfiler.EndSection();
			_setSafeToAddRemoveColliders = true;
			CollisionProfiler.EndSection();
		}

		private bool IsCollisionEnabled(Collider2d colliderA, Collider2d colliderB)
		{
			return !Physics.GetIgnoreLayerCollision(colliderA.Layer, colliderB.Layer) && (_collisionFilter == null || _collisionFilter(colliderA, colliderB));
		}

		private void DoCollisionDetection(Collider2d colliderA, Collider2d colliderB, float dt)
		{
			if (_collisionResult == null || _contactManager == null || !colliderA._bounds.Intersects(ref colliderB._bounds))
			{
				return;
			}
			CollisionProfiler.Increment("num_passed_bounds_test");
			if ((!_isTestEnabled) ? Collision2d.IsCollidersIntersect(colliderA, colliderB, false, dt, _collisionResult) : Collision2d.IsCollidersIntersectV2(colliderA, colliderB, false, dt, _collisionResult))
			{
				bool flag = colliderA.OnCollisionDetected(colliderB, _collisionResult);
				_collisionResult.Flip();
				bool flag2 = colliderB.OnCollisionDetected(colliderA, _collisionResult);
				_collisionResult.Flip();
				if (flag && flag2)
				{
					_contactManager.OnCollisionDetected(colliderA, colliderB, _collisionResult);
				}
			}
		}

		public void FindOverlappingColliders(Bounds2d bounds, int layer, bool isIncludeSmallDynamic, bool isIncludeLargeDynamic, bool isIncludeStatic, bool isIncludeTriggers, List<Collider2d> resultColliders)
		{
			if (resultColliders == null)
			{
				return;
			}
			resultColliders.SafeClear();
			if (!bounds.IsValid)
			{
				return;
			}
			if (isIncludeSmallDynamic)
			{
				foreach (Collider2d smallDynamicCollider in _smallDynamicColliders)
				{
					if ((smallDynamicCollider.IsSolid || isIncludeTriggers) && !Physics.GetIgnoreLayerCollision(layer, smallDynamicCollider.Layer) && bounds.Intersects(ref smallDynamicCollider._bounds))
					{
						resultColliders.Add(smallDynamicCollider);
					}
				}
			}
			if (isIncludeLargeDynamic)
			{
				foreach (Collider2d largeDynamicCollider in _largeDynamicColliders)
				{
					if ((largeDynamicCollider.IsSolid || isIncludeTriggers) && !Physics.GetIgnoreLayerCollision(layer, largeDynamicCollider.Layer) && bounds.Intersects(ref largeDynamicCollider._bounds))
					{
						resultColliders.Add(largeDynamicCollider);
					}
				}
			}
			if (!isIncludeStatic || _staticCollisionTree == null)
			{
				return;
			}
			foreach (Collider2d item in _staticCollisionTree.PossibleOverlaps(bounds))
			{
				if ((item.IsSolid || isIncludeTriggers) && !Physics.GetIgnoreLayerCollision(layer, item.Layer) && bounds.Intersects(ref item._bounds))
				{
					resultColliders.Add(item);
				}
			}
		}

		public void DoRaycast(Ray2d ray, int layer, bool isIncludeSmallDynamic, bool isIncludeLargeDynamic, bool isIncludeStatic, bool isIncludeTriggers, bool isSortHits, List<RaycastHit> resultHits)
		{
			if (ray == null || !ray._pBounds.IsValid || resultHits == null)
			{
				return;
			}
			resultHits.SafeClear();
			Collision2d.Result result = new Collision2d.Result();
			RaycastHit item = default(RaycastHit);
			float pLength = ray._pLength;
			if (isIncludeSmallDynamic)
			{
				foreach (Collider2d smallDynamicCollider in _smallDynamicColliders)
				{
					if ((isIncludeTriggers || smallDynamicCollider.IsSolid) && !Physics.GetIgnoreLayerCollision(layer, smallDynamicCollider.Layer) && Collision2d.IsRayIntersect(ray, smallDynamicCollider, true, result))
					{
						item._collider = smallDynamicCollider;
						item._normal = result._normal;
						item._position = result._position;
						item._length = result._time * pLength;
						resultHits.Add(item);
					}
				}
			}
			if (isIncludeLargeDynamic)
			{
				foreach (Collider2d largeDynamicCollider in _largeDynamicColliders)
				{
					if ((isIncludeTriggers || largeDynamicCollider.IsSolid) && !Physics.GetIgnoreLayerCollision(layer, largeDynamicCollider.Layer) && Collision2d.IsRayIntersect(ray, largeDynamicCollider, true, result))
					{
						item._collider = largeDynamicCollider;
						item._normal = result._normal;
						item._position = result._position;
						item._length = result._time * pLength;
						resultHits.Add(item);
					}
				}
			}
			if (isIncludeStatic && _staticCollisionTree != null)
			{
				foreach (Collider2d item2 in _staticCollisionTree.PossibleOverlaps(ray._pBounds))
				{
					if ((isIncludeTriggers || item2.IsSolid) && !Physics.GetIgnoreLayerCollision(layer, item2.Layer) && Collision2d.IsRayIntersect(ray, item2, true, result))
					{
						item._collider = item2;
						item._normal = result._normal;
						item._position = result._position;
						item._length = result._time * pLength;
						resultHits.Add(item);
					}
				}
			}
			if (isSortHits && resultHits.Count > 1)
			{
				resultHits.Sort((RaycastHit a, RaycastHit b) => a._length.CompareTo(b._length));
			}
		}

		private int GetColliderPendingAddRemove(Collider2d collider)
		{
			return (collider != null && _collidersPendingAddRemove.ContainsKey(collider)) ? _collidersPendingAddRemove[collider] : 0;
		}

		private void SetColliderPendingAddRemove(Collider2d collider, int value)
		{
			if (value != 0 || (collider != null && _collidersPendingAddRemove.ContainsKey(collider)))
			{
				_collidersPendingAddRemove[collider] = value;
			}
		}

		public bool IsColliderAdded(Collider2d collider)
		{
			bool flag = collider != null && (_staticColliders.Contains(collider) || _largeDynamicColliders.Contains(collider) || _smallDynamicColliders.Contains(collider));
			int colliderPendingAddRemove = GetColliderPendingAddRemove(collider);
			return colliderPendingAddRemove > 0 || (flag && colliderPendingAddRemove == 0);
		}

		public void AddCollider(Collider2d collider)
		{
			if (collider.IsStatic && collider.Shape == ColliderShape.Null)
			{
				Debug.LogError("Static shape not set");
			}
			if (_isSafeToAddRemoveColliders)
			{
				if (collider.IsStatic)
				{
					if (!_staticColliders.Contains(collider) && (!_isStaticDynamicChangesAllowed || (!_largeDynamicColliders.Contains(collider) && !_smallDynamicColliders.Contains(collider))))
					{
						_staticColliders.Add(collider);
						AddColliderToStaticCollisionTree(collider);
					}
				}
				else if (collider.IsSmall)
				{
					if (!_smallDynamicColliders.Contains(collider) && (!_isStaticDynamicChangesAllowed || !_staticColliders.Contains(collider)))
					{
						_smallDynamicColliders.Add(collider);
					}
				}
				else if (!_largeDynamicColliders.Contains(collider) && (!_isStaticDynamicChangesAllowed || !_staticColliders.Contains(collider)))
				{
					_largeDynamicColliders.Add(collider);
				}
				if (!_isProcessingCollidersPendingAddRemove)
				{
					SetColliderPendingAddRemove(collider, 0);
				}
				collider.OnAddRemove(this, true);
			}
			else
			{
				SetColliderPendingAddRemove(collider, 1);
			}
		}

		public void RemoveCollider(Collider2d collider)
		{
			if (_isSafeToAddRemoveColliders)
			{
				collider.OnAddRemove(this, false);
				if (collider.IsStatic)
				{
					RemoveColliderFromStaticCollisionTree(collider);
					if (_staticColliders.Contains(collider))
					{
						RemoveColliderFromStaticCollisionTree(collider);
						_staticColliders.Remove(collider);
					}
					else if (_isStaticDynamicChangesAllowed)
					{
						if (_largeDynamicColliders.Contains(collider))
						{
							_largeDynamicColliders.Remove(collider);
						}
						else if (_smallDynamicColliders.Contains(collider))
						{
							_smallDynamicColliders.Remove(collider);
						}
					}
				}
				else if (_largeDynamicColliders.Contains(collider))
				{
					_largeDynamicColliders.Remove(collider);
				}
				else if (_smallDynamicColliders.Contains(collider))
				{
					_smallDynamicColliders.Remove(collider);
				}
				else if (_isStaticDynamicChangesAllowed && _staticColliders.Contains(collider))
				{
					RemoveColliderFromStaticCollisionTree(collider);
					_staticColliders.Remove(collider);
				}
				if (!_isProcessingCollidersPendingAddRemove)
				{
					SetColliderPendingAddRemove(collider, 0);
				}
			}
			else
			{
				SetColliderPendingAddRemove(collider, -1);
			}
		}

		private void ProcessCollidersPendingAddRemove()
		{
			if (!_isSafeToAddRemoveColliders)
			{
				return;
			}
			_isProcessingCollidersPendingAddRemove = true;
			foreach (Collider2d key in _collidersPendingAddRemove.Keys)
			{
				int num = _collidersPendingAddRemove[key];
				if (num > 0)
				{
					AddCollider(key);
				}
				else if (num < 0)
				{
					RemoveCollider(key);
				}
			}
			_isProcessingCollidersPendingAddRemove = false;
			_collidersPendingAddRemove.Clear();
		}

		private void ProcessStaticDynamicChanges()
		{
			if (!_isStaticDynamicChangesAllowed)
			{
				return;
			}
			CollisionProfiler.BeginSection("static_to_dynamic_changes");
			int num = 0;
			while (num < _staticColliders.Count)
			{
				Collider2d collider2d = _staticColliders[num];
				if (collider2d.IsDynamic)
				{
					RemoveColliderFromStaticCollisionTree(collider2d);
					_staticColliders.RemoveAt(num);
					if (collider2d.IsSmall)
					{
						_smallDynamicColliders.Add(collider2d);
					}
					else
					{
						_largeDynamicColliders.Add(collider2d);
					}
				}
				else
				{
					num++;
				}
			}
			CollisionProfiler.EndSection();
			CollisionProfiler.BeginSection("dynamic_to_static_changes");
			int num2 = 0;
			while (num2 < _smallDynamicColliders.Count)
			{
				Collider2d collider2d = _smallDynamicColliders[num2];
				if (collider2d.IsStatic)
				{
					_smallDynamicColliders.RemoveAt(num2);
					_staticColliders.Add(collider2d);
					AddColliderToStaticCollisionTree(collider2d);
				}
				else
				{
					num2++;
				}
			}
			int num3 = 0;
			while (num3 < _largeDynamicColliders.Count)
			{
				Collider2d collider2d = _largeDynamicColliders[num3];
				if (collider2d.IsStatic)
				{
					_largeDynamicColliders.RemoveAt(num3);
					_staticColliders.Add(collider2d);
					AddColliderToStaticCollisionTree(collider2d);
				}
				else
				{
					num3++;
				}
			}
			CollisionProfiler.EndSection();
		}

		public void RefreshStaticCollisionTree()
		{
			CollisionProfiler.BeginSection("static_tree_refresh");
			if (_staticCollisionTree != null)
			{
				_staticCollisionTree.Refresh(_maxStaticCollisionTreeDepth, _maxStaticCollisionTreeNodeSize, true);
			}
			_isStaticCollisionTreeDirty = false;
			CollisionProfiler.EndSection(true);
		}

		public void ReadStaticCollisionTree(TextAsset jsonFile, Func<string, Collider2d> fnGetCollider)
		{
			CollisionProfiler.BeginSection("static_tree_read");
			if (_staticCollisionTree != null)
			{
				_staticCollisionTree.ReadFromFile(jsonFile, fnGetCollider);
			}
			_isStaticCollisionTreeDirty = false;
			CollisionProfiler.EndSection(true);
		}

		private void AddColliderToStaticCollisionTree(Collider2d collider)
		{
			if (_staticCollisionTree != null)
			{
				_staticCollisionTree.AddCollider(collider);
				if (!_staticCollisionTree.Bounds.IsValid || !_staticCollisionTree.Bounds.Contains(ref collider._bounds))
				{
					_isStaticCollisionTreeDirty = true;
				}
			}
		}

		private void RemoveColliderFromStaticCollisionTree(Collider2d collider)
		{
			if (_staticCollisionTree != null)
			{
				_staticCollisionTree.RemoveCollider(collider);
			}
		}

		public void DebugResetStaticCollisionTreeFocus()
		{
			_debugStaticCollisionTreeFocusNodes = new Stack<CollisionTreeNode2d>(_maxStaticCollisionTreeDepth);
		}

		public void DebugInitStaticCollisionTreeFocus()
		{
			DebugResetStaticCollisionTreeFocus();
			_debugStaticCollisionTreeFocusNodes.Push(_staticCollisionTree);
		}

		public void DebugStaticCollisionTreeFocusDown(bool isInfront)
		{
			CollisionTreeNode2d debugStaticCollisionTreeFocus = _debugStaticCollisionTreeFocus;
			if (debugStaticCollisionTreeFocus == null)
			{
				return;
			}
			if (isInfront)
			{
				if (debugStaticCollisionTreeFocus.Infront != null)
				{
					_debugStaticCollisionTreeFocusNodes.Push(debugStaticCollisionTreeFocus.Infront);
				}
			}
			else if (debugStaticCollisionTreeFocus.Behind != null)
			{
				_debugStaticCollisionTreeFocusNodes.Push(debugStaticCollisionTreeFocus.Behind);
			}
		}

		public void DebugStaticCollisionTreeFocusUp()
		{
			CollisionTreeNode2d debugStaticCollisionTreeFocus = _debugStaticCollisionTreeFocus;
			if (debugStaticCollisionTreeFocus != null && _debugStaticCollisionTreeFocusNodes.Count > 1)
			{
				_debugStaticCollisionTreeFocusNodes.Pop();
			}
		}

		public void DebugRenderStaticCollision(Func<Vector3, Vector3> transformPointFn, bool isRenderColliders, bool isRenderShapes, bool isRenderPartitions)
		{
			if (_staticCollisionTree != null)
			{
				if (isRenderPartitions)
				{
					_staticCollisionTree.DebugRenderPartitions(transformPointFn, isRenderColliders, _debugStaticCollisionTreeFocus);
				}
				else if (isRenderColliders)
				{
					_staticCollisionTree.DebugRenderColliders(transformPointFn, isRenderShapes);
				}
			}
		}

		public void DebugRenderColliders(Func<Vector3, Vector3> transformPointFn, bool isRenderShapes)
		{
			if (isRenderShapes)
			{
				foreach (Collider2d staticCollider in _staticColliders)
				{
					staticCollider.DebugRenderShape(transformPointFn, Color.cyan);
				}
				foreach (Collider2d largeDynamicCollider in _largeDynamicColliders)
				{
					largeDynamicCollider.DebugRenderShape(transformPointFn, Color.green);
				}
				{
					foreach (Collider2d smallDynamicCollider in _smallDynamicColliders)
					{
						smallDynamicCollider.DebugRenderShape(transformPointFn, Color.yellow);
					}
					return;
				}
			}
			foreach (Collider2d staticCollider2 in _staticColliders)
			{
				staticCollider2.DebugRenderBounds(transformPointFn, Color.cyan);
			}
			foreach (Collider2d largeDynamicCollider2 in _largeDynamicColliders)
			{
				largeDynamicCollider2.DebugRenderBounds(transformPointFn, Color.green);
			}
			foreach (Collider2d smallDynamicCollider2 in _smallDynamicColliders)
			{
				smallDynamicCollider2.DebugRenderBounds(transformPointFn, Color.yellow);
			}
		}

		public void DebugRenderContacts(Func<Vector3, Vector3> transformPointFn, float normalLength)
		{
			if (_contactManager != null)
			{
				_contactManager.DebugRenderContacts(transformPointFn, normalLength);
			}
		}
	}
}
