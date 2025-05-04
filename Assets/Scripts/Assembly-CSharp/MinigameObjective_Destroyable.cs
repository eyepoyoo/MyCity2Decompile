using System;
using System.Collections.Generic;
using UnityEngine;

public class MinigameObjective_Destroyable : MinigameObjective
{
	public int _hitpoints = 1;

	public GameObject[] _debrisPrefabs;

	public string _takeDamageSound;

	public string _killedSound;

	public static readonly List<MinigameObjective_Destroyable> _all = new List<MinigameObjective_Destroyable>();

	private int _hitpointsInit;

	private float _lastDamageTime = float.NegativeInfinity;

	public Rigidbody _pRigidbody { get; private set; }

	public int _pInitHitpoints
	{
		get
		{
			return _hitpointsInit;
		}
	}

	public float _pNormHitpoints
	{
		get
		{
			return (float)_hitpoints / (float)_hitpointsInit;
		}
	}

	protected virtual float _pInvulnDuration
	{
		get
		{
			return 0.5f;
		}
	}

	public bool _pIsInvuln
	{
		get
		{
			return Time.time - _lastDamageTime < _pInvulnDuration;
		}
	}

	public override float _pNormProgress
	{
		get
		{
			return 1f - _pNormHitpoints;
		}
	}

	protected virtual int _pHpToDebrisMulti
	{
		get
		{
			return 3;
		}
	}

	public event Action<MinigameObjective_Destroyable> _onTookDamage;

	public event Action<MinigameObjective_Destroyable> _onDied;

	protected override void Awake()
	{
		base.Awake();
		_hitpointsInit = _hitpoints;
		_pRigidbody = GetComponent<Rigidbody>();
		_all.Add(this);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (base._pEnabled && !_pIsInvuln && _hitpoints != 0 && VehicleController_Player.IsPlayer(collision.collider))
		{
			OnHitByPlayer(collision);
		}
	}

	protected virtual void OnHitByPlayer(Collision collision)
	{
		TakeDamage(1, collision);
	}

	protected virtual void SpawnDebris(int num, Vector3 pos, Vector3 relativeVelocity)
	{
		if (_debrisPrefabs.Length != 0)
		{
			for (int i = 0; i < num; i++)
			{
				Rigidbody rigidbody = FastPoolManager.GetPool(_debrisPrefabs.GetRandom()).FastInstantiate<Rigidbody>();
				rigidbody.transform.position = pos;
				rigidbody.transform.parent = base.transform.parent;
				OnDebrisPieceSpawned(rigidbody, pos, relativeVelocity);
			}
		}
	}

	protected virtual void OnDebrisPieceSpawned(Rigidbody debris, Vector3 pos, Vector3 relativeVelocity)
	{
		Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
		onUnitSphere.y = Mathf.Abs(onUnitSphere.y);
		debris.velocity = Vector3.up * 5f + onUnitSphere * 8f + _pRigidbody.velocity;
		debris.angularVelocity = UnityEngine.Random.onUnitSphere * 10f;
	}

	public void TakeDamage(int hitpointsToTake, Collision collision, bool ignoreInvuln = false)
	{
		if (_hitpoints > 0 && (ignoreInvuln || !_pIsInvuln))
		{
			TakeDamage(hitpointsToTake, collision.contacts[0].point, collision.relativeVelocity, ignoreInvuln);
		}
	}

	public void TakeDamage(int hitpointsToTake, Vector3 pos, Vector3 relativeVelocity, bool ignoreInvuln = false)
	{
		if (_hitpoints > 0 && (ignoreInvuln || !_pIsInvuln))
		{
			SpawnDebris(hitpointsToTake * _pHpToDebrisMulti, pos, relativeVelocity);
			TakeDamage(hitpointsToTake, ignoreInvuln);
		}
	}

	public void TakeDamage(int hitpointsToTake, bool ignoreInvuln = false)
	{
		if (_hitpoints > 0 && (ignoreInvuln || !_pIsInvuln))
		{
			_lastDamageTime = Time.time;
			_hitpoints = Mathf.Max(0, _hitpoints - hitpointsToTake);
			Progress();
			if (this._onTookDamage != null)
			{
				this._onTookDamage(this);
			}
			if (_hitpoints == 0)
			{
				Kill();
			}
			else if ((bool)SoundFacade._pInstance && !string.IsNullOrEmpty(_takeDamageSound))
			{
				SoundFacade._pInstance.PlayOneShotSFX(_takeDamageSound, base.transform.position, 0f);
			}
		}
	}

	public virtual void Regenerate()
	{
		_hitpoints = _hitpointsInit;
	}

	protected virtual void Kill()
	{
		if (this._onDied != null)
		{
			this._onDied(this);
		}
		if ((bool)SoundFacade._pInstance && !string.IsNullOrEmpty(_killedSound))
		{
			SoundFacade._pInstance.PlayOneShotSFX(_killedSound, base.transform.position, 0f);
		}
		Complete();
	}

	private void OnDestroy()
	{
		_all.Remove(this);
	}
}
