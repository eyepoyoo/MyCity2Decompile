using System;
using UnityEngine;

namespace AmuzoVehicle
{
	public class CarWheels : MonoBehaviour
	{
		[Serializable]
		public class WheelSetupData
		{
			public GameObject _go;

			public float _radius;
		}

		public class WheelData
		{
			public WheelSetupData _setup;

			public GameObject _attachGo;

			public Vector3 _attachPos;

			public Vector3 _rollAxis;

			public float _rollSpeedDeg;

			public float _steerAngleDeg;

			public bool _isOnGround;

			public Vector3 _groundPoint;

			public float _bumpOffset;
		}

		public const int kWheelPositionFrontLeft = 0;

		public const int kWheelPositionFrontRight = 1;

		public const int kWheelPositionRearLeft = 2;

		public const int kWheelPositionRearRight = 3;

		public const int kNumWheelPositions = 4;

		public WheelSetupData _frontLeftWheelSetup;

		public WheelSetupData _frontRightWheelSetup;

		public WheelSetupData _rearLeftWheelSetup;

		public WheelSetupData _rearRightWheelSetup;

		private WheelData[] _wheelData;

		public float _groundCollisionRayLength;

		public LayerMask _groundCollisionLayerMask;

		public Plane _groundPlane;

		public float _bumpLambda;

		private RaycastHit _raycastHit;

		public Vector3 _groundPlaneNormal
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

		public float _groundPlaneDistance
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

		public Plane GroundPlane
		{
			get
			{
				return _groundPlane;
			}
			set
			{
				_groundPlane = value;
			}
		}

		private void Awake()
		{
			_wheelData = new WheelData[4];
			_raycastHit = default(RaycastHit);
		}

		private void Start()
		{
			InitWheelData(0, _frontLeftWheelSetup);
			InitWheelData(1, _frontRightWheelSetup);
			InitWheelData(2, _rearLeftWheelSetup);
			InitWheelData(3, _rearRightWheelSetup);
		}

		private void Update()
		{
			DebugDraw.DrawPlane(base.transform.position, _groundPlaneNormal, _groundPlaneDistance, 4f, Color.green);
		}

		private void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			WheelData[] wheelData = _wheelData;
			foreach (WheelData wheelData2 in wheelData)
			{
				wheelData2._bumpOffset *= 1f / Mathf.Pow(2f, fixedDeltaTime / _bumpLambda);
			}
		}

		private void LateUpdate()
		{
			float deltaTime = Time.deltaTime;
			WheelData[] wheelData = _wheelData;
			foreach (WheelData wheelData2 in wheelData)
			{
				wheelData2._attachGo.transform.localPosition = wheelData2._attachPos + wheelData2._bumpOffset * Vector3.up;
				wheelData2._attachGo.transform.localEulerAngles = new Vector3(0f, wheelData2._steerAngleDeg, 0f);
				wheelData2._setup._go.transform.Rotate(wheelData2._rollSpeedDeg * deltaTime * wheelData2._rollAxis);
			}
		}

		private void InitWheelData(int wheelPos, WheelSetupData wheelSetup)
		{
			if (_wheelData == null)
			{
				return;
			}
			if (_wheelData[wheelPos] == null)
			{
				_wheelData[wheelPos] = new WheelData();
			}
			if (_wheelData[wheelPos] == null)
			{
				return;
			}
			_wheelData[wheelPos]._setup = wheelSetup;
			_wheelData[wheelPos]._attachGo = new GameObject("WheelAttach" + wheelPos);
			if (_wheelData[wheelPos]._attachGo != null)
			{
				_wheelData[wheelPos]._attachGo.transform.parent = base.transform;
				_wheelData[wheelPos]._attachGo.transform.position = wheelSetup._go.transform.position;
				_wheelData[wheelPos]._attachGo.transform.rotation = base.transform.rotation;
				_wheelData[wheelPos]._attachPos = _wheelData[wheelPos]._attachGo.transform.localPosition;
				if (wheelSetup._go != null)
				{
					wheelSetup._go.transform.parent = _wheelData[wheelPos]._attachGo.transform;
					_wheelData[wheelPos]._rollAxis = wheelSetup._go.transform.InverseTransformDirection(base.transform.right);
				}
			}
		}

		public void FindGround()
		{
			Vector3 vector = -base.transform.up;
			Vector3[] array = new Vector3[4];
			Vector3[] array2 = new Vector3[4];
			int num = 0;
			WheelData[] wheelData = _wheelData;
			foreach (WheelData wheelData2 in wheelData)
			{
				wheelData2._isOnGround = wheelData2._attachGo != null && Physics.Raycast(wheelData2._attachGo.transform.position, vector, out _raycastHit, _groundCollisionRayLength, _groundCollisionLayerMask);
				if (wheelData2._isOnGround)
				{
					wheelData2._groundPoint = _raycastHit.point;
					array[num] = _raycastHit.point;
					array2[num] = _raycastHit.normal;
					num++;
				}
			}
			switch (num)
			{
			case 1:
				_groundPlaneNormal = array2[0];
				_groundPlaneDistance = Vector3.Dot(array[0], _groundPlaneNormal);
				break;
			case 2:
			{
				Vector3 lhs = array2[0] + array2[1];
				Vector3 rhs = array[1] - array[0];
				_groundPlaneNormal = Vector3.Dot(rhs, rhs) * lhs - Vector3.Dot(rhs, lhs) * rhs;
				_groundPlaneNormal = _groundPlaneNormal.normalized;
				_groundPlaneDistance = Vector3.Dot(array[0], _groundPlaneNormal);
				break;
			}
			case 3:
			{
				Vector3 lhs = array[1] - array[0];
				Vector3 rhs = array[2] - array[1];
				_groundPlaneNormal = Vector3.Cross(lhs, rhs);
				if (Vector3.Dot(_groundPlaneNormal, vector) > 0f)
				{
					_groundPlaneNormal = -_groundPlaneNormal;
				}
				_groundPlaneNormal = _groundPlaneNormal.normalized;
				_groundPlaneDistance = Vector3.Dot(array[0], _groundPlaneNormal);
				break;
			}
			case 4:
			{
				Vector3 lhs = array[1] - array[0];
				Vector3 rhs = array[2] - array[1];
				_groundPlaneNormal = Vector3.Cross(lhs, rhs);
				if (Vector3.Dot(_groundPlaneNormal, vector) > 0f)
				{
					_groundPlaneNormal = -_groundPlaneNormal;
				}
				lhs = array[3] - array[2];
				rhs = array[0] - array[3];
				Vector3 vector2 = Vector3.Cross(lhs, rhs);
				if (Vector3.Dot(vector2, vector) > 0f)
				{
					vector2 = -vector2;
				}
				_groundPlaneNormal = (_groundPlaneNormal + vector2).normalized;
				_groundPlaneDistance = float.MinValue;
				Vector3[] array3 = array;
				foreach (Vector3 lhs2 in array3)
				{
					float num2 = Vector3.Dot(lhs2, _groundPlaneNormal);
					if (num2 > _groundPlaneDistance)
					{
						_groundPlaneDistance = num2;
					}
				}
				break;
			}
			}
		}

		public void SetOnGround(int wheelPos, bool isOnGround)
		{
			if (_wheelData != null && _wheelData[wheelPos] != null)
			{
				_wheelData[wheelPos]._isOnGround = isOnGround;
				if (isOnGround)
				{
					Vector3 position = _wheelData[wheelPos]._attachGo.transform.position;
					float num = Vector3.Dot(position, _groundPlaneNormal) - _groundPlaneDistance;
					_wheelData[wheelPos]._groundPoint = position - num * _groundPlaneNormal;
				}
			}
		}

		public void SetSteerAngleDeg(int wheelPos, float steerAngle)
		{
			if (_wheelData != null && _wheelData[wheelPos] != null)
			{
				_wheelData[wheelPos]._steerAngleDeg = steerAngle;
			}
		}

		public void SetRollSpeedLinear(int wheelPos, float rollSpeed)
		{
			if (_wheelData != null && _wheelData[wheelPos] != null)
			{
				_wheelData[wheelPos]._rollSpeedDeg = rollSpeed / _wheelData[wheelPos]._setup._radius * 57.29578f;
			}
		}

		public void SetColour(int wheelPos, Color colour)
		{
			if (_wheelData == null || _wheelData[wheelPos] == null || _wheelData[wheelPos]._setup == null || !(_wheelData[wheelPos]._setup._go != null))
			{
				return;
			}
			Renderer[] componentsInChildren = _wheelData[wheelPos]._setup._go.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				if (renderer.material != null)
				{
					renderer.material.color = colour;
				}
			}
		}

		public void AddBump(int wheelPos, float bumpSize)
		{
			if (_wheelData != null && _wheelData[wheelPos] != null)
			{
				_wheelData[wheelPos]._bumpOffset += bumpSize;
			}
		}
	}
}
