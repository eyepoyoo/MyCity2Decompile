using UnityEngine;

public class AffectorArea_Current : AffectorArea
{
	private const float CAM_SPEED_SCALE = 1.2f;

	private static readonly object _speedScaleKey = new object();

	private static int _numPlayerCollisions = 0;

	public TextureScroller _textureScroller;

	public float _force;

	protected override void ApplyForces(Rigidbody rb)
	{
		rb.AddForce(base.transform.forward * _textureScroller.scrollSpeed.x * _force);
	}

	protected override void OnBodyEnter(Rigidbody rb)
	{
		if (VehicleController_Player.IsPlayer(rb) && ++_numPlayerCollisions == 1)
		{
			MinigameController._pInstance._pCamera.AddSpeedScaleRequest(_speedScaleKey, 1.2f);
		}
	}

	protected override void OnBodyExit(Rigidbody rb)
	{
		if (VehicleController_Player.IsPlayer(rb) && --_numPlayerCollisions == 0)
		{
			MinigameController._pInstance._pCamera.RemoveSpeedScaleRequest(_speedScaleKey);
		}
	}

	private void OnDestroy()
	{
		_numPlayerCollisions = 0;
	}
}
