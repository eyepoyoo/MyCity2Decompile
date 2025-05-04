using UnityEngine;

public class TimedDestroyer : MonoBehaviour
{
	public bool _startTimerOnAwake;

	public float _time;

	public bool _doShrink;

	public bool _doDestroyIfOutOfView;

	public bool _disableRatherThanDestroy;

	private float _startTime = -1f;

	private float _shrinkTime = 0.2f;

	private Vector3 _shrinkVector;

	private Collider _collider;

	public void Awake()
	{
		if (_startTimerOnAwake)
		{
			StartTimer();
		}
		if (_doDestroyIfOutOfView)
		{
			InitDestroyIfOutOfView();
		}
	}

	private void InitDestroyIfOutOfView()
	{
		_collider = GetComponent<Collider>();
		if (!_collider)
		{
			_doDestroyIfOutOfView = false;
		}
	}

	private void OnEnable()
	{
		if (_startTimerOnAwake)
		{
			StartTimer();
		}
	}

	public void StartTimer(float time, bool doDestroyIfOutOfView, bool doShrink = false, bool disableRatherThanDestroy = false)
	{
		_time = time;
		_doDestroyIfOutOfView = doDestroyIfOutOfView;
		_doShrink = doShrink;
		_disableRatherThanDestroy = disableRatherThanDestroy;
		if (doDestroyIfOutOfView)
		{
			InitDestroyIfOutOfView();
		}
		StartTimer();
	}

	public void StartTimer()
	{
		_startTime = Time.time;
		_shrinkVector = Vector3.one * (1f / _shrinkTime) * base.gameObject.transform.localScale.x;
	}

	private void Update()
	{
		if (_startTime == -1f)
		{
			return;
		}
		if (_doDestroyIfOutOfView && !Camera.main.AreBoundsInView(_collider.bounds))
		{
			_Destroy();
		}
		else
		{
			if (!(Time.time > _startTime + _time))
			{
				return;
			}
			_doDestroyIfOutOfView = false;
			if (_doShrink)
			{
				base.gameObject.transform.localScale -= _shrinkVector * Time.deltaTime;
				if (base.gameObject.transform.localScale.x <= 0f)
				{
					_Destroy();
				}
			}
			else
			{
				_Destroy();
			}
		}
	}

	private void _Destroy()
	{
		if (_disableRatherThanDestroy)
		{
			base.gameObject.SetActive(false);
			Object.Destroy(this);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
