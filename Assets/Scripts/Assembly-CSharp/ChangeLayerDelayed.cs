using UnityEngine;

public class ChangeLayerDelayed : MonoBehaviour, IFastPoolItem
{
	public int _layer;

	public float _delay;

	private int _initLayer;

	private void Awake()
	{
		_initLayer = base.gameObject.layer;
	}

	private void Start()
	{
		StartTimer();
	}

	private void StartTimer()
	{
		CancelInvoke();
		Invoke("SetLayer", _delay);
	}

	private void SetLayer()
	{
		base.gameObject.layer = _layer;
	}

	public void OnCloned(FastPool pool)
	{
	}

	public void OnFastInstantiate(FastPool pool)
	{
		base.gameObject.layer = _initLayer;
		StartTimer();
	}

	public void OnFastDestroy()
	{
		CancelInvoke();
	}
}
