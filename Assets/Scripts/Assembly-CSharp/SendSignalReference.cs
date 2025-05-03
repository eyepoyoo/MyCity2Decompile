using UnityEngine;

public class SendSignalReference : MonoBehaviour
{
	public string reference;

	private void OnSignal(string signal)
	{
		GameObject gameObject = GameObject.Find(reference);
		if (gameObject == null)
		{
			Debug.LogError("Signal reference [" + reference + "] not found -> cannot send signal [" + signal + "]");
		}
		else
		{
			gameObject.SendMessage(signal);
		}
	}
}
