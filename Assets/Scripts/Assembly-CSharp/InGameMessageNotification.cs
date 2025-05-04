using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameMessageNotification : MonoBehaviour
{
	public enum EDISPLAY_STATE
	{
		IDLE = 0,
		TWEEN_IN = 1,
		SHOWING = 2,
		TWEEN_OUT = 3
	}

	[Serializable]
	public class MessageUIElements
	{
		public UILabel mainLabel;

		public UILabel subLabel;

		public UITexture studTexture;
	}

	public class MessageNotification
	{
		public enum ETYPE
		{
			INVALID = 0,
			SCORE = 1,
			THOUGHT = 2
		}

		public string mainMessage;

		public string subMessage;

		public ETYPE type;
	}

	private const float SHOW_DUR = 2f;

	public MessageUIElements thoughtBubbleElems;

	public MessageUIElements generalBonusElems;

	public Transform offScreen;

	public Transform onScreen;

	public Transform thoughtBubbleRoot;

	public Transform generalBonusRoot;

	[SerializeField]
	private NotificationDisplay _studComboDisplay;

	[SerializeField]
	private NotificationDisplay _airTimeDisplay;

	[SerializeField]
	private NotificationDisplay _hiddenAreaDisplay;

	private List<MessageNotification> _messageList = new List<MessageNotification>();

	private EDISPLAY_STATE _displayState;

	private MessageNotification _currentMessage;

	private MessageNotification _currentStudStreakMessage;

	private float _stateStartTime;

	public void Register()
	{
		MinigameMessagingController._pInstance._onStudComboStarted += OnStudComboStarted;
		MinigameMessagingController._pInstance._onStudComboContinued += OnStudComboContinued;
		MinigameMessagingController._pInstance._onStudComboEnded += OnStudComboEnded;
		MinigameMessagingController._pInstance._onAirBonus += OnAirBonus;
		MinigameMessagingController._pInstance._onAirBonus += EmoticonSystem.OnAirBonus;
		MinigameMessagingController._pInstance._onHiddenAreaDiscovered += OnHiddenAreaDiscovered;
		MinigameMessagingController._pInstance._onOvercameObstacle += OnOvercameObstacle;
	}

	public void OnShowScreen()
	{
		thoughtBubbleRoot.position = offScreen.position;
		generalBonusRoot.position = offScreen.position;
		thoughtBubbleRoot.gameObject.SetActive(false);
		generalBonusRoot.gameObject.SetActive(false);
		_messageList.Clear();
		_displayState = EDISPLAY_STATE.IDLE;
		_studComboDisplay.Reset();
		_airTimeDisplay.Reset();
		_hiddenAreaDisplay.Reset();
	}

	private void OnStudComboStarted(int numStuds)
	{
		_currentStudStreakMessage = new MessageNotification();
		_currentStudStreakMessage.type = MessageNotification.ETYPE.SCORE;
		_currentStudStreakMessage.mainMessage = Localise("PlayerMessaging.StudStreak") + numStuds;
		_currentStudStreakMessage.subMessage = ((numStuds - 1) * numStuds).ToString();
		_currentStudStreakMessage = _studComboDisplay.ShowNotification(_currentStudStreakMessage);
	}

	private void OnStudComboContinued(int numStuds)
	{
		_currentStudStreakMessage.mainMessage = Localise("PlayerMessaging.StudStreak") + numStuds;
		_currentStudStreakMessage.subMessage = ((numStuds - 1) * numStuds).ToString();
		_studComboDisplay.UpdateExistingDisplay(_currentStudStreakMessage);
	}

	private void OnStudComboEnded(int numStuds)
	{
		_currentStudStreakMessage.mainMessage = Localise("PlayerMessaging.StudStreak") + numStuds;
		_currentStudStreakMessage.subMessage = ((numStuds - 1) * numStuds).ToString();
		_studComboDisplay.StopUpdatingDisplay(_currentStudStreakMessage);
		_currentStudStreakMessage = null;
	}

	private void OnAirBonus(float thresh)
	{
		MessageNotification messageNotification = new MessageNotification();
		messageNotification.type = MessageNotification.ETYPE.SCORE;
		messageNotification.mainMessage = Localise("PlayerMessaging.AirTime") + " " + thresh.ToString("F2") + Localise("PlayerMessaging.AirTimeUnit");
		messageNotification.subMessage = Mathf.FloorToInt(thresh).ToString();
		_airTimeDisplay.ShowNotification(messageNotification);
	}

	private void OnHiddenAreaDiscovered(PlayerTrigger trigger)
	{
		if (trigger._type == PlayerTrigger.EType.Hidden)
		{
			MessageNotification messageNotification = new MessageNotification();
			messageNotification.type = MessageNotification.ETYPE.SCORE;
			messageNotification.mainMessage = Localise("PlayerMessaging.HiddenArea");
			messageNotification.subMessage = 50.ToString();
			_hiddenAreaDisplay.ShowNotification(messageNotification);
			if (ScreenMinigameHUD._pInstance != null)
			{
				ScreenMinigameHUD._pInstance.OnHiddenArea();
			}
		}
	}

	private void OnOvercameObstacle(VehiclePart.EObstacleToNegate obstacle, VehiclePart vehiclePart)
	{
		MessageNotification messageNotification = new MessageNotification();
		messageNotification.type = MessageNotification.ETYPE.THOUGHT;
		messageNotification.mainMessage = Localise(vehiclePart.localisationKey);
		messageNotification.subMessage = Localise("PlayerMessaging.Thought." + obstacle);
		Debug.Log("Message: " + messageNotification.mainMessage + " Sub=" + messageNotification.subMessage);
		_messageList.Add(messageNotification);
	}

	private void ChangeState(EDISPLAY_STATE newState)
	{
		_displayState = newState;
		_stateStartTime = Time.time;
	}

	private void Update()
	{
		switch (_displayState)
		{
		case EDISPLAY_STATE.IDLE:
			UpdateIdle();
			break;
		case EDISPLAY_STATE.SHOWING:
			UpdateShowing();
			break;
		case EDISPLAY_STATE.TWEEN_IN:
			UpdateTweenIn();
			break;
		case EDISPLAY_STATE.TWEEN_OUT:
			UpdateTweenOut();
			break;
		}
	}

	private void UpdateIdle()
	{
		if (_messageList.Count > 0)
		{
			DispatchMessage();
		}
	}

	private void UpdateShowing()
	{
		float num = Time.time - _stateStartTime;
		if (num >= 2f)
		{
			ChangeState(EDISPLAY_STATE.TWEEN_OUT);
		}
	}

	private void UpdateTweenIn()
	{
		float num = Time.time - _stateStartTime;
		float num2 = 0.25f;
		if (num < num2)
		{
			float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num, num2, 0f, 1f);
			Vector3 position = Vector3.Lerp(offScreen.position, onScreen.position, t);
			thoughtBubbleRoot.position = position;
			generalBonusRoot.position = position;
		}
		else
		{
			thoughtBubbleRoot.position = onScreen.position;
			generalBonusRoot.position = onScreen.position;
			ChangeState(EDISPLAY_STATE.SHOWING);
		}
	}

	private void UpdateTweenOut()
	{
		float num = Time.time - _stateStartTime;
		float num2 = 0.25f;
		if (num < num2)
		{
			float t = Easing.Ease(Easing.EaseType.EaseInCircle, num, num2, 0f, 1f);
			Vector3 position = Vector3.Lerp(onScreen.position, offScreen.position, t);
			thoughtBubbleRoot.position = position;
			generalBonusRoot.position = position;
		}
		else
		{
			thoughtBubbleRoot.position = offScreen.position;
			generalBonusRoot.position = offScreen.position;
			thoughtBubbleRoot.gameObject.SetActive(false);
			generalBonusRoot.gameObject.SetActive(false);
			_currentMessage = null;
			ChangeState(EDISPLAY_STATE.IDLE);
		}
	}

	private void DispatchMessage()
	{
		_currentMessage = _messageList[0];
		_messageList.RemoveAt(0);
		thoughtBubbleRoot.position = offScreen.position;
		generalBonusRoot.position = offScreen.position;
		if (_currentMessage.type == MessageNotification.ETYPE.SCORE)
		{
			generalBonusElems.mainLabel.text = _currentMessage.mainMessage;
			generalBonusElems.subLabel.text = _currentMessage.subMessage;
			generalBonusElems.studTexture.material = ScenarioManager._pInstance._pCurrentScenario.guiStudMaterial;
			thoughtBubbleRoot.gameObject.SetActive(false);
			generalBonusRoot.gameObject.SetActive(true);
		}
		else if (_currentMessage.type == MessageNotification.ETYPE.THOUGHT)
		{
			thoughtBubbleElems.mainLabel.text = _currentMessage.mainMessage;
			thoughtBubbleElems.subLabel.text = _currentMessage.subMessage;
			thoughtBubbleRoot.gameObject.SetActive(true);
			generalBonusRoot.gameObject.SetActive(false);
		}
		ChangeState(EDISPLAY_STATE.TWEEN_IN);
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
}
