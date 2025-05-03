using System;
using System.Collections.Generic;
using GameDefines;
using UnityEngine;

public class ParticlesFacade : InitialisationObject
{
	[Serializable]
	public class ParticlesTypeInfo
	{
		public EParticlesType _type;

		public ParticleSystemController _controller;
	}

	private const string LOG_TAG = "[ParticlesFacade] ";

	public ParticlesTypeInfo[] _particlesTypes;

	private Dictionary<int, ParticlesTypeInfo> _particlesTypeDict;

	private bool _isInitialized;

	private Transform _keepOnScreenRoot;

	private Vector3 _keepOnScreenOffset = Vector3.zero;

	public Transform _pKeepOnScreenRoot
	{
		set
		{
			_keepOnScreenRoot = value;
		}
	}

	public Vector3 _pKeepOnScreenOffset
	{
		set
		{
			_keepOnScreenOffset = value;
		}
	}

	public Camera _pKeepOnScreenCamera
	{
		set
		{
			_keepOnScreenRoot = value.transform;
			_keepOnScreenOffset = 0.5f * (value.nearClipPlane + value.farClipPlane) * Vector3.forward;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		Facades<ParticlesFacade>.Register(this);
	}

	protected override void OnDestroy()
	{
		Facades<ParticlesFacade>.Register(null);
		base.OnDestroy();
	}

	private void LateUpdate()
	{
		KeepEmittersOnScreen();
	}

	public override void startInitialising()
	{
		_currentState = InitialisationState.INITIALISING;
		Initialize();
		_currentState = InitialisationState.FINISHED;
	}

	public void Emit(EParticlesType particlesType, Vector3 position, Color colour)
	{
		if (!_isInitialized)
		{
			Debug.LogWarning("[ParticlesFacade] Not initialized", base.gameObject);
			return;
		}
		ParticlesTypeInfo particlesTypeInfo = GetParticlesTypeInfo(particlesType);
		if (particlesTypeInfo == null)
		{
			Debug.LogWarning("[ParticlesFacade] No info for particles type: " + particlesType, base.gameObject);
		}
		else if (particlesTypeInfo._controller == null)
		{
			Debug.LogWarning("[ParticlesFacade] No particles for particles type: " + particlesType, base.gameObject);
		}
		else
		{
			particlesTypeInfo._controller.EmitParticles(position, colour);
		}
	}

	private void Initialize()
	{
		RefreshDict();
		_isInitialized = true;
	}

	private void RefreshDict()
	{
		_particlesTypeDict = new Dictionary<int, ParticlesTypeInfo>();
		if (_particlesTypes == null)
		{
			return;
		}
		for (int i = 0; i < _particlesTypes.Length; i++)
		{
			int type = (int)_particlesTypes[i]._type;
			if (_particlesTypeDict.ContainsKey(type))
			{
				Debug.LogError("[ParticlesFacade] Particles type " + i + " is duplicate", base.gameObject);
			}
			else if (_particlesTypes[i]._controller == null)
			{
				Debug.LogError("[ParticlesFacade] Particles type " + i + " has no controller", base.gameObject);
			}
			else
			{
				_particlesTypeDict.Add(type, _particlesTypes[i]);
			}
		}
	}

	private ParticlesTypeInfo GetParticlesTypeInfo(EParticlesType particlesType)
	{
		ParticlesTypeInfo result = null;
		if (_particlesTypeDict != null)
		{
			int key = (int)particlesType;
			if (_particlesTypeDict.ContainsKey(key))
			{
				result = _particlesTypeDict[key];
			}
		}
		else if (_particlesTypes != null)
		{
			result = Array.Find(_particlesTypes, (ParticlesTypeInfo s) => s._type == particlesType);
		}
		return result;
	}

	private void KeepEmittersOnScreen()
	{
		if (!(_keepOnScreenRoot == null))
		{
			for (int i = 0; i < _particlesTypes.Length && !(_particlesTypes[i]._controller == null) && _particlesTypes[i]._controller._pIsAlive; i++)
			{
				_particlesTypes[i]._controller.transform.position = _keepOnScreenRoot.TransformPoint(_keepOnScreenOffset);
			}
		}
	}
}
