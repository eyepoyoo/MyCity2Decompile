using System.Collections.Generic;
using UnityEngine;

public class ActorDummyTarget : ActorBase
{
	private static int _nextIdDummyTarget = 0;

	private static List<ActorDummyTarget> _dummyTargets = new List<ActorDummyTarget>();

	private int _idDummyTarget;

	[SerializeField]
	private float _effectiveRange = 100f;

	[SerializeField]
	private ePriority _priority;

	public static List<ActorDummyTarget> _pListDummyTargets
	{
		get
		{
			return _dummyTargets;
		}
	}

	public int _pIdDummyTarget
	{
		get
		{
			return _idDummyTarget;
		}
	}

	public int _pPriorityScore
	{
		get
		{
			return (int)_priority;
		}
	}

	public float _pEffectiveRange
	{
		get
		{
			return _effectiveRange;
		}
	}

	public ePriority _pPriority
	{
		get
		{
			return _priority;
		}
	}

	public override bool _pIsAlive
	{
		get
		{
			return base.gameObject.activeInHierarchy;
		}
	}

	public override bool _pIsTargetable
	{
		get
		{
			return true;
		}
	}

	public static bool FindActorDummyTarget(int idActor, ref ActorDummyTarget foundDummyTarget)
	{
		foreach (ActorDummyTarget pListDummyTarget in _pListDummyTargets)
		{
			if (pListDummyTarget == null || pListDummyTarget._pIdActor != idActor)
			{
				continue;
			}
			foundDummyTarget = pListDummyTarget;
			return true;
		}
		return false;
	}

	public static bool DoesActorIdBelongToDummyTarget(int idActor)
	{
		foreach (ActorDummyTarget pListDummyTarget in _pListDummyTargets)
		{
			if (pListDummyTarget == null || pListDummyTarget._pIdActor != idActor)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	protected override void Awake()
	{
		base.Awake();
		if (!_dummyTargets.Contains(this))
		{
			_dummyTargets.Add(this);
			_idDummyTarget = _nextIdDummyTarget++;
		}
	}

	public override void Initialise()
	{
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_dummyTargets.Remove(this);
	}

	protected override void FixedUpdate()
	{
	}

	protected override void CreateBrainForActorTypeClass()
	{
		_brain = null;
	}
}
