using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmuzoVehicle
{
	public class CarStats : MonoBehaviour
	{
		[Serializable]
		public class Point
		{
			public float _time;

			public float _pos;

			public float _speed;

			public float this[int i]
			{
				get
				{
					switch (i)
					{
					case 0:
						return _time;
					case 1:
						return _pos;
					case 2:
						return _speed;
					default:
						return -1f;
					}
				}
				set
				{
					switch (i)
					{
					case 0:
						_time = value;
						break;
					case 1:
						_pos = value;
						break;
					case 2:
						_speed = value;
						break;
					}
				}
			}

			public Point(float time, float pos, float speed)
			{
				_time = time;
				_pos = pos;
				_speed = speed;
			}

			public Point(float multicast)
			{
				_time = multicast;
				_pos = multicast;
				_speed = multicast;
			}

			public Point(Point src)
			{
				if (src != null)
				{
					_time = src._time;
					_pos = src._pos;
					_speed = src._speed;
				}
			}

			public static Point Lerp(Point a, Point b, float inValue, int inValueType)
			{
				if (a != null && b != null)
				{
					Point point = new Point(-1f);
					if (point != null)
					{
						point[inValueType] = inValue;
						point[OTHER_TYPE[inValueType, 0]] = MathHelper.Lerp(a[inValueType], a[OTHER_TYPE[inValueType, 0]], b[inValueType], b[OTHER_TYPE[inValueType, 0]], inValue);
						point[OTHER_TYPE[inValueType, 1]] = MathHelper.Lerp(a[inValueType], a[OTHER_TYPE[inValueType, 1]], b[inValueType], b[OTHER_TYPE[inValueType, 1]], inValue);
					}
					return point;
				}
				return null;
			}

			public static Point operator +(Point a, Point b)
			{
				return (a == null || b == null) ? null : new Point(a._time + b._time, a._pos + b._pos, a._speed + b._speed);
			}

			public static Point operator -(Point a, Point b)
			{
				return (a == null || b == null) ? null : new Point(a._time - b._time, a._pos - b._pos, a._speed - b._speed);
			}

			public static Point operator *(Point a, float b)
			{
				return (a == null) ? null : new Point(a._time * b, a._pos * b, a._speed * b);
			}

			public static Point operator *(float a, Point b)
			{
				return (b == null) ? null : new Point(a * b._time, a * b._pos, a * b._speed);
			}

			public static Point operator /(Point a, float b)
			{
				return (a == null) ? null : new Point(a._time / b, a._pos / b, a._speed / b);
			}
		}

		[Serializable]
		public class DataSet
		{
			public Point[] _byTime;

			public Point[] _byPos;

			public Point[] _bySpeed;

			public int Length
			{
				get
				{
					return (_byTime != null) ? _byTime.Length : 0;
				}
			}

			public Point Start
			{
				get
				{
					return (Length < 1) ? null : _byTime[0];
				}
			}

			public Point End
			{
				get
				{
					return (Length < 1) ? null : _byTime[Length - 1];
				}
			}

			public Point Step
			{
				get
				{
					if (Length >= 1)
					{
						if (Length >= 2)
						{
							return (_byTime[Length - 1] - _byTime[0]) / (Length - 1);
						}
						return new Point(0f);
					}
					return null;
				}
			}

			public Point[] this[int i]
			{
				get
				{
					switch (i)
					{
					case 0:
						return _byTime;
					case 1:
						return _byPos;
					case 2:
						return _bySpeed;
					default:
						return null;
					}
				}
				set
				{
					switch (i)
					{
					case 0:
						_byTime = value;
						break;
					case 1:
						_byPos = value;
						break;
					case 2:
						_bySpeed = value;
						break;
					}
				}
			}
		}

		public const int TIME_TYPE = 0;

		public const int POS_TYPE = 1;

		public const int SPEED_TYPE = 2;

		public static int[,] OTHER_TYPE = new int[3, 2]
		{
			{ 1, 2 },
			{ 2, 0 },
			{ 0, 1 }
		};

		public float _maxSpeed;

		public DataSet _accelData;

		public DataSet _brakeData;

		public Point _recordMaxValues;

		public float _recordTimeInterval;

		public float MaxSpeed
		{
			get
			{
				return _maxSpeed;
			}
			set
			{
				_maxSpeed = value;
			}
		}

		public void RecordData(CarSpecs carSpecs, Point max, float timeInterval)
		{
			DateTime utcNow = DateTime.UtcNow;
			Debug.Log(string.Format("[CarStats.RecordData] start at {0}", utcNow));
			_maxSpeed = carSpecs.MaxSpeed;
			Stack<Point> stack = new Stack<Point>();
			CarAccelerator carAccelerator = new CarAccelerator(carSpecs);
			float num = 1f / 60f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			int num6 = 1;
			int num7 = 0;
			stack.Push(new Point(num2, num3, num4));
			while ((max._time == 0f || num2 < max._time) && (max._pos == 0f || num3 < max._pos) && (max._speed == 0f || num4 < max._speed))
			{
				num6 += carAccelerator.GetAutoGearChange(num6, num4, true);
				num5 = carAccelerator.GetAcceleration(num6, num4, 1f, 0f, num);
				num3 += num4 * num + 0.5f * num5 * num * num;
				num4 += num5 * num;
				num2 += num;
				int num8 = (int)(num2 / timeInterval);
				if (num8 != num7)
				{
					if (num8 != num7 + 1)
					{
						Debug.Log(string.Format("[CarStats.RecordData] skipped time index {0} to {1}", num7 + 1, num8 - 1));
					}
					num7 = num8;
					stack.Push(new Point(num2, num3, num4));
				}
			}
			_accelData._byTime = new Point[stack.Count];
			while (stack.Count > 0)
			{
				_accelData._byTime[stack.Count - 1] = stack.Pop();
			}
			FinalizeDataSet(_accelData);
			stack.Clear();
			num3 = 0f;
			num2 = 0f;
			num7 = 0;
			stack.Push(new Point(num2, num3, num4));
			while (num4 > 0f)
			{
				num6 += carAccelerator.GetAutoGearChange(num6, num4, false);
				num5 = carAccelerator.GetAcceleration(num6, num4, 0f, 1f, num);
				num3 += num4 * num + 0.5f * num5 * num * num;
				num4 += num5 * num;
				num2 += num;
				int num8 = (int)(num2 / timeInterval);
				if (num8 != num7)
				{
					if (num8 != num7 + 1)
					{
						Debug.Log(string.Format("[CarStats.RecordData] skipped time index {0} to {1}", num7 + 1, num8 - 1));
					}
					num7 = num8;
					stack.Push(new Point(num2, num3, num4));
				}
			}
			_brakeData._byTime = new Point[stack.Count];
			while (stack.Count > 0)
			{
				_brakeData._byTime[stack.Count - 1] = stack.Pop();
			}
			FinalizeDataSet(_brakeData);
			DateTime utcNow2 = DateTime.UtcNow;
			Debug.Log(string.Format("[CarStats.Record] took {0} seconds", (utcNow2 - utcNow).TotalSeconds));
			Dump();
		}

		private static Point GetInterpolatedPoint(DataSet data, float inValue, int inValueType, int fallbackType = -1)
		{
			Point result = null;
			Point[] array = data[inValueType];
			if (array != null)
			{
				int num = array.Length;
				if (num >= 2)
				{
					Point point = (array[num - 1] - array[0]) / num;
					int num2 = (int)(inValue / point[inValueType]);
					if (num2 < 0)
					{
						num2 += num;
					}
					if (num2 >= num - 1)
					{
						num2 = num - 2;
					}
					else if (num2 < 0)
					{
						num2 = 0;
					}
					if (0 <= num2 && num2 < num - 1)
					{
						result = Point.Lerp(array[num2], array[num2 + 1], inValue, inValueType);
					}
				}
			}
			else
			{
				array = data[fallbackType];
				if (array != null)
				{
					int num3 = array.Length;
					float num4 = (inValue - array[0][inValueType]) / (array[num3 - 1][inValueType] - array[0][inValueType]);
					int i;
					if (num4 <= 0f)
					{
						i = 0;
					}
					else if (num4 >= 1f)
					{
						i = num3 - 2;
					}
					else
					{
						for (i = 0; i < num3 - 1; i++)
						{
							num4 = (inValue - array[i][inValueType]) / (array[i + 1][inValueType] - array[i][inValueType]);
							if (0f <= num4 && num4 <= 1f)
							{
								break;
							}
						}
					}
					if (0 <= i && i < num3 - 1)
					{
						result = Point.Lerp(array[i], array[i + 1], inValue, inValueType);
					}
				}
			}
			return result;
		}

		private static Point GetPointByTime(DataSet data, float time)
		{
			return GetInterpolatedPoint(data, time, 0);
		}

		private static Point GetPointByPos(DataSet data, float pos)
		{
			return GetInterpolatedPoint(data, pos, 1, 0);
		}

		private static Point GetPointBySpeed(DataSet data, float speed)
		{
			return GetInterpolatedPoint(data, speed, 2, 0);
		}

		private static void FinalizeDataSet(DataSet data)
		{
			data._byPos = null;
			data._bySpeed = null;
			int length = data.Length;
			if (length < 1)
			{
				return;
			}
			Point[] array = new Point[length];
			array[0] = new Point(data._byTime[0]);
			Point[] array2 = new Point[length];
			array2[0] = new Point(data._byTime[0]);
			if (length >= 2)
			{
				Point start = data.Start;
				Point step = data.Step;
				for (int i = 1; i < length; i++)
				{
					array[i] = GetPointByPos(data, start._pos + step._pos * (float)i);
					array2[i] = GetPointBySpeed(data, start._speed + step._speed * (float)i);
				}
			}
			data._byPos = array;
			data._bySpeed = array2;
		}

		private void GetSpeedChangePoints(float fromSpeed, float toSpeed, out Point fromPoint, out Point toPoint)
		{
			if (fromSpeed > toSpeed)
			{
				fromPoint = GetPointBySpeed(_brakeData, fromSpeed);
				toPoint = GetPointBySpeed(_brakeData, toSpeed);
			}
			else
			{
				fromPoint = GetPointBySpeed(_accelData, fromSpeed);
				toPoint = GetPointBySpeed(_accelData, toSpeed);
			}
		}

		public float GetSpeedChangeTime(float fromSpeed, float toSpeed)
		{
			Point fromPoint;
			Point toPoint;
			GetSpeedChangePoints(fromSpeed, toSpeed, out fromPoint, out toPoint);
			return toPoint._time - fromPoint._time;
		}

		public float GetSpeedChangeDistance(float fromSpeed, float toSpeed)
		{
			Point fromPoint;
			Point toPoint;
			GetSpeedChangePoints(fromSpeed, toSpeed, out fromPoint, out toPoint);
			return toPoint._pos - fromPoint._pos;
		}

		public void GetSpeedChangeStats(float fromSpeed, float toSpeed, out float time, out float distance)
		{
			Point fromPoint;
			Point toPoint;
			GetSpeedChangePoints(fromSpeed, toSpeed, out fromPoint, out toPoint);
			time = toPoint._time - fromPoint._time;
			distance = toPoint._pos - fromPoint._pos;
		}

		public bool GetSpeedChangeStats(float fromSpeed, float toSpeed, float maxDistance, out float time, out float distance, out float finalSpeed)
		{
			bool result = false;
			Point fromPoint;
			Point toPoint;
			GetSpeedChangePoints(fromSpeed, toSpeed, out fromPoint, out toPoint);
			time = toPoint._time - fromPoint._time;
			distance = toPoint._pos - fromPoint._pos;
			if (distance > maxDistance)
			{
				toPoint = ((!(fromSpeed > toSpeed)) ? GetPointByPos(_accelData, fromPoint._pos + maxDistance) : GetPointByPos(_brakeData, fromPoint._pos + maxDistance));
				time = toPoint._time - fromPoint._time;
				distance = maxDistance;
				finalSpeed = toPoint._speed;
			}
			else
			{
				finalSpeed = toSpeed;
				result = true;
			}
			return result;
		}

		public void GetAccelForTimeStats(float fromSpeed, float time, out float distance, out float toSpeed)
		{
			Point pointBySpeed = GetPointBySpeed(_accelData, fromSpeed);
			Point pointByTime = GetPointByTime(_accelData, pointBySpeed._time + time);
			distance = pointByTime._pos - pointBySpeed._pos;
			toSpeed = pointByTime._speed;
		}

		public void GetBrakeForDistanceStats(float fromSpeed, float distance, out float time, out float toSpeed)
		{
			Point pointBySpeed = GetPointBySpeed(_brakeData, fromSpeed);
			Point pointByPos = GetPointByPos(_brakeData, pointBySpeed._pos + distance);
			time = pointByPos._time - pointBySpeed._time;
			toSpeed = pointByPos._speed;
		}

		public void GetBrakeForDistanceStats2(float toSpeed, float distance, out float time, out float fromSpeed)
		{
			Point pointBySpeed = GetPointBySpeed(_brakeData, toSpeed);
			Point pointByPos = GetPointByPos(_brakeData, pointBySpeed._pos - distance);
			time = pointBySpeed._time - pointByPos._time;
			fromSpeed = pointByPos._speed;
		}

		private static void DumpDataSet(DataSet data)
		{
			Debug.Log("[CarStats] byTime");
			Point[] byTime = data._byTime;
			foreach (Point point in byTime)
			{
				Debug.Log(string.Format("\t{0}, {1}, {2}", point._time, point._pos, point._speed));
			}
			Debug.Log("[CarStats] byPos");
			Point[] byPos = data._byPos;
			foreach (Point point2 in byPos)
			{
				Debug.Log(string.Format("\t{0}, {1}, {2}", point2._time, point2._pos, point2._speed));
			}
			Debug.Log("[CarStats] bySpeed");
			Point[] bySpeed = data._bySpeed;
			foreach (Point point3 in bySpeed)
			{
				Debug.Log(string.Format("\t{0}, {1}, {2}", point3._time, point3._pos, point3._speed));
			}
		}

		private void Dump()
		{
			Debug.Log("[CarStats] accelData");
			DumpDataSet(_accelData);
			Debug.Log("[CarStats] brakeData");
			DumpDataSet(_brakeData);
		}

		private void Awake()
		{
			CarSpecs component = GetComponent<CarSpecs>();
			if (component != null)
			{
				RecordData(component, _recordMaxValues, _recordTimeInterval);
			}
			else
			{
				Debug.Log("[CarStats] No CarSpecs component");
			}
		}
	}
}
