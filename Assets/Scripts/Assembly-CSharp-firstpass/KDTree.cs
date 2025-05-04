using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class KDTree
{
	// Token: 0x06000373 RID: 883 RVA: 0x00014868 File Offset: 0x00012A68
	public KDTree()
	{
		this.lr = new KDTree[2];
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0001487C File Offset: 0x00012A7C
	public static KDTree MakeFromPoints(params Vector3[] points)
	{
		int[] array = KDTree.Iota(points.Length);
		return KDTree.MakeFromPointsInner(0, 0, points.Length - 1, points, array);
	}

	// Token: 0x06000375 RID: 885 RVA: 0x000148A0 File Offset: 0x00012AA0
	private static KDTree MakeFromPointsInner(int depth, int stIndex, int enIndex, Vector3[] points, int[] inds)
	{
		KDTree kdtree = new KDTree();
		kdtree.axis = depth % 3;
		int num = KDTree.FindPivotIndex(points, inds, stIndex, enIndex, kdtree.axis);
		kdtree.pivotIndex = inds[num];
		kdtree.pivot = points[kdtree.pivotIndex];
		int num2 = num - 1;
		if (num2 >= stIndex)
		{
			kdtree.lr[0] = KDTree.MakeFromPointsInner(depth + 1, stIndex, num2, points, inds);
		}
		int num3 = num + 1;
		if (num3 <= enIndex)
		{
			kdtree.lr[1] = KDTree.MakeFromPointsInner(depth + 1, num3, enIndex, points, inds);
		}
		return kdtree;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00014930 File Offset: 0x00012B30
	private static void SwapElements(int[] arr, int a, int b)
	{
		int num = arr[a];
		arr[a] = arr[b];
		arr[b] = num;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0001494C File Offset: 0x00012B4C
	private static int FindSplitPoint(Vector3[] points, int[] inds, int stIndex, int enIndex, int axis)
	{
		float num = points[inds[stIndex]][axis];
		float num2 = points[inds[enIndex]][axis];
		int num3 = (stIndex + enIndex) / 2;
		float num4 = points[inds[num3]][axis];
		if (num > num2)
		{
			if (num4 > num)
			{
				return stIndex;
			}
			if (num2 > num4)
			{
				return enIndex;
			}
			return num3;
		}
		else
		{
			if (num > num4)
			{
				return stIndex;
			}
			if (num4 > num2)
			{
				return enIndex;
			}
			return num3;
		}
	}

	// Token: 0x06000378 RID: 888 RVA: 0x000149C0 File Offset: 0x00012BC0
	public static int FindPivotIndex(Vector3[] points, int[] inds, int stIndex, int enIndex, int axis)
	{
		int num = KDTree.FindSplitPoint(points, inds, stIndex, enIndex, axis);
		Vector3 vector = points[inds[num]];
		KDTree.SwapElements(inds, stIndex, num);
		int i = stIndex + 1;
		int num2 = enIndex;
		while (i <= num2)
		{
			Vector3 vector2 = points[inds[i]];
			if (vector2[axis] > vector[axis])
			{
				KDTree.SwapElements(inds, i, num2);
				num2--;
			}
			else
			{
				KDTree.SwapElements(inds, i - 1, i);
				i++;
			}
		}
		return i - 1;
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00014A4C File Offset: 0x00012C4C
	public static int[] Iota(int num)
	{
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = i;
		}
		return array;
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00014A78 File Offset: 0x00012C78
	public int FindNearest(Vector3 pt)
	{
		float num = 1E+09f;
		int num2 = -1;
		this.Search(pt, ref num, ref num2, -1);
		return num2;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00014A9C File Offset: 0x00012C9C
	public int FindNearest(Vector3 pt, int ignoreIndex)
	{
		float num = 1E+09f;
		int num2 = -1;
		this.Search(pt, ref num, ref num2, ignoreIndex);
		return num2;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00014AC0 File Offset: 0x00012CC0
	private void Search(Vector3 pt, ref float bestSqSoFar, ref int bestIndex, int ignoreIndex)
	{
		float sqrMagnitude = (this.pivot - pt).sqrMagnitude;
		if (sqrMagnitude < bestSqSoFar && this.pivotIndex != ignoreIndex)
		{
			bestSqSoFar = sqrMagnitude;
			bestIndex = this.pivotIndex;
		}
		float num = pt[this.axis] - this.pivot[this.axis];
		int num2 = ((num > 0f) ? 1 : 0);
		if (this.lr[num2] != null)
		{
			this.lr[num2].Search(pt, ref bestSqSoFar, ref bestIndex, ignoreIndex);
		}
		num2 = (num2 + 1) % 2;
		float num3 = num * num;
		if (this.lr[num2] != null && bestSqSoFar > num3)
		{
			this.lr[num2].Search(pt, ref bestSqSoFar, ref bestIndex, ignoreIndex);
		}
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00014B88 File Offset: 0x00012D88
	private float DistFromSplitPlane(Vector3 pt, Vector3 planePt, int axis)
	{
		return pt[axis] - planePt[axis];
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00014B9C File Offset: 0x00012D9C
	public string Dump(int level)
	{
		string text = this.pivotIndex.ToString().PadLeft(level) + "\n";
		if (this.lr[0] != null)
		{
			text += this.lr[0].Dump(level + 2);
		}
		if (this.lr[1] != null)
		{
			text += this.lr[1].Dump(level + 2);
		}
		return text;
	}

	// Token: 0x040001CF RID: 463
	private const int numDims = 3;

	// Token: 0x040001D0 RID: 464
	public KDTree[] lr;

	// Token: 0x040001D1 RID: 465
	public Vector3 pivot;

	// Token: 0x040001D2 RID: 466
	public int pivotIndex;

	// Token: 0x040001D3 RID: 467
	public int axis;
}
