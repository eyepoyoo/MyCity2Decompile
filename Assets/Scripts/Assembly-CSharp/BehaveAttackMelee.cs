using GameDefines;
using UnityEngine;

public class BehaveAttackMelee : BehaveBase
{
	protected int _numMeleeAttacks;

	protected float _rangeMelee = 5f;

	protected float _timeOfLastRequest;

	private int _specialAttackIndex;

	private float _movingAttackDelay = 3f;

	public override int _pBehaviourType
	{
		get
		{
			return 6;
		}
	}

	protected override bool _pShouldUpdate
	{
		get
		{
			return base._pShouldUpdate && _pHasMeleeAttacks;
		}
	}

	protected bool _pHasMeleeAttacks
	{
		get
		{
			return _numMeleeAttacks > 0;
		}
	}

	protected bool _pHasSpecialAttack
	{
		get
		{
			return _specialAttackIndex != -1;
		}
	}

	public BehaveAttackMelee()
	{
		_numMeleeAttacks = 0;
		_specialAttackIndex = -1;
	}

	public override void InitialiseData()
	{
	}

	protected override void OnUpdate()
	{
		if (_pHasMeleeAttacks && !(base._pOwnerBrain._pOwnerActor == null) && !base._pOwnerBrain._pHasAttacksRanged && base._pOwnerBrain._pHasTargetValid)
		{
			base._pOwnerBrain._pOwnerActor._pForward = (base._pOwnerBrain._pTargetInfo._positionTarget - base._pOwnerBrain._pOurPos).normalized;
		}
	}

	protected override void OnFixedUpdate()
	{
		if (_pHasMeleeAttacks && base._pOwnerBrain._pHasTargetValid && base._pOwnerBrain._pTargetInfo._distToTarget <= _rangeMelee && GlobalDefines.IsApproximately(base._pOwnerBrain._pTargetInfo._dirToTarget.x, base._pOwnerBrain._pOurForward.x, 0.15f) && GlobalDefines.IsApproximately(base._pOwnerBrain._pTargetInfo._dirToTarget.z, base._pOwnerBrain._pOurForward.z, 0.15f))
		{
			bool flag = true;
			if (!base._pOwnerBrain._pIsAtCurrentDestination && Time.time - _timeOfLastRequest < _movingAttackDelay)
			{
				flag = false;
			}
			if (flag)
			{
				_timeOfLastRequest = Time.time;
				base._pOwnerBrain.RequestAttackMelee(Random.Range(1, _numMeleeAttacks));
			}
		}
	}

	protected override void OnLateUpdate()
	{
	}

	protected override void OnShutdown()
	{
	}

	protected override void OnPause()
	{
	}

	protected override void OnUnPause()
	{
	}

	public override void OnDrawGizmos()
	{
	}
}
