using UnityEngine;

public class SimpleOffsetCam : SimpleTargetCamBase
{
	public Vector3 _offset;

	protected override void OnUpdate(float dt)
	{
		if (_targetObject != null)
		{
			base.WantCameraPosition = _targetObject.transform.TransformPoint(_offset);
		}
		base.OnUpdate(dt);
	}
}
