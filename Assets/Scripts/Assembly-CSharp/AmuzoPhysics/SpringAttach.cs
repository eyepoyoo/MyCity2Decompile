using System;
using UnityEngine;

namespace AmuzoPhysics
{
	public class SpringAttach : MonoBehaviour
	{
		public const float kDefaultMass = 1f;

		public const float kDefaultSpring = 80f;

		public const float kDefaultDamping = 20f;

		public GameObject _parent;

		protected Vector3 _parentPrevPos;

		protected Vector3 _parentPrevVel;

		protected Vector3 _parentPrevUp;

		protected Vector3 _parentPrevUpVel;

		private float _length;

		private float _angle1;

		private float _angle2;

		public float _restLength;

		public float _minLength;

		public float _maxLength;

		public float _maxAngle1Deg;

		public float _maxAngle2Deg;

		private float _lengthSpeed;

		private float _angle1Speed;

		private float _angle2Speed;

		public float _lengthMass = 1f;

		public float _angle1Mass = 1f;

		public float _angle2Mass = 1f;

		public float _lengthSpringK = 80f;

		public float _angle1SpringK = 80f;

		public float _angle2SpringK = 80f;

		public float _lengthDamping = 20f;

		public float _angle1Damping = 20f;

		public float _angle2Damping = 20f;

		protected float _lengthExtForce;

		protected float _angle1ExtForce;

		protected float _angle2ExtForce;

		public bool _isManualUpdate;

		protected float MaxAngle1
		{
			get
			{
				return _maxAngle1Deg * ((float)Math.PI / 180f);
			}
		}

		protected float MaxAngle2
		{
			get
			{
				return _maxAngle2Deg * ((float)Math.PI / 180f);
			}
		}

		protected float _lengthExtAccel
		{
			get
			{
				return _lengthExtForce / _lengthMass;
			}
			set
			{
				_lengthExtForce = _lengthMass * value;
			}
		}

		protected float _angle1ExtAccel
		{
			get
			{
				return _angle1ExtForce / _angle1Mass;
			}
			set
			{
				_angle1ExtForce = _angle1Mass * value;
			}
		}

		protected float _angle2ExtAccel
		{
			get
			{
				return _angle2ExtForce / _angle2Mass;
			}
			set
			{
				_angle2ExtForce = _angle2Mass * value;
			}
		}

		public bool IsManualUpdate
		{
			set
			{
				_isManualUpdate = value;
			}
		}

		public Vector3 LocalPosition
		{
			get
			{
				float num = _length * Mathf.Cos(_angle2);
				return new Vector3(_length * Mathf.Sin(_angle2), num * Mathf.Cos(_angle1), (0f - num) * Mathf.Sin(_angle1));
			}
			set
			{
				_length = value.magnitude;
				_angle1 = Mathf.Atan2(0f - value.z, value.y);
				float x = Mathf.Sqrt(value.y * value.y + value.z * value.z);
				_angle2 = Mathf.Atan2(value.x, x);
				EnforceLimits();
			}
		}

		public Vector3 Position
		{
			get
			{
				return (!(_parent != null)) ? Vector3.zero : _parent.transform.TransformPoint(LocalPosition);
			}
			set
			{
				LocalPosition = ((!(_parent != null)) ? Vector3.zero : _parent.transform.InverseTransformPoint(value));
			}
		}

		private Vector3 LocalLengthDir
		{
			get
			{
				return LocalPosition.normalized;
			}
		}

		public Vector3 UpAxis
		{
			get
			{
				return (!(_parent != null)) ? Vector3.up : _parent.transform.TransformDirection(LocalLengthDir);
			}
		}

		private Vector3 LocalLengthVel
		{
			get
			{
				return _lengthSpeed * LocalLengthDir;
			}
			set
			{
				_lengthSpeed = Vector3.Dot(value, LocalLengthDir);
			}
		}

		private Vector3 LocalAngle1Axis
		{
			get
			{
				return Vector3.right;
			}
		}

		private Vector3 LocalAngle2Axis
		{
			get
			{
				Vector3 localPosition = LocalPosition;
				return new Vector3(0f, 0f - localPosition.z, localPosition.y).normalized;
			}
		}

		public Vector3 FwdAxis
		{
			get
			{
				return (!(_parent != null)) ? Vector3.forward : _parent.transform.TransformDirection(LocalAngle2Axis);
			}
		}

		private Vector3 LocalAngle1Vel
		{
			get
			{
				return _angle1Speed * LocalAngle1Axis;
			}
			set
			{
				_angle1Speed = Vector3.Dot(value, LocalAngle1Axis);
			}
		}

		private Vector3 LocalAngle2Vel
		{
			get
			{
				return _angle2Speed * LocalAngle2Axis;
			}
			set
			{
				_angle2Speed = Vector3.Dot(value, LocalAngle2Axis);
			}
		}

		private Vector3 LocalAngleVel
		{
			get
			{
				return LocalAngle1Vel + LocalAngle2Vel;
			}
			set
			{
				LocalAngle1Vel = value;
				LocalAngle2Vel = value;
			}
		}

		public Vector3 LocalVelocity
		{
			get
			{
				return LocalLengthVel + Vector3.Cross(LocalPosition, LocalAngleVel);
			}
			set
			{
				LocalLengthVel = value;
				Vector3 localPosition = LocalPosition;
				LocalAngleVel = Vector3.Cross(value, localPosition) / Vector3.Dot(localPosition, localPosition);
				EnforceLimits();
			}
		}

		public Vector3 Velocity
		{
			get
			{
				return (!(_parent != null)) ? Vector3.zero : _parent.transform.TransformDirection(LocalVelocity);
			}
			set
			{
				LocalVelocity = ((!(_parent != null)) ? Vector3.zero : _parent.transform.InverseTransformDirection(value));
			}
		}

		private void EnforceLimits()
		{
			if (_length < _minLength)
			{
				_length = _minLength;
			}
			if (_length > _maxLength)
			{
				_length = _maxLength;
			}
			if (_angle1 > MaxAngle1)
			{
				_angle1 = MaxAngle1;
				if (_angle1Speed > 0f)
				{
					_angle1Speed = 0f;
				}
			}
			else if (_angle1 < 0f - MaxAngle1)
			{
				_angle1 = 0f - MaxAngle1;
				if (_angle1Speed < 0f)
				{
					_angle1Speed = 0f;
				}
			}
			if (_angle2 > MaxAngle2)
			{
				_angle2 = MaxAngle2;
				if (_angle2Speed > 0f)
				{
					_angle2Speed = 0f;
				}
			}
			else if (_angle2 < 0f - MaxAngle2)
			{
				_angle2 = 0f - MaxAngle2;
				if (_angle2Speed < 0f)
				{
					_angle2Speed = 0f;
				}
			}
		}

		public void AddAcceleration(Vector3 accel)
		{
			Vector3 lhs = base.transform.InverseTransformDirection(accel);
			Vector3 lhs2 = Vector3.Cross(lhs, LocalPosition);
			_lengthExtAccel += Vector3.Dot(lhs, LocalLengthDir);
			_angle1ExtAccel += Vector3.Dot(lhs2, LocalAngle1Axis);
			_angle2ExtAccel += Vector3.Dot(lhs2, LocalAngle2Axis);
		}

		private void UpdateParentMotion(float dt)
		{
			Transform transform = ((!(_parent != null)) ? null : ((!(_parent.GetComponent<Rigidbody>() != null)) ? _parent.transform : _parent.GetComponent<Rigidbody>().transform));
			Vector3 vector = ((!(transform != null)) ? _parentPrevPos : transform.position);
			Vector3 vector2 = (vector - _parentPrevPos) / dt;
			Vector3 direction = (vector2 - _parentPrevVel) / dt;
			Vector3 lhs = -base.transform.InverseTransformDirection(direction);
			Vector3 lhs2 = Vector3.Cross(lhs, LocalPosition);
			_lengthExtForce += _lengthMass * Vector3.Dot(lhs, LocalLengthDir);
			_angle1ExtForce += _angle1Mass * Vector3.Dot(lhs2, LocalAngle1Axis);
			_angle2ExtForce += _angle2Mass * Vector3.Dot(lhs2, LocalAngle2Axis);
			_parentPrevPos = vector;
			_parentPrevVel = vector2;
			Vector3 vector3 = ((!(transform != null)) ? _parentPrevUp : transform.up);
			Vector3 vector4 = Vector3.Cross(vector3, _parentPrevUp);
			float num = Mathf.Asin(vector4.magnitude);
			vector4 = num * vector4.normalized / dt;
			Vector3 direction2 = (vector4 - _parentPrevUpVel) / dt;
			Vector3 lhs3 = -base.transform.InverseTransformDirection(direction2);
			_angle1ExtForce += _angle1Mass * Vector3.Dot(lhs3, LocalAngle1Axis);
			_angle2ExtForce += _angle2Mass * Vector3.Dot(lhs3, LocalAngle2Axis);
			_parentPrevUp = vector3;
			_parentPrevUpVel = vector4;
		}

		private void Step(float dt)
		{
			float pos = _length - _restLength;
			MathHelper.StepSpring(ref pos, ref _lengthSpeed, _lengthSpringK, _lengthDamping, _lengthExtForce, _lengthMass, dt);
			_length = _restLength + pos;
			MathHelper.StepSpring(ref _angle1, ref _angle1Speed, _angle1SpringK, _angle1Damping, _angle1ExtForce, _angle1Mass, dt);
			MathHelper.StepSpring(ref _angle2, ref _angle2Speed, _angle2SpringK, _angle2Damping, _angle2ExtForce, _angle2Mass, dt);
			_lengthExtForce = (_angle1ExtForce = (_angle2ExtForce = 0f));
			EnforceLimits();
		}

		private void UpdateTransform()
		{
			base.transform.position = Position;
			base.transform.rotation = Quaternion.LookRotation(FwdAxis, UpAxis);
		}

		private void InternalUpdate(float dt)
		{
			UpdateParentMotion(dt);
			Step(dt);
			UpdateTransform();
		}

		private void Start()
		{
		}

		private void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			if (!_isManualUpdate)
			{
				InternalUpdate(fixedDeltaTime);
			}
		}

		public void ManualUpdate(float dt)
		{
			if (_isManualUpdate)
			{
				InternalUpdate(dt);
			}
			else
			{
				Debug.LogError("[SpringAttach] Not set to manual update");
			}
		}

		private void OnEnable()
		{
			if (_parent != null)
			{
				_parent.SendMessage("OnSpringAttachEnable", this);
			}
		}

		private void OnDisable()
		{
			if (_parent != null)
			{
				_parent.SendMessage("OnSpringAttachDisable", this);
			}
		}
	}
}
