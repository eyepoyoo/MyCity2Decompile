using UnityEngine;

namespace AmuzoPhysics
{
	public class Ray2d
	{
		private const string LOG_TAG = "[Ray2d] ";

		private Vector2 _pos;

		private Vector2 _dir;

		private float _len;

		private Bounds2d _bounds;

		public Vector2 _pStart
		{
			get
			{
				return _pos;
			}
		}

		public Vector2 _pEnd
		{
			get
			{
				return _pos + _len * _dir;
			}
		}

		public Vector2 _pDirection
		{
			get
			{
				return _dir;
			}
		}

		public Vector2 _pVector
		{
			get
			{
				return _len * _dir;
			}
		}

		public float _pLength
		{
			get
			{
				return _len;
			}
		}

		public Bounds2d _pBounds
		{
			get
			{
				return _bounds;
			}
		}

		public Ray2d(Vector2 pos, Vector2 vec)
		{
			_pos = pos;
			_len = vec.magnitude;
			if (_len > 0f)
			{
				_dir = 1f / _len * vec;
			}
			else
			{
				_dir = Vector2.zero;
			}
			RefreshBounds();
		}

		public Ray2d(Vector2 pos, Vector2 vec, float scale)
		{
			_pos = pos;
			_len = vec.magnitude;
			if (_len > 0f)
			{
				_dir = 1f / _len * vec;
			}
			else
			{
				_dir = Vector2.zero;
			}
			_len *= scale;
			RefreshBounds();
		}

		private void RefreshBounds()
		{
			_bounds = new Bounds2d(_pos);
			_bounds.AddPoint(_pos + _len * _dir);
		}
	}
}
