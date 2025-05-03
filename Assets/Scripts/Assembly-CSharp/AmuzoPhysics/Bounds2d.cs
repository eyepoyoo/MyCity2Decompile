using System;
using GameDefines;
using UnityEngine;

namespace AmuzoPhysics
{
	[Serializable]
	public struct Bounds2d
	{
		public static Bounds2d Invalid = new Bounds2d(Vector2.one, -Vector2.one);

		public Vector2 _min;

		public Vector2 _max;

		public Vector2 Min
		{
			get
			{
				return _min;
			}
			set
			{
				_min = value;
			}
		}

		public Vector2 Max
		{
			get
			{
				return _max;
			}
			set
			{
				_max = value;
			}
		}

		public Vector2 Centre
		{
			get
			{
				return 0.5f * (_min + _max);
			}
			set
			{
				SetCentreExtents(value, Extents);
			}
		}

		public Vector2 Extents
		{
			get
			{
				return 0.5f * (_max - _min);
			}
			set
			{
				SetCentreExtents(Centre, value);
			}
		}

		public Vector2 Size
		{
			get
			{
				return 2f * Extents;
			}
			set
			{
				Extents = 0.5f * value;
			}
		}

		private float _pointX
		{
			set
			{
				_min.x = (_max.x = value);
			}
		}

		private float _pointY
		{
			set
			{
				_min.y = (_max.y = value);
			}
		}

		public Vector2 Point
		{
			set
			{
				_pointX = value.x;
				_pointY = value.y;
			}
		}

		private bool _isValidX
		{
			get
			{
				return _min.x <= _max.x;
			}
		}

		private bool _isValidY
		{
			get
			{
				return _min.y <= _max.y;
			}
		}

		public bool IsValid
		{
			get
			{
				return _isValidX && _isValidY;
			}
		}

		public Bounds2d(Bounds2d bounds)
		{
			_min = bounds._min;
			_max = bounds._max;
		}

		public Bounds2d(Vector2 point)
		{
			_min = GlobalDefines.INVALID_VECTOR_2;
			_max = GlobalDefines.INVALID_VECTOR_2;
			Point = point;
		}

		public Bounds2d(Vector2 min, Vector2 max)
		{
			_min = min;
			_max = max;
		}

		public void Reset()
		{
			_min = Vector2.one;
			_max = -Vector2.one;
		}

		private void SetCentreExtents(Vector2 centre, Vector2 extents)
		{
			_min = centre - extents;
			_max = centre + extents;
		}

		private static bool CheckSeparation(Bounds2d boundsA, Bounds2d boundsB)
		{
			return boundsA._max.x < boundsB._min.x || boundsA._min.x > boundsB._max.x || boundsA._max.y < boundsB._min.y || boundsA._min.y > boundsB._max.y;
		}

		private static bool CheckEncapsulation(Bounds2d bounds, Vector2 point)
		{
			return bounds._min.x <= point.x && point.x <= bounds._max.x && bounds._min.y <= point.y && point.y <= bounds._max.y;
		}

		private static bool CheckEncapsulation(Bounds2d boundsA, Bounds2d boundsB)
		{
			return boundsA._min.x <= boundsB._min.x && boundsB._max.x <= boundsA._max.x && boundsA._min.y <= boundsB._min.y && boundsB._max.y <= boundsA._max.y;
		}

		public bool Intersects(ref Bounds2d other)
		{
			return !(_max.x < other._min.x) && !(_min.x > other._max.x) && !(_max.y < other._min.y) && !(_min.y > other._max.y);
		}

		public bool Contains(ref Vector2 point)
		{
			return _min.x <= point.x && point.x <= _max.x && _min.y <= point.y && point.y <= _max.y;
		}

		public bool Contains(ref Bounds2d other)
		{
			return _min.x <= other._min.x && other._max.x <= _max.x && _min.y <= other._min.y && other._max.y <= _max.y;
		}

		public void AddPadding(float leftPadding, float rightPadding, float upPadding, float downPadding)
		{
			_min.x -= leftPadding;
			_max.x += rightPadding;
			_min.y -= downPadding;
			_max.y += upPadding;
		}

		public void AddPadding(float padding)
		{
			AddPadding(padding, padding, padding, padding);
		}

		public void AddPoint(Vector2 point)
		{
			if (_isValidX)
			{
				if (point.x < _min.x)
				{
					_min.x = point.x;
				}
				if (point.x > _max.x)
				{
					_max.x = point.x;
				}
			}
			else
			{
				_pointX = point.x;
			}
			if (_isValidY)
			{
				if (point.y < _min.y)
				{
					_min.y = point.y;
				}
				if (point.y > _max.y)
				{
					_max.y = point.y;
				}
			}
			else
			{
				_pointY = point.y;
			}
		}

		public void SetPoint(Vector2 point)
		{
			_min.x = (_max.x = point.x);
			_min.y = (_max.y = point.y);
		}

		public void AddBounds(Bounds2d bounds)
		{
			if (_isValidX)
			{
				if (bounds._min.x < _min.x)
				{
					_min.x = bounds._min.x;
				}
				if (bounds._max.x > _max.x)
				{
					_max.x = bounds._max.x;
				}
			}
			else
			{
				_min.x = bounds._min.x;
				_max.x = bounds._max.x;
			}
			if (_isValidY)
			{
				if (bounds._min.y < _min.y)
				{
					_min.y = bounds._min.y;
				}
				if (bounds._max.y > _max.y)
				{
					_max.y = bounds._max.y;
				}
			}
			else
			{
				_min.y = bounds._min.y;
				_max.y = bounds._max.y;
			}
		}

		public void Rotate(float angle)
		{
			Centre = MathHelper.RotateVector2(Centre, angle);
			Extents = MathHelper.RotateExtents2(Extents, angle);
		}

		public void Transform(Transform2d transform)
		{
			Centre = transform.TransformPoint(Centre);
			Extents = MathHelper.RotateExtents2(Extents, transform.Rotation);
		}
	}
}
