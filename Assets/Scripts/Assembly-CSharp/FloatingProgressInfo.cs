using System.Text;
using UnityEngine;

public class FloatingProgressInfo : MonoBehaviour
{
	public UILabel progressLabel;

	public UITexture brickIcon;

	private bool _active;

	private int _currentBricks;

	private int _maxBricks;

	private StringBuilder _dummySB = new StringBuilder();

	private UIWidget _widgetRef;

	private UIWidget _pWidgetRef
	{
		get
		{
			if (_widgetRef == null)
			{
				_widgetRef = GetComponent<UIWidget>();
			}
			return _widgetRef;
		}
	}

	public bool DoUpdate()
	{
		bool flag = true;
		if (CameraHUB._pInstance._pCurFocusType != CameraHUB.EFocusType.NONE && CameraHUB._pInstance._pCurFocusType != CameraHUB.EFocusType.PAN_ONLY)
		{
			flag = false;
		}
		if (_active && _pWidgetRef.alpha != 1f && flag)
		{
			_pWidgetRef.alpha = Mathf.MoveTowards(_pWidgetRef.alpha, 1f, Time.deltaTime * 15f);
		}
		else if ((!_active || !flag) && _pWidgetRef.alpha != 0f)
		{
			_pWidgetRef.alpha = Mathf.MoveTowards(_pWidgetRef.alpha, 0f, Time.deltaTime * 15f);
		}
		return _pWidgetRef.alpha != 0f && _pWidgetRef.alpha != 1f;
	}

	public void SetInfo(int currentBricks, int maxBricks, Material brickMaterial)
	{
		brickIcon.material = brickMaterial;
		if (currentBricks != _currentBricks || maxBricks != _maxBricks)
		{
			_currentBricks = currentBricks;
			_maxBricks = maxBricks;
			_dummySB.Length = 0;
			_dummySB.Append(_currentBricks);
			_dummySB.Append("/");
			_dummySB.Append(_maxBricks);
			progressLabel.text = _dummySB.ToString();
		}
	}

	public void SetAlpha(float value)
	{
		_pWidgetRef.alpha = value;
	}

	public void SetActive(bool active)
	{
		_active = active;
	}
}
