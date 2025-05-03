using UnityEngine;

public class FullScreenGUITexture : MonoBehaviour
{
	private void Awake()
	{
		GUITexture component = GetComponent<GUITexture>();
		Rect pixelInset = component.pixelInset;
		float num = pixelInset.width / pixelInset.height;
		float num2 = Screen.height;
		float num3 = num2 * num;
		float x = (0f - num3) * 0.5f;
		float y = (0f - num2) * 0.5f;
		component.pixelInset = new Rect(x, y, num3, num2);
	}
}
