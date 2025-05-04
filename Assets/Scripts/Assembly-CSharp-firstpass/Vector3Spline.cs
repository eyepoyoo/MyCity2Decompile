using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class Vector3Spline : AbstractSpline<Vector3>
{
	// Token: 0x060002F0 RID: 752 RVA: 0x00012498 File Offset: 0x00010698
	private Vector3Spline()
	{
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x000124A4 File Offset: 0x000106A4
	public override Vector3[] GenerateControlPoints(Vector3[] path, bool closedPath)
	{
		int num = path.Length;
		Vector3[] array;
		if (closedPath)
		{
			array = new Vector3[num + 3];
			for (int i = 0; i < num; i++)
			{
				array[i + 1] = path[i];
			}
			array[0] = array[num];
			array[num + 1] = array[1];
			array[num + 2] = array[2];
		}
		else
		{
			array = new Vector3[num + 2];
			for (int j = 0; j < num; j++)
			{
				array[j + 1] = path[j];
			}
			array[0] = 2f * array[1] - array[2];
			array[num + 1] = 2f * array[num] - array[num - 1];
		}
		return array;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x000125E4 File Offset: 0x000107E4
	public override float GetDistance(Vector3[] controlPoints, bool closedPath, float t1, float t2, int subdivisions)
	{
		if (subdivisions <= 0)
		{
			return Vector3.Distance(this.Interpolate(controlPoints, closedPath, t1), this.Interpolate(controlPoints, closedPath, t2));
		}
		float num = (t2 - t1) / (float)subdivisions;
		float num2 = 0f;
		while (t1 < t2)
		{
			num2 += Vector3.Distance(this.Interpolate(controlPoints, closedPath, t1), this.Interpolate(controlPoints, closedPath, t1 + num));
			t1 += num;
		}
		return num2;
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00012650 File Offset: 0x00010850
	public override Vector3 Interpolate(Vector3[] controlPoints, bool closedPath, float t)
	{
		if (closedPath && (t > 1f || t < 0f))
		{
			t -= Mathf.Floor(t);
		}
		int num = controlPoints.Length - 1;
		int num2 = Mathf.FloorToInt(t * (float)num);
		int num3 = num2 - 1;
		int num4 = controlPoints.Length - 3;
		num3 = Mathf.Clamp(num3, 0, num4 - 1);
		float num5 = 1f / (float)num;
		float num6 = MathHelper.Lerp(num5 * (float)(num3 + 1), 0f, num5 * (float)(num3 + 2), 1f, t);
		Vector3 vector = controlPoints[num3];
		Vector3 vector2 = controlPoints[num3 + 1];
		Vector3 vector3 = controlPoints[num3 + 2];
		Vector3 vector4 = controlPoints[num3 + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num6 * num6 * num6) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num6 * num6) + (-vector + vector3) * num6 + 2f * vector2);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x000127C8 File Offset: 0x000109C8
	public virtual Quaternion InterpolateRotation(Vector3[] controlPoints, bool closedPath, float t)
	{
		Vector3 vector = this.Interpolate(controlPoints, closedPath, t - 1E-05f);
		Vector3 vector2 = this.Interpolate(controlPoints, closedPath, t + 1E-05f);
		return Quaternion.LookRotation(vector2 - vector);
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00012804 File Offset: 0x00010A04
	public virtual Quaternion InterpolateRotation(Vector3[] positionControlPoints, Vector3[] upVectorControlPoints, bool closedPath, float t)
	{
		Vector3 vector = this.Interpolate(positionControlPoints, closedPath, t - 1E-05f);
		Vector3 vector2 = this.Interpolate(positionControlPoints, closedPath, t + 1E-05f);
		Vector3 vector3 = this.Interpolate(upVectorControlPoints, closedPath, t);
		return Quaternion.LookRotation(vector2 - vector, vector3);
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00012850 File Offset: 0x00010A50
	public virtual Quaternion InterpolateRotation(Vector3[] positionControlPoints, float[] bankingControlPoints, bool closedPath, float t)
	{
		Vector3 vector = this.Interpolate(positionControlPoints, closedPath, t - 1E-05f);
		Vector3 vector2 = this.Interpolate(positionControlPoints, closedPath, t + 1E-05f);
		float num = AngularInterpolation.instance.Interpolate(bankingControlPoints, closedPath, t);
		return Quaternion.LookRotation(vector2 - vector) * Quaternion.Euler(0f, 0f, num);
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060002F8 RID: 760 RVA: 0x000128B4 File Offset: 0x00010AB4
	public static Vector3Spline instance
	{
		get
		{
			if (Vector3Spline._instance == null)
			{
				Vector3Spline._instance = new Vector3Spline();
			}
			return Vector3Spline._instance;
		}
	}

	// Token: 0x040001B7 RID: 439
	private static Vector3Spline _instance;
}
