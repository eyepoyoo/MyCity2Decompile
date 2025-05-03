using System.Collections.Generic;
using UnityEngine;

public class NotificationDisplay : MonoBehaviour
{
	private const float MESSAGE_BUMP_TIME = 0.5f;

	private const float MESSAGE_IN_TIME = 1.2f;

	[SerializeField]
	private ScreenObjectTransitionWidget _positionTween;

	[SerializeField]
	private UILabel _mainLabel;

	[SerializeField]
	private UILabel _studsLabel;

	[SerializeField]
	private UILabel _flashingLabel;

	private List<InGameMessageNotification.MessageNotification> _messageList = new List<InGameMessageNotification.MessageNotification>();

	private float _stateStartTime;

	private InGameMessageNotification.MessageNotification _currentMessage;

	private InGameMessageNotification.EDISPLAY_STATE _currentState;

	private bool _awaitingPositionFinish;

	private void Update()
	{
		if (_currentState == InGameMessageNotification.EDISPLAY_STATE.SHOWING && Time.time >= _stateStartTime + 1.2f)
		{
			DismissNotification();
		}
		if (_positionTween._pTweenType == ScreenObjectTransitionWidget.TweenType.IDLE && _awaitingPositionFinish)
		{
			_awaitingPositionFinish = false;
			if (_currentState == InGameMessageNotification.EDISPLAY_STATE.TWEEN_IN)
			{
				_stateStartTime = Time.time;
				_currentState = InGameMessageNotification.EDISPLAY_STATE.SHOWING;
			}
			if (_currentState == InGameMessageNotification.EDISPLAY_STATE.TWEEN_OUT)
			{
				_stateStartTime = Time.time;
				_currentState = InGameMessageNotification.EDISPLAY_STATE.IDLE;
			}
		}
		if (_currentState == InGameMessageNotification.EDISPLAY_STATE.SHOWING)
		{
			float num = Time.time - _stateStartTime;
			if (num < 0.3f)
			{
				_flashingLabel.enabled = Frac(num * 10f) < 0.5f;
			}
			else
			{
				_flashingLabel.enabled = false;
			}
		}
	}

	private float Frac(float v)
	{
		return v - Mathf.Floor(v);
	}

	public InGameMessageNotification.MessageNotification ShowNotification(string mainString, string subString)
	{
		InGameMessageNotification.MessageNotification messageNotification = new InGameMessageNotification.MessageNotification();
		messageNotification.mainMessage = mainString;
		messageNotification.subMessage = subString;
		return ShowNotification(messageNotification);
	}

	public InGameMessageNotification.MessageNotification ShowNotification(InGameMessageNotification.MessageNotification newMessage)
	{
		ShowNewNotification(newMessage);
		return newMessage;
	}

	public void UpdateExistingDisplay(InGameMessageNotification.MessageNotification existingMessage)
	{
		if ((_currentState == InGameMessageNotification.EDISPLAY_STATE.SHOWING || _currentState == InGameMessageNotification.EDISPLAY_STATE.TWEEN_IN) && _currentMessage == existingMessage)
		{
			_mainLabel.text = existingMessage.mainMessage;
			_studsLabel.text = existingMessage.subMessage;
			_stateStartTime = Time.time;
		}
	}

	public void StopUpdatingDisplay(InGameMessageNotification.MessageNotification existingMessage)
	{
		if (_currentState == InGameMessageNotification.EDISPLAY_STATE.SHOWING && _currentMessage == existingMessage)
		{
			_mainLabel.text = existingMessage.mainMessage;
			_studsLabel.text = existingMessage.subMessage;
			_stateStartTime = Time.time - 1.2f;
		}
	}

	private void ShowNewNotification(InGameMessageNotification.MessageNotification newMessage)
	{
		_currentMessage = newMessage;
		_mainLabel.text = newMessage.mainMessage;
		_studsLabel.text = newMessage.subMessage;
		_flashingLabel.enabled = false;
		_positionTween.TweenIn();
		_awaitingPositionFinish = true;
		_currentState = InGameMessageNotification.EDISPLAY_STATE.TWEEN_IN;
	}

	private void DismissNotification()
	{
		if (_currentState == InGameMessageNotification.EDISPLAY_STATE.SHOWING)
		{
			_positionTween.TweenOut();
			_flashingLabel.enabled = false;
			_awaitingPositionFinish = true;
			_currentState = InGameMessageNotification.EDISPLAY_STATE.TWEEN_OUT;
		}
	}

	private void NotificationFinished()
	{
		_currentState = InGameMessageNotification.EDISPLAY_STATE.IDLE;
		_currentMessage = null;
		if (_messageList.Count > 0)
		{
			InGameMessageNotification.MessageNotification newMessage = _messageList[0];
			_messageList.RemoveAt(0);
			ShowNewNotification(newMessage);
		}
	}

	public void Reset()
	{
		_currentState = InGameMessageNotification.EDISPLAY_STATE.IDLE;
		_awaitingPositionFinish = false;
		_positionTween.SetStateConsideredTweenedIn(false);
		_positionTween.SetToTweenInStartPos();
	}
}
