using System.Collections;
using UnityEngine;

public class EnvironmentSwitcher : MonoBehaviour
{
	public LightProbes lightProbes;

	public Material newSkyboxMaterial;

	public Color skyDesiredTint1 = Color.white;

	public Color skyDesiredTint2 = Color.white;

	public float skyColorTransitionTime = 2f;

	public bool changeFogSettings;

	public float fogDensity = 0.01f;

	public Color fogColor = Color.white;

	public float fogTransitionTime = 2f;

	private Color _currentTint1 = Color.white;

	private Color _currentTint2 = Color.white;

	private bool _usesTint2;

	private void OnTriggerEnter(Collider other)
	{
		if ((bool)lightProbes)
		{
			LightmapSettings.lightProbes = lightProbes;
		}
		if (changeFogSettings)
		{
			StartCoroutine(InterpolateFogSettings());
		}
		if (newSkyboxMaterial != null)
		{
			RenderSettings.skybox = new Material(newSkyboxMaterial);
			if (RenderSettings.skybox.HasProperty("_Tint"))
			{
				_currentTint1 = RenderSettings.skybox.GetColor("_Tint");
			}
			else if (RenderSettings.skybox.HasProperty("_TintA"))
			{
				_currentTint1 = RenderSettings.skybox.GetColor("_TintA");
			}
			if (RenderSettings.skybox.HasProperty("_TintB"))
			{
				_usesTint2 = true;
				_currentTint2 = RenderSettings.skybox.GetColor("_TintB");
			}
			if (_currentTint1 != skyDesiredTint1 || (_usesTint2 && _currentTint2 != skyDesiredTint2))
			{
				StartCoroutine(InterpolateSkyColor());
			}
		}
	}

	private IEnumerator InterpolateFogSettings()
	{
		bool done = false;
		float startTime = Time.time;
		float endTime = fogTransitionTime;
		Color startColor = RenderSettings.fogColor;
		float startDensity = RenderSettings.fogDensity;
		while (!done)
		{
			float t = (Time.time - startTime) / endTime;
			RenderSettings.fogColor = Color.Lerp(startColor, fogColor, t);
			RenderSettings.fogDensity = Mathf.Lerp(startDensity, fogDensity, t);
			if (t >= 1f)
			{
				RenderSettings.fogColor = fogColor;
				RenderSettings.fogDensity = fogDensity;
				done = true;
			}
			yield return new WaitForSeconds(0f);
		}
	}

	private IEnumerator InterpolateSkyColor()
	{
		bool done = false;
		if (!_usesTint2)
		{
			skyDesiredTint2 = Color.white;
			_currentTint2 = Color.white;
		}
		Color startTint1 = _currentTint1;
		Color startTint2 = _currentTint2;
		float startTime = Time.time;
		float endTime = skyColorTransitionTime;
		while (!done)
		{
			float t = (Time.time - startTime) / endTime;
			_currentTint1 = Color.Lerp(startTint1, skyDesiredTint1, t);
			_currentTint2 = Color.Lerp(startTint2, skyDesiredTint2, t);
			if (RenderSettings.skybox.HasProperty("_Tint"))
			{
				RenderSettings.skybox.SetColor("_Tint", _currentTint1);
			}
			else if (RenderSettings.skybox.HasProperty("_TintA"))
			{
				RenderSettings.skybox.SetColor("_TintA", _currentTint1);
			}
			if (_usesTint2)
			{
				RenderSettings.skybox.SetColor("_TintB", _currentTint2);
			}
			if (_currentTint1 == skyDesiredTint1 && _currentTint2 == skyDesiredTint2)
			{
				done = true;
			}
			yield return new WaitForSeconds(0f);
		}
	}
}
