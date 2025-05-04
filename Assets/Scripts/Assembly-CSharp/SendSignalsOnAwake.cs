using UnityEngine;

public class SendSignalsOnAwake : MonoBehaviour
{
	public SignalSender signals;

	private void Awake()
	{
		signals.SendSignals(this);
	}
}
