using UnityEngine;

[DisallowMultipleComponent]
public class EngineAudio : MonoBehaviour
{
	public enum EEngineType
	{
		None = 0,
		CarSmall = 1,
		CarMedium = 2,
		FourByFourSmall = 3,
		FourByFourMedium = 4,
		FourByFourBig = 5,
		BoatSmall = 6,
		BoatMedium = 7,
		BoatBig = 8,
		HelicopterSmall = 9,
		HelicopterMedium = 10,
		HelicopterBig = 11,
		PlaneBi = 12,
		PlaneJet = 13,
		HotDogStand = 14,
		HotAirBalloon = 15,
		FloatingTyre = 16
	}

	private const float PITCH_FOLLOW_SPEED_TIME = 5f;

	private const float PITCH_FOLLOW_SPEED_TIME_INV = 0.2f;

	public EEngineType _engineType;

	private Vehicle _vehicle;

	private Vehicle_Car _car;

	private AudioSource _sfxIdle;

	private AudioSource _sfxDrive;

	private AudioSource _sfxSkid;

	private bool _isDriveSFXPlaying;

	private bool _isIdleSFXPlaying;

	private bool _hasMoved;

	private bool _isSkiddingSFXPlaying;

	private float _speedFactorSmoothed;

	private string _soundIdIdle;

	private string _soundIdDrive;

	private string _soundIdSkid = "CarSkid";

	private float _initVolDrive;

	private float _initPitchDrive;

	private float _initVolSkid;

	private float _maxPitch;

	private bool _updatesPitch = true;

	private void Start()
	{
		if (!SoundFacade._pInstance)
		{
			base.enabled = false;
		}
		_vehicle = GetComponent<Vehicle>();
		_car = GetComponent<Vehicle_Car>();
		switch (_engineType)
		{
		default:
			_soundIdIdle = "CarSmallIdle";
			_soundIdDrive = "CarSmallDrive";
			_maxPitch = 1.5f;
			break;
		case EEngineType.CarSmall:
			_soundIdIdle = "CarSmallIdle";
			_soundIdDrive = "CarSmallDrive";
			_maxPitch = 1.5f;
			break;
		case EEngineType.CarMedium:
			_soundIdIdle = "CarMediumIdle";
			_soundIdDrive = "CarMediumDrive";
			_maxPitch = 1.5f;
			break;
		case EEngineType.FourByFourBig:
			_soundIdIdle = "4x4BigIdle";
			_soundIdDrive = "4x4BigDrive";
			_maxPitch = 1.5f;
			break;
		case EEngineType.FourByFourMedium:
			_soundIdIdle = "4x4MediumIdle";
			_soundIdDrive = "4x4MediumDrive";
			_maxPitch = 1.5f;
			break;
		case EEngineType.FourByFourSmall:
			_soundIdIdle = "4x4SmallIdle";
			_soundIdDrive = "4x4SmallDrive";
			_maxPitch = 1.5f;
			break;
		case EEngineType.BoatSmall:
			_soundIdIdle = "BoatSmallIdle";
			_soundIdDrive = "BoatSmallDrive";
			_maxPitch = 1.3f;
			break;
		case EEngineType.BoatMedium:
			_soundIdIdle = "BoatMediumIdle";
			_soundIdDrive = "BoatMediumDrive";
			_maxPitch = 1.3f;
			break;
		case EEngineType.BoatBig:
			_soundIdIdle = "BoatBigIdle";
			_soundIdDrive = "BoatBigDrive";
			_maxPitch = 1.3f;
			break;
		case EEngineType.HelicopterSmall:
			_soundIdIdle = "HelicopterSmallIdle";
			_soundIdDrive = "HelicopterSmallDrive";
			_maxPitch = 1f;
			break;
		case EEngineType.HelicopterMedium:
			_soundIdIdle = "HelicopterMediumIdle";
			_soundIdDrive = "HelicopterMediumDrive";
			_maxPitch = 1f;
			break;
		case EEngineType.HelicopterBig:
			_soundIdIdle = "HelicopterBigIdle";
			_soundIdDrive = "HelicopterBigDrive";
			_maxPitch = 1f;
			break;
		case EEngineType.PlaneBi:
			_soundIdIdle = null;
			_soundIdDrive = "BiPlaneDrive";
			_maxPitch = 1.5f;
			break;
		case EEngineType.PlaneJet:
			_soundIdIdle = null;
			_soundIdDrive = "JetPlaneDrive";
			_maxPitch = 1.5f;
			break;
		case EEngineType.HotDogStand:
			_soundIdIdle = null;
			_soundIdDrive = "HotDogStandDrive";
			_maxPitch = 1.3f;
			break;
		case EEngineType.HotAirBalloon:
			_soundIdIdle = null;
			_soundIdDrive = "HotAirBalloonDrive";
			_maxPitch = 1.5f;
			_updatesPitch = false;
			break;
		case EEngineType.FloatingTyre:
			_soundIdIdle = null;
			_soundIdDrive = "FloatingTyreDrive";
			_maxPitch = 1.5f;
			_updatesPitch = false;
			break;
		}
	}

	private void Update()
	{
		if (!_vehicle)
		{
			return;
		}
		if (_vehicle._pSpeedFactor > 0.05f)
		{
			_hasMoved = true;
			if (_isIdleSFXPlaying)
			{
				SoundFacade._pInstance.StopLoopingAudioByAudioSource(_sfxIdle);
				_isIdleSFXPlaying = false;
			}
			if (!_isDriveSFXPlaying || !_sfxDrive)
			{
				_sfxDrive = SoundFacade._pInstance.PlayLoopingSFX(_soundIdDrive, base.transform, 0f);
				if (_sfxDrive != null)
				{
					_initVolDrive = _sfxDrive.volume;
					_initPitchDrive = _sfxDrive.pitch;
				}
				_isDriveSFXPlaying = true;
			}
			_speedFactorSmoothed = Mathf.MoveTowards(_speedFactorSmoothed, _vehicle._pSpeedFactor, (!(_speedFactorSmoothed < _vehicle._pSpeedFactor)) ? 1f : (Time.deltaTime * 0.2f));
			if (_isDriveSFXPlaying && _sfxDrive != null && _updatesPitch)
			{
				_sfxDrive.pitch = Mathf.Lerp(1f, _maxPitch, _speedFactorSmoothed) * _initPitchDrive;
			}
		}
		else if (!_isIdleSFXPlaying && !_hasMoved)
		{
			if (_soundIdIdle != null)
			{
				_sfxIdle = SoundFacade._pInstance.PlayLoopingSFX(_soundIdIdle, base.transform, 0f);
			}
			_isIdleSFXPlaying = true;
		}
		if ((bool)_car)
		{
			UpdateSkidding();
		}
	}

	private void UpdateSkidding()
	{
		if (_car.SkidFactor > 0f)
		{
			if (!_isSkiddingSFXPlaying)
			{
				_sfxSkid = SoundFacade._pInstance.PlayLoopingSFX(_soundIdSkid, base.transform, 0f);
				_initVolSkid = _sfxSkid.volume;
				_isSkiddingSFXPlaying = true;
			}
			if (_sfxSkid != null)
			{
				_sfxSkid.volume = _initVolSkid * _car.SkidFactor;
			}
		}
		else if (_isSkiddingSFXPlaying)
		{
			SoundFacade._pInstance.StopLoopingAudioByAudioSource(_sfxSkid);
			_isSkiddingSFXPlaying = false;
		}
	}

	private void OnDestroy()
	{
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.StopLoopingAudioByAudioSource(_sfxIdle);
			SoundFacade._pInstance.StopLoopingAudioByAudioSource(_sfxDrive);
			SoundFacade._pInstance.StopLoopingAudioByAudioSource(_sfxSkid);
		}
	}
}
