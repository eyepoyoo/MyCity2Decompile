using UnityEngine;

public class BehaveAttackRanged : BehaveBase
{
	private const float _cFireAngle = 25f;

	private Vector3 _dummyDir1;

	private Vector3 _dummyDir2;

	public override int _pBehaviourType
	{
		get
		{
			return 3;
		}
	}

	protected override bool _pShouldUpdate
	{
		get
		{
			return base._pShouldUpdate && _ownerAllegiance != EAllegiance.NEUTRAL;
		}
	}

	public override void InitialiseData()
	{
	}

	protected override void OnUpdate()
	{
		if (base._pOwnerBrain != null && !(base._pOwnerBrain._pOwnerActor == null) && !base._pOwnerBrain._pOwnerActor._pIsPlayer && base._pOwnerBrain._pHasTargetValid)
		{
			base._pOwnerBrain._pOwnerActor._pForward = base._pOwnerBrain._pTargetInfo._positionTarget;
		}
	}

	protected override void OnFixedUpdate()
	{
		if (base._pOwnerBrain != null && !(base._pOwnerBrain._pOwnerActor == null) && !base._pOwnerBrain._pOwnerActor._pIsPlayer && base._pOwnerBrain._pHasTargetValid)
		{
			_dummyDir1 = base._pOwnerBrain._pTargetInfo._dirToTarget;
			_dummyDir2 = base._pOwnerBrain._pOurForward;
			float num = Vector3.Angle(_dummyDir1, _dummyDir2);
			if ((num <= 25f) ? true : false)
			{
				base._pOwnerBrain.RequestAttackRanged();
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
