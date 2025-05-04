using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

[RequireComponent(typeof(GUITexture))]
public class ForcedReset : MonoBehaviour
{
	private void Update()
	{
		if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
		{
			Application.LoadLevelAsync(Application.loadedLevelName);
		}
	}
}
