using UnityEngine;

[RequireComponent(typeof(VehicleController_WaypointFollower))]
public class OtherVehicleDetector : MonoBehaviour
{
	private const float COOLDOWN_TIME = 0.5f;

	private const float RAY_LENGTH = 8f;

	private const float RAY_WIDTH = 1f;

	private const int MAX_FRAMES_BETWEEN_RAYCASTS = 5;

	private Vector3 _rayPointOrigin_Offset = new Vector3(0f, 0f, 0f);

	private Vector3 _rayPointLeftClose_Offset = new Vector3(-1f, 0f, 1f);

	private Vector3 _rayPointRightClose_Offset = new Vector3(1f, 0f, 1f);

	private Vector3 _rayPointLeftFar_Offset;

	private Vector3 _rayPointRightFar_Offset;

	private Vector3 _rayPointOrigin_World;

	private Vector3 _rayPointLeftClose_World;

	private Vector3 _rayPointRightClose_World;

	private Vector3 _rayPointLeftFar_World;

	private Vector3 _rayPointRightFar_World;

	private static int _numActiveScripts;

	private VehicleController_WaypointFollower _wf;

	private int _layerMask;

	private int _updateCounter;

	private float _lastDetectTime = float.NegativeInfinity;

	private Vector3 _raysOrigin;

	public Transform _pLastVehicleHit { get; private set; }

	public bool _pShouldBreak
	{
		get
		{
			return Time.time < _lastDetectTime + 0.5f;
		}
	}

	private void Start()
	{
		_wf = GetComponent<VehicleController_WaypointFollower>();
		_layerMask = LayerMask.GetMask("Vehicle");
		_updateCounter = _numActiveScripts % 5;
		_numActiveScripts++;
		Vehicle_Car component = GetComponent<Vehicle_Car>();
		if ((bool)component)
		{
			_raysOrigin = Vector3.up * (0f - component._pCentreOffsetFromBottom + 1.2f);
		}
		else
		{
			_raysOrigin = Vector3.zero;
		}
	}

	private void FixedUpdate()
	{
		if (--_updateCounter <= 0)
		{
			_updateCounter = _numActiveScripts % 5;
			PerformRaycasts();
		}
	}

	private void PerformRaycasts()
	{
		float num = 1f;
		_rayPointLeftFar_Offset = new Vector3(-1f, 0f, 1f + 8f * num);
		_rayPointRightFar_Offset = new Vector3(1f, 0f, 1f + 8f * num);
		_rayPointLeftClose_World = TransformPoint(_rayPointLeftClose_Offset);
		_rayPointRightClose_World = TransformPoint(_rayPointRightClose_Offset);
		_rayPointLeftFar_World = TransformPoint(_rayPointLeftFar_Offset);
		_rayPointRightFar_World = TransformPoint(_rayPointRightFar_Offset);
		RaycastOtherVehicles(_rayPointLeftClose_World, _rayPointLeftFar_World);
		RaycastOtherVehicles(_rayPointRightClose_World, _rayPointRightFar_World);
	}

	private void RaycastOtherVehicles(Vector3 from, Vector3 to)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(from, to - from, out hitInfo, (from - to).magnitude, _layerMask) && !hitInfo.transform.IsChildOf(base.transform))
		{
			OtherVehicleDetector component = hitInfo.transform.GetComponent<OtherVehicleDetector>();
			if (!component || !component._pShouldBreak || !(component._pLastVehicleHit == base.transform))
			{
				_lastDetectTime = Time.time;
				_pLastVehicleHit = hitInfo.transform;
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = ((!_pShouldBreak) ? Color.grey : Color.white);
		Gizmos.DrawLine(_rayPointLeftClose_World, _rayPointLeftFar_World);
		Gizmos.DrawLine(_rayPointRightClose_World, _rayPointRightFar_World);
	}

	private Vector3 TransformPoint(Vector3 point)
	{
		return base.transform.position + Quaternion.AngleAxis(base.transform.eulerAngles.y + _wf._pThisToTargetAngleOffset, Vector3.up) * (_raysOrigin + point);
	}

	private void OnDestroy()
	{
		_numActiveScripts--;
	}
}
