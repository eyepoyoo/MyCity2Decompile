using UnityEngine;

[RequireComponent(typeof(Animation))]
public class SkyHook : MonoBehaviour, IFastPoolItem
{
	public Transform _rope;

	public ParticleSystem _splashParticlesPrefab;

	private ParticleSystem _instanceParticles;

	private FastPool _pool;

	private Animation _animation;

	private Rigidbody _toHook;

	private bool _positionAtToHook;

	public void Hook(Rigidbody target, bool wantSplashParticles = false)
	{
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.PlayOneShotSFX("CrookRopeHook", target.transform.position, 0f);
		}
		_animation.Rewind();
		_animation.Play();
		_toHook = target;
		_positionAtToHook = true;
		if (_splashParticlesPrefab != null && wantSplashParticles)
		{
			FastPool pool = FastPoolManager.GetPool(_splashParticlesPrefab);
			pool.NotificationType = PoolItemNotificationType.Interface;
			_instanceParticles = pool.FastInstantiate<ParticleSystem>();
			_instanceParticles.transform.position = _toHook.transform.position + Vector3.up * 0.25f;
			if (_instanceParticles.transform.position.y < 0f)
			{
				Vector3 position = _instanceParticles.transform.position;
				position.y = 0.1f;
				_instanceParticles.transform.position = position;
			}
			_instanceParticles.Play();
		}
	}

	public void OnReachedBottom()
	{
		_toHook.useGravity = false;
		_toHook.isKinematic = true;
		_toHook.transform.parent = _rope;
		_positionAtToHook = false;
	}

	public void OnReachedTop()
	{
		_toHook.transform.parent = null;
		_pool.FastDestroy(this);
	}

	private void Update()
	{
		if (!_toHook || !_positionAtToHook)
		{
			return;
		}
		base.transform.position = _toHook.transform.position;
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, _toHook.transform.eulerAngles.y, base.transform.eulerAngles.z);
		if (_instanceParticles != null)
		{
			_instanceParticles.transform.position = base.transform.position + Vector3.up * 0.25f;
			if (_instanceParticles.transform.position.y < 0f)
			{
				Vector3 position = _instanceParticles.transform.position;
				position.y = 0.1f;
				_instanceParticles.transform.position = position;
			}
		}
	}

	public void OnFastInstantiate(FastPool pool)
	{
	}

	public void OnCloned(FastPool pool)
	{
		_pool = pool;
		_animation = GetComponent<Animation>();
	}

	public void OnFastDestroy()
	{
	}
}
