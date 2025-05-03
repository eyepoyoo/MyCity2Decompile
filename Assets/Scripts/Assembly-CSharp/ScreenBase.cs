using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenBase : MonoBehaviour
{
	public enum ScreenTweenType
	{
		Idle = 0,
		TweenIn = 1,
		TweenOut = 2
	}

	public bool isOverlay;

	private Action _onExitComplete;

	private ScreenObjectTransitionWidget[] _transitionWidgets;

	private int _numTransitionWidgets;

	private TextureResourceLoader[] textureLoaders;

	protected bool _isVisible;

	private bool _initialised;

	private float _screenExitTime;

	private float _screenStartTime;

	private Dictionary<GameObject, ScreenObjectTransitionWidget[]> _objectWidgetLookup = new Dictionary<GameObject, ScreenObjectTransitionWidget[]>();

	private ScreenTweenType _tweenType;

	protected bool _isReadyToExitTransition = true;

	public static bool _pIsOverlayShowing
	{
		get
		{
			if (Facades<ScreenFacade>.Instance == null)
			{
				return false;
			}
			ScreenBase[] currentlyActiveScreens = Facades<ScreenFacade>.Instance.GetCurrentlyActiveScreens();
			if (currentlyActiveScreens == null || currentlyActiveScreens.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < currentlyActiveScreens.Length; i++)
			{
				if (!(currentlyActiveScreens[i] == null) && currentlyActiveScreens[i]._pIsOverlay)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected bool _pIsOverlay
	{
		get
		{
			return isOverlay;
		}
	}

	public ScreenTweenType _pCurrentTweenType
	{
		get
		{
			return _tweenType;
		}
	}

	public bool IsReadyToExitTransition
	{
		get
		{
			return _isReadyToExitTransition;
		}
	}

	public void RegisterScreen()
	{
		Facades<ScreenFacade>.Instance.RegisterScreen(this);
		OnRegisterScreen();
	}

	private void Initialise()
	{
		_transitionWidgets = base.gameObject.GetComponentsInChildren<ScreenObjectTransitionWidget>();
		_numTransitionWidgets = _transitionWidgets.Length;
		_initialised = true;
		AmuzoNGUILocalize[] componentsInChildren = base.gameObject.GetComponentsInChildren<AmuzoNGUILocalize>(true);
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			componentsInChildren[i].AssignTextFromKey();
		}
		OnInitialise();
	}

	protected virtual void OnRegisterScreen()
	{
	}

	protected virtual void OnInitialise()
	{
	}

	protected virtual void OnShowScreen()
	{
		LoadTextures();
	}

	protected virtual void OnExitScreen()
	{
	}

	protected virtual void OnScreenShowComplete()
	{
	}

	protected virtual void OnScreenExitComplete()
	{
		UnLoadTextures();
		if (Facades<ScreenFacade>.Instance != null)
		{
			Facades<ScreenFacade>.Instance.OnScreenExitComplete(this);
		}
		base.gameObject.SetActive(false);
		if (_onExitComplete != null)
		{
			_onExitComplete();
			_onExitComplete = null;
		}
	}

	public static string formatCamelCaseForDebug(string camelCase)
	{
		string text = camelCase.Substring(0, 1).ToUpper();
		for (int i = 1; i < camelCase.Length; i++)
		{
			string text2 = camelCase.Substring(i, 1);
			text = ((!(text2 == text2.ToUpper())) ? (text + text2) : (text + " " + text2));
		}
		return text;
	}

	public virtual void ShowScreen()
	{
		if (!_initialised)
		{
			Initialise();
		}
		_isVisible = true;
		OnShowScreen();
		if (_numTransitionWidgets != 0)
		{
			_screenStartTime = 0f;
			for (int i = 0; i < _numTransitionWidgets; i++)
			{
				float num = 0f;
				if (_transitionWidgets[i] == null)
				{
					continue;
				}
				if (_transitionWidgets[i]._pCurrentPropertySet == null)
				{
					Debug.LogWarning("Widget has no property sets: " + _transitionWidgets[i].name, _transitionWidgets[i].gameObject);
				}
				else if (_transitionWidgets[i]._pCurrentPropertySet.autoTweenInOnScreenTransition)
				{
					if (_transitionWidgets[i]._pCurrentPropertySet.adjustsColorIn)
					{
						num = _transitionWidgets[i]._pCurrentPropertySet.colorInDelay + _transitionWidgets[i]._pCurrentPropertySet._pAdditionalDelayColorIn + _transitionWidgets[i]._pCurrentPropertySet.colorInDuration;
					}
					if (_transitionWidgets[i]._pCurrentPropertySet.adjustsPositionIn)
					{
						float b = _transitionWidgets[i]._pCurrentPropertySet.positionInDelay + _transitionWidgets[i]._pCurrentPropertySet._pAdditionalDelayPosIn + _transitionWidgets[i]._pCurrentPropertySet.positionInDuration;
						num = Mathf.Max(num, b);
					}
					if (_transitionWidgets[i]._pCurrentPropertySet.adjustsScaleIn)
					{
						float b2 = _transitionWidgets[i]._pCurrentPropertySet.scaleInDelay + _transitionWidgets[i]._pCurrentPropertySet._pAdditionalDelayScaleIn + _transitionWidgets[i]._pCurrentPropertySet.scaleInDuration;
						num = Mathf.Max(num, b2);
					}
					_transitionWidgets[i].TweenIn(true);
					_screenStartTime = Mathf.Max(_screenStartTime, num);
				}
			}
			_screenStartTime += RealTime.time;
			_tweenType = ScreenTweenType.TweenIn;
			UIPanel component = GetComponent<UIPanel>();
			if (component != null)
			{
				component.SetDirty();
			}
		}
		else if (_numTransitionWidgets == 0)
		{
			OnScreenShowComplete();
		}
	}

	public virtual float ExitScreen(Action onComplete = null, float additionalDelay = 0f)
	{
		OnExitScreen();
		_onExitComplete = (Action)Delegate.Combine(_onExitComplete, onComplete);
		if (_numTransitionWidgets == 0 && additionalDelay == 0f)
		{
			_isVisible = false;
			OnScreenExitComplete();
			return 0f;
		}
		_screenExitTime = 0f;
		for (int i = 0; i < _numTransitionWidgets; i++)
		{
			float num = 0f;
			if (_transitionWidgets[i] == null)
			{
				continue;
			}
			if (_transitionWidgets[i]._pCurrentPropertySet == null)
			{
				Debug.LogWarning("Widget has no property sets: " + _transitionWidgets[i].name, _transitionWidgets[i].gameObject);
			}
			else if (_transitionWidgets[i]._pCurrentPropertySet.autoTweenOutOnScreenTransition)
			{
				if (_transitionWidgets[i]._pCurrentPropertySet.adjustsColorOut)
				{
					num = _transitionWidgets[i]._pCurrentPropertySet.colorOutDelay + _transitionWidgets[i]._pCurrentPropertySet._pAdditionalDelayColorOut + _transitionWidgets[i]._pCurrentPropertySet.colorOutDuration;
				}
				if (_transitionWidgets[i]._pCurrentPropertySet.adjustsPositionOut)
				{
					float b = _transitionWidgets[i]._pCurrentPropertySet.positionOutDelay + _transitionWidgets[i]._pCurrentPropertySet._pAdditionalDelayPosOut + _transitionWidgets[i]._pCurrentPropertySet.positionOutDuration;
					num = Mathf.Max(num, b);
				}
				if (_transitionWidgets[i]._pCurrentPropertySet.adjustsScaleOut)
				{
					float b2 = _transitionWidgets[i]._pCurrentPropertySet.scaleOutDelay + _transitionWidgets[i]._pCurrentPropertySet._pAdditionalDelayScaleOut + _transitionWidgets[i]._pCurrentPropertySet.scaleOutDuration;
					num = Mathf.Max(num, b2);
				}
				_transitionWidgets[i].TweenOut(true);
				_screenExitTime = Mathf.Max(_screenExitTime, num);
			}
		}
		float result = _screenExitTime + additionalDelay;
		_screenExitTime += RealTime.time + additionalDelay;
		_tweenType = ScreenTweenType.TweenOut;
		return result;
	}

	public virtual float getExitTime()
	{
		if (_numTransitionWidgets == 0)
		{
			return 0f;
		}
		_screenExitTime = 0f;
		for (int i = 0; i < _numTransitionWidgets; i++)
		{
			float num = 0f;
			if (!(_transitionWidgets[i] == null) && _transitionWidgets[i]._pCurrentPropertySet != null && _transitionWidgets[i]._pCurrentPropertySet.autoTweenOutOnScreenTransition)
			{
				if (_transitionWidgets[i]._pCurrentPropertySet.adjustsColorOut)
				{
					num = _transitionWidgets[i]._pCurrentPropertySet.colorOutDelay + _transitionWidgets[i]._pCurrentPropertySet._pAdditionalDelayColorOut + _transitionWidgets[i]._pCurrentPropertySet.colorOutDuration;
				}
				if (_transitionWidgets[i]._pCurrentPropertySet.adjustsPositionOut)
				{
					float b = _transitionWidgets[i]._pCurrentPropertySet.positionOutDelay + _transitionWidgets[i]._pCurrentPropertySet._pAdditionalDelayPosOut + _transitionWidgets[i]._pCurrentPropertySet.positionOutDuration;
					num = Mathf.Max(num, b);
				}
				if (_transitionWidgets[i]._pCurrentPropertySet.adjustsScaleOut)
				{
					float b2 = _transitionWidgets[i]._pCurrentPropertySet.scaleOutDelay + _transitionWidgets[i]._pCurrentPropertySet._pAdditionalDelayScaleOut + _transitionWidgets[i]._pCurrentPropertySet.scaleOutDuration;
					num = Mathf.Max(num, b2);
				}
				_screenExitTime = Mathf.Max(_screenExitTime, num);
			}
		}
		return _screenExitTime;
	}

	protected void TryChangeWidgetSets(GameObject rootWidgetObject, string setName)
	{
		if (!_objectWidgetLookup.ContainsKey(rootWidgetObject))
		{
			_objectWidgetLookup[rootWidgetObject] = rootWidgetObject.GetComponentsInChildren<ScreenObjectTransitionWidget>();
		}
		int num = _objectWidgetLookup[rootWidgetObject].Length;
		for (int i = 0; i < num; i++)
		{
			_objectWidgetLookup[rootWidgetObject][i].TryChangeSet(setName);
		}
	}

	protected virtual void Update()
	{
		if (_tweenType == ScreenTweenType.Idle)
		{
			return;
		}
		if (_tweenType == ScreenTweenType.TweenOut)
		{
			if (RealTime.time >= _screenExitTime)
			{
				_isVisible = false;
				_tweenType = ScreenTweenType.Idle;
				OnScreenExitComplete();
			}
		}
		else if (_tweenType == ScreenTweenType.TweenIn && RealTime.time >= _screenStartTime)
		{
			_tweenType = ScreenTweenType.Idle;
			UIPanel component = GetComponent<UIPanel>();
			if (component != null)
			{
				component.SetDirty();
			}
			OnScreenShowComplete();
		}
	}

	protected void Navigate(string linkToFollow, bool immediate = false)
	{
		if (string.IsNullOrEmpty(linkToFollow) || (!_pIsOverlay && _pIsOverlayShowing) || _pCurrentTweenType != ScreenTweenType.Idle)
		{
			Debug.LogWarning("Did not navigate, isNotOverlay? " + !_pIsOverlay + " isOverlayShowing?" + _pIsOverlay + " screenTweenType = " + _pCurrentTweenType);
		}
		else if (Facades<ScreenFacade>.Instance._pIsAnyScreenTweening)
		{
			Debug.LogWarning("Screen already tweening");
		}
		else
		{
			Facades<ScreenFacade>.Instance.ExitCurrentScreenWithFlowChange(linkToFollow, immediate);
		}
	}

	public virtual void PollUpdate()
	{
	}

	public static Color getColourFromArray(Color[] colours, float decimalProgress)
	{
		if (colours.Length < 2)
		{
			Debug.Log("ScreenBase: Must have at least 2 colours in order to use 'getColourFromArray'!");
			return Color.white;
		}
		float num = 1f / (float)colours.Length;
		float t = decimalProgress % num / num;
		float num2 = 0f;
		Color white = Color.white;
		Color white2 = Color.white;
		for (int i = 0; i < colours.Length; i++)
		{
			num2 += num;
			if (num2 > decimalProgress)
			{
				white = colours[i];
				white2 = colours[(i + 1) % colours.Length];
				return Color.Lerp(white, white2, t);
			}
		}
		return Color.white;
	}

	protected string Localise(string key)
	{
		string text = LocalisationFacade.Instance.GetString(key);
		if (text == string.Empty || text == null)
		{
			return key;
		}
		return text;
	}

	protected void LoadTextures()
	{
		if (textureLoaders == null)
		{
			textureLoaders = base.gameObject.GetComponentsInChildren<TextureResourceLoader>(true);
		}
		for (int i = 0; i < textureLoaders.Length; i++)
		{
			textureLoaders[i].AssignTexture();
		}
	}

	protected void UnLoadTextures()
	{
		if (textureLoaders == null)
		{
			textureLoaders = base.gameObject.GetComponentsInChildren<TextureResourceLoader>(true);
		}
		for (int i = 0; i < textureLoaders.Length; i++)
		{
			textureLoaders[i].UnAssignTexture();
		}
	}

	public static void LoadEmptyScene()
	{
		ChangeScene("Empty");
	}

	public static void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
