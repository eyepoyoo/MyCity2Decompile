using System;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
	private static string[] axisNames = new string[19]
	{
		"Horizontal", "Vertical", "Fire1", "Fire2", "Fire3", "Jump", "Mouse X", "Mouse Y", "Mouse ScrollWheel", "JoyAxis0",
		"JoyAxis1", "JoyAxis2", "JoyAxis3", "JoyAxis4", "JoyAxis5", "JoyAxis6", "JoyAxis7", "JoyAxis8", "JoyAxis9"
	};

	public List<KeyCode> keysDown = new List<KeyCode>();

	private KeyCode[] allKeys;

	private void Awake()
	{
		allKeys = (KeyCode[])Enum.GetValues(typeof(KeyCode));
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (keysDown == null || allKeys == null || allKeys.Length == 0)
		{
			return;
		}
		keysDown.Clear();
		for (int i = 0; i < allKeys.Length; i++)
		{
			if (Input.GetKey(allKeys[i]))
			{
				keysDown.Add(allKeys[i]);
			}
		}
	}

	private void OnGUI()
	{
		if (axisNames != null && axisNames.Length != 0)
		{
			for (int i = 0; i < axisNames.Length; i++)
			{
				float axis = Input.GetAxis(axisNames[i]);
				if (axis != 0f)
				{
					GUILayout.Label("--> Axis [" + axisNames[i] + "]: Value [" + axis + "]");
				}
			}
		}
		if (keysDown != null && keysDown.Count != 0)
		{
			for (int j = 0; j < keysDown.Count; j++)
			{
				GUILayout.Label("--> KeyCode [" + keysDown[j].ToString() + "]");
			}
		}
	}
}
