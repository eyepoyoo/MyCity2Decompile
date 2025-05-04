using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputFacade
{
	private static SimpleInputControl[] _controls;

	private Dictionary<SimpleInputControl, int> _controlsDown = new Dictionary<SimpleInputControl, int>();

	private Dictionary<SimpleInputControl, int> _controlsUp = new Dictionary<SimpleInputControl, int>();

	private bool _locked;

	public static SimpleInputControl[] controls
	{
		get
		{
			return _controls ?? (_controls = (SimpleInputControl[])Enum.GetValues(typeof(SimpleInputControl)));
		}
	}

	public virtual bool locked
	{
		get
		{
			return _locked;
		}
		set
		{
			_locked = value;
		}
	}

	public bool isPressingDirection
	{
		get
		{
			return directionPressed.sqrMagnitude > 0f;
		}
	}

	public Vector3 directionPressed
	{
		get
		{
			Vector3 zero = Vector3.zero;
			if (GetControl(SimpleInputControl.Left))
			{
				zero += Vector3.left;
			}
			if (GetControl(SimpleInputControl.Right))
			{
				zero += Vector3.right;
			}
			if (GetControl(SimpleInputControl.Up))
			{
				zero += Vector3.forward;
			}
			if (GetControl(SimpleInputControl.Down))
			{
				zero += Vector3.back;
			}
			return zero.normalized;
		}
	}

	public virtual bool GetControl(SimpleInputControl control)
	{
		return !locked && _controlsDown.ContainsKey(control);
	}

	public virtual bool GetControlDown(SimpleInputControl control)
	{
		return !locked && _controlsDown.ContainsKey(control) && _controlsDown[control] == Time.frameCount;
	}

	public virtual bool GetControlUp(SimpleInputControl control)
	{
		return !locked && _controlsUp.ContainsKey(control) && _controlsUp[control] == Time.frameCount;
	}

	public virtual void SetControlDown(SimpleInputControl control)
	{
		if (!_controlsDown.ContainsKey(control))
		{
			_controlsDown.Add(control, Time.frameCount);
		}
		_controlsUp.Remove(control);
	}

	public virtual void SetControlUp(SimpleInputControl control)
	{
		if (!_controlsUp.ContainsKey(control))
		{
			_controlsUp.Add(control, Time.frameCount);
		}
		_controlsDown.Remove(control);
	}
}
