using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
	public enum EShape
	{
		NULL = 0,
		SPHERE = 1,
		HEMISPHERE = 2
	}

	private const string LOG_TAG = "[ParticleSystemController] ";

	public float _startLifetime;

	public float _startLifetime2;

	public bool _isStartLifetimeRange;

	public float _startSpeed;

	public float _startSpeed2;

	public bool _isStartSpeedRange;

	public float _startSize;

	public float _startSize2;

	public bool _isStartSizeRange;

	public float _startRotation;

	public float _startRotation2;

	public bool _isStartRotationRange;

	public bool _isRandomRotationAxis;

	public Color _startColour;

	public int _emitCount;

	public int _emitCount2;

	public bool _isEmitCountRange;

	public EShape _shape;

	public float _shapeRadius;

	public bool _isEmitFromShell;

	private ParticleSystem _particleSystem;

	private ParticleSystem.Particle _newParticle;

	public bool _pIsAlive
	{
		get
		{
			return _particleSystem != null && _particleSystem.IsAlive();
		}
	}

	private float _pNewStartRotation
	{
		get
		{
			return (!_isStartRotationRange) ? _startRotation : Random.Range(_startRotation, _startRotation2);
		}
	}

	private float _pNewStartSpeed
	{
		get
		{
			return (!_isStartSpeedRange) ? _startSpeed : Random.Range(_startSpeed, _startSpeed2);
		}
	}

	private float _pNewStartAngularVelocity
	{
		get
		{
			return 0f;
		}
	}

	private float _pNewStartSize
	{
		get
		{
			return (!_isStartSizeRange) ? _startSize : Random.Range(_startSize, _startSize2);
		}
	}

	private float _pNewStartLifetime
	{
		get
		{
			return (!_isStartLifetimeRange) ? _startLifetime : Random.Range(_startLifetime, _startLifetime2);
		}
	}

	private Color _pNewStartColour
	{
		get
		{
			return _startColour;
		}
	}

	private Vector3 _pNewStartDirection
	{
		get
		{
			if (_shape == EShape.NULL)
			{
				return Vector3.zero;
			}
			Vector3 onUnitSphere = Random.onUnitSphere;
			if (_shape == EShape.HEMISPHERE && onUnitSphere.y < 0f)
			{
				onUnitSphere.y = 0f - onUnitSphere.y;
			}
			return onUnitSphere;
		}
	}

	private float _pNewStartOffset
	{
		get
		{
			if (_shape == EShape.NULL)
			{
				return 0f;
			}
			return (!_isEmitFromShell) ? Random.Range(0f, _shapeRadius) : _shapeRadius;
		}
	}

	private int _pNewEmitCount
	{
		get
		{
			return (!_isEmitCountRange) ? _emitCount : Random.Range(_emitCount, _emitCount2 + 1);
		}
	}

	private void Awake()
	{
		Construct();
	}

	public void OfflineAwake()
	{
		Construct();
	}

	public void EmitParticles(Vector3 position)
	{
		if (!(_particleSystem == null))
		{
			int pNewEmitCount = _pNewEmitCount;
			for (int i = 0; i < pNewEmitCount; i++)
			{
				EmitParticle(position);
			}
		}
	}

	public void EmitParticles(Vector3 position, Color colour)
	{
		if (!(_particleSystem == null))
		{
			int pNewEmitCount = _pNewEmitCount;
			for (int i = 0; i < pNewEmitCount; i++)
			{
				EmitParticle(position, colour);
			}
		}
	}

	public void EmitParticles(Vector3 position, float rotation, Vector3 velocity, float angVelocity, float size, float lifetime, Color colour)
	{
		if (!(_particleSystem == null))
		{
			int pNewEmitCount = _pNewEmitCount;
			for (int i = 0; i < pNewEmitCount; i++)
			{
				EmitParticle(position, rotation, velocity, angVelocity, size, lifetime, colour);
			}
		}
	}

	private void Construct()
	{
		_particleSystem = GetComponent<ParticleSystem>();
		_newParticle = default(ParticleSystem.Particle);
	}

	private void AdjustPosition(ref Vector3 position)
	{
		position += _pNewStartOffset * _pNewStartDirection;
	}

	private void AdjustPositionGetVelocity(ref Vector3 position, out Vector3 velocity)
	{
		Vector3 pNewStartDirection = _pNewStartDirection;
		Vector3 vector = _pNewStartOffset * pNewStartDirection;
		position += vector;
		velocity = _pNewStartSpeed * pNewStartDirection;
	}

	private void _EmitParticle(Vector3 position, float rotation, Vector3 velocity, float angVelocity, float size, float lifetime, Color32 colour)
	{
		ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
		{
			position = position,
			rotation = rotation,
			axisOfRotation = ((!_isRandomRotationAxis) ? Vector3.forward : Random.onUnitSphere),
			velocity = velocity,
			angularVelocity = angVelocity,
			startSize = size,
			startLifetime = lifetime,
			startColor = colour,
			randomSeed = (uint)Random.Range(1, 1024)
		};
		_particleSystem.Emit(emitParams, 1);
	}

	private void EmitParticle(Vector3 position)
	{
		Vector3 velocity;
		AdjustPositionGetVelocity(ref position, out velocity);
		_EmitParticle(position, _pNewStartRotation, velocity, _pNewStartAngularVelocity, _pNewStartSize, _pNewStartLifetime, _pNewStartColour);
	}

	private void EmitParticle(Vector3 position, Vector3 velocity)
	{
		AdjustPosition(ref position);
		_EmitParticle(position, _pNewStartRotation, velocity, _pNewStartAngularVelocity, _pNewStartSize, _pNewStartLifetime, _pNewStartColour);
	}

	private void EmitParticle(Vector3 position, float size)
	{
		Vector3 velocity;
		AdjustPositionGetVelocity(ref position, out velocity);
		_EmitParticle(position, _pNewStartRotation, velocity, _pNewStartAngularVelocity, size, _pNewStartLifetime, _pNewStartColour);
	}

	private void EmitParticle(Vector3 position, Color colour)
	{
		Vector3 velocity;
		AdjustPositionGetVelocity(ref position, out velocity);
		_EmitParticle(position, _pNewStartRotation, velocity, _pNewStartAngularVelocity, _pNewStartSize, _pNewStartLifetime, colour);
	}

	private void EmitParticle(Vector3 position, Vector3 velocity, float size)
	{
		AdjustPosition(ref position);
		_EmitParticle(position, _pNewStartRotation, velocity, _pNewStartAngularVelocity, size, _pNewStartLifetime, _pNewStartColour);
	}

	private void EmitParticle(Vector3 position, Vector3 velocity, Color colour)
	{
		AdjustPosition(ref position);
		_EmitParticle(position, _pNewStartRotation, velocity, _pNewStartAngularVelocity, _pNewStartSize, _pNewStartLifetime, colour);
	}

	private void EmitParticle(Vector3 position, float size, Color colour)
	{
		Vector3 velocity;
		AdjustPositionGetVelocity(ref position, out velocity);
		_EmitParticle(position, _pNewStartRotation, velocity, _pNewStartAngularVelocity, size, _pNewStartLifetime, colour);
	}

	private void EmitParticle(Vector3 position, Vector3 velocity, float size, Color colour)
	{
		AdjustPosition(ref position);
		_EmitParticle(position, _pNewStartRotation, velocity, _pNewStartAngularVelocity, size, _pNewStartLifetime, colour);
	}

	private void EmitParticle(Vector3 position, float rotation, Vector3 velocity, float angVelocity, float size, float lifetime, Color colour)
	{
		AdjustPosition(ref position);
		_EmitParticle(position, rotation, velocity, angVelocity, size, lifetime, colour);
	}
}
