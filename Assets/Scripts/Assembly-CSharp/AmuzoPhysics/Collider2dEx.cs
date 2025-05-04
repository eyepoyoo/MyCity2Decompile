using UnityEngine;

namespace AmuzoPhysics
{
	public class Collider2dEx : Collider2d
	{
		private Transform2d _transform;

		private Vector2 _velocity;

		public new Transform2d Transform
		{
			get
			{
				return _transform;
			}
			set
			{
				_transform = value;
			}
		}

		public new Vector2 Position
		{
			get
			{
				return _transform.Position;
			}
			set
			{
				_transform.Position = value;
				OnOwnerPositionChanged(true);
			}
		}

		public new float Rotation
		{
			get
			{
				return _transform.Rotation;
			}
			set
			{
				_transform.Rotation = value;
				OnOwnerRotationChanged();
			}
		}

		public new Vector2 Velocity
		{
			get
			{
				return _velocity;
			}
			set
			{
				_velocity = value;
			}
		}

		public Collider2dEx(IOwner owner)
			: base(owner)
		{
			_transform.SetIdentity();
			_velocity = Vector2.zero;
		}

		public void Integrate(float dt, float friction)
		{
			_transform._position += _velocity * dt;
			OnOwnerPositionChanged(false);
			if (friction > 0f)
			{
				Vector2 normalized = _velocity.normalized;
				float magnitude = _velocity.magnitude;
				float num = friction * dt;
				magnitude = ((!(magnitude > num)) ? 0f : (magnitude - num));
				_velocity = magnitude * normalized;
			}
		}
	}
}
