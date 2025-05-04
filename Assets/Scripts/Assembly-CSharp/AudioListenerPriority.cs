using System.Collections.Generic;
using UnityEngine;

public class AudioListenerPriority : MonoBehaviour
{
	private const string LOG_TAG = "[AudioListenerPriority] ";

	private static List<AudioListenerPriority> _instances = new List<AudioListenerPriority>();

	private static int _noRefreshCounter = 0;

	public int _priority;

	private AudioListener _listener;

	private static void RegisterInstance(AudioListenerPriority listener)
	{
		if (_instances != null && !_instances.Contains(listener))
		{
			_instances.Add(listener);
		}
	}

	private static void UnregisterInstance(AudioListenerPriority listener)
	{
		if (_instances != null && _instances.Contains(listener))
		{
			_instances.Remove(listener);
		}
	}

	private static void RefreshEnabledInstance()
	{
		if (_instances == null || _noRefreshCounter > 0)
		{
			return;
		}
		int? num = null;
		for (int i = 0; i < _instances.Count; i++)
		{
			if (!(_instances[i] == null) && _instances[i].enabled && _instances[i].gameObject.activeInHierarchy && (!num.HasValue || _instances[i]._priority > _instances[num.Value]._priority))
			{
				num = i;
			}
		}
		_noRefreshCounter++;
		for (int j = 0; j < _instances.Count; j++)
		{
			if (!(_instances[j] == null))
			{
				if (num.HasValue && j == num.Value)
				{
					_instances[j].SetListenerEnabled(true);
				}
				else
				{
					_instances[j].SetListenerEnabled(false);
				}
			}
		}
		_noRefreshCounter--;
	}

	private void Awake()
	{
		_listener = GetComponent<AudioListener>();
		RegisterInstance(this);
	}

	private void OnEnable()
	{
		RefreshEnabledInstance();
	}

	private void OnDisable()
	{
		RefreshEnabledInstance();
	}

	private void OnDestroy()
	{
		UnregisterInstance(this);
	}

	private void SetListenerEnabled(bool enabled)
	{
		if (!(_listener == null))
		{
			_listener.enabled = enabled;
		}
	}
}
