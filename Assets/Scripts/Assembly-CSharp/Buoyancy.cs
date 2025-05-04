using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{
	public float _waterLevel;

	public float _centreBottomDist;

	public float _targetSubmersion;

	public float _maxSubmersion = float.PositiveInfinity;

	public float _oscillationSpeedLinear = 5f;

	public float _oscillationDampingLinear = 1f;

	public float _oscillationSpeedAngular = 1f;

	public float _oscillationDampingAngular;

	public float _targetRoll;

	public float _targetPitch;

	private Rigidbody _rigidbody;

	public float _pSubmersion { get; private set; }

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		_pSubmersion = _waterLevel - base.transform.position.y + _centreBottomDist;
		if (!(_pSubmersion <= 0f))
		{
			float num = _pSubmersion * _oscillationSpeedLinear - _rigidbody.velocity.y * _oscillationDampingLinear + 20f - _targetSubmersion * _oscillationSpeedLinear;
			_rigidbody.AddForce(Vector3.up * num, ForceMode.Acceleration);
			Vector3 forward = base.transform.forward;
			float num2 = (0f - _oscillationSpeedAngular) * MathHelper.Wrap(base.transform.eulerAngles.z - _targetRoll, -180f, 180f);
			float num3 = (0f - _oscillationDampingAngular) * (Vector3.Dot(forward, _rigidbody.angularVelocity) + num2);
			Vector3 vector = forward * (num2 + num3);
			Vector3 normalized = MathHelper.ClipVector3(base.transform.right, Vector3.up).normalized;
			float num4 = (0f - _oscillationSpeedAngular) * MathHelper.Wrap(base.transform.eulerAngles.x - _targetPitch, -180f, 180f);
			float num5 = (0f - _oscillationDampingAngular) * (Vector3.Dot(normalized, _rigidbody.angularVelocity) + num4);
			Vector3 vector2 = normalized * (num4 + num5);
			_rigidbody.AddTorque(vector + vector2, ForceMode.Acceleration);
			if (base.transform.position.y < _waterLevel + _centreBottomDist - _maxSubmersion)
			{
				base.transform.position = new Vector3(base.transform.position.x, _waterLevel + _centreBottomDist - _maxSubmersion, base.transform.position.z);
				_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
			}
		}
	}
}
