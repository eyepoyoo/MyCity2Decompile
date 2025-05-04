using System;
using System.Collections;
using UnityEngine;

namespace UnitySampleAssets.Characters.ThirdPerson
{
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[Serializable]
		public class AdvancedSettings
		{
			public float stationaryTurnSpeed = 180f;

			public float movingTurnSpeed = 360f;

			public float headLookResponseSpeed = 2f;

			public float crouchHeightFactor = 0.6f;

			public float crouchChangeSpeed = 4f;

			public float autoTurnThresholdAngle = 100f;

			public float autoTurnSpeed = 2f;

			public PhysicMaterial zeroFrictionMaterial;

			public PhysicMaterial highFrictionMaterial;

			public float jumpRepeatDelayTime = 0.25f;

			public float runCycleLegOffset = 0.2f;

			public float groundStickyEffect = 5f;
		}

		private class RayHitComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
			}
		}

		private const float half = 0.5f;

		[SerializeField]
		private float jumpPower = 12f;

		[SerializeField]
		private float airSpeed = 6f;

		[SerializeField]
		private float airControl = 2f;

		[Range(1f, 4f)]
		[SerializeField]
		public float gravityMultiplier = 2f;

		[Range(0.1f, 3f)]
		[SerializeField]
		private float moveSpeedMultiplier = 1f;

		[SerializeField]
		[Range(0.1f, 3f)]
		private float animSpeedMultiplier = 1f;

		[SerializeField]
		private AdvancedSettings advancedSettings;

		public LayerMask groundCheckMask;

		public LayerMask crouchCheckMask;

		private bool onGround;

		private Vector3 currentLookPos;

		private float originalHeight;

		private Animator animator;

		private float lastAirTime;

		private CapsuleCollider capsule;

		private Vector3 moveInput;

		private bool crouchInput;

		private bool jumpInput;

		private float turnAmount;

		private float forwardAmount;

		private Vector3 velocity;

		private IComparer rayHitComparer;

		public float lookBlendTime;

		public float lookWeight;

		public Transform lookTarget { get; set; }

		private void Start()
		{
			animator = GetComponentInChildren<Animator>();
			capsule = GetComponent<Collider>() as CapsuleCollider;
			if (capsule == null)
			{
				Debug.LogError(" collider cannot be cast to CapsuleCollider");
			}
			else
			{
				originalHeight = capsule.height;
				capsule.center = Vector3.up * originalHeight * 0.5f;
			}
			rayHitComparer = new RayHitComparer();
			SetUpAnimator();
			currentLookPos = Camera.main.transform.position;
		}

		private IEnumerator BlendLookWeight()
		{
			float t = 0f;
			while (t < lookBlendTime)
			{
				lookWeight = t / lookBlendTime;
				t += Time.deltaTime;
				yield return null;
			}
			lookWeight = 1f;
		}

		private void OnEnable()
		{
			if (lookWeight == 0f)
			{
				StartCoroutine(BlendLookWeight());
			}
		}

		public void Move(Vector3 move, bool crouch, bool jump, Vector3 lookPos)
		{
			if (move.magnitude > 1f)
			{
				move.Normalize();
			}
			moveInput = move;
			crouchInput = crouch;
			jumpInput = jump;
			currentLookPos = lookPos;
			velocity = GetComponent<Rigidbody>().velocity;
			ConvertMoveInput();
			TurnTowardsCameraForward();
			PreventStandingInLowHeadroom();
			ScaleCapsuleForCrouching();
			ApplyExtraTurnRotation();
			GroundCheck();
			SetFriction();
			if (onGround)
			{
				HandleGroundedVelocities();
			}
			else
			{
				HandleAirborneVelocities();
			}
			UpdateAnimator();
			GetComponent<Rigidbody>().velocity = velocity;
		}

		private void ConvertMoveInput()
		{
			Vector3 vector = base.transform.InverseTransformDirection(moveInput);
			turnAmount = Mathf.Atan2(vector.x, vector.z);
			forwardAmount = vector.z;
		}

		private void TurnTowardsCameraForward()
		{
			if (Mathf.Abs(forwardAmount) < 0.01f)
			{
				Vector3 vector = base.transform.InverseTransformDirection(currentLookPos - base.transform.position);
				float num = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
				if (Mathf.Abs(num) > advancedSettings.autoTurnThresholdAngle)
				{
					turnAmount += num * advancedSettings.autoTurnSpeed * 0.001f;
				}
			}
		}

		private void PreventStandingInLowHeadroom()
		{
			if (!crouchInput)
			{
				Ray ray = new Ray(GetComponent<Rigidbody>().position + Vector3.up * capsule.radius * 0.5f, Vector3.up);
				float maxDistance = originalHeight - capsule.radius * 0.5f;
				if (Physics.SphereCast(ray, capsule.radius * 0.5f, maxDistance, crouchCheckMask))
				{
					crouchInput = true;
				}
			}
		}

		private void ScaleCapsuleForCrouching()
		{
			if (onGround && crouchInput && capsule.height != originalHeight * advancedSettings.crouchHeightFactor)
			{
				capsule.height = Mathf.MoveTowards(capsule.height, originalHeight * advancedSettings.crouchHeightFactor, Time.deltaTime * 4f);
				capsule.center = Vector3.MoveTowards(capsule.center, Vector3.up * originalHeight * advancedSettings.crouchHeightFactor * 0.5f, Time.deltaTime * 2f);
			}
			else if (capsule.height != originalHeight && capsule.center != Vector3.up * originalHeight * 0.5f)
			{
				capsule.height = Mathf.MoveTowards(capsule.height, originalHeight, Time.deltaTime * 4f);
				capsule.center = Vector3.MoveTowards(capsule.center, Vector3.up * originalHeight * 0.5f, Time.deltaTime * 2f);
			}
		}

		private void ApplyExtraTurnRotation()
		{
			float num = Mathf.Lerp(advancedSettings.stationaryTurnSpeed, advancedSettings.movingTurnSpeed, forwardAmount);
			base.transform.Rotate(0f, turnAmount * num * Time.deltaTime, 0f);
		}

		private void GroundCheck()
		{
			Ray ray = new Ray(base.transform.position + Vector3.up * 0.1f, -Vector3.up);
			RaycastHit[] array = Physics.RaycastAll(ray, 0.5f, groundCheckMask);
			Array.Sort(array, rayHitComparer);
			if (velocity.y < jumpPower * 0.5f)
			{
				onGround = false;
				GetComponent<Rigidbody>().useGravity = true;
				RaycastHit[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					RaycastHit raycastHit = array2[i];
					if (!raycastHit.collider.isTrigger)
					{
						if (velocity.y <= 0f)
						{
							GetComponent<Rigidbody>().position = Vector3.MoveTowards(GetComponent<Rigidbody>().position, raycastHit.point, Time.deltaTime * advancedSettings.groundStickyEffect);
						}
						onGround = true;
						GetComponent<Rigidbody>().useGravity = false;
						break;
					}
				}
			}
			if (!onGround)
			{
				lastAirTime = Time.time;
			}
		}

		private void SetFriction()
		{
			if (onGround)
			{
				if (moveInput.magnitude == 0f)
				{
					GetComponent<Collider>().material = advancedSettings.highFrictionMaterial;
				}
				else
				{
					GetComponent<Collider>().material = advancedSettings.zeroFrictionMaterial;
				}
			}
			else
			{
				GetComponent<Collider>().material = advancedSettings.zeroFrictionMaterial;
			}
		}

		private void HandleGroundedVelocities()
		{
			velocity.y = 0f;
			if (moveInput.magnitude == 0f)
			{
				velocity.x = 0f;
				velocity.z = 0f;
			}
			bool flag = animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded");
			bool flag2 = Time.time > lastAirTime + advancedSettings.jumpRepeatDelayTime;
			if (jumpInput && !crouchInput && flag2 && flag)
			{
				onGround = false;
				velocity = moveInput * airSpeed;
				velocity.y = jumpPower;
			}
		}

		private void HandleAirborneVelocities()
		{
			velocity = Vector3.Lerp(b: new Vector3(moveInput.x * airSpeed, velocity.y, moveInput.z * airSpeed), a: velocity, t: Time.deltaTime * airControl);
			GetComponent<Rigidbody>().useGravity = true;
			Vector3 force = Physics.gravity * gravityMultiplier - Physics.gravity;
			GetComponent<Rigidbody>().AddForce(force);
		}

		private void UpdateAnimator()
		{
			animator.applyRootMotion = onGround;
			animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
			animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
			animator.SetBool("Crouch", crouchInput);
			animator.SetBool("OnGround", onGround);
			if (!onGround)
			{
				animator.SetFloat("Jump", velocity.y);
			}
			float num = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + advancedSettings.runCycleLegOffset, 1f);
			float value = (float)((num < 0.5f) ? 1 : (-1)) * forwardAmount;
			if (onGround)
			{
				animator.SetFloat("JumpLeg", value);
			}
			if (onGround && moveInput.magnitude > 0f)
			{
				animator.speed = animSpeedMultiplier;
			}
			else
			{
				animator.speed = 1f;
			}
		}

		private void OnAnimatorIK(int layerIndex)
		{
			animator.SetLookAtWeight(lookWeight, 0.2f, 2.5f);
			if (lookTarget != null)
			{
				currentLookPos = lookTarget.position;
			}
			animator.SetLookAtPosition(currentLookPos);
		}

		private void SetUpAnimator()
		{
			this.animator = GetComponent<Animator>();
			Animator[] componentsInChildren = GetComponentsInChildren<Animator>();
			foreach (Animator animator in componentsInChildren)
			{
				if (animator != this.animator)
				{
					this.animator.avatar = animator.avatar;
					UnityEngine.Object.Destroy(animator);
					break;
				}
			}
		}

		public void OnAnimatorMove()
		{
			GetComponent<Rigidbody>().rotation = animator.rootRotation;
			if (onGround && Time.deltaTime > 0f)
			{
				Vector3 vector = animator.deltaPosition * moveSpeedMultiplier / Time.deltaTime;
				vector.y = GetComponent<Rigidbody>().velocity.y;
				GetComponent<Rigidbody>().velocity = vector;
			}
		}

		private void OnDisable()
		{
			lookWeight = 0f;
		}
	}
}
