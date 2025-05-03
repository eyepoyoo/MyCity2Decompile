using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerTrigger : MonoBehaviour
{
	public enum EType
	{
		None = 0,
		Hidden = 1,
		MudThrough = 2,
		BridgeUnder = 3,
		BridgeOver = 4,
		Ramp = 5,
		BoxPileThrough = 6,
		MudOver = 7,
		BoxPileOver = 8,
		TurbulenceThrough = 9,
		LavaThrough = 10
	}

	public string _mudThroughSoundName = "EnterPuddle";

	public EType _type;

	public bool _rumbleCamera;

	public bool _onlyExitIfPassedAcross;

	private int _currNumPlayerColliders;

	private bool _discovered;

	private Vector3 _playerEnterPos;

	private Vector3? _colliderCentre;

	private Vector3 _pColliderCentre
	{
		get
		{
			if (!_colliderCentre.HasValue)
			{
				_colliderCentre = GetComponent<Collider>().bounds.center;
			}
			return _colliderCentre.Value;
		}
	}

	public static event Action<PlayerTrigger> _onDiscover;

	public static event Action<PlayerTrigger> _onEnter;

	public static event Action<PlayerTrigger> _onExit;

	protected virtual void OnTriggerEnter(Collider other)
	{
		OnEnter(other.transform);
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		OnExit(other.transform);
	}

	private void OnEnter(Transform trans)
	{
		if (!VehicleController_Player.IsPlayer(trans))
		{
			return;
		}
		_currNumPlayerColliders++;
		if (_currNumPlayerColliders == 1)
		{
			if (PlayerTrigger._onEnter != null)
			{
				PlayerTrigger._onEnter(this);
			}
			if (!_discovered && PlayerTrigger._onDiscover != null)
			{
				PlayerTrigger._onDiscover(this);
			}
			if (_type == EType.MudThrough && (bool)SoundFacade._pInstance)
			{
				SoundFacade._pInstance.PlayOneShotSFX(_mudThroughSoundName, trans.position, 0f);
			}
			_discovered = true;
			_playerEnterPos = trans.position;
		}
	}

	private void OnExit(Transform trans)
	{
		if (VehicleController_Player.IsPlayer(trans))
		{
			_currNumPlayerColliders--;
			if (_currNumPlayerColliders == 0 && PlayerTrigger._onExit != null && (!_onlyExitIfPassedAcross || Vector3.Dot(_playerEnterPos - _pColliderCentre, trans.position - _pColliderCentre) < 0f))
			{
				PlayerTrigger._onExit(this);
			}
		}
	}
}
