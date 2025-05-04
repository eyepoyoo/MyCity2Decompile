using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmuzoPhysics
{
	public class Collider2d
	{
		public interface IOwner
		{
			Transform2d Transform { get; }

			Vector2 Velocity { get; }

			bool IsStatic { get; }

			bool IsSmall { get; }

			bool IsEnabled { get; }

			int Layer { get; }

			void OnAddRemove(CollisionWorld2d world, bool isAdd);

			bool OnCollisionDetected(Collider2d otherCollider, Collision2d.Result result);

			void OnBeginContact(Collider2d otherCollider);

			void OnEndContact(Collider2d otherCollider);
		}

		private const int _kNumDebugRenderVertices = 32;

		public static Collider2d DebugInstance;

		private ColliderShape _shape;

		private bool _isTrigger;

		public Vector2 _offset;

		public Vector2 _extents;

		public Bounds2d _bounds;

		public Transform2d _cachedTransform;

		public Vector2 _cachedVelocity;

		public bool _cachedIsStatic;

		private Vector2 _prevPos;

		private IOwner _owner;

		private CollisionWorld2d _world;

		private LinkedList<Contact2d> _contactList;

		private List<Vector2> _vertices;

		private float _vertexShapeRadius;

		private bool _isVerticesTransformed;

		private List<int> _triangles;

		private static Vector3[] _debugRenderVertices = new Vector3[32];

		public ColliderShape Shape
		{
			get
			{
				return _shape;
			}
		}

		public bool IsValid
		{
			get
			{
				return _shape != ColliderShape.Null;
			}
		}

		public bool IsTrigger
		{
			get
			{
				return _isTrigger;
			}
			set
			{
				_isTrigger = value;
			}
		}

		public bool IsSolid
		{
			get
			{
				return !_isTrigger;
			}
			set
			{
				_isTrigger = !value;
			}
		}

		public Vector2 Offset
		{
			get
			{
				return _offset;
			}
		}

		public Vector2 Extents
		{
			get
			{
				return _extents;
			}
		}

		public Bounds2d Bounds
		{
			get
			{
				return _bounds;
			}
		}

		private Vector2 _centre
		{
			get
			{
				return _bounds.Centre;
			}
		}

		public Vector2 Centre
		{
			get
			{
				return _centre;
			}
		}

		public Vector2 DeltaPos
		{
			get
			{
				return (!_cachedIsStatic) ? (_cachedTransform._position - _prevPos) : Vector2.zero;
			}
		}

		public IOwner Owner
		{
			get
			{
				return _owner;
			}
		}

		public Transform2d Transform
		{
			get
			{
				return _owner.Transform;
			}
		}

		public Vector2 Position
		{
			get
			{
				return _owner.Transform.Position;
			}
		}

		public float Rotation
		{
			get
			{
				return _owner.Transform.Rotation;
			}
		}

		public Vector2 Velocity
		{
			get
			{
				return _owner.Velocity;
			}
		}

		public bool IsDynamic
		{
			get
			{
				return !_owner.IsStatic;
			}
		}

		public bool IsStatic
		{
			get
			{
				return _owner.IsStatic;
			}
		}

		public bool IsSmall
		{
			get
			{
				return _owner.IsSmall;
			}
		}

		public bool IsEnabled
		{
			get
			{
				return _owner.IsEnabled;
			}
		}

		public int Layer
		{
			get
			{
				return _owner.Layer;
			}
		}

		public CollisionWorld2d World
		{
			get
			{
				return _world;
			}
		}

		public LinkedList<Contact2d> ContactList
		{
			get
			{
				return _contactList;
			}
		}

		private float _setVertexShapeRadius
		{
			set
			{
				if (_vertexShapeRadius != value)
				{
					_vertexShapeRadius = value;
					RefreshVertexBounds();
				}
			}
		}

		private float _getVertexShapeRadius
		{
			get
			{
				return _isVertexShape ? _vertexShapeRadius : ((_shape != ColliderShape.Sphere) ? 0f : _extents.x);
			}
		}

		public float VertexShapeRadius
		{
			get
			{
				return _getVertexShapeRadius;
			}
			set
			{
				_setVertexShapeRadius = value;
			}
		}

		private bool _isVertexShape
		{
			get
			{
				return _shape == ColliderShape.LineList || _shape == ColliderShape.ConvexVertices || _shape == ColliderShape.Triangles;
			}
		}

		public bool IsVertexShape
		{
			get
			{
				return _isVertexShape;
			}
		}

		public IList<int> _pTriangles
		{
			get
			{
				return _triangles;
			}
		}

		public Collider2d(IOwner owner)
		{
			if (owner == null)
			{
				Debug.LogError("[Collider2d] Must have owner!");
			}
			_owner = owner;
			_shape = ColliderShape.Null;
			_contactList = new LinkedList<Contact2d>();
		}

		private void _ResetPrevPos()
		{
			_prevPos = _owner.Transform.Position;
		}

		public bool IsOwnerType<T>() where T : class
		{
			return _owner is T;
		}

		public T OwnerAsType<T>() where T : class
		{
			return _owner as T;
		}

		public void Initialise(ColliderShape shape, Vector2 offset, Vector2 extents, bool isTrigger = false)
		{
			_shape = shape;
			_isTrigger = isTrigger;
			SetOffsetExtents(offset, extents, true);
		}

		public void SetOffsetExtents(Vector2 offset, Vector2 extents, bool isForceRefresh = false)
		{
			if (_offset != offset || _extents != extents || isForceRefresh)
			{
				_offset = offset;
				_extents = extents;
				RefreshBounds();
			}
		}

		public void OnOwnerPositionChanged(bool isInstantaneous)
		{
			RefreshBounds();
			if (isInstantaneous)
			{
				_ResetPrevPos();
			}
		}

		public void OnOwnerRotationChanged()
		{
			RefreshBounds();
		}

		private void RefreshBounds()
		{
			_extents.x = Mathf.Abs(_extents.x);
			_extents.y = Mathf.Abs(_extents.y);
			_bounds.Centre = _offset;
			_bounds.Extents = _extents;
			RefreshCachedData();
			if (!_isVertexShape || !_isVerticesTransformed)
			{
				_bounds.Transform(_cachedTransform);
			}
		}

		private void RefreshCachedData()
		{
			_cachedTransform = Transform;
			_cachedVelocity = Velocity;
			_cachedIsStatic = IsStatic;
		}

		public bool ContainsPoint(Vector2 point)
		{
			return Collision2d.ColliderContainsPoint(this, point);
		}

		public void OnAddRemove(CollisionWorld2d world, bool isAdd)
		{
			_world = ((!isAdd) ? null : world);
			if (isAdd)
			{
				_ResetPrevPos();
			}
			_owner.OnAddRemove(world, isAdd);
		}

		public bool OnCollisionDetected(Collider2d otherCollider, Collision2d.Result result)
		{
			return _owner.OnCollisionDetected(otherCollider, result);
		}

		public void OnDynamicEndCollisionDetection()
		{
			_prevPos = _cachedTransform._position;
		}

		public void OnBeginContact(Contact2d contact)
		{
			_owner.OnBeginContact(contact.GetOtherCollider(this));
		}

		public void OnEndContact(Contact2d contact)
		{
			_owner.OnEndContact(contact.GetOtherCollider(this));
		}

		public Contact2d FindContactWith(Collider2d otherCollider)
		{
			if (_contactList != null)
			{
				foreach (Contact2d contact in _contactList)
				{
					if (contact.ColliderA == otherCollider || contact.ColliderB == otherCollider)
					{
						return contact;
					}
				}
			}
			return null;
		}

		public void DoSimpleCollisionResponse(float bounce, float friction, out Vector2 deltaPos, out Vector2 deltaVel)
		{
			deltaPos = Vector2.zero;
			deltaVel = Vector2.zero;
			Vector2 velocity = Velocity;
			Vector2 zero = Vector2.zero;
			foreach (Contact2d contact in ContactList)
			{
				Collider2d otherCollider = contact.GetOtherCollider(this);
				if (otherCollider == null || !otherCollider.IsSolid)
				{
					continue;
				}
				Vector2 velocity2 = otherCollider.Velocity;
				Vector2 normal = contact.GetNormal(this);
				float num = contact.Penetration - Vector2.Dot(deltaPos, normal);
				if (!(num > 0f))
				{
					continue;
				}
				deltaPos += num * normal;
				float num2 = 0f - Vector3.Dot(velocity + deltaVel - velocity2, normal);
				if (num2 > 0f)
				{
					if (contact.State == ContactState.New)
					{
						num2 *= 1f + bounce;
					}
					zero.Set(normal.y, 0f - normal.x);
					float num3 = 0f - Vector3.Dot(velocity + deltaVel - velocity2, zero);
					float num4 = friction * num2;
					if (num3 > num4)
					{
						num3 = num4;
					}
					else if (num3 < 0f - num4)
					{
						num3 = 0f - num4;
					}
					deltaVel += num2 * normal + num3 * zero;
				}
			}
		}

		public void ResetVertices()
		{
			if (_isVertexShape)
			{
				_vertices = null;
				_isVerticesTransformed = false;
				RefreshVertexBounds();
				_triangles = null;
			}
			else
			{
				Debug.LogError("[Collider2d] Attempting to reset vertices of non-vertex shape");
			}
		}

		public void AddVertex(Vector2 vertex)
		{
			if (_isVertexShape)
			{
				if (!_isVerticesTransformed)
				{
					if (_vertices == null)
					{
						_vertices = new List<Vector2>();
					}
					if (_vertices != null)
					{
						_vertices.Add(vertex);
						RefreshVertexBounds();
					}
				}
				else
				{
					Debug.LogError("[Collider2d] Attempting to add new vertex after existing vertices have been transformed");
				}
			}
			else
			{
				Debug.LogError("[Collider2d] Attempting to add vertex to non-vertex shape");
			}
		}

		public void TransformVertices()
		{
			if (_isVertexShape)
			{
				if (!_isVerticesTransformed && _vertices != null)
				{
					int count = _vertices.Count;
					RefreshCachedData();
					for (int i = 0; i < count; i++)
					{
						_vertices[i] = _cachedTransform.TransformPoint(_vertices[i]);
					}
					_isVerticesTransformed = true;
					RefreshVertexBounds();
				}
			}
			else
			{
				Debug.LogError("[Collider2d] Attempting to transform vertices of non-vertex shape");
			}
		}

		public List<Vector2> GetTransformedVertices(List<Vector2> vertexBuffer, bool isForceBuffer = false, bool isClearBuffer = true)
		{
			if (isClearBuffer)
			{
				vertexBuffer.Clear();
			}
			if (_isVertexShape)
			{
				int count = _vertices.Count;
				if (_isVerticesTransformed)
				{
					if (isForceBuffer)
					{
						for (int i = 0; i < count; i++)
						{
							vertexBuffer.Add(_vertices[i]);
						}
						return vertexBuffer;
					}
					return _vertices;
				}
				RefreshCachedData();
				for (int j = 0; j < count; j++)
				{
					vertexBuffer.Add(_cachedTransform.TransformPoint(_vertices[j]));
				}
				return vertexBuffer;
			}
			if (_shape == ColliderShape.Sphere)
			{
				vertexBuffer.Add(_centre);
				return vertexBuffer;
			}
			if (_shape == ColliderShape.Box)
			{
				RefreshCachedData();
				Vector2 vector = _cachedTransform.TransformPoint(_offset);
				vertexBuffer.Add(vector - _extents.x * _cachedTransform.Right - _extents.y * _cachedTransform.Up);
				vertexBuffer.Add(vector - _extents.x * _cachedTransform.Right + _extents.y * _cachedTransform.Up);
				vertexBuffer.Add(vector + _extents.x * _cachedTransform.Right - _extents.y * _cachedTransform.Up);
				vertexBuffer.Add(vector + _extents.x * _cachedTransform.Right + _extents.y * _cachedTransform.Up);
				return vertexBuffer;
			}
			Debug.LogError("[Collider2d] Attempting to get transformed vertices of non-vertex shape");
			return null;
		}

		private void RefreshVertexBounds()
		{
			if (_vertices != null)
			{
				int count = _vertices.Count;
				if (count > 0)
				{
					Vector2 vector = _vertices[0];
					Vector2 vector2 = _vertices[0];
					for (int i = 1; i < count; i++)
					{
						if (_vertices[i].x < vector.x)
						{
							vector.x = _vertices[i].x;
						}
						if (_vertices[i].x > vector2.x)
						{
							vector2.x = _vertices[i].x;
						}
						if (_vertices[i].y < vector.y)
						{
							vector.y = _vertices[i].y;
						}
						if (_vertices[i].y > vector2.y)
						{
							vector2.y = _vertices[i].y;
						}
					}
					vector2 += _vertexShapeRadius * Vector2.one;
					vector -= _vertexShapeRadius * Vector2.one;
					_extents = 0.5f * (vector2 - vector);
					_offset = 0.5f * (vector + vector2);
				}
				else
				{
					_extents = Vector2.zero;
					_offset = Vector2.zero;
				}
			}
			else
			{
				_extents = Vector2.zero;
				_offset = Vector2.zero;
			}
			RefreshBounds();
		}

		public void ResetTriangles()
		{
			if (_shape != ColliderShape.Triangles)
			{
				Debug.LogError("[Collider2d] Attempting to reset triangles of non-triangles shape");
			}
			_triangles = null;
		}

		public void AddTriangle(int a, int b, int c)
		{
			if (_shape != ColliderShape.Triangles)
			{
				Debug.LogError("[Collider2d] Attempting to add triangle to non-triangles shape");
				return;
			}
			if (_triangles == null)
			{
				_triangles = new List<int>();
			}
			_triangles.Add(a, b, c);
		}

		public void DebugRenderBounds(Func<Vector3, Vector3> transformPoint, Color colour)
		{
			if (DebugRenderSettings.IsTypeEnabled(DebugRenderSettings.RENDER_TYPE.COLLIDER_2D))
			{
				RefreshCachedData();
				Vector2 centre = _bounds.Centre;
				Vector2 extents = _bounds.Extents;
				_debugRenderVertices[0] = _cachedTransform.Position;
				_debugRenderVertices[1].Set(centre.x - extents.x, centre.y - extents.y, 0f);
				_debugRenderVertices[2].Set(centre.x + extents.x, centre.y - extents.y, 0f);
				_debugRenderVertices[3].Set(centre.x + extents.x, centre.y + extents.y, 0f);
				_debugRenderVertices[4].Set(centre.x - extents.x, centre.y + extents.y, 0f);
				_debugRenderVertices[0] = transformPoint(_debugRenderVertices[0]);
				_debugRenderVertices[1] = transformPoint(_debugRenderVertices[1]);
				_debugRenderVertices[2] = transformPoint(_debugRenderVertices[2]);
				_debugRenderVertices[3] = transformPoint(_debugRenderVertices[3]);
				_debugRenderVertices[4] = transformPoint(_debugRenderVertices[4]);
				DebugDraw.DrawLine(_debugRenderVertices[0], _debugRenderVertices[1], colour);
				DebugDraw.DrawLine(_debugRenderVertices[1], _debugRenderVertices[2], colour);
				DebugDraw.DrawLine(_debugRenderVertices[2], _debugRenderVertices[3], colour);
				DebugDraw.DrawLine(_debugRenderVertices[3], _debugRenderVertices[4], colour);
				DebugDraw.DrawLine(_debugRenderVertices[4], _debugRenderVertices[1], colour);
			}
		}

		public void DebugRenderShape(Func<Vector3, Vector3> transformPoint, Color colour)
		{
			switch (_shape)
			{
			case ColliderShape.Box:
				DebugRenderBox(transformPoint, colour);
				break;
			case ColliderShape.Sphere:
				DebugRenderSphere(transformPoint, colour);
				break;
			case ColliderShape.LineList:
				DebugRenderVertices(transformPoint, colour, false);
				break;
			case ColliderShape.ConvexVertices:
				DebugRenderVertices(transformPoint, colour, true);
				break;
			case ColliderShape.Triangles:
				DebugRenderTriangles(transformPoint, colour);
				break;
			case ColliderShape.Cylinder:
				break;
			}
		}

		private void DebugRenderBox(Func<Vector3, Vector3> transformPoint, Color colour)
		{
			if (DebugRenderSettings.IsTypeEnabled(DebugRenderSettings.RENDER_TYPE.COLLIDER_2D))
			{
				RefreshCachedData();
				Vector2 offset = _offset;
				Vector2 extents = _extents;
				_debugRenderVertices[0] = _cachedTransform.Position;
				_debugRenderVertices[1].Set(offset.x - extents.x, offset.y - extents.y, 0f);
				_debugRenderVertices[2].Set(offset.x + extents.x, offset.y - extents.y, 0f);
				_debugRenderVertices[3].Set(offset.x + extents.x, offset.y + extents.y, 0f);
				_debugRenderVertices[4].Set(offset.x - extents.x, offset.y + extents.y, 0f);
				_debugRenderVertices[0] = transformPoint(_debugRenderVertices[0]);
				_debugRenderVertices[1] = transformPoint(_cachedTransform.TransformPoint(_debugRenderVertices[1]));
				_debugRenderVertices[2] = transformPoint(_cachedTransform.TransformPoint(_debugRenderVertices[2]));
				_debugRenderVertices[3] = transformPoint(_cachedTransform.TransformPoint(_debugRenderVertices[3]));
				_debugRenderVertices[4] = transformPoint(_cachedTransform.TransformPoint(_debugRenderVertices[4]));
				DebugDraw.DrawLine(_debugRenderVertices[0], _debugRenderVertices[1], colour);
				DebugDraw.DrawLine(_debugRenderVertices[1], _debugRenderVertices[2], colour);
				DebugDraw.DrawLine(_debugRenderVertices[2], _debugRenderVertices[3], colour);
				DebugDraw.DrawLine(_debugRenderVertices[3], _debugRenderVertices[4], colour);
				DebugDraw.DrawLine(_debugRenderVertices[4], _debugRenderVertices[1], colour);
			}
		}

		private void DebugRenderSphere(Func<Vector3, Vector3> transformPoint, Color colour)
		{
			if (DebugRenderSettings.IsTypeEnabled(DebugRenderSettings.RENDER_TYPE.COLLIDER_2D))
			{
				RefreshCachedData();
				Vector3 vector = _offset;
				float x = _extents.x;
				float num = Mathf.Cos((float)Math.PI / 8f);
				float num2 = Mathf.Sin((float)Math.PI / 8f);
				_debugRenderVertices[0] = transformPoint(_cachedTransform.Position);
				_debugRenderVertices[1].Set(x, 0f, 0f);
				for (int i = 1; i < 16; i++)
				{
					_debugRenderVertices[i + 1].Set(num * _debugRenderVertices[i].x - num2 * _debugRenderVertices[i].y, num2 * _debugRenderVertices[i].x + num * _debugRenderVertices[i].y, 0f);
					_debugRenderVertices[i] += vector;
					_debugRenderVertices[i] = transformPoint(_cachedTransform.TransformPoint(_debugRenderVertices[i]));
					DebugDraw.DrawLine(_debugRenderVertices[i - 1], _debugRenderVertices[i], colour);
				}
				_debugRenderVertices[16] += vector;
				_debugRenderVertices[16] = transformPoint(_cachedTransform.TransformPoint(_debugRenderVertices[16]));
				DebugDraw.DrawLine(_debugRenderVertices[15], _debugRenderVertices[16], colour);
				DebugDraw.DrawLine(_debugRenderVertices[16], _debugRenderVertices[1], colour);
			}
		}

		private void DebugRenderVertices(Func<Vector3, Vector3> transformPoint, Color colour, bool isClosed)
		{
			if (!DebugRenderSettings.IsTypeEnabled(DebugRenderSettings.RENDER_TYPE.COLLIDER_2D) || _vertices == null || _vertices.Count == 0)
			{
				return;
			}
			RefreshCachedData();
			Vector2 v;
			Action<int, int> action = delegate(int renderVertIndex, int collVertIndex)
			{
				v = _vertices[collVertIndex];
				if (!_isVerticesTransformed)
				{
					v = _cachedTransform.TransformPoint(v);
				}
				_debugRenderVertices[renderVertIndex].Set(v.x, v.y, 0f);
				_debugRenderVertices[renderVertIndex] = transformPoint(_debugRenderVertices[renderVertIndex]);
			};
			action(0, 0);
			Vector3 end = _debugRenderVertices[0];
			for (int num = 1; num < _vertices.Count; num++)
			{
				action(1, num);
				DebugDraw.DrawLine(_debugRenderVertices[0], _debugRenderVertices[1], colour);
				_debugRenderVertices[0] = _debugRenderVertices[1];
			}
			if (isClosed)
			{
				DebugDraw.DrawLine(_debugRenderVertices[0], end, colour);
			}
		}

		private void DebugRenderTriangles(Func<Vector3, Vector3> transformPoint, Color colour)
		{
			if (!DebugRenderSettings.IsTypeEnabled(DebugRenderSettings.RENDER_TYPE.COLLIDER_2D) || _vertices == null || _vertices.Count == 0 || _triangles == null || _triangles.Count == 0)
			{
				return;
			}
			RefreshCachedData();
			Vector2 v;
			Action<int, int> action = delegate(int vertIndex, int triIndex)
			{
				v = _vertices[_triangles[triIndex]];
				if (!_isVerticesTransformed)
				{
					v = _cachedTransform.TransformPoint(v);
				}
				_debugRenderVertices[vertIndex].Set(v.x, v.y, 0f);
				_debugRenderVertices[vertIndex] = transformPoint(_debugRenderVertices[vertIndex]);
			};
			for (int num = 0; num < _triangles.Count - 2; num += 3)
			{
				if (_triangles[num] >= 0 && _triangles[num] < _vertices.Count && _triangles[num + 1] >= 0 && _triangles[num + 1] < _vertices.Count && _triangles[num + 2] >= 0 && _triangles[num + 2] < _vertices.Count)
				{
					action(0, num);
					action(1, num + 1);
					action(2, num + 2);
					DebugDraw.DrawLine(_debugRenderVertices[0], _debugRenderVertices[1], colour);
					DebugDraw.DrawLine(_debugRenderVertices[1], _debugRenderVertices[2], colour);
					DebugDraw.DrawLine(_debugRenderVertices[2], _debugRenderVertices[0], colour);
				}
			}
		}
	}
}
