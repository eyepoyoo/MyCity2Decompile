using UnityEngine;

public class ScreenDemoCTA : ScreenBase
{
	private const float _arrowInitialY = 98f;

	public GameObject[] webObjects;

	public GameObject[] mobileObjects;

	public GameObject[] arrows;

	private string platform;

	private float _arrowAnimTimer;

	private float _arrowAnimDistance = 10f;

	private float _arrowSpeed = 5f;

	protected override void OnShowScreen()
	{
		base.OnShowScreen();
		platform = Extensions.GetBootParam("platform", false);
		if (platform == "WEB")
		{
			for (int i = 0; i < webObjects.Length; i++)
			{
				webObjects[i].SetActive(true);
			}
			for (int j = 0; j < mobileObjects.Length; j++)
			{
				mobileObjects[j].SetActive(false);
			}
		}
		if (platform == "MOBILE")
		{
			for (int k = 0; k < webObjects.Length; k++)
			{
				webObjects[k].SetActive(false);
			}
			for (int l = 0; l < mobileObjects.Length; l++)
			{
				mobileObjects[l].SetActive(true);
			}
		}
	}

	protected override void OnScreenShowComplete()
	{
		base.OnScreenShowComplete();
	}

	protected override void Update()
	{
		base.Update();
		_arrowAnimTimer += Time.unscaledDeltaTime * _arrowSpeed;
		float t = (Mathf.Sin(_arrowAnimTimer) + 1f) * 0.5f;
		if (platform == "WEB")
		{
			arrows[0].transform.localPosition = new Vector3(arrows[0].transform.localPosition.x, Mathf.Lerp(98f, 98f - _arrowAnimDistance, t), arrows[0].transform.localPosition.z);
		}
		if (platform == "MOBILE")
		{
			arrows[1].transform.localPosition = new Vector3(arrows[0].transform.localPosition.x, Mathf.Lerp(98f, 98f - _arrowAnimDistance, t), arrows[0].transform.localPosition.z);
			arrows[2].transform.localPosition = new Vector3(arrows[0].transform.localPosition.x, Mathf.Lerp(98f, 98f - _arrowAnimDistance, t), arrows[0].transform.localPosition.z);
		}
	}

	public void OnReplayDemo()
	{
		Navigate("OnReplayDemo");
	}

	public void OnCallToActionButtonWeb()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		string url = Localise("CallToAction.WebLocation");
		Application.OpenURL(url);
	}

	public void OnCallToActionButtonAndroid()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		string url = Localise("CallToAction.AndroidLocation");
		Application.OpenURL(url);
	}

	public void OnCallToActionButtonIOS()
	{
		SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
		string url = Localise("CallToAction.IOSLocation");
		Application.OpenURL(url);
	}
}
