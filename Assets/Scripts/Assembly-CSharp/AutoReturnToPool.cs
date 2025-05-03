using UnityEngine;

public class AutoReturnToPool : MonoBehaviour, IFastPoolItem
{
	private const float CHECK_INTERVAL = 2f;

	private readonly RepeatedAction _repeatedAction_CheckReturnToPool = new RepeatedAction(2f);

	private static int _raycastMask = -1;

	private Animation _anim;

	public FastPool _pool { get; private set; }

	private void Awake()
	{
		if (_raycastMask == -1)
		{
			_raycastMask = LayerMask.GetMask("Geometry");
		}
		_anim = GetComponent<Animation>();
	}

	private void Update()
	{
		if (_pool != null && _repeatedAction_CheckReturnToPool.Update())
		{
			CheckReturnToPool();
		}
	}

	private void CheckReturnToPool()
	{
		if (!Camera.main.AreBoundsInView(GetComponentInChildren<Renderer>().bounds))
		{
			_pool.FastDestroy(base.gameObject);
			return;
		}
		Vector3 direction = base.transform.position + Vector3.up - Camera.main.transform.position;
		if (Physics.Raycast(Camera.main.transform.position, direction, direction.magnitude, _raycastMask))
		{
			ReturnToPool();
		}
	}

	public void ReturnToPool()
	{
		_pool.FastDestroy(base.gameObject);
	}

	public void OnCloned(FastPool pool)
	{
		_pool = pool;
	}

	public void OnFastInstantiate(FastPool pool)
	{
		_pool = pool;
		if ((bool)_anim)
		{
			_anim.Rewind();
			_anim.Play();
		}
	}

	public void OnFastDestroy()
	{
	}
}
