using UnityEngine;

public class ScreenRoot : ScreenBase
{
	private static ScreenRoot _Instance;

	private Camera _uiCam;

	private UICamera _nguiCam;

	public static ScreenRoot _pInstance
	{
		get
		{
			return _Instance;
		}
	}

	public Camera _pUiCam
	{
		get
		{
			return _uiCam ?? (_uiCam = GetComponentInChildren<Camera>());
		}
	}

	public UICamera _pNGUICam
	{
		get
		{
			return _nguiCam ?? (_nguiCam = GetComponentInChildren<UICamera>());
		}
	}

	private void Awake()
	{
		_Instance = this;
	}

	public void EnableMultiTouch(bool shouldEnable)
	{
		_pNGUICam.allowMultiTouch = shouldEnable;
	}
}
