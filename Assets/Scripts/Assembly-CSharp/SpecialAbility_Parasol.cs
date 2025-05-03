using UnityEngine;

public class SpecialAbility_Parasol : SpecialAbility
{
	private const float ANIM_DURATION = 0.15f;

	public float _gravityMultiplier = 0.5f;

	public Animation _animation;

	private float _normAnimTime = 1f;

	protected override void Awake()
	{
		base.Awake();
		if (_animation != null)
		{
			_animation[_animation.clip.name].speed = 0f;
			_animation.Play();
		}
	}

	protected override void OnStarted()
	{
		base.OnStarted();
		SoundFacade._pInstance.PlayOneShotSFX("UseParasol", 0f);
	}

	private void FixedUpdate()
	{
		if (base._pIsInUse)
		{
			base._pVehicle._pRigidbody.AddForceAtPosition(-Physics.gravity * (1f - _gravityMultiplier), base._pVehicle._pRigidbody.worldCenterOfMass + base._pVehicle.transform.up * 1f, ForceMode.Acceleration);
		}
	}

	protected override void Update()
	{
		base.Update();
		_normAnimTime = Mathf.MoveTowards(_normAnimTime, (!base._pIsInUse) ? 1 : 0, Time.deltaTime / 0.15f);
		_animation[_animation.clip.name].normalizedTime = _normAnimTime;
	}
}
