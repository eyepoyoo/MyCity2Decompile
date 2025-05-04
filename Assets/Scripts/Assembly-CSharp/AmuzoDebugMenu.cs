using System;
using System.Collections.Generic;
using UnityEngine;

public class AmuzoDebugMenu
{
	public static string NEW_LINE = string.Empty + '\n' + '\r';

	public string _debugMenuName;

	private List<AmuzoDebugMenuButton> _debugMenuButtons = new List<AmuzoDebugMenuButton>();

	protected Func<string> _refreshTextAreaFunction;

	protected string _debugText = string.Empty;

	protected bool _hasTextDefined;

	protected Vector2 scrollPosition = Vector2.zero;

	protected Rect _textContentRect;

	public AmuzoDebugMenu(string menuName)
	{
		_debugMenuName = menuName;
		_debugMenuButtons.Add(new AmuzoDebugMenuButton("...", AmuzoDebugMenuManager.NavigateBack));
	}

	public void AddButton(AmuzoDebugMenuButton newButton)
	{
		if (!_debugMenuButtons.Contains(newButton))
		{
			_debugMenuButtons.Add(newButton);
		}
	}

	public void AddInfoTextFunction(Func<string> textAreaFunction, bool addTextRefreshButton = true)
	{
		_refreshTextAreaFunction = textAreaFunction;
		if (addTextRefreshButton)
		{
			AddButton(new AmuzoDebugMenuButton("Refresh Info", RefreshText));
		}
	}

	public virtual void OnShow()
	{
		RefreshText();
		for (int i = 0; i < _debugMenuButtons.Count; i++)
		{
			if (_debugMenuButtons[i] != null)
			{
				_debugMenuButtons[i].OnShow();
			}
		}
	}

	public virtual void OnHide()
	{
	}

	public virtual void OnGUI()
	{
		GUI.Label(AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._titleArea, _debugMenuName, AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._titleTextStyle);
		DrawButtons();
		DrawInfoText();
	}

	protected virtual void RefreshText()
	{
		if (_refreshTextAreaFunction != null)
		{
			_debugText = _refreshTextAreaFunction();
			_hasTextDefined = true;
			_textContentRect = AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._infoTextDisplayArea;
			_textContentRect.width -= AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._scrollBarWidth + 5f;
			_textContentRect.height = AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._infoTextStyle.CalcHeight(new GUIContent(_debugText), _textContentRect.width);
		}
	}

	protected virtual void DrawButtons()
	{
		GUI.enabled = true;
		int i = 0;
		for (int count = _debugMenuButtons.Count; i < count; i++)
		{
			_debugMenuButtons[i].OnGUI(i);
		}
		GUI.enabled = false;
		Color color = GUI.color;
		float a = color.a;
		color.a = 0.5f;
		GUI.color = color;
		int j = _debugMenuButtons.Count;
		for (int num = AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._buttonColoumns * AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._buttonRows; j < num; j++)
		{
			GUI.Button(AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance.GetButtonPosition(j), string.Empty);
		}
		color.a = a;
		GUI.color = color;
		GUI.enabled = true;
	}

	protected virtual void DrawInfoText()
	{
		if (_hasTextDefined)
		{
			scrollPosition = GUI.BeginScrollView(AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._infoTextDisplayArea, scrollPosition, _textContentRect);
			GUI.Label(_textContentRect, _debugText, AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._infoTextStyle);
			GUI.EndScrollView();
		}
	}
}
