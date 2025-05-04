using System;
using UnityEngine;

public class TapjoyManager : MonoBehaviour
{
	private const float TAPJOY_CURRENCY_POLLING_TIME = 10f;

	private const float VIDEO_TEST_DELAY_TIME = 3f;

	private const string LOG_PREFIX = "TapjoyManager: ";

	private const string TAPJOY_DIRECT_PLAY_VIDEO = "AutoPlayVideo";

	private const string TAPJOY_OFFERWALL = "Offerwall";

	private const string TAPJOY_GEM_CURRENCY_ID_ANDROID = "aaa42176-e9a1-487b-b847-52c6e8aa6ba6";

	private const string TAPJOY_GEM_CURRENCY_ID_IOS = "29749be0-8c08-4b53-9c59-64156f1811de";

	public static Action<string, int> _onPremiumCurrencyGained;

	private static bool DO_DEBUG = true;

	public bool _runVideoTest;

	public bool _doFailVideoTest;

	public bool shouldPreload;

	public bool contentIsReadyForPlacement;

	private Action<bool> _onVideoCompleteCallback;

	private bool _isShowingVideo;

	private float _videoTestTImer;

	private int _lastPointsSpent = -1;

	private float _pollTimer;

	private bool _isConnected;

	public static TapjoyManager Instance { get; private set; }

	public bool _pIsConnected
	{
		get
		{
			return _isConnected;
		}
	}

	public bool _pIsDirectPlayVideoAvalible
	{
		get
		{
			return false;
		}
	}

	private void Start()
	{
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Awake()
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: Awaking and adding Tapjoy Events", base.gameObject);
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: Application Pause: " + pause, base.gameObject);
		}
		if (pause)
		{
		}
	}

	private void OnDisable()
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: Disabling and removing Tapjoy Events", base.gameObject);
		}
	}

	private void Update()
	{
		updateVideoTest();
		if (_isConnected)
		{
			if (_pollTimer < 10f)
			{
				_pollTimer += RealTime.deltaTime;
			}
			else
			{
				_pollTimer = 0f;
			}
		}
	}

	public void HandleConnectSuccess()
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: Handle Connect Success");
		}
		_isConnected = true;
	}

	public void HandleConnectFailure()
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: Handle Connect Failure");
		}
	}

	public void HandleViewWillOpen(int viewType)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleViewWillOpen, viewType: " + viewType);
		}
	}

	public void HandleViewDidOpen(int viewType)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleViewDidOpen, viewType: " + viewType);
		}
	}

	public void HandleViewWillClose(int viewType)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleViewWillClose, viewType: " + viewType);
		}
	}

	public void HandleViewDidClose(int viewType)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleViewDidClose, viewType: " + viewType);
		}
	}

	public void HandleShowOffers()
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleShowOffers");
		}
	}

	public void HandleShowOffersFailure(string error)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleShowOffersFailure: " + error);
		}
	}

	public void HandleAwardCurrencyResponse(string currencyName, int balance)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleAwardCurrencySucceeded: currencyName: " + currencyName + ", balance: " + balance);
		}
	}

	public void HandleAwardCurrencyResponseFailure(string error)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleAwardCurrencyResponseFailure: " + error);
		}
	}

	public void HandleGetCurrencyBalanceResponse(string currencyName, int balance)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleGetCurrencyBalanceResponse: currencyName: " + currencyName + ", balance: " + balance);
		}
		if (balance <= 0)
		{
			return;
		}
		if (balance == _lastPointsSpent)
		{
			if (DO_DEBUG)
			{
				Debug.Log("TapjoyManager: Not spending points as a spend points request for the same amount is in progress.");
			}
			return;
		}
		onCurrencyGained(currencyName, balance);
		_lastPointsSpent = balance;
		if (_onPremiumCurrencyGained != null)
		{
			_onPremiumCurrencyGained(currencyName, balance);
		}
	}

	public void HandleGetCurrencyBalanceResponseFailure(string error)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleGetCurrencyBalanceResponseFailure: " + error);
		}
	}

	public void HandleSpendCurrencyResponse(string currencyName, int balance)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleSpendCurrencyResponse: currencyName: " + currencyName + ", balance: " + balance);
		}
		_lastPointsSpent = -1;
	}

	public void HandleSpendCurrencyResponseFailure(string error)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleSpendCurrencyResponseFailure: " + error);
		}
		if (_lastPointsSpent != -1)
		{
			_lastPointsSpent = -1;
		}
	}

	public void HandleEarnedCurrency(string currencyName, int amount)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleEarnedCurrency: currencyName: " + currencyName + ", amount: " + amount);
		}
		if (amount <= 0)
		{
			return;
		}
		if (amount == _lastPointsSpent)
		{
			if (DO_DEBUG)
			{
				Debug.Log("TapjoyManager: Not spending points as a spend points request for the same ammount is in progress.");
			}
			return;
		}
		onCurrencyGained(currencyName, amount);
		_lastPointsSpent = amount;
		if (_onPremiumCurrencyGained != null)
		{
			_onPremiumCurrencyGained(currencyName, amount);
		}
	}

	public void HandleVideoStart()
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleVideoStarted");
		}
	}

	public void HandleVideoError(string status)
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleVideoError, status: " + status);
		}
		_isShowingVideo = false;
		if (_onVideoCompleteCallback != null)
		{
			_onVideoCompleteCallback(false);
		}
		videoOver();
	}

	public void HandleVideoComplete()
	{
		if (DO_DEBUG)
		{
			Debug.Log("TapjoyManager: HandleVideoComplete");
		}
		_isShowingVideo = false;
		if (_onVideoCompleteCallback != null)
		{
			_onVideoCompleteCallback(true);
		}
		videoOver();
	}

	public void showDirectPlayVideoAd(Action<bool> onVideoCompleteCallback)
	{
		if (!_pIsDirectPlayVideoAvalible)
		{
			if (DO_DEBUG)
			{
				Debug.Log("TapjoyManager: No direct play videos avalible.");
			}
			return;
		}
		if (_isShowingVideo)
		{
			if (DO_DEBUG)
			{
				Debug.Log("TapjoyManager: Direct play video in progress.");
			}
			return;
		}
		_onVideoCompleteCallback = onVideoCompleteCallback;
		_isShowingVideo = true;
		if (_runVideoTest)
		{
			_videoTestTImer = 0f;
		}
	}

	public void showOfferwall()
	{
	}

	protected virtual void onCurrencyGained(string currency, int ammount)
	{
	}

	private void updateVideoTest()
	{
		if (_runVideoTest && _isShowingVideo)
		{
			if (_videoTestTImer < 3f)
			{
				_videoTestTImer += RealTime.deltaTime;
			}
			else if (_doFailVideoTest)
			{
				HandleVideoError("TestError");
			}
			else
			{
				HandleVideoComplete();
			}
		}
	}

	private void videoOver()
	{
		_onVideoCompleteCallback = null;
		createNewDirectPlayplacement();
	}

	private void createNewDirectPlayplacement()
	{
	}

	private void createOfferwallplacement()
	{
	}
}
