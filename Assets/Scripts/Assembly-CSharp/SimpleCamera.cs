using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
	public Transform _targetPosition;

	public Transform _targetLookAt;

	public Transform _up;

	public Vector3 _positionEaseTime = Vector3.one * 0.01f;

	public float _rotationEaseTime = 0.01f;

	public float _maxLookUpAngle = 90f;

	public float _maxLookDownAngle = 90f;

	public Vector3 _offsetPosition = Vector3.zero;

	public Vector3 _offsetLookAt = Vector3.zero;

	public Vector3 _defaultForward = Vector3.forward;

	private Vector3 _posVel;

	private Vector3 _alignedOffsetFromDesiredPos;

	private Vector3 _thisToDesiredPos;

	private Vector3 _desiredPos;

	private Vector3 _lookAtPos;

	private void LateUpdate()
	{
		if ((bool)_targetPosition)
		{
			Vector3 vector = ((!_up) ? Vector3.up : _up.up);
			Vector3 normalized = (_targetLookAt ? (_targetLookAt.position - _targetPosition.position) : _defaultForward).normalized;
			Vector3 normalized2 = Vector3.Cross(vector, normalized).normalized;
			_desiredPos = _targetPosition.position + normalized2 * _offsetPosition.x + vector * _offsetPosition.y + normalized * _offsetPosition.z;
			_thisToDesiredPos = _desiredPos - base.transform.position;
			_alignedOffsetFromDesiredPos = new Vector3(Vector3.Dot(_thisToDesiredPos, normalized2), Vector3.Dot(_thisToDesiredPos, vector), Vector3.Dot(_thisToDesiredPos, normalized));
			base.transform.position += normalized2 * _alignedOffsetFromDesiredPos.x * (1f - Mathf.Pow(_positionEaseTime.x, Time.deltaTime));
			base.transform.position += vector * _alignedOffsetFromDesiredPos.y * (1f - Mathf.Pow(_positionEaseTime.y, Time.deltaTime));
			base.transform.position += normalized * _alignedOffsetFromDesiredPos.z * (1f - Mathf.Pow(_positionEaseTime.z, Time.deltaTime));
			_lookAtPos = (_targetLookAt ? (_targetLookAt.position + _targetLookAt.TransformDirection(_offsetLookAt)) : (base.transform.position + _defaultForward));
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(_lookAtPos - base.transform.position, vector), 1f - Mathf.Pow(_rotationEaseTime, Time.deltaTime));
			if (!MathHelper.IsInClosedRange(base.transform.eulerAngles.x, 0f - _maxLookUpAngle, _maxLookDownAngle))
			{
				base.transform.eulerAngles = new Vector3(Mathf.Clamp(base.transform.eulerAngles.x, 0f - _maxLookUpAngle, _maxLookDownAngle), base.transform.eulerAngles.y, base.transform.eulerAngles.z);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(_targetPosition.position, _desiredPos);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(base.transform.position, base.transform.position + _thisToDesiredPos);
		}
	}
}
