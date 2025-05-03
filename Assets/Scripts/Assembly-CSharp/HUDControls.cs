using System.Collections.Generic;
using UnityEngine;

public class HUDControls : MonoBehaviour
{
	public List<EventDelegate> onPress = new List<EventDelegate>();

	public List<EventDelegate> onUnPress = new List<EventDelegate>();

	private void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			EventDelegate.Execute(onPress);
		}
		else
		{
			EventDelegate.Execute(onUnPress);
		}
	}
}
