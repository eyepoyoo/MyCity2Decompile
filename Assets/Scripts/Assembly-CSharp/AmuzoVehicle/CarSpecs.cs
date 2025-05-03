using System;
using UnityEngine;

namespace AmuzoVehicle
{
	public class CarSpecs : MonoBehaviour
	{
		[Serializable]
		public class TorqueCurve
		{
			public Vector2[] _points;

			public float GetTorque(float rpm)
			{
				float result = 0f;
				if (_points != null)
				{
					int num = _points.Length;
					int i;
					for (i = 0; i < num && _points[i].x <= rpm; i++)
					{
					}
					if (i == 0)
					{
						i++;
					}
					else if (i == num)
					{
						i--;
					}
					if (1 <= i && i < num)
					{
						result = MathHelper.Lerp(_points[i - 1].x, _points[i - 1].y, _points[i].x, _points[i].y, rpm);
					}
				}
				return result;
			}
		}

		public const float kMph100 = 44.704f;

		public const float kRpmToRps = (float)Math.PI / 30f;

		public const float kRpsToRpm = 30f / (float)Math.PI;

		public const float kAirDensity = 1.225f;

		public float[] _forwardGearRatios;

		public float[] _reverseGearRatios;

		public float _finalDriveRatio;

		public float _maxRpm;

		public float _idleRpm;

		public float _stallRpm;

		public TorqueCurve _torqueCurve;

		public float _engineBrakeFactor;

		public float _mass;

		public float _width;

		public float _height;

		public float _length;

		public float _driveWheelRadius;

		public float _stopDistanceFrom100mph;

		public float _dragCoeff;

		public int NumForwardGears
		{
			get
			{
				return (_forwardGearRatios != null) ? _forwardGearRatios.Length : 0;
			}
		}

		public int NumReverseGears
		{
			get
			{
				return (_reverseGearRatios != null) ? _reverseGearRatios.Length : 0;
			}
		}

		public float MaxSpeed
		{
			get
			{
				return (NumForwardGears < 1) ? 0f : GetWheelSpeed(NumForwardGears - 1, _maxRpm * ((float)Math.PI / 30f));
			}
		}

		public float BrakeAccelMag
		{
			get
			{
				return 1998.4475f / (2f * _stopDistanceFrom100mph);
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public float GetGearRatio(int gear)
		{
			float result = 0f;
			if (IsForwardGear(gear))
			{
				result = _forwardGearRatios[gear - 1];
			}
			else if (IsReverseGear(gear))
			{
				result = 0f - _reverseGearRatios[-gear - 1];
			}
			return result;
		}

		public float GetTorque(float rpm)
		{
			return (_torqueCurve == null) ? 0f : _torqueCurve.GetTorque(rpm);
		}

		public float GetEngineSpeed(int gear, float wheelSpeed)
		{
			return GetGearRatio(gear) * _finalDriveRatio * wheelSpeed / _driveWheelRadius;
		}

		public float GetRpm(int gear, float wheelSpeed)
		{
			return GetEngineSpeed(gear, wheelSpeed) * (30f / (float)Math.PI);
		}

		public float GetWheelSpeed(int gear, float engineSpeed)
		{
			return engineSpeed * _driveWheelRadius / (GetGearRatio(gear) * _finalDriveRatio);
		}

		public float GetMph(int gear, float wheelSpeed)
		{
			return GetWheelSpeed(gear, wheelSpeed) * 2.2369363f;
		}

		public bool IsForwardGear(int gear)
		{
			return gear > 0 && gear <= NumForwardGears;
		}

		public bool IsReverseGear(int gear)
		{
			return gear < 0 && -gear <= NumReverseGears;
		}

		public bool IsValidGear(int gear)
		{
			return gear == 0 || IsForwardGear(gear) || IsReverseGear(gear);
		}

		public bool IsGearAllowed(int gear, float wheelSpeed)
		{
			return IsValidGear(gear) && GetRpm(gear, wheelSpeed) > _stallRpm && GetRpm(gear, wheelSpeed) <= _maxRpm;
		}

		public bool IsGearShiftAllowed(int gear, int shift, float wheelSpeed)
		{
			return IsGearAllowed(gear + shift, wheelSpeed);
		}

		public int GetLowestGearAllowed(float wheelSpeed)
		{
			int num = (int)Mathf.Sign(wheelSpeed);
			int i;
			for (i = num; GetRpm(i, wheelSpeed) > _maxRpm && IsValidGear(i + num); i += num)
			{
			}
			return i;
		}
	}
}
