using UnityEngine;

[RequireComponent(typeof(Pickupable))]
public class MinigameObjective_PickUp : MinigameObjective
{
	private Pickupable _pickupable;

	private bool _doCheckForPlayerEnterExit;

	private float _playerRangeSqrd;

	private bool _isPlayerInRange;

	public override float _pNormProgress
	{
		get
		{
			return _pickupable._pPickedUp ? 1 : 0;
		}
	}

	private void Start()
	{
		_pickupable = GetComponent<Pickupable>();
		_pickupable._onPickUpStart += OnPickUpStart;
		_pickupable._onPickedUp += OnPickedUp;
		_pickupable._pMinigameObjective = this;
		if (_markerPoint != null)
		{
			SpecialAbility_Lifter componentInChildren = VehicleController_Player._pInstance.GetComponentInChildren<SpecialAbility_Lifter>();
			if (!componentInChildren)
			{
				_markerPoint._pDoShow = false;
				_markerPoint = null;
				return;
			}
			_markerPoint._pRadius = componentInChildren._grabRange;
			_markerPoint._follow = base.transform;
			_playerRangeSqrd = componentInChildren._grabRange * componentInChildren._grabRange;
			_doCheckForPlayerEnterExit = true;
		}
	}

	private void Update()
	{
		if (!_doCheckForPlayerEnterExit || base._pCompleted)
		{
			return;
		}
		bool flag = MathHelper.DistXZSqrd(base.transform.position, VehicleController_Player._pInstance.transform.position) <= _playerRangeSqrd && IsInRangeY(base.transform.position, VehicleController_Player._pInstance.transform.position);
		if (!_isPlayerInRange)
		{
			if (flag)
			{
				_isPlayerInRange = true;
				OnPlayerEntered();
			}
		}
		else if (!flag)
		{
			_isPlayerInRange = false;
			OnPlayerExited();
		}
	}

	private void OnPlayerEntered()
	{
		if (!MinigameController._pInstance._pDoPromptSpecial && VehicleController_Player._pInstance._pVehicle._specialAbility is SpecialAbility_Lifter && !((SpecialAbility_Lifter)VehicleController_Player._pInstance._pVehicle._specialAbility)._pCurrentObject)
		{
			MinigameController._pInstance._pDoPromptSpecial.AddContraryStateRequest(this);
		}
	}

	private void OnPlayerExited()
	{
		if (VehicleController_Player._pInstance._pVehicle._specialAbility is SpecialAbility_Lifter)
		{
			MinigameController._pInstance._pDoPromptSpecial.RemoveContraryStateRequest(this);
		}
	}

	protected override void OnObjectiveEnabled()
	{
		base.OnObjectiveEnabled();
		if ((bool)_markerPoint)
		{
			_markerPoint._follow = base.transform;
		}
	}

	private void OnPickUpStart(Pickupable pickupable)
	{
		MinigameController._pInstance._pDoPromptSpecial.RemoveContraryStateRequest(this);
		if (_markerPoint != null)
		{
			_markerPoint._follow = null;
			_markerPoint._pDoShow = false;
		}
	}

	private void OnPickedUp(Pickupable pickupable)
	{
		Progress();
		Complete();
	}

	public override void Reset(bool toInitialState = false)
	{
		base.Reset(toInitialState);
		MinigameController._pInstance._pDoPromptSpecial.RemoveContraryStateRequest(this);
		_pickupable.enabled = true;
	}

	public static bool IsInRangeY(Vector3 posPickup, Vector3 posToTest)
	{
		return posToTest.y > posPickup.y - 1f && posToTest.y < posPickup.y + 2f;
	}
}
