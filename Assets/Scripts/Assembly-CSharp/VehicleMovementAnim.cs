using UnityEngine;

[RequireComponent(typeof(Vehicle))]
public class VehicleMovementAnim : MonoBehaviour
{
	public Animator _animator;

	public float _speedMulti = 1f;

	private Vehicle _vehicle;

	private float _groundedSpeedMulti = 1f;

	private void Awake()
	{
		_vehicle = GetComponent<Vehicle>();
	}

	private void Update()
	{
		if (_vehicle._pIsGrounded)
		{
			_groundedSpeedMulti = 1f;
		}
		else
		{
			_groundedSpeedMulti = Mathf.MoveTowards(_groundedSpeedMulti, 0f, 3f * Time.deltaTime);
		}
		_animator.speed = _vehicle._pRigidbody.velocity.MagnitudeXZ() * _speedMulti * _groundedSpeedMulti;
	}
}
