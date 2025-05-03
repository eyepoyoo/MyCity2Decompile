using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class LTUtility
{
	// Token: 0x060000EF RID: 239 RVA: 0x000055D0 File Offset: 0x000037D0
	public static Vector3[] reverse(Vector3[] arr)
	{
		int num = arr.Length - 1;
		for (int i = 0; i <= arr.Length / 2; i++)
		{
			Vector3 vector = arr[i];
			arr[i] = arr[num];
			arr[num] = vector;
			num--;
		}
		return arr;
	}
}
