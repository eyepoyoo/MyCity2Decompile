using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

public class SoundFacadePool
{
	private SoundFacadePooledElement[] _poolElements;

	private int[] _unusedElements;

	private int _unusedElementHead;

	private int[] _usedElements;

	private int _usedElementHead;

	private int _poolSize;

	private StringBuilder _dummySB = new StringBuilder();

	public SoundFacadePool(int size, string nameIdentifier)
	{
		Transform transform = SoundFacade._pInstance.transform;
		_poolElements = new SoundFacadePooledElement[size];
		_unusedElements = new int[size];
		_usedElements = new int[size];
		_poolSize = size;
		for (int i = 0; i < size; i++)
		{
			_dummySB.Length = 0;
			_dummySB.Append(nameIdentifier);
			_dummySB.Append("SFXPool");
			_dummySB.Append(i);
			GameObject gameObject = new GameObject(_dummySB.ToString());
			gameObject.transform.parent = transform;
			SoundFacadePooledElement soundFacadePooledElement = gameObject.AddComponent<SoundFacadePooledElement>();
			soundFacadePooledElement._pAudioSource = gameObject.AddComponent<AudioSource>();
			_poolElements[i] = soundFacadePooledElement;
			_unusedElements[i] = i;
			_usedElements[i] = -1;
		}
		_unusedElementHead = size - 1;
		_usedElementHead = -1;
	}

	public string ReportPools()
	{
		_dummySB.Length = 0;
		_dummySB.Append("Unused Elements\n");
		int num = _unusedElements.Length;
		for (int i = 0; i < num && i < _unusedElementHead; i++)
		{
			_dummySB.Append(_poolElements[_unusedElements[i]].name);
			_dummySB.Append("\n");
		}
		_dummySB.Append("\n\nUsed Elements\n");
		int num2 = _usedElements.Length;
		for (int j = 0; j < num2 && j < _usedElementHead; j++)
		{
			_dummySB.Append(_poolElements[_usedElements[j]].name);
			_dummySB.Append("\n");
		}
		return _dummySB.ToString();
	}

	public void UpdateUsedElements()
	{
		if (_usedElementHead >= 0)
		{
			for (int i = 0; i <= _usedElementHead; i++)
			{
				_poolElements[_usedElements[i]].UpdateElement();
			}
		}
	}

	public void LateUpdateUsedElements()
	{
		if (_usedElementHead >= 0)
		{
			for (int i = 0; i <= _usedElementHead; i++)
			{
				_poolElements[_usedElements[i]].LateUpdateElement();
			}
		}
	}

	public void StopAll()
	{
		if (_usedElementHead >= 0)
		{
			for (int i = 0; i <= _usedElementHead; i++)
			{
				_poolElements[_usedElements[i]].Stop();
			}
		}
	}

	public void StopAllByGroupName(string groupName)
	{
		if (_usedElementHead < 0)
		{
			return;
		}
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pSoundEffectInfo.soundGroupName == groupName)
			{
				_poolElements[_usedElements[i]]._pAudioSource.Stop();
			}
		}
	}

	public void StopAllByTag(string tagName)
	{
		if (_usedElementHead < 0)
		{
			return;
		}
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pTag == tagName)
			{
				_poolElements[_usedElements[i]]._pAudioSource.Stop();
			}
		}
	}

	public void StopAllByClipName(string clipName)
	{
		if (_usedElementHead < 0)
		{
			return;
		}
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pAudioSource.clip.name == clipName)
			{
				_poolElements[_usedElements[i]]._pAudioSource.Stop();
			}
		}
	}

	public void StopAllByTransform(Transform trackedTransform)
	{
		if (_usedElementHead < 0)
		{
			return;
		}
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pTrackableTransform == trackedTransform)
			{
				_poolElements[_usedElements[i]]._pAudioSource.Stop();
			}
		}
	}

	public SoundFacadePooledElement FindActiveAudioElementByTransform(Transform tf)
	{
		if (tf == null)
		{
			return null;
		}
		if (_usedElementHead < 0)
		{
			return null;
		}
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pTrackableTransform == tf)
			{
				return _poolElements[_usedElements[i]];
			}
		}
		return null;
	}

	public AudioSource FindActiveAudioSourceByTag(string tagName)
	{
		if (_usedElementHead < 0)
		{
			return null;
		}
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pTag == tagName)
			{
				return _poolElements[_usedElements[i]]._pAudioSource;
			}
		}
		return null;
	}

	public void FindActiveAudioSourcesByTag(string clipName, ref List<AudioSource> targetArray, bool shouldFlush = true)
	{
		if (_usedElementHead < 0)
		{
			return;
		}
		if (shouldFlush)
		{
			targetArray.Clear();
		}
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pTag == clipName)
			{
				targetArray.Add(_poolElements[_usedElements[i]]._pAudioSource);
			}
		}
	}

	public AudioSource FindActiveAudioSource(string clipName)
	{
		if (_usedElementHead < 0)
		{
			return null;
		}
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pAudioSource.clip.name == clipName)
			{
				return _poolElements[_usedElements[i]]._pAudioSource;
			}
		}
		return null;
	}

	public void FindActiveAudioSources(string clipName, ref List<AudioSource> targetArray)
	{
		if (_usedElementHead < 0)
		{
			return;
		}
		targetArray.Clear();
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pAudioSource.clip.name == clipName)
			{
				targetArray.Add(_poolElements[_usedElements[i]]._pAudioSource);
			}
		}
	}

	public void UpdateMuteStates(AudioMixerGroup mixer, bool val)
	{
		if (_usedElementHead < 0)
		{
			return;
		}
		for (int i = 0; i <= _usedElementHead; i++)
		{
			AudioSource pAudioSource = _poolElements[_usedElements[i]]._pAudioSource;
			if (pAudioSource.outputAudioMixerGroup == mixer)
			{
				pAudioSource.mute = val;
			}
		}
	}

	public void ReturnToPool(SoundFacadePooledElement elem)
	{
		bool flag = false;
		int num = -1;
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (!flag && _poolElements[_usedElements[i]] == elem)
			{
				num = _usedElements[i];
				flag = true;
			}
			if (flag)
			{
				if (i < _poolSize - 1)
				{
					_usedElements[i] = _usedElements[i + 1];
				}
				else
				{
					_usedElements[i] = -1;
				}
			}
		}
		if (flag)
		{
			_usedElementHead--;
			_unusedElementHead++;
			_unusedElements[_unusedElementHead] = num;
		}
	}

	public SoundFacadePooledElement GetExistingElementByGroupName(string soundGroupName)
	{
		for (int i = 0; i <= _usedElementHead; i++)
		{
			if (_poolElements[_usedElements[i]]._pSoundEffectInfo.soundGroupName == soundGroupName)
			{
				return _poolElements[_usedElements[i]];
			}
		}
		return null;
	}

	public SoundFacadePooledElement GetFreeElement(int priority)
	{
		if (_unusedElementHead >= 0)
		{
			SoundFacadePooledElement result = _poolElements[_unusedElements[_unusedElementHead]];
			_usedElementHead++;
			_usedElements[_usedElementHead] = _unusedElements[_unusedElementHead];
			_unusedElementHead--;
			return result;
		}
		int num = priority;
		int num2 = -1;
		for (int i = 0; i <= _usedElementHead; i++)
		{
			int pPriority = _poolElements[_usedElements[i]]._pPriority;
			if (pPriority < num)
			{
				num2 = i;
				num = pPriority;
			}
		}
		if (num2 != -1)
		{
			SoundFacadePooledElement soundFacadePooledElement = _poolElements[_usedElements[num2]];
			soundFacadePooledElement.OnHijack();
			return soundFacadePooledElement;
		}
		return null;
	}
}
