using UnityEngine;

public class Emoticon : MonoBehaviour
{
	private enum ESHOW_STATE
	{
		WAITING_TO_BEGIN = 0,
		TWEENING_PANEL_IN = 1,
		TWEENING_FACE_IN = 2,
		SHOWING = 3,
		TWEENING_OUT = 4,
		COMPLETED = 5
	}

	public UIWidget _mainOrientation;

	public UISprite _face;

	private bool _inUse;

	private Transform _trackerTransform;

	private ESHOW_STATE _currentShowState;

	private Plane _dummyPlane;

	private Camera _gameCameraRef;

	private float _stateStartTime;

	private bool _isPlayer;

	private bool _isCrook;

	public bool _pIsCrook
	{
		get
		{
			return _isCrook;
		}
	}

	public bool _pIsPlayer
	{
		get
		{
			return _isPlayer;
		}
	}

	public bool _pInUse
	{
		get
		{
			return _inUse;
		}
	}

	public Transform _pTrackedTransform
	{
		get
		{
			return _trackerTransform;
		}
	}

	private void Awake()
	{
		_dummyPlane = default(Plane);
	}

	public void OnShowScreen()
	{
		_inUse = false;
		base.gameObject.SetActive(false);
		_currentShowState = ESHOW_STATE.WAITING_TO_BEGIN;
	}

	public void Display(Transform t, EmoticonSystem.EFACE_TYPE faceType, string faceName, UIAtlas atlas, bool isPlayer = false, bool isCrook = false)
	{
		GetCamera();
		_dummyPlane.SetNormalAndPosition(_gameCameraRef.transform.position, _gameCameraRef.transform.forward);
		if (_dummyPlane.GetSide(t.position) && _gameCameraRef.IsPointInFrustrum(t.position, 0f))
		{
			PlayEmoticonSound(faceType);
			_isCrook = isCrook;
			_isPlayer = isPlayer;
			_inUse = true;
			_trackerTransform = t;
			base.gameObject.SetActive(true);
			_face.atlas = atlas;
			_face.spriteName = faceName;
			CalculateFlip();
			ChangeState(ESHOW_STATE.TWEENING_PANEL_IN);
			DoUpdate();
			SoundFacade._pInstance.PlayOneShotSFX("ThoughtBubble", 0f);
		}
	}

	public void PlayEmoticonSound(EmoticonSystem.EFACE_TYPE faceType)
	{
		switch (faceType)
		{
		case EmoticonSystem.EFACE_TYPE.CROOK_DIZZY:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonDizzy", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.CROOK_GRINNING:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonHappy", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.EXPLORER_DIZZY:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonDizzyNPC", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.EXPLORER_HAPPY:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonHappy", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.EXPLORER_OVERJOYED:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonOverjoyed", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.FIREMAN_DIZZY:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonDizzyNPC", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.FIREMAN_HAPPY:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonHappy", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.FIREMAN_OVERJOYED:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonOverjoyed", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.NPC_DIZZY:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonDizzyNPC", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.NPC_HAPPY:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonHappy", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.NPC_WORRIED:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonWorried", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.POLICE_DETERMINED:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonDetermined", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.POLICE_DIZZY:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonDizzyNPC", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.POLICE_HAPPY:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonHappy", 0f);
			break;
		case EmoticonSystem.EFACE_TYPE.POLICE_OVERJOYED:
			SoundFacade._pInstance.PlayOneShotSFX("EmoticonOverjoyed", 0f);
			break;
		}
	}

	public void DoUpdate()
	{
		GetCamera();
		_dummyPlane.SetNormalAndPosition(_gameCameraRef.transform.position, _gameCameraRef.transform.forward);
		if (!_dummyPlane.GetSide(_trackerTransform.position) || !_gameCameraRef.IsPointInFrustrum(_trackerTransform.position, 0f))
		{
			_mainOrientation.transform.localScale = Vector3.zero;
			ChangeState(ESHOW_STATE.COMPLETED);
			_inUse = false;
			base.gameObject.SetActive(false);
			return;
		}
		Vector3 position = _gameCameraRef.WorldToViewportPoint(_trackerTransform.position);
		position = ScreenRoot._pInstance._pUiCam.ViewportToWorldPoint(position);
		position.z = 0f;
		base.transform.position = position;
		switch (_currentShowState)
		{
		case ESHOW_STATE.TWEENING_PANEL_IN:
			UpdateTweenPanelIn();
			break;
		case ESHOW_STATE.TWEENING_FACE_IN:
			UpdateTweenFaceIn();
			break;
		case ESHOW_STATE.SHOWING:
			UpdateShowing();
			break;
		case ESHOW_STATE.TWEENING_OUT:
			UpdateTweenOut();
			break;
		}
	}

	private void UpdateTweenPanelIn()
	{
		float num = Time.time - _stateStartTime;
		_face.transform.localScale = Vector3.zero;
		if (num < 0.15f)
		{
			float t = Easing.Ease(Easing.EaseType.EaseOut, num, 0.15f, 0f, 1f);
			_mainOrientation.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, t);
		}
		else
		{
			_mainOrientation.transform.localScale = Vector3.one;
			ChangeState(ESHOW_STATE.TWEENING_FACE_IN);
		}
	}

	private void UpdateTweenFaceIn()
	{
		float num = Time.time - _stateStartTime;
		if (num < 0.2f)
		{
			float t = Easing.Ease(Easing.EaseType.EaseOutBack, num, 0.2f, 0f, 1f);
			_face.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, t);
		}
		else
		{
			_face.transform.localScale = Vector3.one;
			ChangeState(ESHOW_STATE.SHOWING);
		}
	}

	private void UpdateShowing()
	{
		float num = Time.time - _stateStartTime;
		if (num >= 1f)
		{
			ChangeState(ESHOW_STATE.TWEENING_OUT);
		}
	}

	private void UpdateTweenOut()
	{
		float num = Time.time - _stateStartTime;
		_face.transform.localScale = Vector3.zero;
		if (num < 0.2f)
		{
			float t = Easing.Ease(Easing.EaseType.EaseIn, num, 0.2f, 0f, 1f);
			_mainOrientation.transform.localScale = Vector3.LerpUnclamped(Vector3.one, Vector3.zero, t);
			return;
		}
		_mainOrientation.transform.localScale = Vector3.zero;
		ChangeState(ESHOW_STATE.COMPLETED);
		_inUse = false;
		base.gameObject.SetActive(false);
	}

	private void ChangeState(ESHOW_STATE newState)
	{
		_stateStartTime = Time.time;
		_currentShowState = newState;
	}

	private void CalculateFlip()
	{
		GetCamera();
		Vector3 vector = _gameCameraRef.WorldToViewportPoint(_trackerTransform.position);
		if (vector.y > 0.5f && vector.x > 0.5f)
		{
			_mainOrientation.transform.localEulerAngles = new Vector3(0f, 0f, -135f);
			_face.transform.localEulerAngles = new Vector3(0f, 0f, 135f);
		}
		else if (vector.y <= 0.5f && vector.x > 0.5f)
		{
			_mainOrientation.transform.localEulerAngles = new Vector3(0f, 0f, -45f);
			_face.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
		}
		else if (vector.y > 0.5f && vector.x <= 0.5f)
		{
			_mainOrientation.transform.localEulerAngles = new Vector3(0f, 0f, 135f);
			_face.transform.localEulerAngles = new Vector3(0f, 0f, -135f);
		}
		else if (vector.y <= 0.5f && vector.x <= 0.5f)
		{
			_mainOrientation.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
			_face.transform.localEulerAngles = new Vector3(0f, 0f, -45f);
		}
	}

	private void GetCamera()
	{
		if (!(_gameCameraRef != null) && !(MinigameController._pInstance._pCamera == null))
		{
			Camera component = MinigameController._pInstance._pCamera.GetComponent<Camera>();
			_gameCameraRef = component;
		}
	}
}
