using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenLoading : ScreenBase
{
	public float _spinSpeed = 20f;

	[SerializeField]
	private UISprite _spinner;

	private static ScreenLoading _instance;

	private string _navigateLoc;

	private float _waitTime;

	private static string _lastLoadedLevel = string.Empty;

	private static bool _isLoadingLevel;

	public static ScreenLoading _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public static string _pCurrentLevelName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	protected override void Update()
	{
		base.Update();
		UpdateNavLoc();
		UpdateMinigameLoading();
		_spinner.transform.eulerAngles -= new Vector3(0f, 0f, Time.unscaledDeltaTime * _spinSpeed);
	}

	private void UpdateNavLoc()
	{
		if (_navigateLoc != null)
		{
			if (_waitTime >= 0f || Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
			{
				_waitTime -= RealTime.deltaTime;
				return;
			}
			Navigate(_navigateLoc);
			_navigateLoc = null;
		}
	}

	public void DoNavigate(string loc)
	{
		_waitTime = 1f;
		_navigateLoc = loc;
	}

	public static void OnLevelLoadComplete()
	{
		Debug.Log("OnLevelLoadComplete()");
		_instance.NavigateToHUD();
	}

	public static void CoverNewSceneLoading()
	{
		_lastLoadedLevel = string.Empty;
		_isLoadingLevel = true;
	}

	public static void LoadMinigame(string name)
	{
		_lastLoadedLevel = name;
		_isLoadingLevel = true;
		GlobalInGameData.OnLevelWillLoad(_lastLoadedLevel, _pCurrentLevelName);
		Debug.Log("ScreenLoading - Loading Minigame: " + _lastLoadedLevel + ". Previous scene [" + _pCurrentLevelName + "]");
		ScreenBase.ChangeScene(_lastLoadedLevel);
		Debug.Log("LoadLevel was completed");
	}

	public static void Reload()
	{
		_isLoadingLevel = true;
		GlobalInGameData.OnLevelWillLoad(_pCurrentLevelName, _pCurrentLevelName);
		ScreenBase.ChangeScene(_lastLoadedLevel);
	}

	public void NavigateToHUD()
	{
		switch (_pCurrentLevelName)
		{
		case "01HubCity":
			if (!ScenarioManager._pInstance._pIsFreshSave)
			{
				Navigate("Hub");
			}
			else
			{
				Navigate("FTUEWelcome");
			}
			break;
		case "VehicleTester":
			if (GlobalInGameData._pWantFullCarousel)
			{
				Navigate("Garage");
			}
			else
			{
				Navigate("CustomiseVehicle");
			}
			break;
		default:
			Navigate("MinigameHUD");
			break;
		}
	}

	private void UpdateMinigameLoading()
	{
		if (_isLoadingLevel)
		{
			_isLoadingLevel = false;
			OnLevelLoadComplete();
		}
	}
}
