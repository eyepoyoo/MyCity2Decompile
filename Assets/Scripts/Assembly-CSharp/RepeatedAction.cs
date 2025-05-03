using UnityEngine;

public class RepeatedAction
{
	private float _interval;

	private float _timer;

	public RepeatedAction(float interval)
	{
		_interval = interval;
	}

	public bool Update()
	{
		if ((_timer += Time.deltaTime) >= _interval)
		{
			_timer -= _interval;
			return true;
		}
		return false;
	}

	public void ResetTimer()
	{
		_timer = 0f;
	}

	public void RandomiseTimer()
	{
		_timer = (0f - Random.value) * _interval;
	}
}
