using System;
using UnityEngine;

public class AmuzoDebugMenuButton
{
	public string _buttonText;

	public Func<string> _getButtonText;

	public Action _onPress;

	public AmuzoDebugMenu _navigateToOnPress;

	private Rect _UIRect;

	private Vector2 _lastTopLeft;

	public AmuzoDebugMenuButton(string buttonText, Action onPress)
	{
		_buttonText = buttonText;
		_getButtonText = null;
		_onPress = onPress;
	}

	public AmuzoDebugMenuButton(Func<string> getButtonText, Action onPress)
	{
		_buttonText = null;
		_getButtonText = getButtonText;
		_onPress = onPress;
	}

	public AmuzoDebugMenuButton(AmuzoDebugMenu navigateToOnPress)
	{
		_buttonText = navigateToOnPress._debugMenuName;
		_navigateToOnPress = navigateToOnPress;
	}

	public virtual void OnShow()
	{
		RefreshButtonText();
	}

	public void OnGUI(int buttonIndex)
	{
		Rect buttonPosition = AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance.GetButtonPosition(buttonIndex);
		GUI.Label(buttonPosition, _buttonText, AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance._buttonTextStyle);
		if (GUI.Button(buttonPosition, string.Empty))
		{
			if (_onPress != null)
			{
				_onPress();
			}
			else if (_navigateToOnPress != null)
			{
				AmuzoMonoSingleton<AmuzoDebugMenuManager>._pInstance.NavigateTo(_navigateToOnPress);
			}
			RefreshButtonText();
		}
	}

	private void RefreshButtonText()
	{
		if (_getButtonText != null)
		{
			_buttonText = _getButtonText();
		}
	}
}
