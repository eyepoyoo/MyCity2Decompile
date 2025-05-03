using System.Collections.Generic;
using UnityEngine;

public class AmuzoDebugMenuManager : AmuzoMonoSingleton<AmuzoDebugMenuManager>
{
	private const string DEBUG_MENU_VERSION = "1.0";

	private const float HOLD_TIME_TO_SHOW_MENU = 3f;

	private const float TEXT_AREA_PADDING = 5f;

	private static List<AmuzoDebugMenu> _rootMenus = new List<AmuzoDebugMenu>();

	public Rect _titleArea;

	public Rect _infoDisplayArea;

	public Rect _buttonArea;

	public int _buttonColoumns = 6;

	public int _buttonRows = 4;

	public float _buttonSpacing = 5f;

	public float _scrollBarWidth = 20f;

	public GUIStyle _titleTextStyle;

	public GUIStyle _infoTextStyle;

	public GUIStyle _buttonTextStyle;

	public GUISkin _debugMenuSkin;

	[HideInInspector]
	public Rect _infoTextDisplayArea;

	private float _buttonWidth;

	private float _buttonHeight;

	private float _menuTimer;

	private List<AmuzoDebugMenu> _navigationStack = new List<AmuzoDebugMenu>();

	private Texture2D _blackPixel;

	private void Update()
	{
	}

	private void OnGUI()
	{
	}

	public void NavigateToPrevious()
	{
		_navigationStack[_navigationStack.Count - 1].OnHide();
		_navigationStack.RemoveAt(_navigationStack.Count - 1);
		if (_navigationStack.Count > 0)
		{
			_navigationStack[_navigationStack.Count - 1].OnShow();
		}
	}

	public void NavigateTo(AmuzoDebugMenu newDebugMenu)
	{
		_navigationStack.Add(newDebugMenu);
		newDebugMenu.OnShow();
	}

	public Rect GetButtonPosition(int position)
	{
		float x = _buttonArea.x + _buttonSpacing + (_buttonWidth + _buttonSpacing) * (float)(position % _buttonColoumns);
		float y = _buttonArea.y + _buttonSpacing + (_buttonHeight + _buttonSpacing) * Mathf.Floor(position / _buttonColoumns);
		return new Rect(x, y, _buttonWidth, _buttonHeight);
	}

	public static void NavigateBack()
	{
		if (AmuzoMonoSingleton<AmuzoDebugMenuManager>._pExists)
		{
			AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance.NavigateToPrevious();
		}
	}

	public static void RegisterRootDebugMenu(AmuzoDebugMenu newRootMenu)
	{
		if (!_rootMenus.Contains(newRootMenu))
		{
			_rootMenus.Add(newRootMenu);
		}
	}

	protected override void Initialise()
	{
		_titleArea = DecimaltoScreenRect(_titleArea);
		_infoDisplayArea = DecimaltoScreenRect(_infoDisplayArea);
		_buttonArea = DecimaltoScreenRect(_buttonArea);
		_infoTextDisplayArea = _infoDisplayArea;
		_infoTextDisplayArea.x += 5f;
		_infoTextDisplayArea.width -= 10f;
		_infoTextDisplayArea.y += 5f;
		_infoTextDisplayArea.height -= 10f;
		_blackPixel = new Texture2D(1, 1);
		_blackPixel.SetPixel(0, 0, Color.black);
		_blackPixel.Apply();
		_buttonWidth = (_buttonArea.width - _buttonSpacing * (float)(_buttonColoumns + 1)) / (float)_buttonColoumns;
		_buttonHeight = (_buttonArea.height - _buttonSpacing * (float)(_buttonRows + 1)) / (float)_buttonRows;
		AmuzoDebugMenu_SystemInfo newRootMenu = new AmuzoDebugMenu_SystemInfo("SYSTEM INFO");
		RegisterRootDebugMenu(newRootMenu);
		AmuzoDebugMenu_BuildInfo newRootMenu2 = new AmuzoDebugMenu_BuildInfo("BUILD INFO");
		RegisterRootDebugMenu(newRootMenu2);
		float num = (float)Screen.height / 768f;
		_infoTextStyle.fontSize = Mathf.FloorToInt(num * (float)_infoTextStyle.fontSize);
		_buttonTextStyle.fontSize = Mathf.FloorToInt(num * (float)_buttonTextStyle.fontSize);
		_titleTextStyle.fontSize = Mathf.FloorToInt(num * (float)_titleTextStyle.fontSize);
		float f = (_scrollBarWidth - 2f) * num;
		_scrollBarWidth *= num;
		_scrollBarWidth = Mathf.FloorToInt(_scrollBarWidth);
		_debugMenuSkin.verticalScrollbar.fixedWidth = Mathf.FloorToInt(_scrollBarWidth);
		_debugMenuSkin.verticalScrollbarThumb.fixedWidth = Mathf.FloorToInt(_scrollBarWidth);
		_debugMenuSkin.verticalSlider.fixedWidth = Mathf.FloorToInt(f);
		_debugMenuSkin.verticalSliderThumb.fixedWidth = Mathf.FloorToInt(f);
	}

	private void OnShow()
	{
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("MAIN DEBUG MENU");
		amuzoDebugMenu.AddInfoTextFunction(() => "AMUZO DEBUG MENU v1.0", false);
		int num = 0;
		for (int count = _rootMenus.Count; num < count; num++)
		{
			amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton(_rootMenus[num]));
		}
		NavigateTo(amuzoDebugMenu);
	}

	private Rect DecimaltoScreenRect(Rect decimalRect)
	{
		decimalRect.x *= Screen.width;
		decimalRect.y *= Screen.height;
		decimalRect.width *= Screen.width;
		decimalRect.height *= Screen.height;
		return decimalRect;
	}
}
