using UnityEngine;
using UnitySampleAssets.Utility;

namespace UnitySampleAssets.Characters.FirstPerson
{
	public class HeadBob : MonoBehaviour
	{
		public Camera _camera;

		public CurveControlledBob motionBob = new CurveControlledBob();

		public LerpControlledBob jumpAndLandingBob = new LerpControlledBob();

		public RigidbodyFirstPersonController rigidbodyFirstPersonController;

		public float StrideInterval;

		[Range(0f, 1f)]
		public float RunningStrideLengthen;

		private CameraRefocus cameraRefocus;

		private bool previouslyGrounded;

		private Vector3 originalCameraPosition;

		private void Start()
		{
			motionBob.Setup(_camera, StrideInterval);
			originalCameraPosition = _camera.transform.localPosition;
			cameraRefocus = new CameraRefocus(_camera, base.transform.root.transform, _camera.transform.localPosition);
		}

		private void Update()
		{
			cameraRefocus.GetFocusPoint();
			Vector3 localPosition;
			if (rigidbodyFirstPersonController.Velocity.magnitude > 0f && rigidbodyFirstPersonController.Grounded)
			{
				_camera.transform.localPosition = motionBob.DoHeadBob(rigidbodyFirstPersonController.Velocity.magnitude * ((!rigidbodyFirstPersonController.Running) ? 1f : RunningStrideLengthen));
				localPosition = _camera.transform.localPosition;
				localPosition.y = _camera.transform.localPosition.y - jumpAndLandingBob.Offset();
			}
			else
			{
				localPosition = _camera.transform.localPosition;
				localPosition.y = originalCameraPosition.y - jumpAndLandingBob.Offset();
			}
			_camera.transform.localPosition = localPosition;
			if (!previouslyGrounded && rigidbodyFirstPersonController.Grounded)
			{
				StartCoroutine(jumpAndLandingBob.DoBobCycle());
			}
			previouslyGrounded = rigidbodyFirstPersonController.Grounded;
			cameraRefocus.SetFocusPoint();
		}
	}
}
