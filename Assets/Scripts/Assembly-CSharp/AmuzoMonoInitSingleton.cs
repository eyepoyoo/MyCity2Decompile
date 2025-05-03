using UnityEngine;

public class AmuzoMonoInitSingleton<T> : InitialisationObject where T : AmuzoMonoInitSingleton<T>
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
			}
			if (_instance._currentState != InitialisationState.FINISHED)
			{
				Debug.LogError(typeof(T).ToString() + " was not initialised before it's instance was accessed!");
			}
			return _instance;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (_dontDestroyOnLoad)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		if (_instance == null && !_hasBeenFinalDestroyed)
		{
			_instance = this as T;
		}
		else if (_instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_instance = (T)null;
		_hasBeenFinalDestroyed = !_allowBeingDestroyed;
	}
}
