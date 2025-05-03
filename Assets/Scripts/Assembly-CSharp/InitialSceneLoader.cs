using UnityEngine;

public class InitialSceneLoader : MonoBehaviour
{
	private static bool _isLoMode;

	public AnisotropicFiltering _anisotropicFiltering;

	public int antiAliasing;

	public int vSyncCount;

	public int maxQueuedFrames;

	public bool forceLoMode;

	public SignalSender signals;

	public static bool IS_LO_MODE
	{
		get
		{
			return _isLoMode;
		}
	}

	private void OnSignal()
	{
		QualitySettings.anisotropicFiltering = _anisotropicFiltering;
		QualitySettings.antiAliasing = antiAliasing;
		QualitySettings.vSyncCount = vSyncCount;
		QualitySettings.maxQueuedFrames = maxQueuedFrames;
		_isLoMode = false;
		Debug.LogWarning("!!! --- FIDELITY SET TO " + ((!_isLoMode) ? "HIGH" : "LOW") + " --- !!!");
		SendLaunchSignals();
	}

	private void SendLaunchSignals()
	{
		signals.SendSignals(this);
	}
}
