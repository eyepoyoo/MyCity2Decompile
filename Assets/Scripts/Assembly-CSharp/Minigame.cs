using System;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
	public enum EStage
	{
		Waiting = 0,
		InProgress = 1,
		Complete = 2,
		Failed = 3
	}

	public MinigameObjective[] _objectives;

	public bool _sequential = true;

	public bool _randomiseOrder;

	public bool _randomiseOrder_KeepFirst;

	public bool _disableControllersWhileInactive;

	public int _focusOnClosestWhenObjectivesRemainingLessThan = 999;

	public int _numObjectivesRequiredToComplete;

	public readonly List<MinigameObjective> _remainingObjectives = new List<MinigameObjective>();

	private MinigameObjective _primaryObjective;

	private readonly Dictionary<MinigameObjective, bool> _tempObjectiveEnabledStates = new Dictionary<MinigameObjective, bool>();

	private bool _shouldShowDirectorArrow;

	private MinigameObjective _prevCompletedObjective;

	public MinigameObjective _pOverridingObjective { get; private set; }

	public bool _pHasBeenCompleted { get; private set; }

	public EStage _pStage { get; private set; }

	public int _pNumObjectives
	{
		get
		{
			return _objectives.Length;
		}
	}

	public int _pNumObjectivesCompleted { get; private set; }

	public float _pNormProgress
	{
		get
		{
			if (_objectives.Length == 1)
			{
				return _objectives[0]._pNormProgress;
			}
			if (_numObjectivesRequiredToComplete == 0)
			{
				return 0f;
			}
			return (float)_pNumObjectivesCompleted / (float)_numObjectivesRequiredToComplete;
		}
	}

	public MinigameObjective _pPrimaryObjective
	{
		get
		{
			return _primaryObjective;
		}
		private set
		{
			if (!(value == _primaryObjective))
			{
				_primaryObjective = value;
				if (this._onPrimaryObjectiveChanged != null)
				{
					this._onPrimaryObjectiveChanged(this);
				}
				_pShouldShowDirectorArrow = (bool)_primaryObjective && (_focusOnClosestWhenObjectivesRemainingLessThan >= Mathf.Max(1, _remainingObjectives.Count) || _primaryObjective._pForceShowDirectorArrow);
			}
		}
	}

	public bool _pShouldShowDirectorArrow
	{
		get
		{
			return _shouldShowDirectorArrow;
		}
		private set
		{
			_shouldShowDirectorArrow = value;
			if (this._onShouldShowDirectorArrowChanged != null)
			{
				this._onShouldShowDirectorArrowChanged(this, _shouldShowDirectorArrow);
			}
		}
	}

	public event Action<Minigame> _onProgress;

	public event Action<Minigame> _onCompleted;

	public event Action<Minigame> _onFailed;

	public event Action<Minigame> _onPrimaryObjectiveChanged;

	public event Action<MinigameObjective> _onObjectiveComplete;

	public event Action<Minigame, bool> _onShouldShowDirectorArrowChanged;

	private void Awake()
	{
		_pStage = EStage.Waiting;
		bool[] array = new bool[_objectives.Length];
		for (int num = _objectives.Length - 1; num >= 0; num--)
		{
			if (!array[num])
			{
				array[num] = true;
				SetObjectiveEnabled(_objectives[num], false);
				if (_disableControllersWhileInactive)
				{
					VehicleController component = _objectives[num].GetComponent<VehicleController>();
					if (component != null)
					{
						component._enabled = false;
					}
				}
			}
		}
	}

	public void StartMinigame()
	{
		_pStage = EStage.InProgress;
		_remainingObjectives.Clear();
		_remainingObjectives.AddRange(_objectives);
		if (_randomiseOrder)
		{
			_remainingObjectives.Randomise();
			if (_remainingObjectives[0] == _prevCompletedObjective)
			{
				int index = UnityEngine.Random.Range(1, _remainingObjectives.Count);
				_remainingObjectives[0] = _remainingObjectives[index];
				_remainingObjectives[index] = _prevCompletedObjective;
			}
			if (_randomiseOrder_KeepFirst)
			{
				int num = _remainingObjectives.IndexOf(_objectives[0]);
				if (num != 0)
				{
					_remainingObjectives[num] = _remainingObjectives[0];
					_remainingObjectives[0] = _objectives[0];
				}
			}
		}
		for (int num2 = _remainingObjectives.Count - 1; num2 >= 0; num2--)
		{
			SetObjectiveEnabled(_remainingObjectives[num2], num2 == 0 || !_sequential);
			if (_disableControllersWhileInactive)
			{
				VehicleController component = _remainingObjectives[num2].GetComponent<VehicleController>();
				if (component != null)
				{
					component._enabled = true;
				}
			}
		}
	}

	private void RestartMinigame()
	{
		for (int num = _objectives.Length - 1; num >= 0; num--)
		{
			_objectives[num].Reset(true);
		}
		StartMinigame();
	}

	public void UpdateMinigame()
	{
		_pPrimaryObjective = GetClosestObjective();
	}

	public void StopMinigame()
	{
		SetAllRemainingObjectivesEnabled(false);
		if ((bool)_pOverridingObjective)
		{
			SetObjectiveEnabled(_pOverridingObjective, false);
		}
		_pPrimaryObjective = null;
	}

	private void Complete()
	{
		if (_pStage == EStage.InProgress)
		{
			_pStage = EStage.Complete;
			_pHasBeenCompleted = true;
			if (this._onCompleted != null)
			{
				this._onCompleted(this);
			}
		}
	}

	public void Fail()
	{
		if (_pStage == EStage.InProgress)
		{
			_pStage = EStage.Failed;
			StopMinigame();
			if (this._onFailed != null)
			{
				this._onFailed(this);
			}
		}
	}

	private void StoreObjectiveStates()
	{
		_tempObjectiveEnabledStates.Clear();
		for (int num = _objectives.Length - 1; num >= 0; num--)
		{
			if (!(_objectives[num] == null))
			{
				_tempObjectiveEnabledStates.Add(_objectives[num], _objectives[num]._pEnabled);
			}
		}
	}

	private void RestoreObjectiveStates()
	{
		foreach (KeyValuePair<MinigameObjective, bool> tempObjectiveEnabledState in _tempObjectiveEnabledStates)
		{
			SetObjectiveEnabled(tempObjectiveEnabledState.Key, tempObjectiveEnabledState.Value);
		}
	}

	private void SetAllRemainingObjectivesEnabled(bool enabled)
	{
		for (int num = _remainingObjectives.Count - 1; num >= 0; num--)
		{
			SetObjectiveEnabled(_remainingObjectives[num], enabled);
		}
	}

	public void SetObjectiveEnabled(MinigameObjective objective, bool enabled)
	{
		if (!(objective == null))
		{
			objective._onProgress -= OnObjectiveProgress;
			objective._onComplete -= OnObjectiveComplete;
			objective._onFail -= OnObjectiveFail;
			if (enabled)
			{
				objective._onProgress += OnObjectiveProgress;
				objective._onComplete += OnObjectiveComplete;
				objective._onFail += OnObjectiveFail;
				objective.Reset();
			}
			objective._pEnabled = enabled;
		}
	}

	public MinigameObjective GetClosestObjective(Predicate<MinigameObjective> match = null)
	{
		if ((bool)_pOverridingObjective)
		{
			return (match != null && !match(_pOverridingObjective)) ? null : _pOverridingObjective;
		}
		MinigameObjective result = null;
		float num = float.PositiveInfinity;
		for (int num2 = _remainingObjectives.Count - 1; num2 >= 0; num2--)
		{
			if (_remainingObjectives[num2]._pEnabled && !_remainingObjectives[num2]._pCompleted && (match == null || match(_remainingObjectives[num2])))
			{
				float num3 = Vector3.SqrMagnitude(_remainingObjectives[num2].transform.position - VehicleController_Player._pInstance.transform.position);
				if (num3 < num)
				{
					num = num3;
					result = _remainingObjectives[num2];
				}
			}
		}
		return result;
	}

	private void OnObjectiveProgress(MinigameObjective objective, float progress)
	{
		if (_objectives.Length == 1 && this._onProgress != null)
		{
			this._onProgress(this);
		}
	}

	private void OnObjectiveComplete(MinigameObjective objective)
	{
		_prevCompletedObjective = objective;
		SetObjectiveEnabled(objective, false);
		if (!objective._subObjective)
		{
			_pNumObjectivesCompleted++;
		}
		_remainingObjectives.Remove(objective);
		if (this._onObjectiveComplete != null)
		{
			this._onObjectiveComplete(objective);
		}
		if (this._onProgress != null)
		{
			this._onProgress(this);
		}
		if (_pNumObjectivesCompleted == _numObjectivesRequiredToComplete)
		{
			Complete();
			return;
		}
		if (objective == _pOverridingObjective)
		{
			CancelOverridingObjective();
		}
		if ((bool)objective._subObjective)
		{
			_pOverridingObjective = objective._subObjective;
			StoreObjectiveStates();
			SetAllRemainingObjectivesEnabled(false);
			SetObjectiveEnabled(_pOverridingObjective, true);
		}
		else if (_remainingObjectives.Count == 0)
		{
			RestartMinigame();
		}
		else if (_sequential)
		{
			SetObjectiveEnabled(_remainingObjectives[0], true);
		}
	}

	private void OnObjectiveFail(MinigameObjective objective)
	{
		SetObjectiveEnabled(objective, false);
		Fail();
	}

	public void CancelOverridingObjective()
	{
		if ((bool)_pOverridingObjective)
		{
			SetObjectiveEnabled(_pOverridingObjective, false);
			_pOverridingObjective = null;
			RestoreObjectiveStates();
		}
	}
}
