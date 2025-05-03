using System;
using UnityEngine;

public abstract class SpecialAbility : MonoBehaviour
{
	public float _fullDischargeDuration = 1f;

	public float _fullChargeDuration = 1f;

	[NonSerialized]
	public bool _forceIsInUse;

	[NonSerialized]
	public bool _isAutomated;

	private bool _isInUse;

	public float _pNormCharge { get; private set; }

	public Vehicle _pVehicle { get; private set; }

	public bool _pIsInUse
	{
		get
		{
			return _isInUse;
		}
		protected set
		{
			if (value != _isInUse)
			{
				_isInUse = value;
				if (_isInUse)
				{
					OnStarted();
				}
				else
				{
					OnEnded();
				}
			}
		}
	}

	public bool _pStartCondition
	{
		get
		{
			if (_isAutomated)
			{
				return _forceIsInUse;
			}
			return _forceIsInUse || (_pNormCharge == 1f && (bool)_pVehicle && _pVehicle._pController._pWasSpecialPressed);
		}
	}

	public bool _pEndCondition
	{
		get
		{
			if (_isAutomated)
			{
				return !_forceIsInUse;
			}
			return !_forceIsInUse && (_pNormCharge == 0f || ((bool)_pVehicle && !_pVehicle._pController._pIsSpecialDown));
		}
	}

	protected virtual void Awake()
	{
		_pNormCharge = 1f;
	}

	protected virtual void Update()
	{
		if (_pStartCondition)
		{
			_pIsInUse = true;
		}
		if (_pIsInUse)
		{
			if (_pNormCharge > 0f)
			{
				if (float.IsInfinity(_fullDischargeDuration))
				{
					_pNormCharge = 1f;
				}
				else
				{
					_pNormCharge = Math.Max(0f, _pNormCharge - 1f / _fullDischargeDuration * Time.deltaTime);
				}
			}
			if (_pEndCondition)
			{
				_pIsInUse = false;
			}
		}
		else
		{
			if (!(_pNormCharge < 1f) || (bool)_pVehicle._pShouldCoast)
			{
				return;
			}
			if (_fullChargeDuration == 0f)
			{
				_pNormCharge = 1f;
				return;
			}
			_pNormCharge = Math.Min(1f, _pNormCharge + 1f / _fullChargeDuration * Time.deltaTime);
			if (_pNormCharge == 1f)
			{
				VehicleController_Player._pInstance.SetSpecialPressed(false);
			}
		}
	}

	public virtual void AssignToVehicle(Vehicle vehicle)
	{
		if ((bool)_pVehicle && _pVehicle._specialAbility == this)
		{
			_pVehicle._specialAbility = null;
		}
		if ((bool)vehicle)
		{
			vehicle._specialAbility = this;
		}
		_pVehicle = vehicle;
	}

	protected virtual void OnStarted()
	{
	}

	protected virtual void OnEnded()
	{
	}
}
