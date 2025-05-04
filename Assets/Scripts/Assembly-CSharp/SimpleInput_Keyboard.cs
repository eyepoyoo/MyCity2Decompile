using UnityEngine;

public class SimpleInput_Keyboard : InputFacade
{
	public override bool GetControl(SimpleInputControl keyCode)
	{
		if (locked)
		{
			return false;
		}
		switch (keyCode)
		{
		case SimpleInputControl.Left:
			return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
		case SimpleInputControl.Right:
			return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
		case SimpleInputControl.Up:
			return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
		case SimpleInputControl.Down:
			return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
		case SimpleInputControl.Jump:
			return Input.GetKey(KeyCode.Space);
		case SimpleInputControl.Attack:
			return Input.GetMouseButton(0);
		default:
			return false;
		}
	}

	public override bool GetControlDown(SimpleInputControl keyCode)
	{
		if (locked)
		{
			return false;
		}
		switch (keyCode)
		{
		case SimpleInputControl.Left:
			return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
		case SimpleInputControl.Right:
			return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
		case SimpleInputControl.Up:
			return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
		case SimpleInputControl.Down:
			return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
		case SimpleInputControl.Jump:
			return Input.GetKeyDown(KeyCode.Space);
		case SimpleInputControl.Attack:
			return Input.GetMouseButtonDown(0);
		default:
			return false;
		}
	}

	public override bool GetControlUp(SimpleInputControl keyCode)
	{
		if (locked)
		{
			return false;
		}
		switch (keyCode)
		{
		case SimpleInputControl.Left:
			return Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);
		case SimpleInputControl.Right:
			return Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);
		case SimpleInputControl.Up:
			return Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W);
		case SimpleInputControl.Down:
			return Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S);
		case SimpleInputControl.Jump:
			return Input.GetKeyUp(KeyCode.Space);
		case SimpleInputControl.Attack:
			return Input.GetMouseButtonUp(0);
		default:
			return false;
		}
	}

	public override void SetControlDown(SimpleInputControl control)
	{
		Debug.LogError("You cannot set the state of a SumoInput_Keyboard's controls.");
	}

	public override void SetControlUp(SimpleInputControl control)
	{
		Debug.LogError("You cannot set the state of a SumoInput_Keyboard's controls.");
	}
}
