using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : MonoBehaviour
{
	public delegate Vector3 DTweenOffset(float t);

	public static readonly List<Pickupable> _all = new List<Pickupable>();

	public bool _canBeDroppedByPlayer = true;

	public float _drag = 2f;

	public Transform _anchor;

	public float _dropPickupCooldown;

	private Collider[] _colliders;

	private float _initMass;

	private RigidbodyInterpolation _initInterpolation;

	private FixedJoint _joint;

	private bool _isTweening;

	private float _tweenStartTime = float.PositiveInfinity;

	private Vector3 _tweenStartPos;

	private Quaternion _tweenStartRot;

	private Transform _tweenTarget;

	private float _tweenStartAngleY;

	private Action<Pickupable> _onTweenComplete;

	private float _tweenDuration;

	private bool _tweenToOurRotation;

	private DTweenOffset _tweenOffset;

	private float _initVehicleDrag;

	private Vehicle _vehicleToApplyDragTo;

	private Easing.EaseType _tweenEaseType;

	public bool _pPickedUp { get; private set; }

	public Rigidbody _pRigidbody { get; private set; }

	public MinigameObjective _pMinigameObjective { get; set; }

	public bool _pIsTweening
	{
		get
		{
			return _isTweening;
		}
		private set
		{
			if (value != _isTweening)
			{
				_isTweening = value;
				_pRigidbody.isKinematic = value;
				for (int num = _colliders.Length - 1; num >= 0; num--)
				{
					_colliders[num].enabled = !value;
				}
			}
		}
	}

	public event Action<Pickupable> _onPickUpStart;

	public event Action<Pickupable> _onPickedUp;

	public event Action<Pickupable> _onDropped;

	private void Awake()
	{
		_pRigidbody = GetComponent<Rigidbody>();
		_colliders = GetComponentsInChildren<Collider>();
		_initMass = _pRigidbody.mass;
		_initInterpolation = _pRigidbody.interpolation;
		_all.Add(this);
	}

	private void Update()
	{
		if (_pIsTweening)
		{
			Vector3 b = _tweenTarget.position + ((!(_anchor != null)) ? Vector3.zero : (base.transform.position - _anchor.position));
			Quaternion b2 = ((!_tweenToOurRotation) ? _tweenStartRot : (_tweenTarget.rotation * ((!_anchor) ? Quaternion.identity : Quaternion.Inverse(_anchor.localRotation))));
			float num = Easing.Ease(_tweenEaseType, Mathf.Clamp01((Time.time - _tweenStartTime) / _tweenDuration), 1f, 0f, 1f);
			base.transform.position = Vector3.Lerp(_tweenStartPos, b, num);
			base.transform.rotation = Quaternion.Lerp(_tweenStartRot, b2, num);
			if (num == 1f)
			{
				_pIsTweening = false;
				_pRigidbody.isKinematic = false;
				base.transform.position = _tweenTarget.position + ((!(_anchor != null)) ? Vector3.zero : (base.transform.position - _anchor.position));
				if (_onTweenComplete != null)
				{
					_onTweenComplete(this);
				}
			}
		}
		if ((bool)_vehicleToApplyDragTo)
		{
			_vehicleToApplyDragTo._pRigidbody.drag = ((!_vehicleToApplyDragTo._pIsGrounded) ? _initVehicleDrag : _drag);
		}
	}

	public void PickUp(Transform connectedAnchor, Rigidbody connectedRigidbody, Vehicle vehicle = null, Easing.EaseType easeType = Easing.EaseType.EaseInOut, Action<Pickupable> onPickedUp = null)
	{
		if (this._onPickUpStart != null)
		{
			this._onPickUpStart(this);
		}
		TweenTo(connectedAnchor, delegate
		{
			AttachTo(connectedAnchor, connectedRigidbody, vehicle);
			if (this._onPickedUp != null)
			{
				this._onPickedUp(this);
			}
			if (onPickedUp != null)
			{
				onPickedUp(this);
			}
		}, easeType, 0.35f, 0f);
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayOneShotSFX("LifterAttach", 0f);
		}
	}

	private void AttachTo(Transform connectedAnchor, Rigidbody connectedRigidbody, Vehicle vehicle = null)
	{
		_joint = base.gameObject.AddComponent<FixedJoint>();
		_joint.connectedBody = connectedRigidbody;
		_joint.connectedAnchor = connectedAnchor.localPosition;
		_pRigidbody.mass = 0.001f;
		_pRigidbody.interpolation = connectedRigidbody.interpolation;
		_pPickedUp = true;
		if ((bool)vehicle)
		{
			_initVehicleDrag = vehicle._pRigidbody.drag;
			_vehicleToApplyDragTo = vehicle;
		}
	}

	public void Drop()
	{
		UnityEngine.Object.Destroy(_joint);
		_pRigidbody.mass = _initMass;
		_pRigidbody.interpolation = _initInterpolation;
		_pPickedUp = false;
		_pIsTweening = false;
		if ((bool)_vehicleToApplyDragTo)
		{
			_vehicleToApplyDragTo._pRigidbody.drag = _initVehicleDrag;
			_vehicleToApplyDragTo = null;
		}
		if (this._onDropped != null)
		{
			this._onDropped(this);
		}
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayOneShotSFX("LifterDetach", 0f);
		}
	}

	public void TweenTo(Transform target, Action<Pickupable> onComplete = null, Easing.EaseType easeType = Easing.EaseType.EaseInOut, float duration = 0.35f, float delay = 0f, bool tweenToOurRotation = true, DTweenOffset tweenOffset = null)
	{
		_tweenStartTime = Time.time + delay;
		_tweenStartPos = base.transform.position;
		_tweenStartRot = base.transform.rotation;
		_tweenStartAngleY = base.transform.eulerAngles.y;
		_tweenTarget = target;
		_onTweenComplete = onComplete;
		_tweenEaseType = easeType;
		_tweenDuration = duration;
		_tweenToOurRotation = tweenToOurRotation;
		_tweenOffset = tweenOffset;
		_pIsTweening = true;
		_pRigidbody.isKinematic = true;
	}

	private void OnDestroy()
	{
		_all.Remove(this);
	}
}
