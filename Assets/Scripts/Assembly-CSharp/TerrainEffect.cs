using UnityEngine;

[RequireComponent(typeof(Vehicle))]
public abstract class TerrainEffect : MonoBehaviour
{
	private float _removeStartTime = float.PositiveInfinity;

	private bool _isRemoving;

	protected Vehicle _vehicle;

	private AudioSource _audioSource;

	private float _speedFactorSmoothed;

	protected virtual float _pRemoveDuration
	{
		get
		{
			return 0f;
		}
	}

	public float _pNormRemovingTime
	{
		get
		{
			return (_pRemoveDuration != 0f) ? (Mathf.Clamp(Time.time - _removeStartTime, 0f, _pRemoveDuration) / _pRemoveDuration) : 0f;
		}
	}

	protected Vehicle _pVehicle
	{
		get
		{
			return _vehicle;
		}
	}

	protected virtual string _pSoundId
	{
		get
		{
			return string.Empty;
		}
	}

	protected virtual void Awake()
	{
		_vehicle = GetComponent<Vehicle>();
		if ((bool)SoundFacade._pInstance && !string.IsNullOrEmpty(_pSoundId))
		{
			_audioSource = SoundFacade._pInstance.PlayLoopingSFX(_pSoundId, base.transform, 0f);
		}
	}

	protected virtual void Update()
	{
		if (_isRemoving && Time.time - _removeStartTime > _pRemoveDuration)
		{
			RemoveInstant();
		}
		_speedFactorSmoothed = Mathf.MoveTowards(_speedFactorSmoothed, _vehicle._pSpeedFactor, Time.deltaTime * 3f);
		if ((bool)_audioSource)
		{
			_audioSource.volume = Mathf.InverseLerp(0.2f, 1f, _speedFactorSmoothed) * 0.2f;
		}
	}

	public virtual void Reset()
	{
		_isRemoving = false;
		_removeStartTime = float.PositiveInfinity;
	}

	public virtual void StartRemove()
	{
		_removeStartTime = Time.time;
		_isRemoving = true;
	}

	public virtual void RemoveInstant()
	{
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.StopLoopingAudioByAudioSource(_audioSource);
		}
		Object.Destroy(this);
	}
}
