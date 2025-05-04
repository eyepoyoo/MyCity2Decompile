using UnityEngine;

public class Clock : MonoBehaviour
{
	[SerializeField]
	private UISprite radialRevealSprite;

	[SerializeField]
	private UIWidget rotationalWidget;

	public void SetClockDecimal(float decimalProgress)
	{
		decimalProgress = Mathf.Clamp01(decimalProgress);
		if (rotationalWidget != null)
		{
			rotationalWidget.transform.localEulerAngles = new Vector3(0f, 0f, (0f - decimalProgress) * 360f);
		}
		if (radialRevealSprite != null)
		{
			radialRevealSprite.fillAmount = decimalProgress;
		}
	}
}
