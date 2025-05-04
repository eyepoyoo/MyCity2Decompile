using System;
using UnityEngine;

public class ObjectiveProgressBar : MonoBehaviour
{
	public const float MIN_BAR_DECIMAL = 0.06f;

	private const float DECIMAL_FILL_PER_SECOND = 1f;

	private const float RUMBLE_TIME = 0.5f;

	private const float RUMBLE_MOVEMENT = 40f;

	private const float RUMBLE_MOVE_TIME = 0.025f;

	[SerializeField]
	private UIWidget _progressBarContainer;

	[SerializeField]
	private UIWidget _progressBar;

	[SerializeField]
	private UISprite _objectiveSprite;

	private Vector3 _containerStartPos = Vector3.zero;

	private float _targetDecimal;

	private float _tempDelta;

	private float _rumbleTimer;

	private float _rumbleRatio;

	private float _rumbleMoveTimer;

	private float _rumbleMoveRatio;

	private Vector3 _rumbleMoveToLocation;

	private Vector3 _rumbleMoveFromLocation;

	public float _pTargetDecimal
	{
		get
		{
			return _targetDecimal;
		}
	}

	public event Action<ObjectiveProgressBar> _onChanged;

	private void Awake()
	{
		if (_containerStartPos == Vector3.zero)
		{
			_containerStartPos = _progressBarContainer.cachedTransform.localPosition;
		}
	}

	public void ResetProgressBar()
	{
		SetProgressBarDecimal(0f, false, true);
	}

	public void SetProgressBarDecimal(float decimalFill, bool doRumbleIfChanged = false, bool doInstant = false)
	{
		_targetDecimal = Mathf.Clamp01(Mathf.Max(0.06f, decimalFill));
		if (doRumbleIfChanged && _targetDecimal != _progressBar.rightAnchor.relative && _rumbleTimer <= 0f)
		{
			SoundFacade._pInstance.PlayOneShotSFX("GUIBarShake", 0f);
			_rumbleTimer = 0.5f;
		}
		if (doInstant)
		{
			_progressBar.rightAnchor.relative = _targetDecimal;
		}
		if (this._onChanged != null)
		{
			this._onChanged(this);
		}
	}

	private void Update()
	{
		UpdateDecimalFill();
		UpdateRumble();
	}

	private void UpdateRumble()
	{
		if (_rumbleTimer <= 0f)
		{
			return;
		}
		_rumbleTimer -= RealTime.deltaTime;
		if (_rumbleTimer <= 0f)
		{
			_progressBarContainer.cachedTransform.localPosition = _containerStartPos;
			return;
		}
		_rumbleMoveTimer -= RealTime.deltaTime;
		if (_rumbleMoveTimer <= 0f)
		{
			_rumbleRatio = _rumbleTimer / 0.5f;
			_rumbleMoveTimer = 0.025f;
			_rumbleMoveFromLocation = _progressBarContainer.cachedTransform.localPosition;
			Vector2 vector = UnityEngine.Random.insideUnitCircle * 40f * _rumbleRatio;
			_rumbleMoveToLocation = _containerStartPos + new Vector3(vector.x, vector.y, 0f);
		}
		_rumbleMoveRatio = 1f - Mathf.Clamp01(_rumbleMoveTimer / 0.025f);
		_progressBarContainer.cachedTransform.localPosition = Vector3.Lerp(_rumbleMoveFromLocation, _rumbleMoveToLocation, _rumbleMoveRatio);
	}

	private void UpdateDecimalFill()
	{
		if (_progressBar.rightAnchor.relative != _targetDecimal)
		{
			_progressBar.rightAnchor.relative = Mathf.MoveTowards(_progressBar.rightAnchor.relative, _targetDecimal, 1f * Time.unscaledDeltaTime);
			if (this._onChanged != null)
			{
				this._onChanged(this);
			}
		}
	}
}
