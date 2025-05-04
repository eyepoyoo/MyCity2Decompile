using UnityEngine;

public class SendSignalsOnTrigger : MonoBehaviour
{
	public SignalSender signals;

	public string layer;

	private int _layer;

	private void Awake()
	{
		_layer = LayerMask.NameToLayer(layer);
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.layer == _layer)
		{
			signals.SendSignals(this);
		}
	}
}
