using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class LTBezier
{
	// Token: 0x0600018A RID: 394 RVA: 0x0000B8C0 File Offset: 0x00009AC0
	public LTBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float precision)
	{
		this.a = a;
		this.aa = -a + 3f * (b - c) + d;
		this.bb = 3f * (a + c) - 6f * b;
		this.cc = 3f * (b - a);
		this.len = 1f / precision;
		this.arcLengths = new float[(int)this.len + 1];
		this.arcLengths[0] = 0f;
		Vector3 vector = a;
		float num = 0f;
		int num2 = 1;
		while ((float)num2 <= this.len)
		{
			Vector3 vector2 = this.bezierPoint((float)num2 * precision);
			num += (vector - vector2).magnitude;
			this.arcLengths[num2] = num;
			vector = vector2;
			num2++;
		}
		this.length = num;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000B9C4 File Offset: 0x00009BC4
	private float map(float u)
	{
		float num = u * this.arcLengths[(int)this.len];
		int i = 0;
		int num2 = (int)this.len;
		int num3 = 0;
		while (i < num2)
		{
			num3 = i + ((int)((float)(num2 - i) / 2f) | 0);
			if (this.arcLengths[num3] < num)
			{
				i = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		if (this.arcLengths[num3] > num)
		{
			num3--;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		return ((float)num3 + (num - this.arcLengths[num3]) / (this.arcLengths[num3 + 1] - this.arcLengths[num3])) / this.len;
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000BA68 File Offset: 0x00009C68
	private Vector3 bezierPoint(float t)
	{
		return ((this.aa * t + this.bb) * t + this.cc) * t + this.a;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000BAA4 File Offset: 0x00009CA4
	public Vector3 point(float t)
	{
		return this.bezierPoint(this.map(t));
	}

	// Token: 0x040000EB RID: 235
	public float length;

	// Token: 0x040000EC RID: 236
	private Vector3 a;

	// Token: 0x040000ED RID: 237
	private Vector3 aa;

	// Token: 0x040000EE RID: 238
	private Vector3 bb;

	// Token: 0x040000EF RID: 239
	private Vector3 cc;

	// Token: 0x040000F0 RID: 240
	private float len;

	// Token: 0x040000F1 RID: 241
	private float[] arcLengths;
}
