using System.Collections.Generic;
using UnityEngine;

public class BehavePool
{
	internal class BehaveData
	{
		private bool _inUse;

		internal BehaveBase _behaveInstance;

		~BehaveData()
		{
			ResetAll();
		}

		internal void ResetAll()
		{
			_inUse = false;
			_behaveInstance = null;
		}

		internal void StartUse(ref BrainActorBase ownerBrain)
		{
			_inUse = true;
			if (_behaveInstance != null)
			{
				_behaveInstance._pOwnerBrain = ownerBrain;
			}
		}

		internal void StopUse()
		{
			_inUse = false;
			if (_behaveInstance != null)
			{
				_behaveInstance.Shutdown();
			}
		}

		internal bool IsInUse()
		{
			return _inUse;
		}
	}

	private static BehavePool _instance;

	private BehaveData _tempData;

	private List<BehaveData> _tempList;

	private Dictionary<int, List<BehaveData>> _behaves = new Dictionary<int, List<BehaveData>>();

	public static BehavePool _pInstance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new BehavePool();
			}
			return _instance;
		}
	}

	private BehavePool()
	{
	}

	~BehavePool()
	{
		ClearBehavePool();
		_behaves = null;
	}

	public void GetBehaviour(int behaviour, ref BehaveBase behave, BrainActorBase ownerBrain)
	{
		if (behave != null)
		{
			behave = null;
		}
		if (behaviour == 1)
		{
			return;
		}
		if (!_behaves.ContainsKey(behaviour))
		{
			_behaves.Add(behaviour, new List<BehaveData>());
		}
		if (!FindExistingBehaviour(behaviour))
		{
			_tempData = new BehaveData();
			ownerBrain.CreateBehaviour(behaviour, ref _tempData._behaveInstance);
			if (_tempData._behaveInstance == null)
			{
				Debug.LogWarning("BehavePool::GetBehaviour - SETUP ISSUE behave failed to be created");
				_tempData = null;
			}
			else
			{
				_behaves[behaviour].Add(_tempData);
			}
		}
		if (_tempData != null)
		{
			behave = _tempData._behaveInstance;
			_tempData.StartUse(ref ownerBrain);
		}
		_tempData = null;
	}

	private bool FindExistingBehaviour(int behaviour)
	{
		_tempList = _behaves[behaviour];
		foreach (BehaveData temp in _tempList)
		{
			if (temp == null || temp.IsInUse() || temp._behaveInstance == null)
			{
				continue;
			}
			_tempData = temp;
			return true;
		}
		_tempList = null;
		return false;
	}

	public void FreeBehaviour(int behaviour, int idBehave)
	{
		_tempList = _behaves[behaviour];
		foreach (BehaveData temp in _tempList)
		{
			if (temp != null && temp.IsInUse() && temp._behaveInstance != null && temp._behaveInstance._pIdBehave == idBehave)
			{
				temp.StopUse();
			}
		}
		_tempList = null;
	}

	public void ClearBehavePool()
	{
		foreach (KeyValuePair<int, List<BehaveData>> behafe in _behaves)
		{
			if (behafe.Value == null)
			{
				continue;
			}
			foreach (BehaveData item in behafe.Value)
			{
				if (item != null)
				{
					item.ResetAll();
				}
			}
			behafe.Value.Clear();
		}
	}
}
