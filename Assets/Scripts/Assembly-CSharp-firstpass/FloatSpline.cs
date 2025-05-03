using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class FloatSpline : AbstractSpline<float>
{
	// Token: 0x060002E4 RID: 740 RVA: 0x0001208C File Offset: 0x0001028C
	private FloatSpline()
	{
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00012098 File Offset: 0x00010298
	public override float[] GenerateControlPoints(float[] path, bool closedPath)
	{
		int num = path.Length;
		float[] array;
		if (closedPath)
		{
			array = new float[num + 3];
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
			array = new float[num + 2];
			for (int j = 0; j < num; j++)
			{
				array[j + 1] = path[j];
			}
			array[0] = 2f * array[1] - array[2];
			array[num + 1] = 2f * array[num] - array[num - 1];
		}
		return array;
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00012138 File Offset: 0x00010338
	public override float GetDistance(float[] controlPoints, bool closedPath, float t1, float t2, int subdivisions)
	{
		if (subdivisions <= 0)
		{
			return Mathf.Abs(this.Interpolate(controlPoints, closedPath, t1) - this.Interpolate(controlPoints, closedPath, t2));
		}
		float num = (t2 - t1) / (float)subdivisions;
		float num2 = 0f;
		while (t1 < t2)
		{
			num2 = Mathf.Abs(this.Interpolate(controlPoints, closedPath, t1) - this.Interpolate(controlPoints, closedPath, t1 + num));
			t1 += num;
		}
		return num2;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x000121A4 File Offset: 0x000103A4
	public override float Interpolate(float[] controlPoints, bool closedPath, float t)
	{
		if (closedPath && (t > 1f || t < 0f))
		{
			t -= Mathf.Floor(t);
		}
		int num = controlPoints.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		num2 = Mathf.Clamp(num2, 0, num - 1);
		float num3 = t * (float)num - (float)num2;
		float num4 = controlPoints[num2];
		float num5 = controlPoints[num2 + 1];
		float num6 = controlPoints[num2 + 2];
		float num7 = controlPoints[num2 + 3];
		return 0.5f * ((-num4 + 3f * num5 - 3f * num6 + num7) * (num3 * num3 * num3) + (2f * num4 - 5f * num5 + 4f * num6 - num7) * (num3 * num3) + (-num4 + num6) * num3 + 2f * num5);
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060002E9 RID: 745 RVA: 0x00012274 File Offset: 0x00010474
	public static FloatSpline instance
	{
		get
		{
			if (FloatSpline._instance == null)
			{
				FloatSpline._instance = new FloatSpline();
			}
			return FloatSpline._instance;
		}
	}

	// Token: 0x040001B5 RID: 437
	private static FloatSpline _instance;
}
