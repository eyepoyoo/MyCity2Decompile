using UnityEngine;

public class TimedReturnToPool : MonoBehaviour, IFastPoolItem
{
	public float _delay = 2f;

	private float _time;

	public FastPool _pool { get; private set; }

	private void OnEnable()
	{
		_time = _delay;
	}

	private void Update()
	{
		_time -= Time.deltaTime;
		if (_time <= 0f)
		{
			ReturnToPool();
		}
	}

	public void ReturnToPool()
	{
		if (_pool != null)
		{
			_pool.FastDestroy(base.gameObject);
		}
	}

	public void OnCloned(FastPool pool)
	{
		_pool = pool;
	}

	public void OnFastInstantiate(FastPool pool)
	{
		_pool = pool;
	}

	public void OnFastDestroy()
	{
	}
}
