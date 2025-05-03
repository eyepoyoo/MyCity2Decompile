using System.Collections;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;

namespace UnitySampleAssets.Characters.FirstPerson
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(AudioSource))]
	public class FirstPersonController : MonoBehaviour
	{
		[SerializeField]
		private bool _isWalking;

		[SerializeField]
		private float walkSpeed;

		[SerializeField]
		private float runSpeed;

		[Range(0f, 1f)]
		[SerializeField]
		private float runstepLenghten;

		[SerializeField]
		private float jumpSpeed;

		[SerializeField]
		private float stickToGroundForce;

		[SerializeField]
		private float _gravityMultiplier;

		[SerializeField]
		private MouseLook _mouseLook;

		[SerializeField]
		private bool useFOVKick;

		[SerializeField]
		private FOVKick _fovKick = new FOVKick();

		[SerializeField]
		private bool useHeadBob;

		[SerializeField]
		private CurveControlledBob _headBob = new CurveControlledBob();

		[SerializeField]
		private LerpControlledBob _jumpBob = new LerpControlledBob();

		[SerializeField]
		private float _stepInterval;

		[SerializeField]
		private AudioClip[] _footstepSounds;

		[SerializeField]
		private AudioClip _jumpSound;

		[SerializeField]
		private AudioClip _landSound;

		private Camera _camera;

		private bool _jump;

		private float _yRotation;

		private CameraRefocus _cameraRefocus;

		private Vector2 _input;

		private Vector3 _moveDir = Vector3.zero;

		private CharacterController _characterController;

		private CollisionFlags _collisionFlags;

		private bool _previouslyGrounded;

		private Vector3 _originalCameraPosition;

		private float _stepCycle;

		private float _nextStep;

		private bool _jumping;

		private void Start()
		{
			_characterController = GetComponent<CharacterController>();
			_camera = Camera.main;
			_originalCameraPosition = _camera.transform.localPosition;
			_cameraRefocus = new CameraRefocus(_camera, base.transform, _camera.transform.localPosition);
			_fovKick.Setup(_camera);
			_headBob.Setup(_camera, _stepInterval);
			_stepCycle = 0f;
			_nextStep = _stepCycle / 2f;
			_jumping = false;
		}

		private void Update()
		{
			RotateView();
			if (!_jump)
			{
				_jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
			if (!_previouslyGrounded && _characterController.isGrounded)
			{
				StartCoroutine(_jumpBob.DoBobCycle());
				PlayLandingSound();
				_moveDir.y = 0f;
				_jumping = false;
			}
			if (!_characterController.isGrounded && !_jumping && _previouslyGrounded)
			{
				_moveDir.y = 0f;
			}
			_previouslyGrounded = _characterController.isGrounded;
		}

		private void PlayLandingSound()
		{
			GetComponent<AudioSource>().clip = _landSound;
			GetComponent<AudioSource>().Play();
			_nextStep = _stepCycle + 0.5f;
		}

		private void FixedUpdate()
		{
			float speed;
			GetInput(out speed);
			Vector3 vector = _camera.transform.forward * _input.y + _camera.transform.right * _input.x;
			RaycastHit hitInfo;
			Physics.SphereCast(base.transform.position, _characterController.radius, Vector3.down, out hitInfo, _characterController.height / 2f);
			vector = Vector3.ProjectOnPlane(vector, hitInfo.normal).normalized;
			_moveDir.x = vector.x * speed;
			_moveDir.z = vector.z * speed;
			if (_characterController.isGrounded)
			{
				_moveDir.y = 0f - stickToGroundForce;
				if (_jump)
				{
					_moveDir.y = jumpSpeed;
					PlayJumpSound();
					_jump = false;
					_jumping = true;
				}
			}
			else
			{
				_moveDir += Physics.gravity * _gravityMultiplier;
			}
			_collisionFlags = _characterController.Move(_moveDir * Time.fixedDeltaTime);
			ProgressStepCycle(speed);
			UpdateCameraPosition(speed);
		}

		private void PlayJumpSound()
		{
			GetComponent<AudioSource>().clip = _jumpSound;
			GetComponent<AudioSource>().Play();
		}

		private void ProgressStepCycle(float speed)
		{
			if (_characterController.velocity.sqrMagnitude > 0f && (_input.x != 0f || _input.y != 0f))
			{
				_stepCycle += (_characterController.velocity.magnitude + speed * ((!_isWalking) ? runstepLenghten : 1f)) * Time.fixedDeltaTime;
			}
			if (_stepCycle > _nextStep)
			{
				_nextStep = _stepCycle + _stepInterval;
				PlayFootStepAudio();
			}
		}

		private void PlayFootStepAudio()
		{
			if (_characterController.isGrounded)
			{
				int num = Random.Range(1, _footstepSounds.Length);
				GetComponent<AudioSource>().clip = _footstepSounds[num];
				GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
				_footstepSounds[num] = _footstepSounds[0];
				_footstepSounds[0] = GetComponent<AudioSource>().clip;
			}
		}

		private void UpdateCameraPosition(float speed)
		{
			if (useHeadBob)
			{
				Vector3 localPosition;
				if (_characterController.velocity.magnitude > 0f && _characterController.isGrounded)
				{
					_camera.transform.localPosition = _headBob.DoHeadBob(_characterController.velocity.magnitude + speed * ((!_isWalking) ? runstepLenghten : 1f));
					localPosition = _camera.transform.localPosition;
					localPosition.y = _camera.transform.localPosition.y - _jumpBob.Offset();
				}
				else
				{
					localPosition = _camera.transform.localPosition;
					localPosition.y = _originalCameraPosition.y - _jumpBob.Offset();
				}
				_camera.transform.localPosition = localPosition;
				_cameraRefocus.SetFocusPoint();
			}
		}

		private void GetInput(out float speed)
		{
			float axis = CrossPlatformInputManager.GetAxis("Horizontal");
			float axis2 = CrossPlatformInputManager.GetAxis("Vertical");
			bool isWalking = _isWalking;
			speed = ((!_isWalking) ? runSpeed : walkSpeed);
			_input = new Vector2(axis, axis2);
			if (_input.sqrMagnitude > 1f)
			{
				_input.Normalize();
			}
			if (_isWalking != isWalking && useFOVKick && _characterController.velocity.sqrMagnitude > 0f)
			{
				StopAllCoroutines();
				IEnumerator routine;
				if (!_isWalking)
				{
					IEnumerator enumerator = _fovKick.FOVKickUp();
					routine = enumerator;
				}
				else
				{
					routine = _fovKick.FOVKickDown();
				}
				StartCoroutine(routine);
			}
		}

		private void RotateView()
		{
			Vector2 vector = _mouseLook.Clamped(_yRotation, base.transform.localEulerAngles.y);
			_camera.transform.localEulerAngles = new Vector3(0f - vector.y, _camera.transform.localEulerAngles.y, _camera.transform.localEulerAngles.z);
			_yRotation = vector.y;
			base.transform.localEulerAngles = new Vector3(0f, vector.x, 0f);
			_cameraRefocus.GetFocusPoint();
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
			if (!(attachedRigidbody == null) && !attachedRigidbody.isKinematic && _collisionFlags != CollisionFlags.Below)
			{
				attachedRigidbody.AddForceAtPosition(_characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
			}
		}
	}
}
