using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class IntSpline : AbstractSpline<int>
{
	// Token: 0x060002EA RID: 746 RVA: 0x00012290 File Offset: 0x00010490
	private IntSpline()
	{
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0001229C File Offset: 0x0001049C
	public override int[] GenerateControlPoints(int[] path, bool closedPath)
	{
		int num = path.Length;
		int[] array;
		if (closedPath)
		{
			array = new int[num + 3];
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
			array = new int[num + 2];
			for (int j = 0; j < num; j++)
			{
				array[j + 1] = path[j];
			}
			array[0] = 2 * array[1] - array[2];
			array[num + 1] = 2 * array[num] - array[num - 1];
		}
		return array;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00012334 File Offset: 0x00010534
	public override float GetDistance(int[] controlPoints, bool closedPath, float t1, float t2, int subdivisions)
	{
		if (subdivisions <= 0)
		{
			return (float)Mathf.Abs(this.Interpolate(controlPoints, closedPath, t1) - this.Interpolate(controlPoints, closedPath, t2));
		}
		float num = (t2 - t1) / (float)subdivisions;
		float num2 = 0f;
		while (t1 < t2)
		{
			num2 = (float)Mathf.Abs(this.Interpolate(controlPoints, closedPath, t1) - this.Interpolate(controlPoints, closedPath, t1 + num));
			t1 += num;
		}
		return num2;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x000123A4 File Offset: 0x000105A4
	public override int Interpolate(int[] controlPoints, bool closedPath, float t)
	{
		if (closedPath && (t > 1f || t < 0f))
		{
			t -= Mathf.Floor(t);
		}
		int num = controlPoints.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		num2 = Mathf.Clamp(num2, 0, num - 1);
		float num3 = t * (float)num - (float)num2;
		float num4 = (float)controlPoints[num2];
		float num5 = (float)controlPoints[num2 + 1];
		float num6 = (float)controlPoints[num2 + 2];
		float num7 = (float)controlPoints[num2 + 3];
		return Mathf.RoundToInt(0.5f * ((-num4 + 3f * num5 - 3f * num6 + num7) * (num3 * num3 * num3) + (2f * num4 - 5f * num5 + 4f * num6 - num7) * (num3 * num3) + (-num4 + num6) * num3 + 2f * num5));
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060002EF RID: 751 RVA: 0x0001247C File Offset: 0x0001067C
	public static IntSpline instance
	{
		get
		{
			if (IntSpline._instance == null)
			{
				IntSpline._instance = new IntSpline();
			}
			return IntSpline._instance;
		}
	}

	// Token: 0x040001B6 RID: 438
	private static IntSpline _instance;
}
