using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviour
{
	private const float GROUNDED_GRAVITY = -1f;

	private const float SQR_MIN_MOVE_SPEED = 1.1f;

	public float _moveSpeed = 6f;

	public Transform _movementRelativeTransform;

	public Vector3 _relativeForward = Vector3.forward;

	public Vector3 _relativeRight = Vector3.right;

	public float _maxGravityVel = 10f;

	public CharacterController _characterController;

	public SimpleInput_Keyboard _input;

	protected float _acceleration = 10f;

	protected float _damping = 1E-05f;

	protected float _jumpVel = 8f;

	protected float _rotTime = 0.05f;

	private float _rotVel;

	private Vector3 _velocity = Vector3.zero;

	private Vector3 _prevPosLocal;

	public float _pLocalHorizontalSpeed
	{
		get
		{
			float num = base.transform.localPosition.x - _prevPosLocal.x;
			float num2 = base.transform.localPosition.z - _prevPosLocal.z;
			return Mathf.Sqrt(num * num + num2 * num2) / Time.deltaTime;
		}
	}

	public Vector3 _pVelocity
	{
		get
		{
			return _velocity;
		}
	}

	private void Awake()
	{
		_characterController = GetComponent<CharacterController>();
		_prevPosLocal = base.transform.localPosition;
	}

	protected virtual void Update()
	{
		if (_input != null)
		{
			if (_input.isPressingDirection)
			{
				Vector3 directionPressed = _input.directionPressed;
				Move(_relativeForward * directionPressed.z + _relativeRight * directionPressed.x, _moveSpeed * (float)((!Input.GetKey(KeyCode.LeftShift)) ? 1 : 4));
			}
			if (_characterController.isGrounded && _input.GetControlDown(SimpleInputControl.Jump))
			{
				Jump();
			}
			if (_input.GetControlDown(SimpleInputControl.Attack))
			{
				HandleInput(SimpleInputControl.Attack);
			}
		}
		base.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(base.transform.eulerAngles.y, Mathf.Atan2(_velocity.x, _velocity.z) * 57.29578f, ref _rotVel, _rotTime);
		if (!_characterController.isGrounded || _velocity.sqrMagnitude > 1.1f)
		{
			_characterController.Move(_velocity * Time.deltaTime);
		}
		_velocity.x *= Mathf.Pow(_damping, Time.deltaTime);
		_velocity.z *= Mathf.Pow(_damping, Time.deltaTime);
		if (!_characterController.isGrounded)
		{
			if (_velocity.y > 0f - _maxGravityVel)
			{
				_velocity += Physics.gravity * Time.deltaTime;
			}
		}
		else
		{
			_velocity.y = -1f;
		}
	}

	protected virtual void HandleInput(SimpleInputControl control)
	{
	}

	private void LateUpdate()
	{
		_prevPosLocal = base.transform.localPosition;
	}

	private void Move(Vector3 direction, float speed)
	{
		if ((bool)_movementRelativeTransform)
		{
			direction = _movementRelativeTransform.TransformDirection(direction);
		}
		direction.y = 0f;
		direction.Normalize();
		float num = Mathf.Max(0f, speed - Vector3.Dot(_velocity, direction));
		_velocity += direction * num * Mathf.Clamp01(_acceleration * Time.deltaTime);
	}

	protected virtual void Jump()
	{
		_velocity += Vector3.up * _jumpVel;
	}
}
