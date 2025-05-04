using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmuzoPhysics
{
	public static class Collision2d
	{
		public class Result
		{
			public Vector2 _normal;

			public Vector2 _position;

			public float _penetration;

			public float _time;

			public Vector2 _pSurfacePosition1
			{
				get
				{
					return _position;
				}
			}

			public Vector2 _pSurfacePosition2
			{
				get
				{
					return _position - _penetration * _normal;
				}
			}

			public Vector2 _pAveragePosition
			{
				get
				{
					return _position - 0.5f * _penetration * _normal;
				}
			}

			public void Flip()
			{
				_position -= _penetration * _normal;
				_normal = -_normal;
			}
		}

		private struct CachedData
		{
			public Collider2d _collider;

			public Vector2 _boundsMin;

			public Vector2 _boundsMax;

			public Vector2 _boundsCentre;

			public Vector2 _boundsExtents;

			public Vector2 _localOffset;

			public Vector2 _localExtents;

			public Transform2d _transform;

			public Vector2 _velocity;

			public Vector2 _deltaPos;
		}

		private const int _kMaxCaches = 4;

		private const int _kSortMapSize = 16;

		private const float _kSameNormalTolerance = 0.999f;

		private const float GJK_ZERO_TOLERANCE = 0.0001f;

		private static CachedData[] _cachedData = new CachedData[4];

		private static int _numCachesInUse = 0;

		private static Gjk2dSolver _gjk = new Gjk2dSolver();

		private static List<Vector2> _vertexBuffer = new List<Vector2>();

		private static Result[] _resultsBuffer;

		private static int[] _sortMap = new int[16];

		private static Vector2[] TriangleOverlapsRect_vb1 = new Vector2[6];

		private static Vector2[] TriangleOverlapsRect_vb2 = new Vector2[6];

		private static void ResetCaches()
		{
			_numCachesInUse = 0;
		}

		private static int CacheCollider(Collider2d collider)
		{
			if (_numCachesInUse < 4)
			{
				_cachedData[_numCachesInUse]._collider = collider;
				_cachedData[_numCachesInUse]._boundsMin = collider._bounds._min;
				_cachedData[_numCachesInUse]._boundsMax = collider._bounds._max;
				_cachedData[_numCachesInUse]._boundsCentre = 0.5f * (_cachedData[_numCachesInUse]._boundsMin + _cachedData[_numCachesInUse]._boundsMax);
				_cachedData[_numCachesInUse]._boundsExtents = _cachedData[_numCachesInUse]._boundsMax - _cachedData[_numCachesInUse]._boundsCentre;
				_cachedData[_numCachesInUse]._localOffset = collider._offset;
				_cachedData[_numCachesInUse]._localExtents = collider._extents;
				_cachedData[_numCachesInUse]._transform = collider._cachedTransform;
				_cachedData[_numCachesInUse]._velocity = collider._cachedVelocity;
				_cachedData[_numCachesInUse]._deltaPos = collider.DeltaPos;
				return _numCachesInUse++;
			}
			Debug.Log("[Collision2d] Failed to cache collider, all " + 4 + " cache slots in use");
			return -1;
		}

		private static int CacheBounds(ref Vector2 min, ref Vector2 max)
		{
			if (_numCachesInUse < 4)
			{
				_cachedData[_numCachesInUse]._collider = null;
				_cachedData[_numCachesInUse]._boundsMin = min;
				_cachedData[_numCachesInUse]._boundsMax = max;
				_cachedData[_numCachesInUse]._boundsCentre = 0.5f * (_cachedData[_numCachesInUse]._boundsMin + _cachedData[_numCachesInUse]._boundsMax);
				_cachedData[_numCachesInUse]._boundsExtents = _cachedData[_numCachesInUse]._boundsMax - _cachedData[_numCachesInUse]._boundsCentre;
				_cachedData[_numCachesInUse]._localOffset = Vector2.zero;
				_cachedData[_numCachesInUse]._localExtents = Vector2.zero;
				_cachedData[_numCachesInUse]._transform = Transform2d.Identity;
				_cachedData[_numCachesInUse]._velocity = Vector2.zero;
				_cachedData[_numCachesInUse]._deltaPos = Vector2.zero;
				return _numCachesInUse++;
			}
			Debug.Log("[Collision2d] Failed to cache bounds, all " + 4 + " cache slots in use");
			return -1;
		}

		public static bool IsBoundsIntersect(Collider2d collA, Collider2d collB, float dt)
		{
			return collA._bounds.Intersects(ref collB._bounds);
		}

		private static bool IsBoundsIntersect(int cacheA, int cacheB)
		{
			return !(_cachedData[cacheA]._boundsMax.x < _cachedData[cacheB]._boundsMin.x) && !(_cachedData[cacheA]._boundsMin.x > _cachedData[cacheB]._boundsMax.x) && !(_cachedData[cacheA]._boundsMax.y < _cachedData[cacheB]._boundsMin.y) && !(_cachedData[cacheA]._boundsMin.y > _cachedData[cacheB]._boundsMax.y);
		}

		public static bool IsCollidersIntersect(Collider2d collA, Collider2d collB, bool isCheckBounds, float dt, Result result)
		{
			if (isCheckBounds && !IsBoundsIntersect(collA, collB, dt))
			{
				return false;
			}
			CollisionProfiler.Increment("num_passed_bounds_test");
			bool flag = false;
			if (collA.Shape == ColliderShape.Sphere && collB.Shape == ColliderShape.Sphere)
			{
				CollisionProfiler.Increment("num_circle_circle_tests");
				flag = CircleCircleTest(collA, collB, dt, result);
			}
			else if (collA.Shape == ColliderShape.Sphere && collB.Shape == ColliderShape.Box)
			{
				CollisionProfiler.Increment("num_circle_box_tests");
				flag = CircleBoxTest(collA, collB, dt, result);
			}
			else if (collA.Shape == ColliderShape.Box && collB.Shape == ColliderShape.Sphere)
			{
				CollisionProfiler.Increment("num_circle_box_tests");
				flag = CircleBoxTest(collB, collA, dt, result);
				result.Flip();
			}
			else if (collA.Shape == ColliderShape.Box && collA.Rotation == 0f && collB.Shape == ColliderShape.Box && collB.Rotation == 0f)
			{
				CollisionProfiler.Increment("num_axis_aligned_box_tests");
				flag = AxisAlignedBoxBoxTest(collA, collB, dt, result);
			}
			else if ((collA.Shape == ColliderShape.Sphere || collA.Shape == ColliderShape.Box) && collB.Shape == ColliderShape.Triangles)
			{
				CollisionProfiler.Increment("num_circle_triangles_tests");
				flag = PrimitiveTrianglesTest(collA, collB, dt, result);
			}
			else if (collA.Shape == ColliderShape.Triangles && (collB.Shape == ColliderShape.Sphere || collB.Shape == ColliderShape.Box))
			{
				CollisionProfiler.Increment("num_circle_triangles_tests");
				flag = PrimitiveTrianglesTest(collB, collA, dt, result);
				result.Flip();
			}
			else
			{
				CollisionProfiler.Increment("num_generic_tests");
				flag = GenericGjkTest(collA, collB, dt, result);
			}
			return flag;
		}

		public static bool IsCollidersIntersectV2(Collider2d collA, Collider2d collB, bool isCheckBounds, float dt, Result result)
		{
			ResetCaches();
			int num = CacheCollider(collA);
			int num2 = CacheCollider(collB);
			if (isCheckBounds && !IsBoundsIntersect(num, num2))
			{
				return false;
			}
			CollisionProfiler.Increment("num_passed_bounds_test");
			bool flag = false;
			if (collA.Shape == ColliderShape.Sphere && collB.Shape == ColliderShape.Sphere)
			{
				CollisionProfiler.Increment("num_circle_circle_tests");
				flag = CircleCircleTest(num, num2, dt, result);
			}
			else if (collA.Shape == ColliderShape.Sphere && collB.Shape == ColliderShape.Box)
			{
				CollisionProfiler.Increment("num_circle_box_tests");
				flag = CircleBoxTest(num, num2, dt, result);
			}
			else if (collA.Shape == ColliderShape.Box && collB.Shape == ColliderShape.Sphere)
			{
				CollisionProfiler.Increment("num_circle_box_tests");
				flag = CircleBoxTest(num2, num, dt, result);
				result.Flip();
			}
			else if (collA.Shape == ColliderShape.Box && collA.Rotation == 0f && collB.Shape == ColliderShape.Box && collB.Rotation == 0f)
			{
				CollisionProfiler.Increment("num_axis_aligned_box_tests");
				flag = AxisAlignedBoxBoxTest(num, num2, dt, result);
			}
			else if ((collA.Shape == ColliderShape.Sphere || collA.Shape == ColliderShape.Box) && collB.Shape == ColliderShape.Triangles)
			{
				CollisionProfiler.Increment("num_circle_triangles_tests");
				flag = PrimitiveTrianglesTest(num, num2, dt, result);
			}
			else if (collA.Shape == ColliderShape.Triangles && (collB.Shape == ColliderShape.Sphere || collB.Shape == ColliderShape.Box))
			{
				CollisionProfiler.Increment("num_circle_triangles_tests");
				flag = PrimitiveTrianglesTest(num2, num, dt, result);
				result.Flip();
			}
			else
			{
				CollisionProfiler.Increment("num_generic_tests");
				flag = GenericGjkTest(num, num2, dt, result);
			}
			return flag;
		}

		private static bool QuickBoundsTest(Collider2d collA, Collider2d collB, float dt, Result result)
		{
			return QuickBoundsTest(ref collA._bounds._min, ref collA._bounds._max, ref collB._bounds._min, ref collB._bounds._max, result);
		}

		private static bool QuickBoundsTest(int cacheA, int cacheB, float dt, Result result)
		{
			return QuickBoundsTest(ref _cachedData[cacheA]._boundsMin, ref _cachedData[cacheA]._boundsMax, ref _cachedData[cacheB]._boundsMin, ref _cachedData[cacheB]._boundsMax, result);
		}

		private static bool QuickBoundsTest(ref Vector2 minA, ref Vector2 maxA, ref Vector2 minB, ref Vector2 maxB, Result result)
		{
			float num = maxB.y - minA.y;
			float num2 = maxA.y - minB.y;
			float num3 = maxA.x - minB.x;
			float num4 = maxB.x - minA.x;
			if (num < num2)
			{
				if (num < num3)
				{
					if (num < num4)
					{
						result._normal = Vector2.up;
						result._penetration = num;
					}
					else
					{
						result._normal = Vector2.right;
						result._penetration = num4;
					}
				}
				else if (num3 < num4)
				{
					result._normal = -Vector2.right;
					result._penetration = num3;
				}
				else
				{
					result._normal = Vector2.right;
					result._penetration = num4;
				}
			}
			else if (num2 < num3)
			{
				if (num2 < num4)
				{
					result._normal = -Vector2.up;
					result._penetration = num2;
				}
				else
				{
					result._normal = Vector2.right;
					result._penetration = num4;
				}
			}
			else if (num3 < num4)
			{
				result._normal = -Vector2.right;
				result._penetration = num3;
			}
			else
			{
				result._normal = Vector2.right;
				result._penetration = num4;
			}
			Vector2 vector = minA;
			Vector2 vector2 = maxA;
			if (vector.x < minB.x)
			{
				vector.x = minB.x;
			}
			if (vector.y < minB.y)
			{
				vector.y = minB.y;
			}
			if (vector2.x > maxB.x)
			{
				vector2.x = maxB.x;
			}
			if (vector2.y > maxB.y)
			{
				vector2.y = maxB.y;
			}
			Vector2 vector3 = 0.5f * (vector + vector2);
			result._position = vector3 + 0.5f * result._penetration * result._normal;
			result._time = 0f;
			return result._penetration >= 0f;
		}

		private static Result[] _GetResultsBuffer(int numResults)
		{
			if (_resultsBuffer == null || _resultsBuffer.Length < numResults)
			{
				_resultsBuffer = new Result[numResults];
				for (int i = 0; i < numResults; i++)
				{
					_resultsBuffer[i] = new Result();
				}
			}
			return _resultsBuffer;
		}

		private static bool AxisAlignedBoxBoxTest(Collider2d collA, Collider2d collB, float dt, Result result)
		{
			Vector2 vector = collA._bounds.Centre - collA._bounds.Extents;
			Vector2 vector2 = collA._bounds.Centre + collA._bounds.Extents;
			Vector2 vector3 = collB._bounds.Centre - collB._bounds.Extents;
			Vector2 vector4 = collB._bounds.Centre + collB._bounds.Extents;
			Vector2 vector5 = vector;
			Vector2 vector6 = vector2;
			if (vector5.x < vector3.x)
			{
				vector5.x = vector3.x;
			}
			if (vector5.y < vector3.y)
			{
				vector5.y = vector3.y;
			}
			if (vector6.x > vector4.x)
			{
				vector6.x = vector4.x;
			}
			if (vector6.y > vector4.y)
			{
				vector6.y = vector4.y;
			}
			Vector2 position = 0.5f * (vector5 + vector6);
			Result[] array = _GetResultsBuffer(4);
			array[0]._normal = Vector2.up;
			array[0]._position = position;
			array[0]._position.y = vector4.y;
			array[0]._penetration = vector4.y - vector.y;
			array[1]._normal = -Vector2.up;
			array[1]._position = position;
			array[1]._position.y = vector3.y;
			array[1]._penetration = vector2.y - vector3.y;
			array[2]._normal = -Vector2.right;
			array[2]._position = position;
			array[2]._position.x = vector3.x;
			array[2]._penetration = vector2.x - vector3.x;
			array[3]._normal = Vector2.right;
			array[3]._position = position;
			array[3]._position.x = vector4.x;
			array[3]._penetration = vector4.x - vector.x;
			int num = ChooseResult(collA, collB, dt, array, 4);
			if (0 <= num && num < 4)
			{
				result._normal = array[num]._normal;
				result._position = array[num]._position;
				result._penetration = array[num]._penetration;
			}
			result._time = 0f;
			return result._penetration >= 0f;
		}

		private static bool AxisAlignedBoxBoxTest(int cacheA, int cacheB, float dt, Result result)
		{
			Vector2 boundsMin = _cachedData[cacheA]._boundsMin;
			Vector2 boundsMax = _cachedData[cacheA]._boundsMax;
			Vector2 boundsMin2 = _cachedData[cacheB]._boundsMin;
			Vector2 boundsMax2 = _cachedData[cacheB]._boundsMax;
			Vector2 vector = boundsMin;
			Vector2 vector2 = boundsMax;
			if (vector.x < boundsMin2.x)
			{
				vector.x = boundsMin2.x;
			}
			if (vector.y < boundsMin2.y)
			{
				vector.y = boundsMin2.y;
			}
			if (vector2.x > boundsMax2.x)
			{
				vector2.x = boundsMax2.x;
			}
			if (vector2.y > boundsMax2.y)
			{
				vector2.y = boundsMax2.y;
			}
			Vector2 position = 0.5f * (vector + vector2);
			Result[] array = _GetResultsBuffer(4);
			array[0]._normal = Vector2.up;
			array[0]._position = position;
			array[0]._position.y = boundsMax2.y;
			array[0]._penetration = boundsMax2.y - boundsMin.y;
			array[1]._normal = -Vector2.up;
			array[1]._position = position;
			array[1]._position.y = boundsMin2.y;
			array[1]._penetration = boundsMax.y - boundsMin2.y;
			array[2]._normal = -Vector2.right;
			array[2]._position = position;
			array[2]._position.x = boundsMin2.x;
			array[2]._penetration = boundsMax.x - boundsMin2.x;
			array[3]._normal = Vector2.right;
			array[3]._position = position;
			array[3]._position.x = boundsMax2.x;
			array[3]._penetration = boundsMax2.x - boundsMin.x;
			int num = ChooseResult(cacheA, cacheB, dt, array, 4);
			if (0 <= num && num < 4)
			{
				result._normal = array[num]._normal;
				result._position = array[num]._position;
				result._penetration = array[num]._penetration;
			}
			result._time = 0f;
			return result._penetration >= 0f;
		}

		private static int SortResultsByPenetration(Result[] results, int numResults)
		{
			int num = 0;
			int num2 = numResults - 1;
			for (int i = 0; i < numResults; i++)
			{
				_sortMap[i] = i;
			}
			int num3;
			do
			{
				num3 = 0;
				for (int j = 0; j < num2; j++)
				{
					if (results[_sortMap[j]]._penetration > results[_sortMap[j + 1]]._penetration)
					{
						int num4 = _sortMap[j];
						_sortMap[j] = _sortMap[j + 1];
						_sortMap[j + 1] = num4;
						num3++;
					}
				}
				num += num3;
			}
			while (num3 > 0);
			return num;
		}

		private static int ChooseResult(Collider2d collA, Collider2d collB, float dt, Result[] results, int numResults)
		{
			int i = -1;
			Vector2 lhs = collA.DeltaPos - collB.DeltaPos;
			Contact2d contact2d = collA.FindContactWith(collB);
			if (contact2d != null)
			{
				Vector2 normal = contact2d.GetNormal(collA);
				float num = 0f;
				for (int j = 0; j < numResults; j++)
				{
					num = Vector2.Dot(results[j]._normal, normal);
					if (num >= 0.999f)
					{
						i = j;
						break;
					}
				}
			}
			else
			{
				if (numResults > 16)
				{
					Debug.LogWarning("[Collision2d] Only sorting first " + 16 + " results!");
					numResults = 16;
				}
				SortResultsByPenetration(results, numResults);
				for (i = 0; i < numResults && Vector2.Dot(lhs, results[_sortMap[i]]._normal) > 0f; i++)
				{
				}
				if (i < numResults)
				{
					i = _sortMap[i];
				}
			}
			if (i >= numResults)
			{
				i = -1;
			}
			return i;
		}

		private static int ChooseResult(int cacheA, int cacheB, float dt, Result[] results, int numResults)
		{
			int i = -1;
			Vector2 lhs = _cachedData[cacheA]._deltaPos - _cachedData[cacheB]._deltaPos;
			Contact2d contact2d = _cachedData[cacheA]._collider.FindContactWith(_cachedData[cacheB]._collider);
			if (contact2d != null)
			{
				Vector2 normal = contact2d.GetNormal(_cachedData[cacheA]._collider);
				float num = 0f;
				for (int j = 0; j < numResults; j++)
				{
					num = Vector2.Dot(results[j]._normal, normal);
					if (num >= 0.999f)
					{
						i = j;
						break;
					}
				}
			}
			else
			{
				if (numResults > 16)
				{
					Debug.LogWarning("[Collision2d] Only sorting first " + 16 + " results!");
					numResults = 16;
				}
				SortResultsByPenetration(results, numResults);
				for (i = 0; i < numResults && Vector2.Dot(lhs, results[_sortMap[i]]._normal) > 0f; i++)
				{
				}
				if (i < numResults)
				{
					i = _sortMap[i];
				}
			}
			if (i >= numResults)
			{
				i = -1;
			}
			return i;
		}

		private static bool CircleCircleTest(Collider2d circleA, Collider2d circleB, float dt, Result result)
		{
			result._normal = circleA.Centre - circleB.Centre;
			result._penetration = circleA.Extents.x + circleB.Extents.x - result._normal.magnitude;
			result._normal.Normalize();
			result._position = circleB.Centre + circleB.Extents.x * result._normal;
			result._time = 0f;
			return result._penetration >= 0f;
		}

		private static bool CircleCircleTest(int cacheA, int cacheB, float dt, Result result)
		{
			result._normal = _cachedData[cacheA]._boundsCentre - _cachedData[cacheB]._boundsCentre;
			result._penetration = _cachedData[cacheA]._localExtents.x + _cachedData[cacheB]._localExtents.x - result._normal.magnitude;
			result._normal.Normalize();
			result._position = _cachedData[cacheB]._boundsCentre + _cachedData[cacheB]._localExtents.x * result._normal;
			result._time = 0f;
			return result._penetration >= 0f;
		}

		private static bool CircleBoxTest(Collider2d circle, Collider2d box, float dt, Result result)
		{
			Vector2 minA = box._cachedTransform.InverseTransformPoint(circle.Position);
			Vector2 minB = box.Offset - box.Extents;
			Vector2 maxB = box.Offset + box.Extents;
			Vector2 zero = Vector2.zero;
			if (minA.x < minB.x)
			{
				zero.x = minB.x;
				if (minA.y < minB.y)
				{
					zero.y = minB.y;
				}
				else if (minA.y > maxB.y)
				{
					zero.y = maxB.y;
				}
				else
				{
					zero.y = minA.y;
				}
			}
			else if (minA.x > maxB.x)
			{
				zero.x = maxB.x;
				if (minA.y < minB.y)
				{
					zero.y = minB.y;
				}
				else if (minA.y > maxB.y)
				{
					zero.y = maxB.y;
				}
				else
				{
					zero.y = minA.y;
				}
			}
			else
			{
				zero.x = minA.x;
				if (minA.y < minB.y)
				{
					zero.y = minB.y;
				}
				else if (minA.y > maxB.y)
				{
					zero.y = maxB.y;
				}
				else
				{
					zero.y = minA.y;
				}
			}
			if (zero == minA)
			{
				bool flag = QuickBoundsTest(ref minA, ref minA, ref minB, ref maxB, result);
				if (flag)
				{
					result._normal = box._cachedTransform.TransformDirection(result._normal);
				}
				return flag;
			}
			result._normal = box._cachedTransform.TransformDirection((minA - zero).normalized);
			result._position = box._cachedTransform.TransformPoint(zero);
			result._penetration = circle.Extents.x - (minA - zero).magnitude;
			result._time = 0f;
			return result._penetration >= 0f;
		}

		private static bool CircleBoxTest(int cacheCircle, int cacheBox, float dt, Result result)
		{
			Vector2 minA = _cachedData[cacheBox]._transform.InverseTransformPoint(_cachedData[cacheCircle]._transform._position);
			Vector2 minB = _cachedData[cacheBox]._localOffset - _cachedData[cacheBox]._localExtents;
			Vector2 maxB = _cachedData[cacheBox]._localOffset + _cachedData[cacheBox]._localExtents;
			Vector2 zero = Vector2.zero;
			if (minA.x < minB.x)
			{
				zero.x = minB.x;
				if (minA.y < minB.y)
				{
					zero.y = minB.y;
				}
				else if (minA.y > maxB.y)
				{
					zero.y = maxB.y;
				}
				else
				{
					zero.y = minA.y;
				}
			}
			else if (minA.x > maxB.x)
			{
				zero.x = maxB.x;
				if (minA.y < minB.y)
				{
					zero.y = minB.y;
				}
				else if (minA.y > maxB.y)
				{
					zero.y = maxB.y;
				}
				else
				{
					zero.y = minA.y;
				}
			}
			else
			{
				zero.x = minA.x;
				if (minA.y < minB.y)
				{
					zero.y = minB.y;
				}
				else if (minA.y > maxB.y)
				{
					zero.y = maxB.y;
				}
				else
				{
					zero.y = minA.y;
				}
			}
			if (zero == minA)
			{
				bool flag = QuickBoundsTest(ref minA, ref minA, ref minB, ref maxB, result);
				if (flag)
				{
					result._normal = _cachedData[cacheBox]._transform.TransformDirection(result._normal);
				}
				return flag;
			}
			result._normal = _cachedData[cacheBox]._transform.TransformDirection((minA - zero).normalized);
			result._position = _cachedData[cacheBox]._transform.TransformPoint(zero);
			result._penetration = _cachedData[cacheCircle]._localExtents.x - (minA - zero).magnitude;
			result._time = 0f;
			return result._penetration >= 0f;
		}

		private static bool PrimitiveTrianglesTest(Collider2d primCollider, Collider2d trianglesCollider, float dt, Result result)
		{
			if (_gjk == null)
			{
				return false;
			}
			primCollider.GetTransformedVertices(_vertexBuffer, true);
			int numPrimVerts = _vertexBuffer.Count;
			trianglesCollider.GetTransformedVertices(_vertexBuffer, true, false);
			float minSeparation = float.MaxValue;
			Vector2 minPointA = Vector2.zero;
			Vector2 minPointB = Vector2.zero;
			Vector2 posDiff;
			float separation;
			Utils.ProcessTriangles(trianglesCollider._pTriangles, delegate(int i, int a, int b, int c)
			{
				_gjk.Reset();
				for (int j = 0; j < numPrimVerts; j++)
				{
					_gjk.AddVertexA(_vertexBuffer[j]);
				}
				_gjk.AddVertexB(_vertexBuffer[numPrimVerts + a]);
				_gjk.AddVertexB(_vertexBuffer[numPrimVerts + b]);
				_gjk.AddVertexB(_vertexBuffer[numPrimVerts + c]);
				_gjk.Solve();
				posDiff = _gjk.ResultPointA - _gjk.ResultPointB;
				separation = posDiff.magnitude - (primCollider.VertexShapeRadius + trianglesCollider.VertexShapeRadius);
				if (separation < minSeparation)
				{
					minSeparation = separation;
					minPointA = _gjk.ResultPointA;
					minPointB = _gjk.ResultPointB;
				}
				return separation > 0.0001f;
			});
			if (minSeparation == float.MaxValue)
			{
				return false;
			}
			result._normal = minPointA - minPointB;
			result._normal.Normalize();
			result._penetration = 0f - minSeparation;
			result._position = minPointB + trianglesCollider.VertexShapeRadius * result._normal;
			result._time = 0f;
			return result._penetration >= -0.0001f;
		}

		private static bool PrimitiveTrianglesTest(int cachePrim, int cacheTriangles, float dt, Result result)
		{
			return PrimitiveTrianglesTest(_cachedData[cachePrim]._collider, _cachedData[cacheTriangles]._collider, dt, result);
		}

		private static bool GenericGjkTest(Collider2d collA, Collider2d collB, float dt, Result result)
		{
			collA.GetTransformedVertices(_vertexBuffer, true);
			int count = _vertexBuffer.Count;
			collB.GetTransformedVertices(_vertexBuffer, true, false);
			int count2 = _vertexBuffer.Count;
			int num = count2 - count;
			if (count > 0 && num > 0 && _gjk != null)
			{
				_gjk.Reset();
				for (int i = 0; i < count; i++)
				{
					_gjk.AddVertexA(_vertexBuffer[i]);
				}
				for (int j = count; j < count2; j++)
				{
					_gjk.AddVertexB(_vertexBuffer[j]);
				}
				_gjk.Solve();
				result._normal = _gjk.ResultPointA - _gjk.ResultPointB;
				result._penetration = collA.VertexShapeRadius + collB.VertexShapeRadius - result._normal.magnitude;
				result._normal.Normalize();
				result._position = _gjk.ResultPointB + collB.VertexShapeRadius * result._normal;
				result._time = 0f;
				return result._penetration >= -0.0001f;
			}
			return false;
		}

		private static bool GenericGjkTest(int cacheA, int cacheB, float dt, Result result)
		{
			return GenericGjkTest(_cachedData[cacheA]._collider, _cachedData[cacheB]._collider, dt, result);
		}

		public static bool IsRayIntersect(Ray2d ray, Collider2d collider, bool isCheckBounds, Result result)
		{
			if (isCheckBounds && !ray._pBounds.Intersects(ref collider._bounds))
			{
				return false;
			}
			bool result2 = false;
			switch (collider.Shape)
			{
			case ColliderShape.Sphere:
				result2 = RayCircleTest(ray, collider, result);
				break;
			case ColliderShape.Box:
				result2 = RayBoxTest(ray, collider, result);
				break;
			default:
				Debug.LogWarning("[Collision2d] Raycast with collider type '" + collider.Shape.ToString() + "' is unsupported");
				break;
			}
			return result2;
		}

		private static bool RayCircleTest(Ray2d ray, Collider2d circle, Result result)
		{
			float? num = RayCircleTest(ray, circle._cachedTransform._position, circle._extents.x);
			if (!num.HasValue)
			{
				return false;
			}
			if (num.Value < 0f || num.Value > 1f)
			{
				return false;
			}
			result._position = ray._pStart + num.Value * ray._pVector;
			result._normal = result._position - circle._cachedTransform._position;
			result._normal.Normalize();
			result._penetration = 0f;
			result._time = num.Value;
			return true;
		}

		private static bool RayBoxTest(Ray2d ray, Collider2d box, Result result)
		{
			Vector2 vector = box._cachedTransform.InverseTransformPoint(ray._pStart);
			Vector2 vector2 = box._cachedTransform.InverseTransformDirection(ray._pVector);
			Vector2 vector3 = vector + vector2;
			Vector2 vector4 = box.Offset - box.Extents;
			Vector2 vector5 = box.Offset + box.Extents;
			Vector2 vector6 = Vector2.zero;
			Vector2 zero = Vector2.zero;
			float time = 0f;
			if (vector2.x > 0f)
			{
				if (vector.x <= vector4.x && vector3.x >= vector4.x)
				{
					float num = MathHelper.Lerp(vector.x, vector.y, vector3.x, vector3.y, vector4.x);
					if (num >= vector4.y && num <= vector5.y)
					{
						vector6 = -Vector2.right;
						zero.x = vector4.x;
						zero.y = num;
						time = (vector4.x - vector.x) / vector2.x;
					}
				}
			}
			else if (vector2.x < 0f && vector.x >= vector5.x && vector3.x <= vector5.x)
			{
				float num = MathHelper.Lerp(vector.x, vector.y, vector3.x, vector3.y, vector5.x);
				if (num >= vector4.y && num <= vector5.y)
				{
					vector6 = Vector2.right;
					zero.x = vector5.x;
					zero.y = num;
					time = (vector5.x - vector.x) / vector2.x;
				}
			}
			if (vector2.y > 0f)
			{
				if (vector.y <= vector4.y && vector3.y >= vector4.y)
				{
					float num = MathHelper.Lerp(vector.y, vector.x, vector3.y, vector3.x, vector4.y);
					if (num >= vector4.x && num <= vector5.x)
					{
						vector6 = -Vector2.up;
						zero.x = num;
						zero.y = vector4.y;
						time = (vector4.y - vector.y) / vector2.y;
					}
				}
			}
			else if (vector2.y < 0f && vector.y >= vector5.y && vector3.y <= vector5.y)
			{
				float num = MathHelper.Lerp(vector.y, vector.x, vector3.y, vector3.x, vector5.y);
				if (num >= vector4.x && num <= vector5.x)
				{
					vector6 = Vector2.up;
					zero.x = num;
					zero.y = vector5.y;
					time = (vector5.y - vector.y) / vector2.y;
				}
			}
			if (vector6 == Vector2.zero)
			{
				return false;
			}
			result._normal = box._cachedTransform.TransformDirection(vector6);
			result._position = box._cachedTransform.TransformPoint(zero);
			result._penetration = 0f;
			result._time = time;
			return true;
		}

		private static float? RayCircleTest(Ray2d ray, Vector2 circlePos, float circleRadius)
		{
			float? result = null;
			Vector2 lhs = ray._pStart - circlePos;
			float r;
			float r2;
			if (MathHelper.SolveQuadratic(ray._pVector.sqrMagnitude, 2f * Vector2.Dot(lhs, ray._pVector), lhs.sqrMagnitude - circleRadius * circleRadius, out r, out r2))
			{
				result = Mathf.Min(r, r2);
			}
			return result;
		}

		public static bool ColliderContainsPoint(Collider2d collider, Vector2 point)
		{
			bool isOutside = true;
			if (collider.Shape == ColliderShape.Box)
			{
				point = collider.Transform.InverseTransformPoint(point) - collider.Offset;
				isOutside = Mathf.Abs(point.x) > collider.Extents.x || Mathf.Abs(point.y) > collider.Extents.y;
			}
			else if (collider.Shape == ColliderShape.Sphere)
			{
				point -= collider.Centre;
				isOutside = point.sqrMagnitude > collider.Extents.x * collider.Extents.x;
			}
			else if (collider.Shape == ColliderShape.LineList)
			{
				List<Vector2> transformedVertices = collider.GetTransformedVertices(_vertexBuffer);
				int num = ((transformedVertices != null) ? transformedVertices.Count : 0);
				if (num > 0)
				{
					Bounds2d invalid = Bounds2d.Invalid;
					Vector2 lineEnd = transformedVertices[0];
					float num2 = collider.VertexShapeRadius * collider.VertexShapeRadius;
					for (int i = 1; i < num; i++)
					{
						if (!isOutside)
						{
							break;
						}
						Vector2 lineStart = lineEnd;
						lineEnd = transformedVertices[i];
						invalid.SetPoint(lineStart);
						invalid.AddPoint(lineEnd);
						invalid.AddPadding(collider.VertexShapeRadius);
						if (invalid.Contains(ref point))
						{
							Vector2 vector = ClosestPointOnLine(ref lineStart, ref lineEnd, ref point);
							float sqrMagnitude = (point - vector).sqrMagnitude;
							if (sqrMagnitude <= num2)
							{
								isOutside = false;
							}
						}
					}
				}
			}
			else if (collider.Shape == ColliderShape.ConvexVertices)
			{
				List<Vector2> transformedVertices2 = collider.GetTransformedVertices(_vertexBuffer);
				int num3 = ((transformedVertices2 != null) ? transformedVertices2.Count : 0);
				if (num3 > 0 && _gjk != null)
				{
					_gjk.Reset();
					_gjk.AddVertexA(point);
					for (int j = 0; j < num3; j++)
					{
						_gjk.AddVertexB(transformedVertices2[j]);
					}
					_gjk.Solve();
					float sqrMagnitude2 = (point - _gjk.ResultPointB).sqrMagnitude;
					float num4 = collider.VertexShapeRadius + 0.0001f;
					isOutside = sqrMagnitude2 > num4 * num4;
				}
			}
			else if (collider.Shape == ColliderShape.Triangles)
			{
				List<Vector2> vertices = collider.GetTransformedVertices(_vertexBuffer);
				Utils.ProcessTriangles(collider._pTriangles, delegate(int num5, int a, int b, int c)
				{
					if (TriangleContainsPoint(vertices[a], vertices[b], vertices[c], point, false))
					{
						isOutside = false;
						return false;
					}
					return true;
				});
			}
			return !isOutside;
		}

		private static Vector2 ClosestPointOnLine(ref Vector2 lineStart, ref Vector2 lineEnd, ref Vector2 toPoint)
		{
			Vector2 zero = Vector2.zero;
			float num = Vector2.Dot(toPoint - lineEnd, lineStart - lineEnd);
			float num2 = Vector2.Dot(toPoint - lineStart, lineEnd - lineStart);
			if (num > 0f && num2 > 0f)
			{
				num /= num + num2;
				num2 = 1f - num;
			}
			else if (num2 <= 0f)
			{
				num = 1f;
				num2 = 0f;
			}
			else
			{
				num = 0f;
				num2 = 1f;
			}
			return num * lineStart + num2 * lineEnd;
		}

		public static bool TriangleContainsPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 p, bool isStrictInside)
		{
			float num = Vector2.Dot(p, a);
			float num2 = Vector2.Dot(p, b);
			float num3 = Vector2.Dot(p, c);
			float num4 = Vector2.Dot(a, a);
			float num5 = Vector2.Dot(a, b);
			float num6 = Vector2.Dot(a, c);
			float num7 = num5;
			float num8 = Vector2.Dot(b, b);
			float num9 = Vector2.Dot(b, c);
			float num10 = num6;
			float num11 = num9;
			float num12 = Vector2.Dot(c, c);
			float num13 = (num - num5 - num2 + num8) * (num - num6 - num3 + num12) - (num - num6 - num2 + num9) * (num - num5 - num3 + num11);
			float num14 = (num2 - num9 - num3 + num12) * (num2 - num7 - num + num4) - (num2 - num7 - num3 + num10) * (num2 - num9 - num + num6);
			float num15 = (num3 - num10 - num + num4) * (num3 - num11 - num2 + num8) - (num3 - num11 - num + num5) * (num3 - num10 - num2 + num7);
			if (isStrictInside)
			{
				return num13 > 0f && num14 > 0f && num15 > 0f;
			}
			return num13 >= 0f && num14 >= 0f && num15 >= 0f;
		}

		public static bool TriangleOverlapsRect(Vector2 a, Vector2 b, Vector2 c, Rect r)
		{
			Vector2[] iv = TriangleOverlapsRect_vb1;
			Vector2[] ov = TriangleOverlapsRect_vb2;
			iv[0] = a;
			iv[1] = b;
			iv[2] = c;
			int ivc = 3;
			int ovc = 0;
			float xMin = r.x;
			float yMin = r.y;
			float xMax = r.x + r.width;
			float yMax = r.y + r.height;
			Action<Predicate<int>, Func<int, int, float>, Func<int, int, float>> action = delegate(Predicate<int> testInside, Func<int, int, float> isectX, Func<int, int, float> isectY)
			{
				int i = 0;
				int num = ivc - 1;
				for (; i < ivc; i++)
				{
					if (testInside(i))
					{
						if (!testInside(num))
						{
							ov[ovc].x = isectX(num, i);
							ov[ovc].y = isectY(num, i);
							ovc++;
						}
						ov[ovc].x = iv[i].x;
						ov[ovc].y = iv[i].y;
						ovc++;
					}
					else if (testInside(num))
					{
						ov[ovc].x = isectX(num, i);
						ov[ovc].y = isectY(num, i);
						ovc++;
					}
					num = i;
				}
			};
			action((int i) => iv[i].x >= xMin, (int s, int e) => xMin, (int s, int e) => MathHelper.Lerp(iv[s].x, iv[s].y, iv[e].x, iv[e].y, xMin));
			iv = TriangleOverlapsRect_vb2;
			ov = TriangleOverlapsRect_vb1;
			ivc = ovc;
			ovc = 0;
			action((int i) => iv[i].x <= xMax, (int s, int e) => xMax, (int s, int e) => MathHelper.Lerp(iv[s].x, iv[s].y, iv[e].x, iv[e].y, xMax));
			iv = TriangleOverlapsRect_vb1;
			ov = TriangleOverlapsRect_vb2;
			ivc = ovc;
			ovc = 0;
			action((int i) => iv[i].y >= yMin, (int s, int e) => MathHelper.Lerp(iv[s].y, iv[s].x, iv[e].y, iv[e].x, yMin), (int s, int e) => yMin);
			iv = TriangleOverlapsRect_vb2;
			ov = TriangleOverlapsRect_vb1;
			ivc = ovc;
			ovc = 0;
			action((int i) => iv[i].y <= yMax, (int s, int e) => MathHelper.Lerp(iv[s].y, iv[s].x, iv[e].y, iv[e].x, yMax), (int s, int e) => yMax);
			return ovc > 0;
		}
	}
}
