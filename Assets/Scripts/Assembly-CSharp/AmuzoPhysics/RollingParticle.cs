using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace AmuzoPhysics
{
	public class RollingParticle
	{
		private class Wall
		{
			public Plane _plane;

			public float _bounce;

			public Wall(Vector3 point, Vector3 normal, float bounce)
			{
				_plane.normal = normal;
				_plane.distance = Vector3.Dot(point, normal);
				_bounce = bounce;
			}
		}

		[Serializable]
		public class WallCollisionParams
		{
			public float _radius;

			public float _extraRadius;

			public float _heightAboveGround;

			public float _maxPreserveSpeedAngleDeg;

			public float _extraBounceForce;

			public LayerMask _wallCollisionLayerMask;

			public bool _isIgnoreMaterialBounce;

			public bool _isRotateToResultVel;

			public float _maxPreserveSpeedAngle
			{
				get
				{
					return _maxPreserveSpeedAngleDeg * ((float)Math.PI / 180f);
				}
				set
				{
					_maxPreserveSpeedAngleDeg = value * 57.29578f;
				}
			}

			public void Copy(WallCollisionParams src)
			{
				_radius = src._radius;
				_extraRadius = src._extraRadius;
				_heightAboveGround = src._heightAboveGround;
				_maxPreserveSpeedAngleDeg = src._maxPreserveSpeedAngleDeg;
				_extraBounceForce = src._extraBounceForce;
				_wallCollisionLayerMask = src._wallCollisionLayerMask;
				_isIgnoreMaterialBounce = src._isIgnoreMaterialBounce;
				_isRotateToResultVel = src._isRotateToResultVel;
			}

			public void SetConfigValue(ConfigData.IPath path, JsonData valueData)
			{
				switch (path._pFullPath)
				{
				case "radius":
					ConfigData.ReadFloat(valueData, delegate(float value)
					{
						_radius = value;
					});
					break;
				case "extraRadius":
					ConfigData.ReadFloat(valueData, delegate(float value)
					{
						_extraRadius = value;
					});
					break;
				case "heightAboveGround":
					ConfigData.ReadFloat(valueData, delegate(float value)
					{
						_heightAboveGround = value;
					});
					break;
				case "maxPreserveSpeedAngleDeg":
					ConfigData.ReadFloat(valueData, delegate(float value)
					{
						_maxPreserveSpeedAngleDeg = value;
					});
					break;
				case "extraBounceForce":
					ConfigData.ReadFloat(valueData, delegate(float value)
					{
						_extraBounceForce = value;
					});
					break;
				case "wallCollisionLayerMask":
					ConfigData.ReadInt(valueData, delegate(int value)
					{
						_wallCollisionLayerMask = value;
					});
					break;
				case "isIgnoreMaterialBounce":
					ConfigData.ReadBool(valueData, delegate(bool value)
					{
						_isIgnoreMaterialBounce = value;
					});
					break;
				case "isRotateToResultVel":
					ConfigData.ReadBool(valueData, delegate(bool value)
					{
						_isRotateToResultVel = value;
					});
					break;
				}
			}

			public JsonData GetConfigValue(ConfigData.IPath path)
			{
				JsonData result = null;
				switch (path._pFullPath)
				{
				case "radius":
					result = new JsonData(_radius);
					break;
				case "extraRadius":
					result = new JsonData(_extraRadius);
					break;
				case "heightAboveGround":
					result = new JsonData(_heightAboveGround);
					break;
				case "maxPreserveSpeedAngleDeg":
					result = new JsonData(_maxPreserveSpeedAngleDeg);
					break;
				case "extraBounceForce":
					result = new JsonData(_extraBounceForce);
					break;
				case "wallCollisionLayerMask":
					result = new JsonData(_wallCollisionLayerMask);
					break;
				case "isIgnoreMaterialBounce":
					result = new JsonData(_isIgnoreMaterialBounce);
					break;
				case "isRotateToResultVel":
					result = new JsonData(_isRotateToResultVel);
					break;
				}
				return result;
			}
		}

		public delegate float SafeTimeDelegate(Vector3 pos, Vector3 vel, float dt, out Vector3 newVel);

		private SimpleTransform _transform;

		private Plane _groundPlane;

		private Vector3 _gravity;

		private Vector3 _velocity;

		private float _rollSpeed;

		public float _mass;

		public float _startSlipSpeed = 0.1f;

		public float _stopSlipSpeed = 0.01f;

		public float _staticFriction = 3f;

		public float _dynamicFriction = 2f;

		public float _frictionScale = 1f;

		public float _groundDrag;

		private bool _isLocked;

		private bool _isSlipping;

		private int _numSlippingStateLocks;

		private bool _isOnGround;

		private float _onGroundTolerance;

		private bool _isAirAllowed;

		private Vector3 _groundImpulse;

		private Vector3 _wallImpulse;

		private Vector3 _frictionImpulse;

		private List<Wall> _walls;

		private Vector3 _groundPlaneNormal
		{
			get
			{
				return _groundPlane.normal;
			}
			set
			{
				_groundPlane.normal = value;
			}
		}

		private float _groundPlaneDistance
		{
			get
			{
				return _groundPlane.distance;
			}
			set
			{
				_groundPlane.distance = value;
			}
		}

		private float _invMass
		{
			get
			{
				return 1f / _mass;
			}
		}

		public bool IsSlipping
		{
			get
			{
				return _isSlipping;
			}
		}

		private bool _isSlippingStateLocked
		{
			get
			{
				return _numSlippingStateLocks > 0;
			}
		}

		private bool _lockSlippingState
		{
			set
			{
				if (value)
				{
					_numSlippingStateLocks++;
					return;
				}
				_numSlippingStateLocks--;
				if (_numSlippingStateLocks < 0)
				{
					_numSlippingStateLocks = 0;
				}
				if (_numSlippingStateLocks == 0)
				{
					CheckSlipping();
				}
			}
		}

		public bool IsOnGround
		{
			get
			{
				return _isOnGround;
			}
			set
			{
				_isOnGround = value;
			}
		}

		public float OnGroundTolerance
		{
			set
			{
				_onGroundTolerance = value;
			}
		}

		public bool IsAirAllowed
		{
			get
			{
				return _isAirAllowed;
			}
			set
			{
				_isAirAllowed = value;
			}
		}

		public Vector3 groundImpulse
		{
			get
			{
				return _mass * _groundImpulse;
			}
		}

		public Vector3 wallImpulse
		{
			get
			{
				return _wallImpulse;
			}
		}

		public Vector3 collisionImpulse
		{
			get
			{
				return groundImpulse + wallImpulse;
			}
		}

		public Vector3 frictionImpulse
		{
			get
			{
				return _frictionImpulse;
			}
		}

		public float Mass
		{
			get
			{
				return _mass;
			}
			set
			{
				_mass = value;
			}
		}

		public Vector3 Position
		{
			get
			{
				return _transform.position;
			}
			set
			{
				_transform.position = value;
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return _transform.rotation;
			}
			set
			{
				_lockSlippingState = true;
				_transform.rotation = value;
				_lockSlippingState = false;
			}
		}

		public Vector3 TransformRight
		{
			get
			{
				return _transform.right;
			}
		}

		public Vector3 TransformUp
		{
			get
			{
				return _transform.up;
			}
			set
			{
				Rotation = Quaternion.LookRotation(Vector3.Cross(TransformRight, value), value);
			}
		}

		public Vector3 TransformForward
		{
			get
			{
				return _transform.forward;
			}
			set
			{
				Rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.Cross(TransformUp, value), TransformUp), TransformUp);
			}
		}

		public Vector3 Velocity
		{
			get
			{
				return _velocity;
			}
			set
			{
				_lockSlippingState = true;
				_velocity = value;
				_lockSlippingState = false;
			}
		}

		public float RollSpeed
		{
			get
			{
				return _rollSpeed;
			}
			set
			{
				_lockSlippingState = true;
				_rollSpeed = value;
				_lockSlippingState = false;
			}
		}

		public Vector3 GroundVelocity
		{
			get
			{
				return MathHelper.ClipVector3(Velocity, TransformUp);
			}
			set
			{
				Velocity = MathHelper.ProjectVector3(Velocity, TransformUp) + MathHelper.ClipVector3(value, TransformUp);
			}
		}

		public Vector3 RollVelocity
		{
			get
			{
				return RollSpeed * TransformForward;
			}
		}

		public Vector3 SlipVelocity
		{
			get
			{
				return GroundVelocity - RollVelocity;
			}
		}

		public bool IsLocked
		{
			get
			{
				return _isLocked;
			}
			set
			{
				_isLocked = value;
				if (_isLocked)
				{
					RollSpeed = 0f;
				}
			}
		}

		public Vector3 Gravity
		{
			get
			{
				return _gravity;
			}
			set
			{
				_gravity = value;
			}
		}

		public float _pFrictionScale
		{
			get
			{
				return _frictionScale;
			}
			set
			{
				_frictionScale = value;
			}
		}

		public RollingParticle()
		{
			_mass = 1f;
			_transform.SetIdentity();
			_velocity = Vector3.zero;
			_gravity = -9.8f * Vector3.up;
			_rollSpeed = 0f;
			_isLocked = false;
			_isSlipping = false;
			_numSlippingStateLocks = 0;
			_groundPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);
			_walls = new List<Wall>();
		}

		public void DoFriction(float dt)
		{
			if (!_isOnGround)
			{
				return;
			}
			Vector3 slipVelocity = SlipVelocity;
			Vector3 vector = -slipVelocity.normalized;
			float magnitude = slipVelocity.magnitude;
			float num = CalculateFrictionMagnitude(_isSlipping, dt);
			Vector3 vector2;
			if (_isLocked)
			{
				float num2 = _mass * (magnitude / dt);
				if (num > num2)
				{
					num = num2;
				}
				vector2 = num * vector;
			}
			else
			{
				float num3 = Vector3.Dot(slipVelocity, TransformForward);
				float num4 = Vector3.Dot(slipVelocity, TransformRight);
				float num5 = _mass * (0f - num3 / (2f * dt));
				float num6 = _mass * (0f - num4 / dt);
				vector2 = num * vector;
				float num7 = Vector3.Dot(vector2, TransformForward);
				float num8 = Vector3.Dot(vector2, TransformRight);
				if (num5 < 0f && num7 < num5)
				{
					num7 = num5;
				}
				else if (num5 > 0f && num7 > num5)
				{
					num7 = num5;
				}
				if (num6 < 0f && num8 < num6)
				{
					num8 = num6;
				}
				else if (num6 > 0f && num8 > num6)
				{
					num8 = num6;
				}
				vector2 = num7 * TransformForward + num8 * TransformRight;
			}
			_lockSlippingState = true;
			ApplyForce(vector2, dt);
			ApplyRollForce(-vector2, dt);
			_lockSlippingState = false;
			_frictionImpulse = vector2 * dt;
		}

		private float CalculateFrictionMagnitude(bool isSlipping, float dt)
		{
			float num = _staticFriction;
			if (isSlipping)
			{
				num = _dynamicFriction;
			}
			float a = _frictionScale * num * _mass * Vector3.Dot(_groundImpulse, TransformUp) / dt;
			return Mathf.Max(a, 0f);
		}

		public void ApplyForce(Vector3 force, float dt, bool isUseMass = true)
		{
			float num = ((!isUseMass) ? 1f : _invMass);
			Velocity += dt * num * force;
		}

		public void ApplyImpulse(Vector3 impulse, bool isUseMass = true)
		{
			float num = ((!isUseMass) ? 1f : _invMass);
			Velocity += num * impulse;
		}

		public void ApplyGroundImpulse(Vector3 impulse, bool isUseMass = true)
		{
			float num = ((!isUseMass) ? 1f : _invMass);
			impulse *= num;
			ApplyImpulse(impulse, false);
			_groundImpulse += impulse;
		}

		public void ApplyRollForce(float force, float dt)
		{
			if (!_isLocked)
			{
				Vector3 slipVelocity = SlipVelocity;
				_lockSlippingState = true;
				RollSpeed += dt / _mass * force;
				AttemptRestoreSlipVelocity(slipVelocity, _isSlipping, dt);
				_lockSlippingState = false;
			}
		}

		public void ApplyRollForce(Vector3 force, float dt)
		{
			ApplyRollForce(Vector3.Dot(force, _transform.forward), dt);
		}

		public void ApplyBrakeForce(float force, float dt)
		{
			float num = _mass * ((0f - _rollSpeed) / dt);
			if (force > Mathf.Abs(num))
			{
				force = Mathf.Abs(num);
			}
			if (num < 0f)
			{
				force = 0f - force;
			}
			ApplyRollForce(force, dt);
		}

		public void IntegratePosition(float dt, SafeTimeDelegate safeTimeCallback = null)
		{
			if (_isOnGround)
			{
				float num = _groundDrag * GroundVelocity.sqrMagnitude;
				Vector3 force = (0f - num) * GroundVelocity.normalized;
				ApplyForce(force, dt);
			}
			Velocity += Vector3.Project(Gravity, TransformUp) * dt;
			if (safeTimeCallback != null)
			{
				_lockSlippingState = true;
				while (dt > 0f)
				{
					Vector3 newVel;
					float num2 = safeTimeCallback(Position, Velocity, dt, out newVel);
					Position += Velocity * num2;
					Velocity = newVel;
					dt -= num2;
				}
				_lockSlippingState = false;
			}
			else
			{
				Position += Velocity * dt;
			}
		}

		private void EnforceGroundPlane(bool isAirAllowed = false)
		{
			_lockSlippingState = true;
			TransformUp = _groundPlaneNormal;
			float num = Vector3.Dot(Position, _groundPlaneNormal) - _groundPlaneDistance;
			if (!isAirAllowed || num < 0f)
			{
				Position -= num * _groundPlaneNormal;
				float num2 = Vector3.Dot(Velocity, _groundPlaneNormal);
				if (!isAirAllowed || num2 < 0f)
				{
					Vector3 groundVelocity = GroundVelocity;
					_groundImpulse = groundVelocity - Velocity;
					Velocity = groundVelocity;
				}
			}
			_lockSlippingState = false;
		}

		public void DoGroundCollision(Plane groundPlane, float dt)
		{
			_groundPlane = groundPlane;
			if (_isAirAllowed)
			{
				float num = Vector3.Dot(Position, _groundPlane.normal);
				if (num > _groundPlane.distance + _onGroundTolerance)
				{
					_isOnGround = false;
					return;
				}
				EnforceGroundPlane(true);
				_isOnGround = true;
			}
			else
			{
				EnforceGroundPlane();
				_isOnGround = true;
			}
		}

		public void SetRotation(Quaternion newRot, bool isWantRestoreSlipVel, float dt)
		{
			if (Rotation != newRot)
			{
				_lockSlippingState = true;
				Vector3 slipVelocity = SlipVelocity;
				Rotation = newRot;
				if (isWantRestoreSlipVel)
				{
					AttemptRestoreSlipVelocity(slipVelocity, _isSlipping, dt);
				}
				_lockSlippingState = false;
			}
		}

		private void CheckSlipping()
		{
			if (!_isSlippingStateLocked)
			{
				float magnitude = SlipVelocity.magnitude;
				if (_isSlipping && magnitude <= _stopSlipSpeed)
				{
					_isSlipping = false;
				}
				else if (!_isSlipping && magnitude > _startSlipSpeed)
				{
					_isSlipping = true;
				}
			}
		}

		private void AttemptRestoreSlipVelocity(Vector3 oldSlipVel, bool wasSlipping, float dt)
		{
			if (!_isOnGround)
			{
				return;
			}
			Vector3 slipVelocity = SlipVelocity;
			Vector3 vector = slipVelocity - oldSlipVel;
			Vector3 vector2 = _mass * -vector / dt;
			float num = vector2.magnitude;
			Vector3 normalized = vector2.normalized;
			float num2 = CalculateFrictionMagnitude(wasSlipping, dt);
			bool flag = false;
			if (num > num2)
			{
				num = num2;
			}
			else if (num < 0f - num2)
			{
				num = 0f - num2;
			}
			else
			{
				flag = true;
			}
			ApplyForce(num * normalized, dt);
			if (flag)
			{
				slipVelocity = SlipVelocity;
				if ((slipVelocity - oldSlipVel).magnitude > 0.0001f)
				{
					Debug.LogWarning("CC: Not same!");
				}
			}
		}

		public void ResetWalls()
		{
			if (_walls != null)
			{
				_walls.SafeClear();
			}
		}

		public void AddWall(Vector3 point, Vector3 normal, float bounce)
		{
			if (_walls != null)
			{
				_walls.Add(new Wall(point, normal, bounce));
			}
		}

		public void FindWallsByRaycast(WallCollisionParams parameters)
		{
			FindWallsByRaycast(parameters._radius + parameters._extraRadius, parameters._heightAboveGround, parameters._wallCollisionLayerMask);
		}

		private void FindWallsByRaycast(float rayLength, float rayStartOffsetY, LayerMask wallCollisionLayerMask)
		{
			Vector3 origin = Position + rayStartOffsetY * TransformUp;
			Vector3 transformUp = TransformUp;
			Vector3 normalized = Vector3.Cross(transformUp, Velocity).normalized;
			Vector3 vector = Vector3.Cross(normalized, transformUp);
			float[] array = new float[5]
			{
				-(float)Math.PI / 2f,
				-(float)Math.PI / 4f,
				0f,
				(float)Math.PI / 4f,
				(float)Math.PI / 2f
			};
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Vector3 direction = Mathf.Sin(array[i]) * normalized + Mathf.Cos(array[i]) * vector;
				RaycastHit hitInfo;
				if (Physics.Raycast(origin, direction, out hitInfo, rayLength, wallCollisionLayerMask))
				{
					Vector3 normalized2 = MathHelper.ClipVector3(hitInfo.normal, transformUp).normalized;
					float bounce = ((!(hitInfo.collider != null) || !(hitInfo.collider.material != null)) ? 0f : hitInfo.collider.material.bounciness);
					AddWall(hitInfo.point, normalized2, bounce);
				}
			}
		}

		public void DoWallCollision(WallCollisionParams parameters, float dt, RollingParticle[] connectedParticles)
		{
			_wallImpulse = Vector3.zero;
			if (_walls == null)
			{
				return;
			}
			float num = Mathf.Cos(parameters._maxPreserveSpeedAngle);
			_lockSlippingState = true;
			foreach (Wall wall in _walls)
			{
				float num2 = Vector3.Dot(Position, wall._plane.normal) - parameters._radius - wall._plane.distance;
				if (!(num2 < 0f))
				{
					continue;
				}
				Position -= num2 * wall._plane.normal;
				float magnitude = Velocity.magnitude;
				if (!(magnitude > 0f))
				{
					continue;
				}
				Vector3 normalized = Vector3.Cross(wall._plane.normal, TransformUp).normalized;
				float num3 = Vector3.Dot(Velocity, normalized);
				float f = num3 / magnitude;
				float num4 = Vector3.Dot(Velocity, wall._plane.normal);
				num4 = (parameters._isIgnoreMaterialBounce ? (num4 * (0f - wall._bounce)) : 0f);
				Vector3 vector = num3 * normalized + num4 * wall._plane.normal;
				Vector3 vector2 = Mass * (vector - GroundVelocity) / dt;
				_wallImpulse += vector2 * dt;
				if (vector.magnitude < magnitude && Mathf.Abs(f) > num)
				{
					vector = vector.normalized * magnitude;
					vector2 = Mass * (vector - GroundVelocity) / dt;
				}
				ApplyForce(vector2, dt);
				if (connectedParticles != null)
				{
					foreach (RollingParticle rollingParticle in connectedParticles)
					{
						Vector3 normalized2 = (rollingParticle.Position - Position).normalized;
						Vector3 force = MathHelper.ProjectVector3(vector2, normalized2);
						rollingParticle.ApplyForce(force, dt);
					}
				}
				ApplyForce(wall._plane.normal * parameters._extraBounceForce, dt);
				if (parameters._isRotateToResultVel)
				{
					TransformForward = vector.normalized;
				}
			}
			_lockSlippingState = false;
		}
	}
}
