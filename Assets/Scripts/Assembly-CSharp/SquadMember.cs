public class SquadMember
{
	internal bool _waitingToKick;

	internal int _idActorMember;

	internal int _idActorTarget;

	internal float _targetScore;

	internal float _targetingStartTime;

	internal ActorBase _memberActor;

	internal bool _pHasTarget
	{
		get
		{
			return _idActorTarget != -1;
		}
	}

	internal SquadMember()
	{
		Reset();
	}

	internal void Reset()
	{
		_idActorMember = -1;
		_idActorTarget = -1;
		_targetScore = 0f;
		_targetingStartTime = -1f;
		_memberActor = null;
	}
}
