using System.Collections.Generic;
using UnityEngine;

public class ScreenCodeEntry : ScreenBase
{
	public UILabel _textEntry;

	public Color _borderNormal;

	public Color _borderSuccess;

	public Color _borderFailure;

	public UIButton _confirmButton;

	public ScreenObjectTransitionWidget _validCodeTransition;

	public ScreenObjectTransitionWidget _invalidCodeTransition;

	public ScreenObjectTransitionWidget _regularInfo;

	public UISprite _borderSprite;

	public void OnSubmit()
	{
		bool flag = false;
		string text = _textEntry.text.ToLowerInvariant();
		if (HandleCode("2isgr8"))
		{
			flag = true;
			GlobalInGameData._pUnclaimedDailyRewardChests++;
		}
		if (!_regularInfo._pConsideredTweenedIn)
		{
			_regularInfo.SetToTweenInStartScale();
			_regularInfo.SetStateConsideredTweenedIn(false);
			_regularInfo.TweenIn();
		}
		if (flag)
		{
			_validCodeTransition.SetToTweenInStartScale();
			_validCodeTransition.SetStateConsideredTweenedIn(false);
			_validCodeTransition.TweenIn();
			_borderSprite.color = _borderSuccess;
			_invalidCodeTransition.SetToTweenInStartScale();
			_invalidCodeTransition.SetStateConsideredTweenedIn(false);
		}
		else
		{
			_validCodeTransition.SetToTweenInStartScale();
			_validCodeTransition.SetStateConsideredTweenedIn(false);
			_borderSprite.color = _borderFailure;
			_invalidCodeTransition.SetToTweenInStartScale();
			_invalidCodeTransition.SetStateConsideredTweenedIn(false);
			_invalidCodeTransition.TweenIn();
		}
		if (Facades<TrackingFacade>.Instance != null)
		{
			Facades<TrackingFacade>.Instance.LogParameterMetric("Code", new Dictionary<string, string> { 
			{
				"Valid",
				flag.ToString()
			} });
			Facades<TrackingFacade>.Instance.LogEvent("ValidCode");
		}
	}

	public void OnChange()
	{
		_confirmButton.isEnabled = _textEntry.text.Length > 0;
	}

	public void OnBack()
	{
		Navigate("Back");
	}

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		_validCodeTransition.SetToTweenInStartScale();
		_validCodeTransition.SetStateConsideredTweenedIn(false);
		_invalidCodeTransition.SetToTweenInStartScale();
		_invalidCodeTransition.SetStateConsideredTweenedIn(false);
		_regularInfo.SetToTweenInStartScale();
		_regularInfo.SetStateConsideredTweenedIn(false);
		_confirmButton.isEnabled = false;
		_textEntry.text = string.Empty;
		_textEntry.GetComponent<UIInput>().text = string.Empty;
		_borderSprite.color = _borderNormal;
	}

	private bool HandleCode(string codeName)
	{
		string text = _textEntry.text.ToLowerInvariant();
		Debug.Log("CodeEntered: " + text + " vs " + codeName);
		if (text == codeName && !GlobalInGameData.TriggeredCode(codeName))
		{
			GlobalInGameData.SetCodeTriggered(codeName, true);
			return true;
		}
		return false;
	}
}
