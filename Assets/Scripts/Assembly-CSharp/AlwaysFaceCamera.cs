using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
	public Camera _camera;

	private void Start()
	{
		if (_camera == null)
		{
			_camera = Object.FindObjectOfType(typeof(Camera)) as Camera;
		}
	}

	private void LateUpdate()
	{
		if (_camera != null)
		{
			Vector3 vector = -_camera.transform.forward;
			Vector3 rhs = Vector3.Cross(Vector3.up, vector);
			Vector3 normalized = Vector3.Cross(vector, rhs).normalized;
			base.transform.rotation = Quaternion.LookRotation(vector, normalized);
		}
	}
}
