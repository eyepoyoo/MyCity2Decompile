using UnityEngine;

public class DebugRenderSettings : MonoBehaviour
{
	public enum RENDER_TYPE
	{
		PLAYER = 0,
		SPAWNERS = 1,
		BEHAVIOUR = 2,
		JUMPLINK = 3,
		COLLIDER_2D = 4,
		COLLISION_TREE_NODES = 5,
		CONTACTS = 6,
		WEAPONS = 7,
		PATHS = 8,
		KNOWLEDGE = 9,
		ANIMATION_CONTROLLER = 10,
		UPGRADE_SCREEN = 11,
		MISC_TEMP = 12,
		FLOW = 13,
		PURCHASE = 14,
		CREDITS = 15
	}

	private static DebugRenderSettings _instance;

	public bool _player;

	public bool _spawners;

	public bool _behaviour;

	public bool _jumpLink;

	public bool _collider2d;

	public bool _collisionTreeNodes;

	public bool _contacts;

	public bool _weapons;

	public bool _paths;

	public bool _knowledge;

	public bool _animationController;

	public bool _upgradeScreen;

	public bool _miscTemp;

	public bool _flow;

	public bool _purchase;

	public bool _credits;

	public static DebugRenderSettings _pInstance
	{
		get
		{
			if (_instance == null && Application.isPlaying)
			{
				GameObject gameObject = new GameObject("DebugRenderSettings", typeof(DebugRenderSettings));
				_instance = gameObject.GetComponent<DebugRenderSettings>();
			}
			return _instance;
		}
		private set
		{
			if (_instance != null && _instance != value)
			{
				Debug.LogWarning("DebugRenderSettings._pInstance - we are setting an instance, but we already have one, this is bad and will invoke an on the spot destroy, investigate.");
				Object.Destroy(_instance.gameObject);
			}
			_instance = value;
		}
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_pInstance = this;
	}

	public static bool IsTypeEnabled(RENDER_TYPE type)
	{
		if (!Application.isEditor)
		{
			return false;
		}
		if (_instance == null)
		{
			return true;
		}
		switch (type)
		{
		case RENDER_TYPE.PLAYER:
			return _pInstance._player;
		case RENDER_TYPE.SPAWNERS:
			return _pInstance._spawners;
		case RENDER_TYPE.BEHAVIOUR:
			return _pInstance._behaviour;
		case RENDER_TYPE.JUMPLINK:
			return _pInstance._jumpLink;
		case RENDER_TYPE.COLLIDER_2D:
			return _pInstance._collider2d;
		case RENDER_TYPE.COLLISION_TREE_NODES:
			return _pInstance._collisionTreeNodes;
		case RENDER_TYPE.CONTACTS:
			return _pInstance._contacts;
		case RENDER_TYPE.WEAPONS:
			return _pInstance._weapons;
		case RENDER_TYPE.PATHS:
			return _pInstance._paths;
		case RENDER_TYPE.KNOWLEDGE:
			return _pInstance._knowledge;
		case RENDER_TYPE.ANIMATION_CONTROLLER:
			return _pInstance._animationController;
		case RENDER_TYPE.UPGRADE_SCREEN:
			return _pInstance._upgradeScreen;
		case RENDER_TYPE.MISC_TEMP:
			return _pInstance._miscTemp;
		case RENDER_TYPE.FLOW:
			return _pInstance._flow;
		case RENDER_TYPE.PURCHASE:
			return _pInstance._purchase;
		case RENDER_TYPE.CREDITS:
			return _pInstance._credits;
		default:
			return false;
		}
	}

	private void Update()
	{
		if (Application.isEditor)
		{
		}
	}
}
