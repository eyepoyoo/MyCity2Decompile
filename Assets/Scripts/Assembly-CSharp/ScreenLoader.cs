using System.Collections.Generic;
using UnityEngine;

public class ScreenLoader : InitialisationObject
{
	public bool initialiseGameSceneAfterScreenLoad = true;

	public bool dontDestroyOnLoad = true;

	public bool emulateLowMemInEditor;

	[HideInInspector]
	public List<ScreenLoaderInfo> screensToLoad;

	[HideInInspector]
	public bool showScreens;

	private int _currentScreenLoad;

	private int _numScreensToLoad;

	private bool _isLoading;

	protected override void Awake()
	{
		if (dontDestroyOnLoad)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		base.Awake();
	}

	public override void startInitialising()
	{
		_currentState = InitialisationState.INITIALISING;
		LoadScreens();
	}

	public void LoadScreens()
	{
		_currentScreenLoad = 0;
		_numScreensToLoad = screensToLoad.Count;
		_isLoading = true;
	}

	private bool IsLowMemory()
	{
		if (Facades<FidelityFacade>.Instance != null && Facades<FidelityFacade>.Instance.fidelity == Fidelity.Low)
		{
			return true;
		}
		return false;
	}

	private void Update()
	{
		if (!_isLoading)
		{
			return;
		}
		if (_currentScreenLoad < _numScreensToLoad)
		{
			if (screensToLoad[_currentScreenLoad] != null && screensToLoad[_currentScreenLoad].screenBase != null)
			{
				ScreenBase screenBase = Object.Instantiate(screensToLoad[_currentScreenLoad].screenBase);
				screenBase.gameObject.name = screensToLoad[_currentScreenLoad].screenBase.name;
				screenBase.RegisterScreen();
				screenBase.gameObject.SetActive(screensToLoad[_currentScreenLoad].immediatelyShowOnLoad);
				screenBase.transform.parent = Facades<ScreenFacade>.Instance.transform;
				screenBase.transform.localScale = Vector3.one;
				if (screensToLoad[_currentScreenLoad].immediatelyShowOnLoad)
				{
					Facades<ScreenFacade>.Instance.SetCurrentlyActiveScreen(screenBase);
					screenBase.ShowScreen();
				}
			}
			_currentScreenLoad++;
		}
		else
		{
			_isLoading = false;
			_currentState = InitialisationState.FINISHED;
		}
	}
}
