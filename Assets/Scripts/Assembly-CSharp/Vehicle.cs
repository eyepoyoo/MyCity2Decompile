using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Connectors))]
public abstract class Vehicle : MonoBehaviour
{
	public enum EType
	{
		Land = 0,
		Air = 1,
		Boat = 2
	}

	public enum ESize
	{
		Small = 0,
		Medium = 1,
		Large = 2
	}

	private const float MID_AIR_STABILISE_ACCELERATION = 0.3f;

	private const float MID_AIR_STABILISE_DAMPING = 0.2f;

	private const float BOUNCE_OFF_WALL_MAX_ANGLE_Y = 45f;

	private const float BOUNCE_OFF_WALL_MAX_ANGLE_X = 45f;

	private const float BOUNCE_OFF_WALL_MIN_INTERVAL = 0.2f;

	private const float CHECK_PICKUP_COLLISIONS_INTERVAL = 0.05f;

	private const float NOT_MOVING_SPEED = 0.1f;

	private const float NOT_MOVING_SPEED_SQ = 0.010000001f;

	private const float STUCK_TIME = 1f;

	public ESize _size = ESize.Medium;

	[NonSerialized]
	public SpecialAbility _specialAbility;

	[NonSerialized]
	public bool _isInSpotlight;

	private readonly Dictionary<VehiclePart.EObstacleToNegate, VehiclePart> _negatedObstacles = new Dictionary<VehiclePart.EObstacleToNegate, VehiclePart>();

	private Bounds[] _colliderBounds;

	private Bounds[] _colliderBounds_Pickups;

	private float _lastBounceAwayFromImpactTime = float.NegativeInfinity;

	private int _layerMaskGeom;

	private bool _doCheckPickupCollisions;

	private bool _collisionBoundsCalculated;

	private readonly RepeatedAction _repeatedAction_CheckPickupCollisions = new RepeatedAction(0.05f);

	private bool _isStuck;

	private float _stuckTimer;

	private bool _isOfficiallyStuck;

	public bool _stabiliseInMidAir;

	[NonSerialized]
	private float _brakeTimer;

	private VehicleController _controller;

	private IPickerUpper _pickerUpper;

	private static float _lastNudgeSoundTime = 0f;

	private bool _isCoasting;

	private float _coastStartTime = float.PositiveInfinity;

	public static readonly List<Vehicle> _all = new List<Vehicle>();

	public bool _pIsPlayer { get; private set; }

	public Rigidbody _pRigidbody { get; private set; }

	public Connectors _pConnectors { get; private set; }

	public BooleanStateRequestsSimple _pShouldCoast { get; private set; }

	public float _pInitDrag { get; private set; }

	public bool _pIsBraking
	{
		get
		{
			return _brakeTimer > 0f || _isInSpotlight;
		}
	}

	public float _pDistOffGround
	{
		get
		{
			return base.transform.position.y - _pCentreOffsetFromBottom;
		}
	}

	public VehicleController _pController
	{
		get
		{
			return _controller ?? (_pController = GetComponent<VehicleController>());
		}
		private set
		{
			_controller = value;
			_pIsPlayer = VehicleController_Player.IsPlayer(this);
			_doCheckPickupCollisions = _pIsPlayer;
			if (_doCheckPickupCollisions)
			{
				CalculateColliderBounds();
			}
		}
	}

	public abstract bool _pIsGrounded { get; }

	public abstract float _pBounceOffWallAngVelMulti { get; }

	public virtual float _pCentreOffsetFromBottom
	{
		get
		{
			return 0f;
		}
	}

	public abstract float _pSpeedFactor { get; }

	public IPickerUpper _pPickerUpper
	{
		get
		{
			return _pickerUpper ?? (_pickerUpper = GetComponentInChildren<IPickerUpper>());
		}
	}

	public float _pTimeCoasting
	{
		get
		{
			return Mathf.Max(0f, Time.time - _coastStartTime);
		}
	}

	public float _pCoastingBrake
	{
		get
		{
			return _isCoasting ? Mathf.InverseLerp(5f, 10f, _pTimeCoasting) : 0f;
		}
	}

	public event Action<Vehicle, Collision> _onHitVehicle;

	public event Action _onStuck;

	public event Action _onUnstuck;

	protected virtual void Awake()
	{
		if (!_all.Contains(this))
		{
			_all.Add(this);
		}
		_layerMaskGeom = LayerMask.GetMask("Geometry", "GeometryInvisible");
		_pRigidbody = GetComponentInParent<Rigidbody>();
		if ((bool)_pRigidbody)
		{
			_pInitDrag = _pRigidbody.drag;
		}
		_pConnectors = GetComponent<Connectors>();
		Reset();
		Array.ForEach(GetComponents<VehiclePart>(), delegate(VehiclePart vp)
		{
			OnPartAttached(vp);
		});
		_pShouldCoast = new BooleanStateRequestsSimple(false, OnCoastingRequestsChanged);
	}

	private void OnDestroy()
	{
		if (_all.Contains(this))
		{
			_all.Remove(this);
		}
	}

	protected virtual void Update()
	{
		if (_doCheckPickupCollisions && _repeatedAction_CheckPickupCollisions.Update())
		{
			CheckPickupCollisions();
		}
		UpdateStuck(Time.deltaTime);
		if (_brakeTimer > 0f)
		{
			_brakeTimer = Mathf.Max(0f, _brakeTimer - Time.deltaTime);
		}
	}

	protected virtual void FixedUpdate()
	{
		if (_stabiliseInMidAir && !_pIsGrounded)
		{
			float num = MathHelper.Wrap(base.transform.eulerAngles.x, -180f, 180f);
			float num2 = MathHelper.Wrap(base.transform.eulerAngles.z, -180f, 180f);
			float num3 = Vector3.Dot(_pRigidbody.angularVelocity, base.transform.right) * 57.29578f;
			float num4 = Vector3.Dot(_pRigidbody.angularVelocity, base.transform.forward) * 57.29578f;
			_pRigidbody.AddTorque(base.transform.right * ((0f - num) * 0.3f - num3 * 0.2f) + base.transform.forward * ((0f - num2) * 0.3f - num4 * 0.2f), ForceMode.Acceleration);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		BounceAwayFromWallCheck(collision);
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		BounceAwayFromWallCheck(collision);
		Vehicle component = collision.transform.GetComponent<Vehicle>();
		if ((bool)component)
		{
			OnHitVehicle(component, collision);
		}
		if (!SoundFacade._pInstance || !(Time.time - _lastNudgeSoundTime > 0.25f))
		{
			return;
		}
		_lastNudgeSoundTime = Time.time;
		if (MinigameManager._pInstance._pCurrentVehicleTypeForMinigame == MinigameManager.EVEHICLE_TYPE.WATER)
		{
			if ((bool)component)
			{
				VehiclePart component2 = component.GetComponent<VehiclePart>();
				if (component2 != null && component2.uniqueID == VehiclePart.EUNIQUE_ID.BODY_SHARK)
				{
					SoundFacade._pInstance.PlayOneShotSFX("ImpactBoatxShark", base.transform.position, 0f);
				}
				else
				{
					SoundFacade._pInstance.PlayOneShotSFX("ImpactBoatxBoat", base.transform.position, 0f);
				}
			}
			else
			{
				SoundFacade._pInstance.PlayOneShotSFX("ImpactBoatxBoat", base.transform.position, 0f);
			}
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("InvincibleCube"))
		{
			SoundFacade._pInstance.PlayOneShotSFX("ImpactBoxes", base.transform.position, 0f);
		}
		else
		{
			SoundFacade._pInstance.PlayOneShotSFX("ImpactGeneric", base.transform.position, 0f);
		}
	}

	private void CalculateColliderBounds()
	{
		Vector3 position = base.transform.position;
		Quaternion rotation = base.transform.rotation;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		_colliderBounds = new Bounds[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			_colliderBounds[i] = componentsInChildren[i].bounds;
		}
		_colliderBounds_Pickups = new Bounds[_colliderBounds.Length];
		_colliderBounds.CopyTo(_colliderBounds_Pickups, 0);
		for (int num = _colliderBounds_Pickups.Length - 1; num >= 0; num--)
		{
			_colliderBounds_Pickups[num].Expand(0.5f);
		}
		base.transform.position = position;
		base.transform.rotation = rotation;
		_collisionBoundsCalculated = true;
	}

	public virtual void Move(float steerInput, float accelBrakeInput)
	{
	}

	private void CheckPickupCollisions()
	{
		if (!_collisionBoundsCalculated)
		{
			Debug.Log("Collision bounds not calculated");
			return;
		}
		int numItemsReturned = 0;
		Pickup[] pickupsAtPos = Pickup.GetPickupsAtPos(base.transform.position, out numItemsReturned);
		for (int i = 0; i < numItemsReturned; i++)
		{
			Pickup pickup = pickupsAtPos[i];
			if (pickup.enabled && pickup.gameObject.activeInHierarchy)
			{
				if (!pickup._pIsGravitating && TestPointCollision_Pickups(pickup.transform.position))
				{
					pickup.Collect(pickup.transform.position);
				}
				else if (MathHelper.DistSqrd(base.transform.position, pickup.transform.position) < 10f)
				{
					pickup.Gravitate(base.transform);
				}
			}
		}
	}

	public bool TestPointCollision(Vector3 worldPoint)
	{
		if (!_collisionBoundsCalculated)
		{
			Debug.Log("Collision bounds not calculated");
			return false;
		}
		Vector3 point = base.transform.InverseTransformPoint(worldPoint);
		for (int num = _colliderBounds.Length - 1; num >= 0; num--)
		{
			if (_colliderBounds[num].Contains(point))
			{
				return true;
			}
		}
		return false;
	}

	public bool TestPointCollision_Pickups(Vector3 worldPoint)
	{
		if (!_collisionBoundsCalculated)
		{
			Debug.Log("Collision bounds not calculated");
			return false;
		}
		Vector3 point = base.transform.InverseTransformPoint(worldPoint);
		for (int num = _colliderBounds_Pickups.Length - 1; num >= 0; num--)
		{
			if (_colliderBounds_Pickups[num].Contains(point))
			{
				return true;
			}
		}
		return false;
	}

	private void OnHitVehicle(Vehicle vehicle, Collision collision)
	{
		if (this._onHitVehicle != null)
		{
			this._onHitVehicle(vehicle, collision);
		}
	}

	private void BounceAwayFromWallCheck(Collision collision)
	{
		if (!(Time.time < _lastBounceAwayFromImpactTime + 0.2f) && ((1 << collision.gameObject.layer) & _layerMaskGeom) != 0 && !(45f < Vector3.Angle(collision.contacts[0].normal, -base.transform.forward)) && !(45f < Vector3.Angle(base.transform.up, Vector3.up)))
		{
			BounceAwayFromImpact(collision.contacts[0].normal);
		}
	}

	public virtual void BounceAwayFromImpact(Vector3 normal, bool doSpinAway = true)
	{
		normal.y = 0f;
		normal.Normalize();
		_pRigidbody.velocity = normal * 15f + Vector3.up * 3f;
		if (doSpinAway)
		{
			float f = 0f - MathHelper.GetAngle(base.transform.forward, -normal, Vector3.up);
			_pRigidbody.angularVelocity = Vector3.up * (90f - Mathf.Abs(f)) * Mathf.Sign(f) * _pBounceOffWallAngVelMulti;
		}
		_lastBounceAwayFromImpactTime = Time.time;
	}

	private void UpdateStuck(float dt)
	{
		if (!(_pRigidbody == null))
		{
			Vector3 velocity = _pRigidbody.velocity;
			velocity.y = 0f;
			float sqrMagnitude = velocity.sqrMagnitude;
			_isStuck = sqrMagnitude <= 0.010000001f && _pIsGrounded && !_pIsBraking;
			if (_isStuck)
			{
				_stuckTimer += dt;
			}
			else
			{
				_stuckTimer = 0f;
			}
			CheckStuck();
		}
	}

	private void CheckStuck()
	{
		bool isOfficiallyStuck = _isOfficiallyStuck;
		bool flag = _pController != null && _pController._enabled;
		_isOfficiallyStuck = flag && _isStuck && _stuckTimer > 1f;
		if (_isOfficiallyStuck && !isOfficiallyStuck)
		{
			if (this._onStuck != null)
			{
				this._onStuck();
			}
		}
		else if (isOfficiallyStuck && !_isOfficiallyStuck && this._onUnstuck != null)
		{
			this._onUnstuck();
		}
	}

	public virtual void Reset()
	{
		if (_pRigidbody != null)
		{
			_pRigidbody.velocity = Vector3.zero;
			_pRigidbody.angularVelocity = Vector3.zero;
		}
		TerrainEffect[] componentsInChildren = GetComponentsInChildren<TerrainEffect>();
		foreach (TerrainEffect terrainEffect in componentsInChildren)
		{
			terrainEffect.RemoveInstant();
		}
	}

	public virtual void OnPartAttached(VehiclePart part)
	{
		VehiclePart.EObstacleToNegate[] negatedObstacles = part._negatedObstacles;
		foreach (VehiclePart.EObstacleToNegate key in negatedObstacles)
		{
			if (!_negatedObstacles.ContainsKey(key))
			{
				_negatedObstacles.Add(key, part);
			}
		}
	}

	public static Vehicle TryGetVehicleComponentOfType(GameObject obj, MinigameManager.EVEHICLE_TYPE vehicleType, bool returnOtherTypeIfNotFound = true)
	{
		Vehicle[] components = obj.GetComponents<Vehicle>();
		Vehicle_Air vehicle_Air = null;
		Vehicle_Boat vehicle_Boat = null;
		Vehicle_Car vehicle_Car = null;
		int num = components.Length;
		for (int i = 0; i < num; i++)
		{
			if (components[i] is Vehicle_Air)
			{
				vehicle_Air = components[i] as Vehicle_Air;
			}
			else if (components[i] is Vehicle_Boat)
			{
				vehicle_Boat = components[i] as Vehicle_Boat;
			}
			else if (components[i] is Vehicle_Car)
			{
				vehicle_Car = components[i] as Vehicle_Car;
			}
			else
			{
				Debug.LogError("Unknown vehicle type on object: " + obj.name);
			}
		}
		switch (vehicleType)
		{
		case MinigameManager.EVEHICLE_TYPE.AIR:
			if (vehicle_Air != null)
			{
				return vehicle_Air;
			}
			break;
		case MinigameManager.EVEHICLE_TYPE.LAND:
			if (vehicle_Car != null)
			{
				return vehicle_Car;
			}
			break;
		case MinigameManager.EVEHICLE_TYPE.WATER:
			if (vehicle_Boat != null)
			{
				return vehicle_Boat;
			}
			break;
		}
		if (returnOtherTypeIfNotFound)
		{
			if (vehicle_Air != null)
			{
				return vehicle_Air;
			}
			if (vehicle_Boat != null)
			{
				return vehicle_Boat;
			}
			if (vehicle_Car != null)
			{
				return vehicle_Car;
			}
		}
		return null;
	}

	public void Brake(float duration = 0.1f)
	{
		_brakeTimer = duration;
	}

	public void UnBrake()
	{
		_brakeTimer = 0f;
	}

	public bool NegatesObstacle(VehiclePart.EObstacleToNegate obstacle)
	{
		return _negatedObstacles.ContainsKey(obstacle);
	}

	public bool NegatesObstacle(params VehiclePart.EObstacleToNegate[] obstacles)
	{
		for (int num = obstacles.Length - 1; num >= 0; num--)
		{
			if (_negatedObstacles.ContainsKey(obstacles[num]))
			{
				return true;
			}
		}
		return false;
	}

	public VehiclePart GetVehiclePartThatNegatesObstacle(VehiclePart.EObstacleToNegate obstacle)
	{
		return _negatedObstacles[obstacle];
	}

	public static Type GetVehicleType(EType _type)
	{
		switch (_type)
		{
		case EType.Land:
			return typeof(Vehicle_Car);
		case EType.Air:
			return typeof(Vehicle_Air);
		case EType.Boat:
			return typeof(Vehicle_Boat);
		default:
			return null;
		}
	}

	public virtual void OnCoastingRequestsChanged(bool newState)
	{
		_coastStartTime = ((!newState) ? float.PositiveInfinity : Time.time);
		_isCoasting = newState;
	}

	public bool DropPickupable(bool directBackToObjective = false)
	{
		if (_pPickerUpper == null || !_pPickerUpper._pCurrentObject)
		{
			return false;
		}
		if ((bool)_pPickerUpper._pCurrentObject._pMinigameObjective)
		{
			_pPickerUpper._pCurrentObject._pMinigameObjective.CancelSubObjectiveAndReenable();
		}
		if (directBackToObjective && (bool)_pPickerUpper._pCurrentObject.GetComponent<MinigameObjective>())
		{
			_pPickerUpper._pCurrentObject.GetComponent<MinigameObjective>()._showDirectorArrowUntilComplete = true;
		}
		_pPickerUpper.Drop();
		return true;
	}

	public static Vehicle GetClosestToPlayer(Predicate<Vehicle> match = null)
	{
		Vehicle result = null;
		float num = float.PositiveInfinity;
		for (int num2 = _all.Count - 1; num2 >= 0; num2--)
		{
			if (!(_all[num2] == VehicleController_Player._pInstance._pVehicle) && (match == null || match(_all[num2])))
			{
				float num3 = Vector3.SqrMagnitude(_all[num2].transform.position - VehicleController_Player._pInstance.transform.position);
				if (num3 < num)
				{
					num = num3;
					result = _all[num2];
				}
			}
		}
		return result;
	}
}
