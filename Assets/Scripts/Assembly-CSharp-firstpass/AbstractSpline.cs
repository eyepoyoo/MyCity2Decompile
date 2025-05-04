using System;

// Token: 0x0200002E RID: 46
public abstract class AbstractSpline<T>
{
	// Token: 0x060002CB RID: 715 RVA: 0x000117AC File Offset: 0x0000F9AC
	public float GetDistance(T[] controlPoints, bool closedPath, float t1, float t2)
	{
		return this.GetDistance(controlPoints, closedPath, t1, t2, AbstractSpline<T>.DEFAULT_SUBDIVISIONS);
	}

	// Token: 0x060002CC RID: 716
	public abstract T[] GenerateControlPoints(T[] path, bool closedPath);

	// Token: 0x060002CD RID: 717
	public abstract float GetDistance(T[] controlPoints, bool closedPath, float t1, float t2, int subdivisions);

	// Token: 0x060002CE RID: 718
	public abstract T Interpolate(T[] controlPoints, bool closedPath, float t);

	// Token: 0x04000197 RID: 407
	public static int DEFAULT_SUBDIVISIONS = 20;
}
