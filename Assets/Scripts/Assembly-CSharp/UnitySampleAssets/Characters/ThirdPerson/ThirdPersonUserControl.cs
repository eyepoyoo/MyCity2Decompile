using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(ThirdPersonCharacter))]
	public class ThirdPersonUserControl : MonoBehaviour
	{
		public bool walkByDefault;

		public bool lookInCameraDirection = true;

		private Vector3 lookPos;

		private ThirdPersonCharacter character;

		private Transform cam;

		private Vector3 camForward;

		private Vector3 move;

		private bool jump;

		private void Start()
		{
			if (Camera.main != null)
			{
				cam = Camera.main.transform;
			}
			else
			{
				Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
			}
			character = GetComponent<ThirdPersonCharacter>();
		}

		private void Update()
		{
			if (!jump)
			{
				jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
		}

		private void FixedUpdate()
		{
			bool flag = false;
			float axis = CrossPlatformInputManager.GetAxis("Horizontal");
			float axis2 = CrossPlatformInputManager.GetAxis("Vertical");
			flag = Input.GetKey(KeyCode.C);
			if (cam != null)
			{
				camForward = Vector3.Scale(cam.forward, new Vector3(1f, 0f, 1f)).normalized;
				move = axis2 * camForward + axis * cam.right;
			}
			else
			{
				move = axis2 * Vector3.forward + axis * Vector3.right;
			}
			if (move.magnitude > 1f)
			{
				move.Normalize();
			}
			lookPos = ((!lookInCameraDirection || !(cam != null)) ? (base.transform.position + base.transform.forward * 100f) : (base.transform.position + cam.forward * 100f));
			character.Move(move, flag, jump, lookPos);
			jump = false;
		}
	}
}
