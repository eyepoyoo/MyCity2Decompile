using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AutoProbe : MonoBehaviour
{
	public float spacing = 1f;

	public float heightFromGround = 1f;

	public float volumeHeight = 1f;

	public int volumeLevels = 1;

	public LayerMask testLayers;

	public void GenerateNodes()
	{
		if (!GetComponent<LightProbeGroup>())
		{
			base.gameObject.AddComponent<LightProbeGroup>();
		}
		LightProbeGroup component = GetComponent<LightProbeGroup>();
		List<Vector3> list = new List<Vector3>();
		RaycastHit hitInfo;
		for (Vector3 position = base.transform.position; Physics.Raycast(position + new Vector3(0f, 10f, 0f), -Vector3.up, out hitInfo, 200f, testLayers); position += new Vector3(spacing, 0f, 0f))
		{
			list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround, 0f)));
			for (int i = 1; i <= volumeLevels; i++)
			{
				list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround + volumeHeight * (float)i, 0f)));
			}
			for (Vector3 vector = position + new Vector3(0f, 0f, spacing); Physics.Raycast(vector + new Vector3(0f, 10f, 0f), -Vector3.up, out hitInfo, 200f, testLayers); vector += new Vector3(0f, 0f, spacing))
			{
				list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround, 0f)));
				for (int j = 1; j <= volumeLevels; j++)
				{
					list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround + volumeHeight * (float)j, 0f)));
				}
			}
			for (Vector3 vector = position - new Vector3(0f, 0f, spacing); Physics.Raycast(vector + new Vector3(0f, 10f, 0f), -Vector3.up, out hitInfo, 200f, testLayers); vector -= new Vector3(0f, 0f, spacing))
			{
				list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround, 0f)));
				for (int k = 1; k <= volumeLevels; k++)
				{
					list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround + volumeHeight * (float)k, 0f)));
				}
			}
		}
		for (Vector3 position = (base.transform.position -= new Vector3(spacing, 0f, 0f)); Physics.Raycast(position - new Vector3(0f, 10f, 0f), -Vector3.up, out hitInfo, 200f, testLayers); position -= new Vector3(spacing, 0f, 0f))
		{
			list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround, 0f)));
			for (int l = 1; l <= volumeLevels; l++)
			{
				list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround + volumeHeight * (float)l, 0f)));
			}
			for (Vector3 vector2 = position + new Vector3(0f, 0f, spacing); Physics.Raycast(vector2 + new Vector3(0f, 10f, 0f), -Vector3.up, out hitInfo, 200f, testLayers); vector2 += new Vector3(0f, 0f, spacing))
			{
				list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround, 0f)));
				for (int m = 1; m <= volumeLevels; m++)
				{
					list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround + volumeHeight * (float)m, 0f)));
				}
			}
			for (Vector3 vector2 = position - new Vector3(0f, 0f, spacing); Physics.Raycast(vector2 + new Vector3(0f, 10f, 0f), -Vector3.up, out hitInfo, 200f, testLayers); vector2 -= new Vector3(0f, 0f, spacing))
			{
				list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround, 0f)));
				for (int n = 1; n <= volumeLevels; n++)
				{
					list.Add(base.transform.InverseTransformPoint(hitInfo.point + new Vector3(0f, heightFromGround + volumeHeight * (float)n, 0f)));
				}
			}
		}
		component.probePositions = list.ToArray();
	}
}
