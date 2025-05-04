using UnityEngine;

public class DebugCamera : MonoBehaviour
{
	private void OnGUI()
	{
		Camera component = GetComponent<Camera>();
		if (component != null)
		{
			if (GUILayout.Button("Skybox"))
			{
				component.clearFlags = CameraClearFlags.Skybox;
			}
			if (GUILayout.Button("Depth"))
			{
				component.clearFlags = CameraClearFlags.Depth;
			}
			if (GUILayout.Button("Nothing"))
			{
				component.clearFlags = CameraClearFlags.Nothing;
			}
			if (GUILayout.Button("SolidColor"))
			{
				component.clearFlags = CameraClearFlags.Color;
			}
		}
	}
}
