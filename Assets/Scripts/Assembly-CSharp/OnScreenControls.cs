using System;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class OnScreenControls : MonoBehaviour
{
	private void Awake()
	{
		CrossPlatformInputManager.RegisterVirtualButton(new CrossPlatformInputManager.VirtualButton("Special"));
	}

	private void OnGUI()
	{
		DrawOnScreenControls();
	}

	private void DrawOnScreenControls()
	{
		CrossPlatformInputManager.SetAxisZero("Horizontal");
		CrossPlatformInputManager.SetAxisZero("Vertical");
		Rect leftButtonRect = new Rect(0f, 0f, 100f, 100f);
		bool flag = Array.Exists(Input.touches, (Touch touch) => leftButtonRect.Contains(touch.position));
		GUI.RepeatButton(InputRectToGUIRect(leftButtonRect), "<");
		Rect rightButtonRect = new Rect(Screen.width - 100, 0f, 100f, 100f);
		bool flag2 = Array.Exists(Input.touches, (Touch touch) => rightButtonRect.Contains(touch.position));
		GUI.RepeatButton(InputRectToGUIRect(rightButtonRect), ">");
		Rect specialButtonRect = new Rect(Screen.width - 100, Screen.height / 2 - 50, 100f, 100f);
		bool flag3 = Array.Exists(Input.touches, (Touch touch) => touch.phase == TouchPhase.Began && specialButtonRect.Contains(touch.position));
		bool flag4 = Array.Exists(Input.touches, (Touch touch) => touch.phase == TouchPhase.Ended);
		if (flag3)
		{
			CrossPlatformInputManager.SetButtonDown("Special");
		}
		if (flag4)
		{
			CrossPlatformInputManager.SetButtonUp("Special");
		}
		GUI.RepeatButton(InputRectToGUIRect(specialButtonRect), "Special");
		if (flag && flag2)
		{
			CrossPlatformInputManager.SetAxisNegative("Vertical");
		}
		else if (flag)
		{
			CrossPlatformInputManager.SetAxisNegative("Horizontal");
		}
		else if (flag2)
		{
			CrossPlatformInputManager.SetAxisPositive("Horizontal");
		}
	}

	private Rect InputRectToGUIRect(Rect rect)
	{
		float t = Mathf.InverseLerp(0f, (float)Screen.height - rect.height, rect.y);
		rect.y = Mathf.Lerp((float)Screen.height - rect.height, 0f, t);
		return rect;
	}
}
