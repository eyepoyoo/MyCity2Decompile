using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class VehicleController_Player : VehicleController
{
	public bool _doAutoAccelerate = true;

	public float _steerSensitivity = 1.25f;

	private Vehicle_Car _car;

	private float _steerValue;

	public static VehicleController_Player _pInstance { get; set; }

	public override bool _pWasSpecialPressed
	{
		get
		{
			return _enabled && CrossPlatformInputManager.GetButtonDown("Special");
		}
	}

	public override bool _pIsSpecialDown
	{
		get
		{
			return _enabled && CrossPlatformInputManager.GetButton("Special");
		}
	}

	public override bool _pWasSpecialReleased
	{
		get
		{
			return _enabled && CrossPlatformInputManager.GetButtonUp("Special");
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_pInstance = this;
		_car = GetComponent<Vehicle_Car>();
	}

	private void FixedUpdate()
	{
		float num = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		if (_pShouldZeroStearing)
		{
			num = 0f;
		}
		num = Mathf.Clamp(num + _steerValue, -1f, 1f);
		float num2 = CrossPlatformInputManager.GetAxisRaw("Vertical");
		if (_pShouldZeroAcceleration)
		{
			num2 = 0f;
		}
		else if (_doAutoAccelerate && num2 >= 0f)
		{
			num2 = 1f;
		}
		base._pVehicle.Move(num, num2);
	}

	public void SetSteerValue(float steerValue)
	{
		_steerValue = Mathf.Clamp(steerValue * _steerSensitivity, -1f, 1f);
	}

	public static bool IsPlayer(Component component)
	{
		return (bool)component && (bool)_pInstance && component.GetComponentInParent<VehicleController_Player>() == _pInstance;
	}

	private void OnDestroy()
	{
		if (_pInstance == this)
		{
			_pInstance = null;
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		FollowTransform followTransform = Object.FindObjectOfType<FollowTransform>();
		if (followTransform != null && followTransform.gameObject.name == "SingleDrawCallSkybox")
		{
			followTransform.followTarget = base.transform;
		}
	}

	public override void SetSpecialPressed(bool pressed)
	{
		if (pressed)
		{
			if (!CrossPlatformInputManager.GetButton("Special"))
			{
				CrossPlatformInputManager.SetButtonDown("Special");
			}
		}
		else if (CrossPlatformInputManager.GetButton("Special"))
		{
			CrossPlatformInputManager.SetButtonUp("Special");
		}
	}
}
