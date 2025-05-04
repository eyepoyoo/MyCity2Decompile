using System;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class SunlightControl : MonoBehaviour
{
	public Color lightColour;

	public float lightIntensity;

	public float shadowIntensity;

	private GameObject lightCasterPrv;

	private GameObject shadowCasterPrv;

	public SunlightControl()
	{
		lightColour = new Color(1f, 1f, 1f, 1f);
		lightIntensity = 1f;
		shadowIntensity = 0.5f;
	}

	public virtual void Start()
	{
		lightCasterPrv = GameObject.Find("LightCaster");
		shadowCasterPrv = GameObject.Find("ShadowCaster");
	}

	public virtual void Update()
	{
		if (!(shadowIntensity <= 1f))
		{
			shadowIntensity = 1f;
		}
		if (!(shadowIntensity >= 0f))
		{
			shadowIntensity = 0f;
		}
		float num = shadowIntensity;
		float num2 = 1f - shadowIntensity;
		lightCasterPrv.gameObject.GetComponent<Light>().intensity = lightIntensity * num2;
		lightCasterPrv.gameObject.GetComponent<Light>().color = lightColour;
		shadowCasterPrv.gameObject.GetComponent<Light>().intensity = lightIntensity * num;
		shadowCasterPrv.gameObject.GetComponent<Light>().color = lightColour;
	}

	public virtual void Main()
	{
	}
}
