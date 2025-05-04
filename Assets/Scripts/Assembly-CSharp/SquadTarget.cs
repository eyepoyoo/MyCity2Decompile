public class SquadTarget
{
	internal int _idActor;

	internal int _numTargeters;

	internal ActorBase _targetActor;

	internal SquadTarget()
	{
		Reset();
	}

	internal void Reset()
	{
		_idActor = -1;
		_numTargeters = 0;
		_targetActor = null;
	}

	internal void IncrementNumTargeters()
	{
		_numTargeters++;
	}

	internal void DecrementNumTargeters()
	{
		if (_numTargeters > 0)
		{
			_numTargeters--;
		}
	}
}
