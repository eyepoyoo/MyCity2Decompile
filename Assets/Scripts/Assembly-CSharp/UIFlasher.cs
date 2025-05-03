using UnityEngine;

public class UIFlasher : MonoBehaviour
{
	[SerializeField]
	private UIWidget[] _flashingElements;

	[SerializeField]
	private float _flashTime;

	[SerializeField]
	private bool _doFlashScaleX;

	[SerializeField]
	private bool _doFlashScaleY;

	[SerializeField]
	private bool _doFlashScaleZ;

	[SerializeField]
	private float _flashStartScale;

	[SerializeField]
	private float _flashEndScale;

	[SerializeField]
	private float _flashStartAlpha;

	[SerializeField]
	private float _flashEndAlpha;

	[SerializeField]
	private bool _doFlashAlpha;

	[SerializeField]
	private bool _doOnlyOneFlash;

	private float _flashTimer;

	private float _flashRatio;

	private float _currAlpha = 1f;

	private float _currScale;

	private Vector3 _currScaleV3 = Vector3.one;

	private void OnEnable()
	{
		_flashTimer = 0f;
		UpdateFlash();
	}

	private void Update()
	{
		UpdateFlash();
	}

	private void UpdateFlash()
	{
		if (_flashingElements == null || _flashingElements.Length == 0)
		{
			return;
		}
		_flashTimer += Time.deltaTime;
		if (_flashTimer > _flashTime)
		{
			if (_doOnlyOneFlash)
			{
				base.gameObject.SetActive(false);
				return;
			}
			_flashTimer -= _flashTime;
		}
		_flashRatio = Mathf.Clamp01(_flashTimer / _flashTime);
		if (_doFlashAlpha)
		{
			_currAlpha = Mathf.Lerp(_flashStartAlpha, _flashEndAlpha, _flashRatio);
		}
		_currScale = Mathf.Lerp(_flashStartScale, _flashEndScale, _flashRatio);
		if (_doFlashScaleX)
		{
			_currScaleV3.x = _currScale;
		}
		if (_doFlashScaleY)
		{
			_currScaleV3.y = _currScale;
		}
		if (_doFlashScaleZ)
		{
			_currScaleV3.z = _currScale;
		}
		for (int i = 0; i < _flashingElements.Length; i++)
		{
			if (!(_flashingElements[i] == null))
			{
				if (!_doFlashScaleX)
				{
					_currScaleV3.x = _flashingElements[i].transform.localScale.x;
				}
				if (!_doFlashScaleY)
				{
					_currScaleV3.y = _flashingElements[i].transform.localScale.y;
				}
				if (!_doFlashScaleZ)
				{
					_currScaleV3.z = _flashingElements[i].transform.localScale.z;
				}
				if (_doFlashAlpha)
				{
					_flashingElements[i].alpha = _currAlpha;
				}
				if (_doFlashScaleX || _doFlashScaleY || _doFlashScaleZ)
				{
					_flashingElements[i].transform.localScale = _currScaleV3;
				}
			}
		}
	}
}
