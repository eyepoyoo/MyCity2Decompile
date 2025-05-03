using System;
using UnityEngine;

namespace AmuzoVehicle
{
	public class CarAccelerator
	{
		private CarSpecs _carSpecs;

		private float FinalDriveRatio
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._finalDriveRatio;
			}
		}

		private float MaxRpm
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._maxRpm;
			}
		}

		private float IdleRpm
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._idleRpm;
			}
		}

		private float StallRpm
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._stallRpm;
			}
		}

		private float EngineBrakeFactor
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._engineBrakeFactor;
			}
		}

		private float Mass
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._mass;
			}
		}

		private float Width
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._width;
			}
		}

		private float Height
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._height;
			}
		}

		private float Length
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._length;
			}
		}

		private float DriveWheelRadius
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._driveWheelRadius;
			}
		}

		private float BrakeAccelMag
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs.BrakeAccelMag;
			}
		}

		private float DragCoeff
		{
			get
			{
				return (!(_carSpecs != null)) ? 0f : _carSpecs._dragCoeff;
			}
		}

		private float MinThrottle
		{
			get
			{
				return IdleRpm / MaxRpm;
			}
		}

		private float MaxEngineSpeed
		{
			get
			{
				return MaxRpm * ((float)Math.PI / 30f);
			}
		}

		private float DragForceFactor
		{
			get
			{
				return 0.6125f * DragCoeff * Width * Height;
			}
		}

		public CarAccelerator(CarSpecs carSpecs)
		{
			_carSpecs = carSpecs;
		}

		private CarAccelerator()
		{
		}

		private float GetGearRatio(int gear)
		{
			return (!(_carSpecs != null)) ? 0f : _carSpecs.GetGearRatio(gear);
		}

		private float GetEngineTorque(float engineSpeed)
		{
			return (!(_carSpecs != null)) ? 0f : _carSpecs.GetTorque(engineSpeed * (30f / (float)Math.PI));
		}

		private float GetEngineSpeed(int gear, float speed)
		{
			float driveRatio = GetDriveRatio(gear);
			return driveRatio * speed / DriveWheelRadius;
		}

		private float GetEngineTorque(float engineSpeed, float throttle)
		{
			float result = 0f;
			if (throttle < MinThrottle)
			{
				throttle = MinThrottle;
			}
			float num = throttle * MaxEngineSpeed;
			float num2 = num - engineSpeed;
			if (num2 > 0f)
			{
				result = throttle * GetEngineTorque(engineSpeed);
			}
			else if (num2 < 0f)
			{
				result = EngineBrakeFactor * num2;
			}
			return result;
		}

		private float GetDriveRatio(int gear)
		{
			return GetGearRatio(gear) * FinalDriveRatio;
		}

		private float GetEngineAccel(int gear, float speed, float throttle)
		{
			float result = 0f;
			float engineSpeed = GetEngineSpeed(gear, speed);
			float engineTorque = GetEngineTorque(engineSpeed, throttle);
			float driveRatio = GetDriveRatio(gear);
			if (driveRatio != 0f)
			{
				result = engineTorque * driveRatio / (DriveWheelRadius * Mass);
			}
			return result;
		}

		private float GetBrakeAccel(float speed, float currAccel, float brake, float dt)
		{
			float result = 0f;
			if (brake > 0f && dt > 0f)
			{
				float num = Mathf.Abs(speed) / dt;
				float num2 = BrakeAccelMag * brake;
				float num3 = Mathf.Sign(speed);
				currAccel *= num3;
				if (num2 > num + currAccel)
				{
					num2 = num + currAccel;
				}
				result = (0f - num3) * num2;
			}
			return result;
		}

		private float GetDragAccel(float speed)
		{
			return (0f - Mathf.Sign(speed)) * DragForceFactor * speed * speed / Mass;
		}

		public float GetAcceleration(int gear, float speed, float throttle, float brake, float dt)
		{
			float engineAccel = GetEngineAccel(gear, speed, throttle);
			float dragAccel = GetDragAccel(speed);
			float brakeAccel = GetBrakeAccel(speed, engineAccel + dragAccel, brake, dt);
			return engineAccel + dragAccel + brakeAccel;
		}

		public int GetAutoGearChange(int gear, float speed, bool isAccelerating)
		{
			int result = 0;
			if (_carSpecs != null)
			{
				int num = 1;
				int num2 = -1;
				if (gear < 0)
				{
					num = -1;
					num2 = 1;
				}
				bool flag = _carSpecs.IsGearShiftAllowed(gear, num, speed);
				bool flag2 = _carSpecs.IsGearShiftAllowed(gear, num2, speed);
				if (isAccelerating && flag)
				{
					float acceleration = GetAcceleration(gear, speed, 1f, 0f, 0f);
					float acceleration2 = GetAcceleration(gear + num, speed, 1f, 0f, 0f);
					if (acceleration2 > acceleration)
					{
						result = num;
					}
				}
				else if (!isAccelerating && flag2 && gear >= 2)
				{
					float acceleration3 = GetAcceleration(gear, speed, 0f, 0f, 0f);
					float acceleration4 = GetAcceleration(gear + num2, speed, 0f, 0f, 0f);
					if (acceleration4 < acceleration3)
					{
						result = num2;
					}
				}
			}
			return result;
		}
	}
}
