using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
	[Serializable]
	public class VehicleToSpawn
	{
		public VehicleConstructor _vehicle;

		public int _numToSpawn = 1;

		public WaypointPath _path;

		public bool _keepNearPath = true;

		public float _lookAheadDist = 10f;

		public float _wobbleAmount;

		public float _wobbleFreq = 0.8f;

		public bool _avoidOtherVehicles = true;

		public bool _enableSounds = true;

		public Vehicle SpawnVehicle()
		{
			if (_vehicle == null)
			{
				Debug.LogError("Cannot spawn vehicle, no _vehicle reference");
				return null;
			}
			Vehicle vehicle = _vehicle.ConstructVehicle();
			VehicleController_WaypointFollower vehicleController_WaypointFollower = vehicle.gameObject.AddComponent<VehicleController_WaypointFollower>();
			vehicleController_WaypointFollower._path = _path;
			vehicleController_WaypointFollower._lookAheadDist = _lookAheadDist;
			vehicleController_WaypointFollower._keepNearPath = _keepNearPath;
			vehicleController_WaypointFollower._wobbleAmount = _wobbleAmount;
			vehicleController_WaypointFollower._wobbleFreq = _wobbleFreq;
			if (_avoidOtherVehicles)
			{
				vehicle.gameObject.AddComponent<OtherVehicleDetector>();
			}
			if (!_enableSounds)
			{
				UnityEngine.Object.Destroy(vehicle.GetComponent<EngineAudio>());
			}
			if (vehicle is Vehicle_Car)
			{
				if (_keepNearPath)
				{
					vehicle.gameObject.AddComponent<CarStabiliser>();
				}
				vehicle.gameObject.AddComponent<BumpAwayFromVehicleCollision>();
			}
			else if (!(vehicle is Vehicle_Boat) && vehicle is Vehicle_Air)
			{
				vehicle.GetComponentInChildren<AirVehicleEngineProperties>()._acceleration = 10f;
				vehicleController_WaypointFollower._laneOffset = 4f;
			}
			return vehicle;
		}
	}

	private struct SpawnPoint
	{
		public Waypoint _wp1;

		public Waypoint _wp2;

		public float _t;

		private Vector3 _laneOffset;

		public Vector3 _pPos { get; private set; }

		public SpawnPoint(Waypoint wp1, Waypoint wp2, float t)
		{
			_wp1 = wp1;
			_wp2 = wp2;
			_t = t;
			if (_wp1._includeLaneOffset || _wp2._includeLaneOffset)
			{
				Vector3 vector = new Vector3(_wp2.transform.position.z - _wp1.transform.position.z, 0f, _wp1.transform.position.x - _wp2.transform.position.x);
				_laneOffset = vector.normalized * 2.75f;
			}
			else
			{
				_laneOffset = Vector3.zero;
			}
			_pPos = Vector3.Lerp(_wp1.transform.position + ((!_wp1._includeLaneOffset) ? Vector3.zero : _laneOffset), _wp2.transform.position + ((!_wp2._includeLaneOffset) ? Vector3.zero : _laneOffset), _t);
		}

		public Quaternion GetRot()
		{
			return Quaternion.LookRotation(_wp2.transform.position + ((!_wp2._includeLaneOffset) ? Vector3.zero : _laneOffset) - _wp1.transform.position + ((!_wp1._includeLaneOffset) ? Vector3.zero : _laneOffset));
		}
	}

	private delegate bool DIsValidWaypoint(Waypoint wp);

	private delegate bool DIsValidSpawnPoint(Vector3 pos);

	private const int NUM_INTERPOLATED_POINTS = 5;

	private const float NUM_INTERPOLATED_POINTS_INV = 0.25f;

	private const float MIN_DIST_BETWEEN = 20f;

	private const float MIN_DIST_BETWEEN_SQRD = 400f;

	private const float AVOID_SIREN_RADIUS = 50f;

	private const float AVOID_SIREN_RADIUS_SQRD = 2500f;

	private const float NPC_AVOIDED_SIREN_DIST = 10f;

	private const float NPC_AVOIDED_SIREN_DIST_SQRD = 100f;

	private const bool LOG_WARNINGS = true;

	private static readonly bool[] _isValidSpawnPointCache = new bool[9];

	private static readonly float _isValidSpawnPointCache_LengthInv = 1 / (_isValidSpawnPointCache.Length - 1);

	public bool _initialiseOnAwake = true;

	public bool _dontInitialiseIfLowFidelity;

	public float _radius;

	public float _repositionNpcsInterval = 1f;

	public bool _spawnWaypointsMustBeInCamFrustrum = true;

	public bool _performRaycasts = true;

	public bool _mustSpawnInFrontOfCam = true;

	public VehicleToSpawn[] _vehiclesToSpawn;

	private Camera _camera;

	private VehicleController_WaypointFollower[] _npcs;

	private RepeatedAction _repeatedAction_RepositionNpcs;

	private float _radiusSqrd;

	private int _occluderRaycastLayerMask;

	private readonly List<Vector3> _currentCamRaycastsPos = new List<Vector3>();

	private readonly List<bool> _currentCamRaycastsValue = new List<bool>();

	private bool _hasInited;

	private SpecialAbility_Siren _siren;

	private bool _wasSirenInUse;

	private bool _wereThereVehiclesInAvoidedSirenDist;

	private readonly List<WaypointPath> _pathOptions_Paths = new List<WaypointPath>();

	private readonly List<List<SpawnPoint>> _pathOptions_Options = new List<List<SpawnPoint>>();

	private List<Waypoint> _tempListWaypoint = new List<Waypoint>();

	private List<Waypoint[]> _tempListWaypoints = new List<Waypoint[]>();

	private List<WaypointPath.Node> _tempListNode = new List<WaypointPath.Node>();

	private List<VehicleController_WaypointFollower> _tempListNpcs = new List<VehicleController_WaypointFollower>();

	private readonly CoroutineManager _coroutines = new CoroutineManager();

	public event Action _onNpcAvoidedSiren;

	private void Awake()
	{
		if (_initialiseOnAwake && (!_dontInitialiseIfLowFidelity || FidelityFacade.Instance.fidelity != Fidelity.Low))
		{
			Initialise();
		}
		_repeatedAction_RepositionNpcs = new RepeatedAction(_repositionNpcsInterval);
	}

	public void Initialise()
	{
		_radiusSqrd = _radius * _radius;
		_occluderRaycastLayerMask = LayerMask.GetMask("Geometry");
		_camera = Camera.main;
		if ((bool)VehicleController_Player._pInstance)
		{
			_siren = VehicleController_Player._pInstance._pVehicle._specialAbility as SpecialAbility_Siren;
		}
		_coroutines.AddCoroutine(InitialiseVehicles(), delegate
		{
			_coroutines.AddCoroutine(PositionNpcs(_npcs, (_repositionNpcsInterval != float.PositiveInfinity) ? new DIsValidWaypoint(IsValidWaypoint_Initial) : ((DIsValidWaypoint)((Waypoint w) => true)), (_repositionNpcsInterval != float.PositiveInfinity) ? new DIsValidSpawnPoint(IsValidSpawnPoint_Initial) : ((DIsValidSpawnPoint)((Vector3 w) => true))), delegate
			{
				_hasInited = true;
			});
		});
	}

	private IEnumerator InitialiseVehicles()
	{
		List<VehicleController_WaypointFollower> npcsTemp = new List<VehicleController_WaypointFollower>();
		npcsTemp.AddRange(GetComponentsInChildren<VehicleController_WaypointFollower>());
		for (int i = _vehiclesToSpawn.Length - 1; i >= 0; i--)
		{
			for (int j = _vehiclesToSpawn[i]._numToSpawn - 1; j >= 0; j--)
			{
				Vehicle vehicle = _vehiclesToSpawn[i].SpawnVehicle();
				vehicle.gameObject.SetActive(false);
				vehicle.transform.parent = base.transform;
				vehicle.transform.localPosition = Vector3.zero;
				npcsTemp.Add((VehicleController_WaypointFollower)vehicle._pController);
				yield return null;
			}
		}
		_npcs = npcsTemp.ToArray();
	}

	private void Update()
	{
		_coroutines.Update();
		if (_hasInited)
		{
			if (_repeatedAction_RepositionNpcs.Update())
			{
				_coroutines.AddCoroutine(PositionNpcs(GetNpcsToMove(), IsValidWaypoint, IsValidSpawnPoint), null, true);
			}
			UpdateSiren();
		}
	}

	private IEnumerator PositionNpcs(IList<VehicleController_WaypointFollower> toMove, DIsValidWaypoint isValidWaypoint, DIsValidSpawnPoint isValidSpawnPoint)
	{
		_currentCamRaycastsPos.Clear();
		_currentCamRaycastsValue.Clear();
		_pathOptions_Paths.Clear();
		_pathOptions_Options.Clear();
		for (int i = toMove.Count - 1; i >= 0; i--)
		{
			VehicleController_WaypointFollower npc = toMove[i];
			int pathOptionsIndex = _pathOptions_Paths.IndexOf(npc._path);
			List<SpawnPoint> options = ((pathOptionsIndex == -1) ? null : _pathOptions_Options[pathOptionsIndex]);
			if (pathOptionsIndex == -1)
			{
				_coroutines.AddCoroutine(GetValidSpawnPoints(npc._path, isValidWaypoint, isValidSpawnPoint, delegate(List<SpawnPoint> spawnPoints)
				{
					options = spawnPoints;
					_pathOptions_Paths.Add(npc._path);
					_pathOptions_Options.Add(options);
				}));
				yield return null;
			}
			if (options.Count == 0)
			{
				Debug.LogWarning("PositionNpcs: ...no spawn points for path " + npc._path);
			}
			else
			{
				SpawnPoint chosenOption = options.GetRandom(true);
				PositionNpc(npc, chosenOption);
				yield return null;
			}
		}
	}

	private IEnumerator GetValidSpawnPoints(WaypointPath path, DIsValidWaypoint isValidWaypoint, DIsValidSpawnPoint isValidSpawnPoint, Action<List<SpawnPoint>> onRetrieved)
	{
		if (!path)
		{
			Debug.LogError("GetValidSpawnPoints: No path provided...");
			onRetrieved(new List<SpawnPoint>(0));
			yield break;
		}
		List<Waypoint> waypointsPrev = _tempListWaypoint;
		List<Waypoint[]> waypointsNext = _tempListWaypoints;
		_coroutines.AddCoroutine(GetValidWaypoints(path, waypointsPrev, waypointsNext, isValidWaypoint));
		yield return null;
		if (waypointsPrev.Count == 0)
		{
			Debug.LogWarning("GetValidSpawnPoints: No valid waypoints...");
			onRetrieved(new List<SpawnPoint>(0));
			yield break;
		}
		List<Waypoint> waypointsPrev2 = new List<Waypoint>();
		List<Waypoint> waypointsNext2 = new List<Waypoint>();
		List<float> optionsT = new List<float>();
		int counter = 0;
		for (int p = waypointsPrev.Count - 1; p >= 0; p--)
		{
			Vector3 posPrev = waypointsPrev[p].transform.position + Vector3.up;
			for (int n = waypointsNext[p].Length - 1; n >= 0; n--)
			{
				Vector3 posNext = waypointsNext[p][n].transform.position + Vector3.up;
				for (float t = 0f; t < 4f; t += 1f)
				{
					float tc = 0.25f * (t + 0.5f);
					Vector3 posInterpC = Vector3.Lerp(posPrev, posNext, tc);
					if (!IsTooCloseToPlayer(posInterpC))
					{
						float tl = 0.25f * t;
						float tr = 0.25f * (t + 1f);
						Vector3 posInterpL = Vector3.Lerp(posPrev, posNext, tl);
						Vector3 posInterpR = Vector3.Lerp(posPrev, posNext, tr);
						if (isValidSpawnPoint(posInterpC) && isValidSpawnPoint(posInterpL) && isValidSpawnPoint(posInterpR) && !IsTooCloseToOtherNpcs(posInterpC))
						{
							waypointsPrev2.Add(waypointsPrev[p]);
							waypointsNext2.Add(waypointsNext[p][n]);
							optionsT.Add(tc);
						}
					}
				}
				int num;
				counter = (num = counter + 1);
				if (num == 5)
				{
					counter = 0;
					yield return null;
				}
			}
		}
		if (waypointsPrev2.Count == 0)
		{
			Debug.LogWarning("GetValidSpawnPoints: No valid points between valid waypoints...");
			onRetrieved(new List<SpawnPoint>(0));
			yield break;
		}
		List<SpawnPoint> spawnPoints = new List<SpawnPoint>(waypointsPrev2.Count);
		for (int i = waypointsPrev2.Count - 1; i >= 0; i--)
		{
			spawnPoints.Add(new SpawnPoint(waypointsPrev2[i], waypointsNext2[i], optionsT[i]));
		}
		onRetrieved(spawnPoints);
	}

	private IEnumerator GetValidWaypoints(WaypointPath path, List<Waypoint> optionsA, List<Waypoint[]> optionsB, DIsValidWaypoint isValidWaypoint)
	{
		optionsA.Clear();
		optionsB.Clear();
		_tempListNode.Clear();
		for (int i = path._nodes.Length - 1; i >= 0; i--)
		{
			WaypointPath.Node node = path._nodes[i];
			if (!isValidWaypoint(node._waypoint))
			{
				_tempListNode.Add(node);
			}
			else
			{
				optionsA.Add(node._waypoint);
				optionsB.Add(new Waypoint[0]);
				for (int j = node._nextWaypoints.Length - 1; j >= 0; j--)
				{
					int indexOfNextInA = optionsA.IndexOf(node._nextWaypoints[j]);
					if (indexOfNextInA == -1 || !optionsB[indexOfNextInA].Contains(node._waypoint))
					{
						optionsB[optionsB.Count - 1] = optionsB[optionsB.Count - 1].Add(node._nextWaypoints[j]);
					}
				}
			}
		}
		yield return null;
		for (int i2 = _tempListNode.Count - 1; i2 >= 0; i2--)
		{
			WaypointPath.Node node2 = _tempListNode[i2];
			bool hasAddedPrev = false;
			for (int j2 = node2._nextWaypoints.Length - 1; j2 >= 0; j2--)
			{
				if (isValidWaypoint(node2._nextWaypoints[j2]))
				{
					if (!hasAddedPrev)
					{
						optionsA.Add(node2._waypoint);
						optionsB.Add(new Waypoint[1] { node2._nextWaypoints[j2] });
						hasAddedPrev = true;
					}
					else
					{
						optionsB[optionsB.Count - 1] = optionsB[optionsB.Count - 1].Add(node2._nextWaypoints[j2]);
					}
				}
			}
		}
	}

	private void PositionNpc(VehicleController_WaypointFollower vehicle, SpawnPoint sp)
	{
		vehicle.gameObject.SetActive(true);
		vehicle.transform.position = sp._pPos + Vector3.up * vehicle._pVehicle._pCentreOffsetFromBottom;
		if (vehicle._pVehicle is Vehicle_Boat)
		{
			vehicle.transform.position -= Vector3.up * ((Vehicle_Boat)vehicle._pVehicle)._pBuoyancy._targetSubmersion;
		}
		vehicle.transform.rotation = sp.GetRot();
		vehicle.GetComponent<Rigidbody>().velocity = Vector3.zero;
		vehicle.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		vehicle.SetTargetWaypoint(sp._wp2, sp._wp1, true);
		VertexColourChanger componentInChildren = vehicle.GetComponentInChildren<VertexColourChanger>();
		if ((bool)componentInChildren)
		{
			componentInChildren.DoColourChange();
		}
		vehicle._isMovedToSide = false;
		vehicle._pVehicle.Reset();
	}

	private IList<VehicleController_WaypointFollower> GetNpcsToMove()
	{
		_tempListNpcs.Clear();
		for (int num = _npcs.Length - 1; num >= 0; num--)
		{
			if (ShouldMoveNpc(_npcs[num].transform.position))
			{
				_tempListNpcs.Add(_npcs[num]);
			}
		}
		return _tempListNpcs;
	}

	private bool ShouldMoveNpc(Vector3 pos)
	{
		return !IsInFrontOfCam(pos) || (!IsInRange(pos) && !CanCamSee(pos));
	}

	private bool IsValidWaypoint_Initial(Waypoint waypoint)
	{
		float num = MathHelper.DistXZSqrd(waypoint.transform.position, _camera.transform.position);
		return num < _radiusSqrd && num > 2500f;
	}

	private bool IsValidWaypoint(Waypoint waypoint)
	{
		if (!IsInRange(waypoint.transform.position))
		{
			return false;
		}
		if (!_mustSpawnInFrontOfCam)
		{
			return true;
		}
		if (_spawnWaypointsMustBeInCamFrustrum)
		{
			return _camera.IsPointInFrustrum(waypoint.transform.position, 0f);
		}
		return IsInFrontOfCam(waypoint.transform.position);
	}

	private bool IsValidSpawnPoint_Initial(Vector3 pos)
	{
		return IsInRange(pos);
	}

	private bool IsValidSpawnPoint(Vector3 pos)
	{
		return (!_mustSpawnInFrontOfCam || IsInFrontOfCam(pos)) && !CanCamSee(pos);
	}

	private bool IsInRange(Vector3 pos)
	{
		return MathHelper.DistXZSqrd(_camera.transform.position, pos) < _radiusSqrd;
	}

	private bool IsTooCloseToPlayer(Vector3 pos)
	{
		return (bool)VehicleController_Player._pInstance && MathHelper.DistXZSqrd(VehicleController_Player._pInstance.transform.position, pos) < 400f;
	}

	private bool IsTooCloseToOtherNpcs(Vector3 pos)
	{
		for (int num = _npcs.Length - 1; num >= 0; num--)
		{
			if (MathHelper.DistXZSqrd(_npcs[num].transform.position, pos) < 400f)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsInFrontOfCam(Vector3 pos)
	{
		return Vector3.Dot(_camera.transform.forward, pos - _camera.transform.position) > 0f;
	}

	private bool CanCamSee(Vector3 pos)
	{
		return _camera.IsPointInFrustrum(pos, 0f) && !IsCamRaycastObstructed(pos);
	}

	private bool IsCamRaycastObstructed(Vector3 pos)
	{
		if (!_performRaycasts)
		{
			return false;
		}
		for (int num = _currentCamRaycastsPos.Count - 1; num >= 0; num--)
		{
			if (pos.x == _currentCamRaycastsPos[num].x && pos.y == _currentCamRaycastsPos[num].y && pos.z == _currentCamRaycastsPos[num].z)
			{
				return _currentCamRaycastsValue[num];
			}
		}
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(_camera.transform.position, pos - _camera.transform.position, out hitInfo, (pos - _camera.transform.position).magnitude, _occluderRaycastLayerMask);
		_currentCamRaycastsPos.Add(pos);
		_currentCamRaycastsValue.Add(flag && hitInfo.transform.tag != "IgnoreRaycast");
		return flag;
	}

	private void UpdateSiren()
	{
		if (!_siren)
		{
			return;
		}
		bool flag = _siren._pIsInUse && AreThereNpcsInRangeOfPlayerSqrd(100f);
		if (_siren._pIsInUse)
		{
			UpdateNpcsAvoidingSiren(true);
			if (!_wereThereVehiclesInAvoidedSirenDist && flag)
			{
				OnNpcAvoidedSiren();
			}
		}
		else if (_wasSirenInUse)
		{
			UpdateNpcsAvoidingSiren(false);
		}
		_wasSirenInUse = _siren._pIsInUse;
		_wereThereVehiclesInAvoidedSirenDist = flag;
	}

	private void UpdateNpcsAvoidingSiren(bool isSirenActive)
	{
		for (int num = _npcs.Length - 1; num >= 0; num--)
		{
			Vector3 vector = _npcs[num].transform.position - VehicleController_Player._pInstance.transform.position;
			_npcs[num]._isMovedToSide = isSirenActive && Vector3.Dot(VehicleController_Player._pInstance.transform.forward, vector) > 0f && vector.SqrMagnitudeXZ() < 2500f;
		}
	}

	private void OnNpcAvoidedSiren()
	{
		if (this._onNpcAvoidedSiren != null)
		{
			this._onNpcAvoidedSiren();
		}
	}

	private bool AreThereNpcsInRangeOfPlayerSqrd(float radiusSqrd)
	{
		if (!VehicleController_Player._pInstance)
		{
			return false;
		}
		for (int num = _npcs.Length - 1; num >= 0; num--)
		{
			if (MathHelper.DistXZSqrd(VehicleController_Player._pInstance.transform.position, _npcs[num].transform.position) < radiusSqrd)
			{
				return true;
			}
		}
		return false;
	}

	private void OnDrawGizmos()
	{
		if ((bool)_camera)
		{
			Gizmos.color = Color.white;
			GizmosPlus.drawCircle(_camera.transform.position, _radius, 100);
		}
	}
}
