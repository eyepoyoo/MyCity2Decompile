using UnityEngine;

public class OnGUIForwarder : MonoBehaviour
{
	public delegate void OnGUIFunction();

	public OnGUIFunction onGUIFunctionToForwardTo;

	public void OnGUI()
	{
		if (onGUIFunctionToForwardTo != null)
		{
			onGUIFunctionToForwardTo();
		}
	}
}
