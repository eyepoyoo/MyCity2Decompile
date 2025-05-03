using System;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Characters.FirstPerson
{
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyFirstPersonController : MonoBehaviour
	{
		[Serializable]
		public class MovementSettings
		{
			public float ForwardSpeed = 8f;

			public float BackwardSpeed = 4f;

			public float StrafeSpeed = 4f;

			public float SprintSpeed = 10f;

			public float JumpForce = 30f;

			public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90f, 1f), new Keyframe(0f, 1f), new Keyframe(90f, 0f));

			[HideInInspector]
			public float CurrentTargetSpeed = 8f;

			private bool running;

			public bool Running
			{
				get
				{
					return running;
				}
			}

			public void UpdateDesiredTargetSpeed()
			{
				if (CrossPlatformInputManager.GetButton("Fire1"))
				{
					CurrentTargetSpeed = SprintSpeed;
					running = true;
				}
				else
				{
					CurrentTargetSpeed = ForwardSpeed;
					running = false;
				}
			}
		}

		[Serializable]
		public class AdvancedSettings
		{
			public float groundCheckDistance = 0.01f;

			public float stickToGroundHelperDistance = 0.5f;

			public float slowDownRate = 20f;

			public bool airControl;
		}

		public Camera _camera;

		public MovementSettings movementSettings = new MovementSettings();

		public MouseLook mouseLook = new MouseLook();

		public AdvancedSettings advancedSettings = new AdvancedSettings();

		private Rigidbody RigidBody;

		private CapsuleCollider Capsule;

		private float yRotation;

		private Vector3 groundContactNormal;

		private bool jump;

		private bool previouslyGrounded;

		private bool jumping;

		private bool isGrounded;

		public Vector3 Velocity
		{
			get
			{
				return RigidBody.velocity;
			}
		}

		public bool Grounded
		{
			get
			{
				return isGrounded;
			}
		}

		public bool Jumping
		{
			get
			{
				return jumping;
			}
		}

		public bool Running
		{
			get
			{
				return movementSettings.Running;
			}
		}

		private void Start()
		{
			RigidBody = GetComponent<Rigidbody>();
			Capsule = GetComponent<CapsuleCollider>();
		}

		private void Update()
		{
			RotateView();
			if (CrossPlatformInputManager.GetButtonDown("Jump") && !jump)
			{
				jump = true;
			}
		}

		private void FixedUpdate()
		{
			GroundCheck();
			Vector2 input = GetInput();
			if ((input.x != 0f || input.y != 0f) && (advancedSettings.airControl || isGrounded))
			{
				Vector3 vector = _camera.transform.forward * input.y + _camera.transform.right * input.x;
				vector = (vector - Vector3.Project(vector, groundContactNormal)).normalized;
				vector.x *= movementSettings.CurrentTargetSpeed;
				vector.z *= movementSettings.CurrentTargetSpeed;
				vector.y *= movementSettings.CurrentTargetSpeed;
				if (RigidBody.velocity.sqrMagnitude < movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed)
				{
					RigidBody.AddForce(vector * SlopeMultiplier(), ForceMode.Impulse);
				}
			}
			if (isGrounded)
			{
				RigidBody.drag = 5f;
				if (jump)
				{
					RigidBody.drag = 0f;
					RigidBody.velocity = new Vector3(RigidBody.velocity.x, 0f, RigidBody.velocity.z);
					RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
					jumping = true;
				}
				if (!jumping && input.x == 0f && input.y == 0f && RigidBody.velocity.magnitude < 1f)
				{
					RigidBody.Sleep();
				}
			}
			else
			{
				RigidBody.drag = 0f;
				if (previouslyGrounded && !jumping)
				{
					StickToGroundHelper();
				}
			}
			jump = false;
		}

		private float SlopeMultiplier()
		{
			float time = Vector3.Angle(groundContactNormal, Vector3.up);
			return movementSettings.SlopeCurveModifier.Evaluate(time);
		}

		private void StickToGroundHelper()
		{
			RaycastHit hitInfo;
			if (Physics.SphereCast(base.transform.position, Capsule.radius, Vector3.down, out hitInfo, Capsule.height / 2f - Capsule.radius + advancedSettings.stickToGroundHelperDistance) && Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
			{
				RigidBody.velocity -= Vector3.Project(RigidBody.velocity, hitInfo.normal);
			}
		}

		private Vector2 GetInput()
		{
			movementSettings.UpdateDesiredTargetSpeed();
			return new Vector2
			{
				x = CrossPlatformInputManager.GetAxis("Horizontal"),
				y = CrossPlatformInputManager.GetAxis("Vertical")
			};
		}

		private void RotateView()
		{
			float y = base.transform.eulerAngles.y;
			Vector2 vector = mouseLook.Clamped(yRotation, base.transform.localEulerAngles.y);
			_camera.transform.localEulerAngles = new Vector3(0f - vector.y, _camera.transform.localEulerAngles.y, _camera.transform.localEulerAngles.z);
			yRotation = vector.y;
			base.transform.localEulerAngles = new Vector3(0f, vector.x, 0f);
			if (isGrounded || advancedSettings.airControl)
			{
				Quaternion quaternion = Quaternion.AngleAxis(base.transform.eulerAngles.y - y, Vector3.up);
				RigidBody.velocity = quaternion * RigidBody.velocity;
			}
		}

		private void GroundCheck()
		{
			previouslyGrounded = isGrounded;
			RaycastHit hitInfo;
			if (Physics.SphereCast(base.transform.position, Capsule.radius, Vector3.down, out hitInfo, Capsule.height / 2f - Capsule.radius + advancedSettings.groundCheckDistance))
			{
				isGrounded = true;
				groundContactNormal = hitInfo.normal;
			}
			else
			{
				isGrounded = false;
				groundContactNormal = Vector3.up;
			}
			if (!previouslyGrounded && isGrounded && jumping)
			{
				jumping = false;
			}
		}
	}
}
