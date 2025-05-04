using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
[Serializable]
public class LTSpline
{
	// Token: 0x06000199 RID: 409 RVA: 0x0000BE90 File Offset: 0x0000A090
	public LTSpline(params Vector3[] pts)
	{
		this.pts = new Vector3[pts.Length];
		Array.Copy(pts, this.pts, pts.Length);
		this.numSections = pts.Length - 3;
		float num = float.PositiveInfinity;
		Vector3 vector = this.pts[1];
		float num2 = 0f;
		for (int i = 2; i < this.pts.Length - 2; i++)
		{
			float num3 = Vector3.Distance(this.pts[i], vector);
			if (num3 < num)
			{
				num = num3;
			}
			num2 += num3;
		}
		float num4 = num / (float)LTSpline.SUBLINE_COUNT;
		int num5 = (int)Mathf.Ceil(num2 / num4) * LTSpline.DISTANCE_COUNT;
		this.ptsAdj = new Vector3[num5];
		vector = this.interp(0f);
		int num6 = 0;
		for (int j = 0; j < num5; j++)
		{
			float num7 = ((float)j + 1f) / (float)num5;
			Vector3 vector2 = this.interp(num7);
			float num8 = Vector3.Distance(vector2, vector);
			if (num8 >= num4)
			{
				this.ptsAdj[num6] = vector2;
				vector = vector2;
				num6++;
			}
		}
		this.ptsAdjLength = num6;
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000BFE0 File Offset: 0x0000A1E0
	public Vector3 map(float u)
	{
		if (u >= 1f)
		{
			return this.pts[this.pts.Length - 2];
		}
		float num = u * (float)(this.ptsAdjLength - 1);
		int num2 = (int)Mathf.Floor(num);
		int num3 = (int)Mathf.Ceil(num);
		Vector3 vector = this.ptsAdj[num2];
		Vector3 vector2 = this.ptsAdj[num3];
		float num4 = num - (float)num2;
		return vector + (vector2 - vector) * num4;
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000C074 File Offset: 0x0000A274
	public Vector3 interp(float t)
	{
		this.currPt = Mathf.Min(Mathf.FloorToInt(t * (float)this.numSections), this.numSections - 1);
		float num = t * (float)this.numSections - (float)this.currPt;
		Vector3 vector = this.pts[this.currPt];
		Vector3 vector2 = this.pts[this.currPt + 1];
		Vector3 vector3 = this.pts[this.currPt + 2];
		Vector3 vector4 = this.pts[this.currPt + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num * num * num) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num * num) + (-vector + vector3) * num + 2f * vector2);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000C1C0 File Offset: 0x0000A3C0
	public Vector3 point(float ratio)
	{
		float num = ((ratio <= 1f) ? ratio : 1f);
		return this.map(num);
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000C1EC File Offset: 0x0000A3EC
	public void place2d(Transform transform, float ratio)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.position;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.eulerAngles = new Vector3(0f, 0f, num);
		}
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000C260 File Offset: 0x0000A460
	public void placeLocal2d(Transform transform, float ratio)
	{
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = transform.parent.TransformPoint(this.point(ratio)) - transform.localPosition;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.eulerAngles = new Vector3(0f, 0f, num);
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000C2DC File Offset: 0x0000A4DC
	public void place(Transform transform, float ratio)
	{
		this.place(transform, ratio, Vector3.up);
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000C2EC File Offset: 0x0000A4EC
	public void place(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(this.point(ratio), worldUp);
		}
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000C328 File Offset: 0x0000A528
	public void placeLocal(Transform transform, float ratio)
	{
		this.placeLocal(transform, ratio, Vector3.up);
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000C338 File Offset: 0x0000A538
	public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(transform.parent.TransformPoint(this.point(ratio)), worldUp);
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000C380 File Offset: 0x0000A580
	public void gizmoDraw(float t = -1f)
	{
		Vector3 vector = this.point(0f);
		for (int i = 1; i <= 120; i++)
		{
			float num = (float)i / 120f;
			Vector3 vector2 = this.point(num);
			Gizmos.DrawLine(vector2, vector);
			vector = vector2;
		}
	}

	// Token: 0x040000FA RID: 250
	public static int DISTANCE_COUNT = 30;

	// Token: 0x040000FB RID: 251
	public static int SUBLINE_COUNT = 50;

	// Token: 0x040000FC RID: 252
	public Vector3[] pts;

	// Token: 0x040000FD RID: 253
	public Vector3[] ptsAdj;

	// Token: 0x040000FE RID: 254
	public int ptsAdjLength;

	// Token: 0x040000FF RID: 255
	public bool orientToPath;

	// Token: 0x04000100 RID: 256
	public bool orientToPath2d;

	// Token: 0x04000101 RID: 257
	private int numSections;

	// Token: 0x04000102 RID: 258
	private int currPt;

	// Token: 0x04000103 RID: 259
	private float totalLength;
}
