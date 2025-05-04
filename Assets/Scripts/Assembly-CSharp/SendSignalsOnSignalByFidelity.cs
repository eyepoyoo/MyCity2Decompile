using UnityEngine;

public class SendSignalsOnSignalByFidelity : MonoBehaviour
{
	public SignalSender lowSignals;

	public SignalSender highSignals;

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
			Fidelity fidelity = Facades<FidelityFacade>.Instance.fidelity;
			if (fidelity == Fidelity.Low)
			{
				lowSignals.SendSignals(this);
			}
			else
			{
				highSignals.SendSignals(this);
			}
			base.enabled = false;
		}
	}
}
