using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class MathHelper
{
	// Token: 0x060002FB RID: 763 RVA: 0x000128F4 File Offset: 0x00010AF4
	public static Vector3 RoundVector3ToPrecision(Vector3 v, int decimalPlaces)
	{
		return Vector3.right * MathHelper.RoundToPrecision(v.x, decimalPlaces) + Vector3.up * MathHelper.RoundToPrecision(v.y, decimalPlaces) + Vector3.forward * MathHelper.RoundToPrecision(v.z, decimalPlaces);
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00012950 File Offset: 0x00010B50
	public static float RoundToPrecision(float n, int decimalPlaces)
	{
		float num = MathHelper._powersOf10[decimalPlaces];
		return (float)((int)(n * num)) / num;
	}

	// Token: 0x060002FD RID: 765 RVA: 0x0001296C File Offset: 0x00010B6C
	public static double RoundToPrecision(double n, int decimalPlaces)
	{
		double num = (double)MathHelper._powersOf10[decimalPlaces];
		return (double)((int)(n * num)) / num;
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0001298C File Offset: 0x00010B8C
	public static bool ApproxEquals(float a, float b, float tolerance)
	{
		return Mathf.Abs(a - b) <= tolerance;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0001299C File Offset: 0x00010B9C
	public static bool ApproxEquals(Vector3 a, Vector3 b, float tolerance)
	{
		return (a - b).magnitude <= tolerance;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x000129C0 File Offset: 0x00010BC0
	public static float GetAngle(Vector3 v1, Vector3 v2, Vector3 normal)
	{
		return Mathf.Atan2(Vector3.Dot(normal, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x000129E4 File Offset: 0x00010BE4
	public static float GetArcDistance(Vector3 originPosition, Quaternion originRotation, Vector3 destinationPosition)
	{
		float circleRadius = MathHelper.GetCircleRadius(originPosition, originRotation, destinationPosition);
		if (float.IsInfinity(circleRadius) || float.IsNaN(circleRadius))
		{
			return float.PositiveInfinity;
		}
		Vector3 circleCenter = MathHelper.GetCircleCenter(originPosition, originRotation, destinationPosition);
		float num = Vector3.Angle(originPosition - circleCenter, destinationPosition - circleCenter);
		return 6.2831855f * circleRadius * num / 360f;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00012A44 File Offset: 0x00010C44
	public static Vector3 GetCircleCenter(Vector3 originPosition, Quaternion originRotation, Vector3 destinationPosition)
	{
		float circleRadius = MathHelper.GetCircleRadius(originPosition, originRotation, destinationPosition);
		float num = Mathf.Sign(MathHelper.GetAngle(originRotation * Vector3.forward, destinationPosition - originPosition, originRotation * Vector3.up));
		return originPosition + originRotation * Vector3.left * num * circleRadius;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00012AA4 File Offset: 0x00010CA4
	public static float GetCircleRadius(Vector3 originPosition, Quaternion originRotation, Vector3 destinationPosition)
	{
		float num = 0.017453292f * Vector3.Angle(originRotation * Vector3.forward, destinationPosition - originPosition);
		return Mathf.Abs(Vector3.Distance(originPosition, destinationPosition) * Mathf.Cos(num) / Mathf.Sin(num));
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00012AEC File Offset: 0x00010CEC
	public static float GetCircleRadius(float maxSpeed, float grip, float gravity)
	{
		return Mathf.Abs(maxSpeed * maxSpeed / (grip * gravity));
	}

	// Token: 0x06000305 RID: 773 RVA: 0x00012AFC File Offset: 0x00010CFC
	public static float GetMaxSpeed(float radius, float grip, float gravity)
	{
		if (float.IsInfinity(radius) || float.IsNaN(radius))
		{
			radius = 10000f;
		}
		return Mathf.Sqrt(Mathf.Abs(radius * grip * gravity));
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00012B38 File Offset: 0x00010D38
	public static float GetMaxSpeed(Vector3 originPosition, Quaternion originRotation, Vector3 destinationPosition, float destinationSpeed, float grip, float deceleration, float gravity)
	{
		float circleRadius = MathHelper.GetCircleRadius(originPosition, originRotation, destinationPosition);
		float arcDistance = MathHelper.GetArcDistance(originPosition, originRotation, destinationPosition);
		return Mathf.Max(1f, Mathf.Min(MathHelper.GetMaxSpeed(circleRadius, grip, gravity), destinationSpeed + arcDistance * deceleration));
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00012B78 File Offset: 0x00010D78
	public static float Lerp(float x1, float y1, float x2, float y2, float x)
	{
		return y1 + (y2 - y1) / (x2 - x1) * (x - x1);
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00012B88 File Offset: 0x00010D88
	public static Vector2 LerpVector2(float x1, Vector2 y1, float x2, Vector2 y2, float x)
	{
		return y1 + (y2 - y1) / (x2 - x1) * (x - x1);
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00012BA8 File Offset: 0x00010DA8
	public static Vector3 LerpVector3(float x1, Vector3 y1, float x2, Vector3 y2, float x)
	{
		return y1 + (y2 - y1) / (x2 - x1) * (x - x1);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00012BC8 File Offset: 0x00010DC8
	public static float StepSpring(ref float pos, ref float vel, float spring, float damping, float extForce, float mass, float dt)
	{
		float num = 0.5f * (mass * vel * vel + spring * pos * pos);
		float num2 = (extForce - spring * pos - damping * vel) / mass;
		pos += vel * dt + 0.5f * num2 * dt * dt;
		vel += num2 * dt;
		return 0.5f * (mass * vel * vel + spring * pos * pos) - num;
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00012C38 File Offset: 0x00010E38
	public static float LambdaDecay(float value, float lambda, float dt)
	{
		if (lambda <= 0f)
		{
			return value;
		}
		return value / Mathf.Pow(2f, dt / lambda);
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00012C58 File Offset: 0x00010E58
	public static void CombineTransforms(Vector3 posA, Quaternion rotA, Vector3 posB, Quaternion rotB, out Vector3 posR, out Quaternion rotR)
	{
		posR = posA + rotA * posB;
		rotR = rotA * rotB;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00012C88 File Offset: 0x00010E88
	public static void InvertTransform(Vector3 pos, Quaternion rot, out Vector3 invPos, out Quaternion invRot)
	{
		invRot = Quaternion.Inverse(rot);
		invPos = invRot * -pos;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00012CB0 File Offset: 0x00010EB0
	public static Vector3 ProjectVector3(Vector3 v, Vector3 axis)
	{
		return Vector3.Dot(v, axis) * axis;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00012CC0 File Offset: 0x00010EC0
	public static Vector2 ProjectVector2(Vector2 v, Vector2 axis)
	{
		return Vector2.Dot(v, axis) * axis;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00012CD0 File Offset: 0x00010ED0
	public static Vector3 ClipVector3(Vector3 v, Vector3 axis)
	{
		return v - MathHelper.ProjectVector3(v, axis);
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00012CE0 File Offset: 0x00010EE0
	public static Vector2 ClipVector2(Vector2 v, Vector2 axis)
	{
		return v - MathHelper.ProjectVector2(v, axis);
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00012CF0 File Offset: 0x00010EF0
	public static Vector3 RotateVector3(Vector3 v, Vector3 axis, float angle)
	{
		float num = Mathf.Cos(angle);
		float num2 = Mathf.Sin(angle);
		return num * v + (1f - num) * Vector3.Dot(v, axis) * axis - num2 * Vector3.Cross(v, axis);
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00012D40 File Offset: 0x00010F40
	public static Vector2 RotateVector2(Vector2 v, float angle)
	{
		float num = Mathf.Cos(angle);
		float num2 = Mathf.Sin(angle);
		return new Vector2(num * v.x - num2 * v.y, num2 * v.x + num * v.y);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00012D88 File Offset: 0x00010F88
	public static Vector2 RotateExtents2(Vector2 e, float angle)
	{
		float num = Mathf.Abs(Mathf.Cos(angle));
		float num2 = Mathf.Abs(Mathf.Sin(angle));
		return new Vector2(num * e.x + num2 * e.y, num2 * e.x + num * e.y);
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00012DD8 File Offset: 0x00010FD8
	public static Vector3 TransformExtents(Transform tm, Vector3 extents)
	{
		Matrix4x4 localToWorldMatrix = tm.localToWorldMatrix;
		Vector3 vector;
		vector.x = Mathf.Abs(extents.x * localToWorldMatrix[0, 0]) + Mathf.Abs(extents.y * localToWorldMatrix[1, 0]) + Mathf.Abs(extents.z * localToWorldMatrix[2, 0]);
		vector.y = Mathf.Abs(extents.x * localToWorldMatrix[0, 1]) + Mathf.Abs(extents.y * localToWorldMatrix[1, 1]) + Mathf.Abs(extents.z * localToWorldMatrix[2, 1]);
		vector.z = Mathf.Abs(extents.x * localToWorldMatrix[0, 2]) + Mathf.Abs(extents.y * localToWorldMatrix[1, 2]) + Mathf.Abs(extents.z * localToWorldMatrix[2, 2]);
		return vector;
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00012ED0 File Offset: 0x000110D0
	public static Bounds TransformBounds(Transform tm, Bounds bounds)
	{
		Bounds bounds2 = bounds;
		bounds2.extents = MathHelper.TransformExtents(tm, bounds.extents);
		bounds2.center = tm.TransformPoint(bounds.center);
		return bounds2;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00012F08 File Offset: 0x00011108
	public static Vector3 TransformExtents(SimpleTransform tm, Vector3 extents)
	{
		Vector3 vector;
		vector.x = Mathf.Abs(extents.x * tm[0, 0]) + Mathf.Abs(extents.y * tm[1, 0]) + Mathf.Abs(extents.z * tm[2, 0]);
		vector.y = Mathf.Abs(extents.x * tm[0, 1]) + Mathf.Abs(extents.y * tm[1, 1]) + Mathf.Abs(extents.z * tm[2, 1]);
		vector.z = Mathf.Abs(extents.x * tm[0, 2]) + Mathf.Abs(extents.y * tm[1, 2]) + Mathf.Abs(extents.z * tm[2, 2]);
		return vector;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00012FF8 File Offset: 0x000111F8
	public static Bounds TransformBounds(SimpleTransform tm, Bounds bounds)
	{
		Bounds bounds2 = bounds;
		bounds2.extents = MathHelper.TransformExtents(tm, bounds.extents);
		bounds2.center = tm.TransformPoint(bounds.center);
		return bounds2;
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00013034 File Offset: 0x00011234
	public static Vector3 InverseTransformExtents(Transform tm, Vector3 extents)
	{
		Matrix4x4 localToWorldMatrix = tm.localToWorldMatrix;
		Vector3 vector;
		vector.x = Mathf.Abs(extents.x * localToWorldMatrix[0, 0]) + Mathf.Abs(extents.y * localToWorldMatrix[0, 1]) + Mathf.Abs(extents.z * localToWorldMatrix[0, 2]);
		vector.y = Mathf.Abs(extents.x * localToWorldMatrix[1, 0]) + Mathf.Abs(extents.y * localToWorldMatrix[1, 1]) + Mathf.Abs(extents.z * localToWorldMatrix[1, 2]);
		vector.z = Mathf.Abs(extents.x * localToWorldMatrix[2, 0]) + Mathf.Abs(extents.y * localToWorldMatrix[2, 1]) + Mathf.Abs(extents.z * localToWorldMatrix[2, 2]);
		return vector;
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0001312C File Offset: 0x0001132C
	public static Bounds InverseTransformBounds(Transform tm, Bounds bounds)
	{
		Bounds bounds2 = bounds;
		bounds2.center = tm.InverseTransformPoint(bounds.center);
		bounds2.extents = MathHelper.InverseTransformExtents(tm, bounds.extents);
		return bounds2;
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00013164 File Offset: 0x00011364
	public static Vector3 InverseTransformExtents(SimpleTransform tm, Vector3 extents)
	{
		Vector3 vector;
		vector.x = Mathf.Abs(extents.x * tm[0, 0]) + Mathf.Abs(extents.y * tm[0, 1]) + Mathf.Abs(extents.z * tm[0, 2]);
		vector.y = Mathf.Abs(extents.x * tm[1, 0]) + Mathf.Abs(extents.y * tm[1, 1]) + Mathf.Abs(extents.z * tm[1, 2]);
		vector.z = Mathf.Abs(extents.x * tm[2, 0]) + Mathf.Abs(extents.y * tm[2, 1]) + Mathf.Abs(extents.z * tm[2, 2]);
		return vector;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00013254 File Offset: 0x00011454
	public static Bounds InverseTransformBounds(SimpleTransform tm, Bounds bounds)
	{
		Bounds bounds2 = bounds;
		bounds2.center = tm.InverseTransformPoint(bounds.center);
		bounds2.extents = MathHelper.InverseTransformExtents(tm, bounds.extents);
		return bounds2;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00013290 File Offset: 0x00011490
	public static float ExtentInDirection(Vector2 extents, Vector2 direction)
	{
		return Mathf.Abs(extents.x * direction.x) + Mathf.Abs(extents.y * direction.y);
	}

	// Token: 0x0600031E RID: 798 RVA: 0x000132BC File Offset: 0x000114BC
	public static float ExtentInDirection(Vector3 extents, Vector3 direction)
	{
		return Mathf.Abs(extents.x * direction.x) + Mathf.Abs(extents.y * direction.y) + Mathf.Abs(extents.z * direction.z);
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00013308 File Offset: 0x00011508
	public static Plane TransformPlane(Transform tm, Plane plane)
	{
		Plane plane2 = plane;
		if (tm != null)
		{
			Vector3 vector = plane.normal * plane.distance;
			plane2.SetNormalAndPosition(tm.TransformDirection(plane.normal), tm.TransformPoint(vector));
		}
		return plane2;
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00013354 File Offset: 0x00011554
	public static Plane InverseTransformPlane(Transform tm, Plane plane)
	{
		Plane plane2 = plane;
		if (tm != null)
		{
			Vector3 vector = plane.normal * plane.distance;
			plane2.SetNormalAndPosition(tm.InverseTransformDirection(plane.normal), tm.InverseTransformDirection(vector));
		}
		return plane2;
	}

	// Token: 0x06000321 RID: 801 RVA: 0x000133A0 File Offset: 0x000115A0
	public static float Vector2Cross(Vector2 a, Vector2 b)
	{
		return a.x * b.y - a.y * b.x;
	}

	// Token: 0x06000322 RID: 802 RVA: 0x000133C4 File Offset: 0x000115C4
	public static Vector2 Vector2Perp(Vector2 a)
	{
		return new Vector2(-a.y, a.x);
	}

	// Token: 0x06000323 RID: 803 RVA: 0x000133DC File Offset: 0x000115DC
	public static Vector3 GeneratePerpVector3(Vector3 a)
	{
		Vector3 vector = Vector3.zero;
		float num = Mathf.Abs(a.x);
		float num2 = Mathf.Abs(a.y);
		float num3 = Mathf.Abs(a.z);
		if (num > num2)
		{
			if (num > num3)
			{
				vector = Vector3.Cross(Vector3.forward, a);
			}
			else
			{
				vector = Vector3.Cross(Vector3.up, a);
			}
		}
		else if (num2 > num3)
		{
			vector = Vector3.Cross(Vector3.right, a);
		}
		else
		{
			vector = Vector3.Cross(Vector3.up, a);
		}
		return vector.normalized;
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00013474 File Offset: 0x00011674
	public static bool SolveQuadratic(float a, float b, float c, out float r1, out float r2)
	{
		if (a != 0f)
		{
			float num = b * b - 4f * a * c;
			if (num >= 0f)
			{
				r1 = (-b - Mathf.Sqrt(num)) / (2f * a);
				r2 = (-b + Mathf.Sqrt(num)) / (2f * a);
				return true;
			}
		}
		r1 = (r2 = 0f);
		return false;
	}

	// Token: 0x06000325 RID: 805 RVA: 0x000134DC File Offset: 0x000116DC
	public static float Wrap(float n, float max)
	{
		return (n % max + max) % max;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x000134E8 File Offset: 0x000116E8
	public static float Wrap(float n, float min, float max)
	{
		return MathHelper.Wrap(n - min, max - min) + min;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000134F8 File Offset: 0x000116F8
	public static float Wrap01(float val)
	{
		return MathHelper.Wrap(val, 1f);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00013508 File Offset: 0x00011708
	public static int Wrap(int n, int max)
	{
		return (n % max + max) % max;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00013514 File Offset: 0x00011714
	public static int Wrap(int n, int min, int max)
	{
		return MathHelper.Wrap(n - min, max - min) + min;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00013524 File Offset: 0x00011724
	public static int Wrap01(int val)
	{
		return MathHelper.Wrap(val, 1);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00013530 File Offset: 0x00011730
	public static bool IsInOpenRange(float val, float min, float max)
	{
		return val > min && val < max;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00013540 File Offset: 0x00011740
	public static bool IsInClosedRange(float val, float min, float max)
	{
		return val >= min && val <= max;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00013554 File Offset: 0x00011754
	public static bool IsBetween01(float val)
	{
		return MathHelper.IsInOpenRange(val, 0f, 1f);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00013568 File Offset: 0x00011768
	public static bool IsWholeNumber(float num)
	{
		return Math.Round((double)num) == (double)num;
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00013578 File Offset: 0x00011778
	public static float Sqr(float n)
	{
		return n * n;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00013580 File Offset: 0x00011780
	public static float GetTurnAngle(Transform from, Vector3 toPos)
	{
		return MathHelper.Wrap(Mathf.Atan2(toPos.x - from.position.x, toPos.z - from.position.z) * 57.29578f - MathHelper.Wrap(from.eulerAngles.y, -180f, 180f), -180f, 180f);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x000135F4 File Offset: 0x000117F4
	public static Vector3 GetClosestPointOnLineSeg(Vector3 a, Vector3 b, Vector3 p, out float normProgress, bool clampA = true, bool clampB = true)
	{
		Vector3 vector = p - a;
		Vector3 vector2 = b - a;
		normProgress = Vector3.Dot(vector, vector2) / vector2.sqrMagnitude;
		return a + vector2 * Mathf.Clamp(normProgress, (!clampA) ? float.NegativeInfinity : 0f, (!clampB) ? float.PositiveInfinity : 1f);
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00013664 File Offset: 0x00011864
	public static void EaseTowards(ref float value, float to, float halfLife, float dt)
	{
		value += (to - value) * MathHelper.GetEaseFactor(halfLife, dt);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00013678 File Offset: 0x00011878
	public static void EaseTowards(ref float value, float to, float halfLife, float dt, float threshold)
	{
		if (Mathf.Abs(value - to) > threshold)
		{
			MathHelper.EaseTowards(ref value, to, halfLife, dt);
			if (Mathf.Abs(value - to) < threshold)
			{
				value = to;
			}
		}
	}

	// Token: 0x06000334 RID: 820 RVA: 0x000136B4 File Offset: 0x000118B4
	public static void EaseTowardsAngle(ref float value, float to, float halfLife, float dt)
	{
		value += MathHelper.Wrap(to - value, -180f, 180f) * MathHelper.GetEaseFactor(halfLife, dt);
	}

	// Token: 0x06000335 RID: 821 RVA: 0x000136E4 File Offset: 0x000118E4
	public static Quaternion EaseTowardsRotation(Quaternion value, Quaternion to, float halfLife, float dt)
	{
		return Quaternion.Lerp(value, to, MathHelper.GetEaseFactor(halfLife, dt));
	}

	// Token: 0x06000336 RID: 822 RVA: 0x000136F4 File Offset: 0x000118F4
	public static float GetEaseFactor(float halfLife, float dt)
	{
		return (halfLife != 0f) ? (1f - Mathf.Pow(0.5f, dt / halfLife)) : 1f;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0001372C File Offset: 0x0001192C
	public static float Dist(float ax, float ay, float bx, float by)
	{
		return Mathf.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00013744 File Offset: 0x00011944
	public static float DistSqrd(float ax, float ay, float bx, float by)
	{
		return (bx - ax) * (bx - ax) + (by - ay) * (by - ay);
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00013758 File Offset: 0x00011958
	public static float DistXZ(Vector3 v1, Vector3 v2)
	{
		return Mathf.Sqrt((v2.x - v1.x) * (v2.x - v1.x) + (v2.z - v1.z) * (v2.z - v1.z));
	}

	// Token: 0x0600033A RID: 826 RVA: 0x000137AC File Offset: 0x000119AC
	public static float DistXZSqrd(Vector3 v1, Vector3 v2)
	{
		return (v2.x - v1.x) * (v2.x - v1.x) + (v2.z - v1.z) * (v2.z - v1.z);
	}

	// Token: 0x0600033B RID: 827 RVA: 0x000137F8 File Offset: 0x000119F8
	public static float DistSqrd(Vector3 v1, Vector3 v2)
	{
		return (v2.x - v1.x) * (v2.x - v1.x) + (v2.y - v1.y) * (v2.y - v1.y) + (v2.z - v1.z) * (v2.z - v1.z);
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600033C RID: 828 RVA: 0x00013864 File Offset: 0x00011A64
	public static int _pPlusOrMinusOne
	{
		get
		{
			return (global::UnityEngine.Random.value >= 0.5f) ? 1 : (-1);
		}
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001387C File Offset: 0x00011A7C
	public static bool GetElevation(float yStart, float yEnd, float radius, float launchSpeed, out float launchAngle)
	{
		launchAngle = 0f;
		float num = yEnd - yStart;
		float num2 = Physics.gravity.y * radius * radius / (2f * launchSpeed * launchSpeed);
		float num3;
		float num4;
		if (!MathHelper.SolveQuadratic(num2, radius, num2 - num, out num3, out num4))
		{
			return false;
		}
		launchAngle = Mathf.Atan(num4) * 57.29578f;
		return true;
	}

	// Token: 0x0600033E RID: 830 RVA: 0x000138E0 File Offset: 0x00011AE0
	public static Vector3 GetRandomPointOnSector(float forwardAngle, float arcAngle, float radiusOuter, float radiusInner = 0f)
	{
		float num = (forwardAngle + global::UnityEngine.Random.Range(-arcAngle / 2f, arcAngle / 2f)) * 0.017453292f;
		float num2 = global::UnityEngine.Random.Range(radiusInner, radiusOuter);
		return new Vector3(Mathf.Sin(num), 0f, Mathf.Cos(num)) * num2;
	}

	// Token: 0x040001B8 RID: 440
	public const float TAU = 6.2831855f;

	// Token: 0x040001B9 RID: 441
	private static float[] _powersOf10 = new float[]
	{
		1f, 10f, 100f, 1000f, 10000f, 100000f, 1000000f, 10000000f, 100000000f, 1E+09f,
		1E+10f
	};

	// Token: 0x02000037 RID: 55
	public class SimEqSolver
	{
		// Token: 0x06000340 RID: 832 RVA: 0x00013938 File Offset: 0x00011B38
		private void Initialise(float[,] m)
		{
			this._nr = m.GetLength(0);
			this._nc = m.GetLength(1);
			this._lm = new float[this._nr, this._nc];
			for (int i = 0; i < this._nr; i++)
			{
				for (int j = 0; j < this._nc; j++)
				{
					this._lm[i, j] = m[i, j];
				}
			}
			this._rm = new int[this._nr];
			this._cm = new int[this._nc];
			this._cmi = new int[this._nc];
			for (int k = 0; k < this._nr; k++)
			{
				this._rm[k] = k;
			}
			for (int l = 0; l < this._nc; l++)
			{
				this._cm[l] = l;
			}
			this._pr = -1;
			this._pc = -1;
			this._pv = 0f;
			this._sv = new float[this._nr];
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00013A58 File Offset: 0x00011C58
		private void FindPivot(int sr)
		{
			this._pr = -1;
			this._pc = -1;
			this._pv = 0f;
			for (int i = sr; i < this._nr; i++)
			{
				for (int j = sr; j < this._nr; j++)
				{
					float num = this._lm[this._rm[i], this._cm[j]];
					if (Mathf.Abs(num) > Mathf.Abs(this._pv))
					{
						this._pr = i;
						this._pc = j;
						this._pv = num;
					}
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00013AF4 File Offset: 0x00011CF4
		public float[] Solve(float[,] m)
		{
			this.Initialise(m);
			for (int i = 0; i < this._nr; i++)
			{
				this.FindPivot(i);
				if (this._pv == 0f)
				{
					return null;
				}
				if (this._pr != i)
				{
					int num = this._rm[i];
					this._rm[i] = this._rm[this._pr];
					this._rm[this._pr] = num;
				}
				if (this._pc != i)
				{
					int num2 = this._cm[i];
					this._cm[i] = this._cm[this._pc];
					this._cm[this._pc] = num2;
				}
				if (this._pv != 0f)
				{
					this._lm[this._rm[i], this._cm[i]] = 1f;
					for (int j = i + 1; j < this._nc; j++)
					{
						this._lm[this._rm[i], this._cm[j]] = this._lm[this._rm[i], this._cm[j]] / this._pv;
					}
				}
				for (int k = i + 1; k < this._nr; k++)
				{
					float num3 = this._lm[this._rm[k], this._cm[i]];
					if (num3 != 0f)
					{
						this._lm[this._rm[k], this._cm[i]] = 0f;
						for (int j = i + 1; j < this._nc; j++)
						{
							this._lm[this._rm[k], this._cm[j]] = this._lm[this._rm[k], this._cm[j]] - num3 * this._lm[this._rm[i], this._cm[j]];
						}
					}
				}
			}
			for (int i = this._nr - 1; i >= 0; i--)
			{
				for (int k = 0; k < i; k++)
				{
					float num3 = this._lm[this._rm[k], this._cm[i]];
					if (num3 != 0f)
					{
						this._lm[this._rm[k], this._cm[i]] = 0f;
						this._lm[this._rm[k], this._cm[this._nc - 1]] = this._lm[this._rm[k], this._cm[this._nc - 1]] - num3 * this._lm[this._rm[i], this._cm[this._nc - 1]];
					}
				}
			}
			for (int j = 0; j < this._nc; j++)
			{
				this._cmi[this._cm[j]] = j;
			}
			for (int i = 0; i < this._nr; i++)
			{
				this._sv[i] = this._lm[this._rm[this._cmi[i]], this._cm[this._nc - 1]];
			}
			return this._sv;
		}

		// Token: 0x040001BA RID: 442
		private float[,] _lm;

		// Token: 0x040001BB RID: 443
		private int _nr;

		// Token: 0x040001BC RID: 444
		private int _nc;

		// Token: 0x040001BD RID: 445
		private int[] _rm;

		// Token: 0x040001BE RID: 446
		private int[] _cm;

		// Token: 0x040001BF RID: 447
		private int[] _cmi;

		// Token: 0x040001C0 RID: 448
		private int _pr;

		// Token: 0x040001C1 RID: 449
		private int _pc;

		// Token: 0x040001C2 RID: 450
		private float _pv;

		// Token: 0x040001C3 RID: 451
		private float[] _sv;
	}
}
