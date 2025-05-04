using UnityEngine;

public class SendSignalsOnSignal : MonoBehaviour
{
	public SignalSender signals;

	public int signalCount = 1;

	private int signalsReceived;

	private void Awake()
	{
		signalsReceived = 0;
	}

	private void OnSignal()
	{
		signalsReceived++;
		if (signalsReceived >= signalCount)
		{
			Debug.Log(">> " + base.name + " got all " + signalCount + " signal(s) it needed to send its signals");
			signals.SendSignals(this);
			base.enabled = false;
		}
	}
}
