using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class LTBezierPath
{
	// Token: 0x0600018E RID: 398 RVA: 0x0000BAB4 File Offset: 0x00009CB4
	public LTBezierPath()
	{
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000BABC File Offset: 0x00009CBC
	public LTBezierPath(Vector3[] pts_)
	{
		this.setPoints(pts_);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000BACC File Offset: 0x00009CCC
	public void setPoints(Vector3[] pts_)
	{
		if (pts_.Length < 4)
		{
			LeanTween.logError("LeanTween - When passing values for a vector path, you must pass four or more values!");
		}
		if (pts_.Length % 4 != 0)
		{
			LeanTween.logError("LeanTween - When passing values for a vector path, they must be in sets of four: controlPoint1, controlPoint2, endPoint2, controlPoint2, controlPoint2...");
		}
		this.pts = pts_;
		int num = 0;
		this.beziers = new LTBezier[this.pts.Length / 4];
		this.lengthRatio = new float[this.beziers.Length];
		this.length = 0f;
		for (int i = 0; i < this.pts.Length; i += 4)
		{
			this.beziers[num] = new LTBezier(this.pts[i], this.pts[i + 2], this.pts[i + 1], this.pts[i + 3], 0.05f);
			this.length += this.beziers[num].length;
			num++;
		}
		for (int i = 0; i < this.beziers.Length; i++)
		{
			this.lengthRatio[i] = this.beziers[i].length / this.length;
		}
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000BC08 File Offset: 0x00009E08
	public Vector3 point(float ratio)
	{
		float num = 0f;
		for (int i = 0; i < this.lengthRatio.Length; i++)
		{
			num += this.lengthRatio[i];
			if (num >= ratio)
			{
				return this.beziers[i].point((ratio - (num - this.lengthRatio[i])) / this.lengthRatio[i]);
			}
		}
		return this.beziers[this.lengthRatio.Length - 1].point(1f);
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000BC84 File Offset: 0x00009E84
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

	// Token: 0x06000193 RID: 403 RVA: 0x0000BCF8 File Offset: 0x00009EF8
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

	// Token: 0x06000194 RID: 404 RVA: 0x0000BD74 File Offset: 0x00009F74
	public void place(Transform transform, float ratio)
	{
		this.place(transform, ratio, Vector3.up);
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000BD84 File Offset: 0x00009F84
	public void place(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(this.point(ratio), worldUp);
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000BDC0 File Offset: 0x00009FC0
	public void placeLocal(Transform transform, float ratio)
	{
		this.placeLocal(transform, ratio, Vector3.up);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000BDD0 File Offset: 0x00009FD0
	public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(transform.parent.TransformPoint(this.point(ratio)), worldUp);
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000BE18 File Offset: 0x0000A018
	public void gizmoDraw(float t = -1f)
	{
		Vector3 vector = this.point(0f);
		for (int i = 1; i <= 120; i++)
		{
			float num = (float)i / 120f;
			Vector3 vector2 = this.point(num);
			Gizmos.color = ((this.previousBezier != this.currentBezier) ? Color.grey : Color.magenta);
			Gizmos.DrawLine(vector2, vector);
			vector = vector2;
			this.previousBezier = this.currentBezier;
		}
	}

	// Token: 0x040000F2 RID: 242
	public Vector3[] pts;

	// Token: 0x040000F3 RID: 243
	public float length;

	// Token: 0x040000F4 RID: 244
	public bool orientToPath;

	// Token: 0x040000F5 RID: 245
	public bool orientToPath2d;

	// Token: 0x040000F6 RID: 246
	private LTBezier[] beziers;

	// Token: 0x040000F7 RID: 247
	private float[] lengthRatio;

	// Token: 0x040000F8 RID: 248
	private int currentBezier;

	// Token: 0x040000F9 RID: 249
	private int previousBezier;
}
