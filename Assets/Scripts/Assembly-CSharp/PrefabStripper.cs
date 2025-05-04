using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabStripper : MonoBehaviour
{
	private void Start()
	{
		MonoBehaviour[] componentsInChildren = GetComponentsInChildren<MonoBehaviour>();
		for (int num = componentsInChildren.Length - 1; num >= 0; num--)
		{
			if (!(componentsInChildren[num] == null) && !(componentsInChildren[num] == this))
			{
				Object.DestroyImmediate(componentsInChildren[num]);
			}
		}
		componentsInChildren = null;
		SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int num2 = componentsInChildren2.Length - 1; num2 >= 0; num2--)
		{
			if (!(componentsInChildren2[num2] == null))
			{
				componentsInChildren2[num2].bounds.extents.Set(1f, 1f, 1f);
			}
		}
		componentsInChildren2 = null;
		Collider[] componentsInChildren3 = GetComponentsInChildren<Collider>();
		for (int num3 = componentsInChildren3.Length - 1; num3 >= 0; num3--)
		{
			if (!(componentsInChildren3[num3] == null))
			{
				Object.DestroyImmediate(componentsInChildren3[num3]);
			}
		}
		componentsInChildren3 = null;
		Transform[] componentsInChildren4 = GetComponentsInChildren<Transform>();
		List<GameObject> list = new List<GameObject>();
		for (int num4 = componentsInChildren4.Length - 1; num4 >= 0; num4--)
		{
			if (!(componentsInChildren4[num4] == null) && componentsInChildren4[num4].gameObject.name == "Jumper_Clutch_Posed")
			{
				list.Add(componentsInChildren4[num4].gameObject);
			}
		}
		for (int num5 = list.Count - 1; num5 >= 0; num5--)
		{
			Object.DestroyImmediate(list[num5]);
		}
		componentsInChildren4 = null;
		Object.DestroyImmediate(this);
	}
}
