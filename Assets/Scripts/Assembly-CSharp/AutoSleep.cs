using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AutoSleep : MonoBehaviour
{
	public enum EWhen
	{
		Awake = 0,
		Start = 1
	}

	public EWhen _when;

	private void Awake()
	{
		if (_when == EWhen.Awake)
		{
			Sleep();
		}
	}

	private void Start()
	{
		if (_when == EWhen.Start)
		{
			Sleep();
		}
	}

	public void Sleep()
	{
		GetComponent<Rigidbody>().Sleep();
	}
}
