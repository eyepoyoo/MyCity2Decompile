using UnityEngine;

public class BonusTime : MonoBehaviour
{
	private const float SCALE_TWEEN_DUR = 1f;

	private const float MOVE_TWEEN_DUR = 1f;

	private const float MOVE_TWEEN_DELAY = 1.2f;

	public UIWidget riseHeight;

	public UIWidget rootWidget;

	public UILabel numberLabel;

	public UIWidget[] renderableWidgets;

	private Vector3 _startPos;

	private Vector3 _targetPos;

	private float _riseStartTime;

	private float _scaleStartTime;

	private bool _tweening;

	public void OnShowScreen()
	{
		SetObjectsEnabled(false);
	}

	private void SetObjectsEnabled(bool enabled)
	{
		int num = renderableWidgets.Length;
		for (int i = 0; i < num; i++)
		{
			renderableWidgets[i].enabled = enabled;
		}
	}

	public void ShowBonusTime(Vector3 worldPos, float time)
	{
		Camera pCamera = MinigameController._pInstance._pCamera._pCamera;
		Camera pUiCam = ScreenRoot._pInstance._pUiCam;
		if (pCamera == null)
		{
			Debug.Log("Null world Cam");
		}
		Vector3 position = pCamera.WorldToViewportPoint(worldPos);
		position = pUiCam.ViewportToWorldPoint(position);
		position.z = 0f;
		rootWidget.transform.position = position;
		_startPos = rootWidget.transform.position;
		_targetPos = riseHeight.transform.position;
		_riseStartTime = Time.time + 1.2f;
		_scaleStartTime = Time.time;
		_tweening = true;
		if (time <= 60f)
		{
			numberLabel.text = "+00:" + TimeManager.FormatTime(Mathf.CeilToInt(time), false, true, false);
		}
		else
		{
			numberLabel.text = "+" + TimeManager.FormatTime(Mathf.CeilToInt(time), false, true, false);
		}
		SetObjectsEnabled(true);
		rootWidget.alpha = 1f;
	}

	private void Update()
	{
		if (!_tweening)
		{
			return;
		}
		float num = Time.time - _riseStartTime;
		float num2 = Time.time - _scaleStartTime;
		if (num2 < 1f)
		{
			float t = Easing.Ease(Easing.EaseType.EaseOutCircle, num2, 1f, 0f, 1f);
			float arc = GetArc(0f, 1f, 0f, t);
			rootWidget.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(2f, 2f, 2f), arc);
		}
		else
		{
			rootWidget.transform.localScale = Vector3.one;
		}
		if (!(num < 0f))
		{
			if (num < 1f)
			{
				float num3 = Easing.Ease(Easing.EaseType.EaseOutCircle, num, 1f, 0f, 1f);
				rootWidget.transform.position = Vector3.Lerp(_startPos, _targetPos, num3);
				rootWidget.alpha = 1f - num3 * num3;
			}
			else
			{
				SetObjectsEnabled(false);
				_tweening = false;
			}
		}
	}

	private float GetArc(float start, float mid, float end, float t)
	{
		return Mathf.Lerp(Mathf.Lerp(start, mid, t), Mathf.Lerp(mid, end, t), t);
	}

	private float Frac(float v)
	{
		return v - Mathf.Floor(v);
	}
}
