using UnityEngine;

public class SimpleDistanceCam : SimpleTargetCamBase
{
	public float _heightOffset;

	public float _distanceXZ;

	protected override void OnUpdate(float dt)
	{
		if (_targetObject != null)
		{
			Vector3 vector = base.transform.position - _targetObject.transform.position;
			vector.Set(vector.x, 0f, vector.z);
			Vector3 normalized = vector.normalized;
			base.WantCameraPosition = _targetObject.transform.position + _heightOffset * Vector3.up + _distanceXZ * normalized;
		}
		base.OnUpdate(dt);
	}
}
