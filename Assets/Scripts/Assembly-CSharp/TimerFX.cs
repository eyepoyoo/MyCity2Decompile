using UnityEngine;

[ExecuteInEditMode]
public class TimerFX : MonoBehaviour
{
	public UILabel labelRef;

	public Transform clockTransform;

	public UILabel lastDigitRef;

	public Vector3[] digitOffsets;

	public Vector3 normalDigitScale;

	public Vector3 largeDigitScale;

	public float _timeVal;

	public Color normalColor;

	public Color warningColor;

	public Vector3 clockScaleUp;

	public Vector3 mainScaleUp;

	public float mainScaleSpeed = 5f;

	private bool _isPlayingBeep;

	public void SetTime(float t)
	{
		_timeVal = t;
	}

	private void Update()
	{
		if (labelRef == null || clockTransform == null || lastDigitRef == null)
		{
			return;
		}
		if (IsFlashTime())
		{
			float num = Frac(_timeVal);
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, mainScaleUp, Time.deltaTime * mainScaleSpeed);
			float num2 = Frac(num * 3f);
			labelRef.color = ((!(num2 < 0.5f)) ? warningColor : normalColor);
			lastDigitRef.enabled = false;
		}
		else if (_timeVal <= 9.5f && _timeVal > 0f)
		{
			lastDigitRef.enabled = true;
			float num3 = Frac(_timeVal);
			lastDigitRef.text = labelRef.text.Substring(labelRef.text.Length - 1, 1);
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, mainScaleUp, Time.deltaTime * mainScaleSpeed);
			if (num3 < 0.5f)
			{
				switch (lastDigitRef.text)
				{
				case "0":
					lastDigitRef.text = "9";
					lastDigitRef.transform.localPosition = digitOffsets[9];
					break;
				case "1":
					lastDigitRef.text = "0";
					lastDigitRef.transform.localPosition = digitOffsets[0];
					break;
				case "2":
					lastDigitRef.text = "1";
					lastDigitRef.transform.localPosition = digitOffsets[1];
					break;
				case "3":
					lastDigitRef.text = "2";
					lastDigitRef.transform.localPosition = digitOffsets[2];
					break;
				case "4":
					lastDigitRef.text = "3";
					lastDigitRef.transform.localPosition = digitOffsets[3];
					break;
				case "5":
					lastDigitRef.text = "4";
					lastDigitRef.transform.localPosition = digitOffsets[4];
					break;
				case "6":
					lastDigitRef.text = "5";
					lastDigitRef.transform.localPosition = digitOffsets[5];
					break;
				case "7":
					lastDigitRef.text = "6";
					lastDigitRef.transform.localPosition = digitOffsets[6];
					break;
				case "8":
					lastDigitRef.text = "7";
					lastDigitRef.transform.localPosition = digitOffsets[7];
					break;
				case "9":
					lastDigitRef.text = "8";
					lastDigitRef.transform.localPosition = digitOffsets[8];
					break;
				}
				num3 *= 2f;
				lastDigitRef.transform.localScale = Vector3.Lerp(normalDigitScale, largeDigitScale, num3);
				lastDigitRef.color = normalColor;
				lastDigitRef.alpha = Easing.Ease(Easing.EaseType.EaseInSine, 1f - num3, 1f, 0f, 1f);
				labelRef.color = normalColor;
				_isPlayingBeep = false;
				return;
			}
			switch (lastDigitRef.text)
			{
			case "0":
				lastDigitRef.transform.localPosition = digitOffsets[0];
				break;
			case "1":
				lastDigitRef.transform.localPosition = digitOffsets[1];
				break;
			case "2":
				lastDigitRef.transform.localPosition = digitOffsets[2];
				break;
			case "3":
				lastDigitRef.transform.localPosition = digitOffsets[3];
				break;
			case "4":
				lastDigitRef.transform.localPosition = digitOffsets[4];
				break;
			case "5":
				lastDigitRef.transform.localPosition = digitOffsets[5];
				break;
			case "6":
				lastDigitRef.transform.localPosition = digitOffsets[6];
				break;
			case "7":
				lastDigitRef.transform.localPosition = digitOffsets[7];
				break;
			case "8":
				lastDigitRef.transform.localPosition = digitOffsets[8];
				break;
			case "9":
				lastDigitRef.transform.localPosition = digitOffsets[9];
				break;
			}
			num3 = (num3 - 0.5f) * 2f;
			lastDigitRef.transform.localScale = Vector3.Lerp(largeDigitScale, normalDigitScale, num3);
			lastDigitRef.color = warningColor;
			lastDigitRef.alpha = Easing.Ease(Easing.EaseType.EaseInSine, num3, 1f, 0f, 1f);
			labelRef.color = warningColor;
			clockTransform.localScale = Vector3.Lerp(Vector3.one, clockScaleUp, GetArc(0f, 1f, 0f, num3));
			if (!_isPlayingBeep)
			{
				if (_timeVal > 5.5f)
				{
					SoundFacade._pInstance.PlayOneShotSFX("GUITimeTen", 0.05f);
					_isPlayingBeep = true;
				}
				else
				{
					SoundFacade._pInstance.PlayOneShotSFX("GUITimeFive", 0.05f);
					_isPlayingBeep = true;
				}
			}
		}
		else if (_timeVal <= 0f)
		{
			lastDigitRef.enabled = false;
		}
		else
		{
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, Vector3.one, Time.deltaTime * mainScaleSpeed);
			lastDigitRef.enabled = false;
		}
	}

	private bool IsFlashTime()
	{
		return Mathf.Ceil(_timeVal) % 10f == 0f || (_timeVal < 30f && _timeVal > 9.5f);
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
