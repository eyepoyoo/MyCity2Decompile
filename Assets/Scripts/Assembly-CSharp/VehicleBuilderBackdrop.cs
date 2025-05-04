using UnityEngine;

public class VehicleBuilderBackdrop : AmuzoMonoSingleton<VehicleBuilderBackdrop>
{
	private const float TRANSITION_TIME = 0.5f;

	[SerializeField]
	private UIWidget _vehiclePartLightStrip;

	public UIWidget _skylineRaisedPos;

	public UIWidget _skylineNormalPos;

	public GameObject _magnifyBox;

	public GameObject _grid;

	public GameObject _regularBuildBackdrop;

	public GameObject _garageBackdrop;

	private float _stripDefaultAlpha;

	private float _targetAlpha;

	private float _tempDelta;

	private bool _isTweeningSkyline;

	private Vector3 _skylineTweenStart;

	private Vector3 _skylineTweenEnd;

	private float _tweenStartTime;

	protected override void Initialise()
	{
		base.Initialise();
		_stripDefaultAlpha = _vehiclePartLightStrip.alpha;
		_targetAlpha = _stripDefaultAlpha;
	}

	public void TweenSkylineUp()
	{
		_skylineTweenStart = _regularBuildBackdrop.transform.position;
		_skylineTweenEnd = _skylineRaisedPos.transform.position;
		_isTweeningSkyline = true;
		_tweenStartTime = RealTime.time;
	}

	public void TweenSkylineNormal()
	{
		_skylineTweenStart = _regularBuildBackdrop.transform.position;
		_skylineTweenEnd = _skylineNormalPos.transform.position;
		_isTweeningSkyline = true;
		_tweenStartTime = RealTime.time;
	}

	public void UpdateSkylineTween(float t)
	{
		_regularBuildBackdrop.transform.position = Vector3.Lerp(_skylineTweenStart, _skylineTweenEnd, t);
	}

	public void StopTweeningSkyline()
	{
		_regularBuildBackdrop.transform.position = _skylineTweenEnd;
		_isTweeningSkyline = false;
	}

	public void Prepare(bool isGarage)
	{
		_grid.SetActive(!isGarage);
		_regularBuildBackdrop.SetActive(!isGarage);
		_garageBackdrop.SetActive(isGarage);
		_regularBuildBackdrop.transform.position = _skylineNormalPos.transform.position;
	}

	public void ShowStrip(bool doImmediate = true)
	{
		Debug.Log("ShowStrip()");
		if (doImmediate)
		{
			_vehiclePartLightStrip.gameObject.SetActive(true);
			_vehiclePartLightStrip.alpha = _stripDefaultAlpha;
			_targetAlpha = _stripDefaultAlpha;
		}
		else
		{
			_targetAlpha = _stripDefaultAlpha;
		}
	}

	public void HideStrip(bool doImmediate = true)
	{
		Debug.Log("HideStrip()");
		if (doImmediate)
		{
			_vehiclePartLightStrip.gameObject.SetActive(false);
		}
		else
		{
			_targetAlpha = 0f;
		}
	}

	private void Update()
	{
		if (_isTweeningSkyline)
		{
			float num = RealTime.time - _tweenStartTime;
			if (num < 1f)
			{
				float t = Easing.Ease(Easing.EaseType.EaseInOut, num, 1f, 0f, 1f);
				UpdateSkylineTween(t);
			}
			else
			{
				StopTweeningSkyline();
			}
		}
		if (_vehiclePartLightStrip.alpha != _targetAlpha)
		{
			_tempDelta = _stripDefaultAlpha / 0.5f * RealTime.deltaTime;
			if (Mathf.Abs(_targetAlpha - _vehiclePartLightStrip.alpha) < _tempDelta)
			{
				_vehiclePartLightStrip.alpha = _targetAlpha;
			}
			else
			{
				_vehiclePartLightStrip.alpha = Mathf.MoveTowards(_vehiclePartLightStrip.alpha, _targetAlpha, _tempDelta);
			}
		}
	}
}
