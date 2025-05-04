using UnityEngine;

public class AmuzoMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	[SerializeField]
	private bool _dontDestroyOnLoad = true;

	[SerializeField]
	private bool _allowBeingDestroyed;

	private static bool _hasBeenFinalDestroyed;

	public static bool _pExists
	{
		get
		{
			return _instance != null;
		}
	}

	public static T _pInstance
	{
		get
		{
			if (_hasBeenFinalDestroyed)
			{
				return (T)null;
			}
			if (_instance == null)
			{
				_instance = (T)Object.FindObjectOfType(typeof(T));
				if (_instance == null)
				{
					GameObject gameObject = new GameObject(typeof(T).ToString());
					_instance = gameObject.AddComponent<T>();
				}
				if (_instance is AmuzoMonoSingleton<T>)
				{
					(_instance as AmuzoMonoSingleton<T>).Initialise();
				}
			}
			return _instance;
		}
	}

	protected virtual void Initialise()
	{
	}

	protected virtual void Awake()
	{
		if (_dontDestroyOnLoad)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		if (_instance == null && !_hasBeenFinalDestroyed)
		{
			_instance = this as T;
			Initialise();
		}
		else if (_instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	protected virtual void OnDestroy()
	{
		_instance = (T)null;
		_hasBeenFinalDestroyed = !_allowBeingDestroyed;
	}
}
