using UnityEngine;

[ExecuteInEditMode]
public class SendSignalsOnGUIEvent : MonoBehaviour
{
	public enum GUIEvent
	{
		None = 0,
		ScreenActivated = 1,
		ScreenDeactivated = 2,
		Both = 3
	}

	public GUIEvent guiEvent;

	public bool always;

	public SignalSender signals;

	public void SendOnScreenActivated(bool isActive)
	{
		if (guiEvent != GUIEvent.ScreenDeactivated && (!isActive || always))
		{
			signals.SendSignals(this);
		}
	}

	public void SendOnScreenDeactivated(bool isActive)
	{
		if (guiEvent != GUIEvent.ScreenActivated && (isActive || always))
		{
			signals.SendSignals(this);
		}
	}
}
