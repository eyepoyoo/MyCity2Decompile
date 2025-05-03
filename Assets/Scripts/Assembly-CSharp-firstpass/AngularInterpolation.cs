using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class AngularInterpolation
{
	// Token: 0x060002CF RID: 719 RVA: 0x000117C0 File Offset: 0x0000F9C0
	private AngularInterpolation()
	{
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x000117CC File Offset: 0x0000F9CC
	public float[] GenerateControlPoints(float[] path, bool closedPath)
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

	// Token: 0x060002D2 RID: 722 RVA: 0x0001186C File Offset: 0x0000FA6C
	public float Interpolate(float[] controlPoints, bool closedPath, float t)
	{
		if (closedPath && (t > 1f || t < 0f))
		{
			t -= Mathf.Floor(t);
		}
		int num = controlPoints.Length - 2;
		float num2 = 1f / (float)num;
		int num3 = 1 + Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		return Mathf.LerpAngle(controlPoints[num3], controlPoints[num3 + 1], Mathf.InverseLerp(num2 * (float)num3, num2 * (float)(num3 + 1), t));
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060002D3 RID: 723 RVA: 0x000118E0 File Offset: 0x0000FAE0
	public static AngularInterpolation instance
	{
		get
		{
			if (AngularInterpolation._instance == null)
			{
				AngularInterpolation._instance = new AngularInterpolation();
			}
			return AngularInterpolation._instance;
		}
	}

	// Token: 0x04000198 RID: 408
	private static AngularInterpolation _instance;
}
