using UnityEngine;

public class JointsStabiliser : MonoBehaviour
{
	public float _jointDistThreshold = 0.5f;

	private float _jointDistThresholdSqrd;

	private Joint[] _joints;

	private Rigidbody[] _rigidbodies;

	private void Awake()
	{
		_jointDistThresholdSqrd = _jointDistThreshold * _jointDistThreshold;
		_joints = GetComponentsInChildren<Joint>();
		_rigidbodies = new Rigidbody[_joints.Length];
		for (int i = 0; i < _joints.Length; i++)
		{
			_rigidbodies[i] = _joints[i].GetComponent<Rigidbody>();
		}
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < _joints.Length; i++)
		{
			Joint joint = _joints[i];
			if (MathHelper.DistSqrd(joint.connectedBody.transform.InverseTransformPoint(joint.transform.position), joint.connectedAnchor) > _jointDistThresholdSqrd)
			{
				joint.transform.position = joint.connectedBody.transform.TransformPoint(joint.connectedAnchor);
				_rigidbodies[i].velocity = Vector3.zero;
				_rigidbodies[i].angularVelocity = Vector3.zero;
			}
		}
	}
}
