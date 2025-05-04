using UnityEngine;

public class DuplicateCameraFOV : MonoBehaviour
{
	public Camera _thisCamera;

	public Camera _sourceCamera;

	private void LateUpdate()
	{
		_thisCamera.fieldOfView = _sourceCamera.fieldOfView;
	}
}
