using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
	protected enum State
	{
		Normal = 0,
		Hover = 1,
		Pressed = 2,
		Disabled = 3
	}

	private static Dictionary<string, List<ToggleButton>> _toggleSets;

	public string setName = "defaultToggleSet";

	public bool selectedByDefault;

	public Color selectedNormal = Color.white;

	public Color selectedHover = Color.grey;

	public Color selectedPressed = Color.grey;

	public Color selectedDisabled = Color.grey;

	public Color unselectedNormal = Color.grey;

	public Color unselectedHover = Color.grey;

	public Color unselectedPressed = Color.grey;

	public Color unselectedDisabled = Color.grey;

	public List<EventDelegate> onClick = new List<EventDelegate>();

	protected bool _initDone;

	private UIWidget _widget;

	private State _currentState;

	private bool _isHovered;

	private bool _isSelected;

	public bool _pEnabled
	{
		get
		{
			return _currentState != State.Disabled;
		}
		set
		{
			if (!value)
			{
				SetState(State.Disabled);
			}
			else if (_isHovered)
			{
				SetState(State.Hover);
			}
			else
			{
				SetState(State.Normal);
			}
		}
	}

	private void Awake()
	{
		if (!_initDone)
		{
			OnInit();
		}
	}

	public static void SelectButton(ToggleButton button)
	{
		if (button != null)
		{
			button.OnClick();
		}
	}

	protected void Deselect()
	{
		_isSelected = false;
		SetState(_currentState);
	}

	protected void Select()
	{
		_isSelected = true;
		SetState(_currentState);
	}

	protected virtual void OnInit()
	{
		_initDone = true;
		_widget = GetComponent<UIWidget>();
		if (_toggleSets == null)
		{
			_toggleSets = new Dictionary<string, List<ToggleButton>>();
		}
		if (!_toggleSets.ContainsKey(setName))
		{
			_toggleSets[setName] = new List<ToggleButton>();
		}
		_toggleSets[setName].Add(this);
		if (selectedByDefault)
		{
			Select();
		}
		SetState(State.Normal);
	}

	protected virtual void OnHover(bool isOver)
	{
		_isHovered = isOver;
		if (_isHovered)
		{
			SetState(State.Hover);
		}
		else
		{
			SetState(State.Normal);
		}
	}

	protected virtual void OnClick()
	{
		if (_pEnabled)
		{
			List<ToggleButton> list = _toggleSets[setName];
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Deselect();
			}
			Select();
			EventDelegate.Execute(onClick);
		}
	}

	private void SetState(State newState)
	{
		_currentState = newState;
		switch (newState)
		{
		case State.Disabled:
			_widget.color = ((!_isSelected) ? unselectedDisabled : selectedDisabled);
			break;
		case State.Hover:
			_widget.color = ((!_isSelected) ? unselectedHover : selectedHover);
			break;
		case State.Normal:
			_widget.color = ((!_isSelected) ? unselectedNormal : selectedNormal);
			break;
		case State.Pressed:
			_widget.color = ((!_isSelected) ? unselectedPressed : selectedPressed);
			break;
		}
	}
}
