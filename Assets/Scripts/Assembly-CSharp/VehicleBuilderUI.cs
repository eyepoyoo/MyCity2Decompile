using UnityEngine;

[RequireComponent(typeof(VehicleBuilder))]
public class VehicleBuilderUI : MonoBehaviour
{
	private const float HEIGHT_THRESHOLD = -5f;

	public bool _doDrawCarouselRotationButtons;

	public bool _doDrawCarouselSelectionButtons;

	public bool _drawGUI;

	private VehicleBuilder _vehicleBuilder;

	private bool _isMovingDown;

	private bool _isMovingUp;

	private bool _hasDoneUp;

	private PlanarTracker _selectionTracker;

	private PlanarTracker _carouselTracker;

	private Rect _carouselRect = new Rect(0.2f, 0.6f, 0.6f, 0.4f);

	private Rect _selectionRect = new Rect(0.35f, 0.3f, 0.3f, 0.5f);

	private float _viewportHeightToIndex = 0.21f;

	private float _screenWidthToViewportWidth = 1f / (float)Screen.width;

	private float _screenHeightToViewportWidth = 1f / (float)Screen.height;

	private bool _canPlayMovedSFX = true;

	private bool _canPlayPickupSFX = true;

	private bool _isPointerOverUI = true;

	public bool _pIsPointerOverUI
	{
		get
		{
			return _isPointerOverUI;
		}
		set
		{
			_isPointerOverUI = value;
		}
	}

	private void Awake()
	{
		_vehicleBuilder = GetComponent<VehicleBuilder>();
		_selectionTracker = new PlanarTracker();
		_selectionTracker._doRequireMouseDown = true;
		_selectionTracker._pViewportTrackingRect = _selectionRect;
		_carouselTracker = new PlanarTracker();
		_carouselTracker._doRequireMouseDown = true;
		_carouselTracker._pViewportTrackingRect = _carouselRect;
	}

	private void OnEnable()
	{
		UICamera.fallThrough = base.gameObject;
		_isPointerOverUI = true;
	}

	private void OnDisable()
	{
		UICamera.fallThrough = null;
	}

	private void Update()
	{
		if (!ScreenCustomiseVehicle._pInstance._isInInfoPanel && !ScreenCustomiseVehicle._pInstance._isInTutorial && !ScreenGarage._pInstance._isInInfoPanel)
		{
			UpdateTrackers();
			UpdatePartCarousel();
			UpdatePartSelection();
			UpdatePartReturn();
		}
	}

	private void OnPress(bool isPressed)
	{
		_isPointerOverUI = !isPressed;
	}

	private void UpdateTrackers()
	{
		_carouselTracker.Update();
		_selectionTracker.Update();
	}

	private void UpdatePartSelection()
	{
		if (_isMovingUp || _vehicleBuilder._pCurrCarousel == null || !_selectionTracker._pIsTracking)
		{
			_isMovingDown = false;
			_canPlayPickupSFX = true;
		}
		else
		{
			if (!_vehicleBuilder._pCurrCarousel._pHasItems)
			{
				return;
			}
			if (_vehicleBuilder._pCurrCarousel._pCurrItem._pHeightManualOverride < -5f)
			{
				_vehicleBuilder.SelectCurrentPiece();
				if (_canPlayMovedSFX)
				{
					SoundFacade._pInstance.PlayOneShotSFX("GUINext", 0f);
					_canPlayMovedSFX = false;
				}
				return;
			}
			if (_selectionTracker._pCurrentDiff.y < -5f)
			{
				_isMovingDown = true;
				_canPlayMovedSFX = true;
			}
			if (_selectionTracker._pInputID > -1 && _canPlayPickupSFX)
			{
				SoundFacade._pInstance.PlayOneShotSFX("GUIPop", 0f);
				_canPlayPickupSFX = false;
			}
			if (!_vehicleBuilder._pCurrCarousel._pCurrItem._pIsSilhouetted)
			{
				_vehicleBuilder._pCurrCarousel._pCurrItem.SetHeightOverride(Mathf.Min(0f, _vehicleBuilder._pCurrCarousel._pCurrItem._pHeightManualOverride - _selectionTracker._pDeltaPos.y * _screenHeightToViewportWidth / _viewportHeightToIndex * _vehicleBuilder._pCurrCarousel.itemSpacing * 0.7f));
			}
		}
	}

	private void UpdatePartCarousel()
	{
		if (!(_vehicleBuilder._pCurrCarousel == null) && _carouselTracker._pIsTracking)
		{
			float easedIndexOveride = _vehicleBuilder._pCurrCarousel._pCurrEasedIndex - _carouselTracker._pDeltaPos.x * _screenWidthToViewportWidth / _viewportHeightToIndex;
			_vehicleBuilder._pCurrCarousel.SetEasedIndexOveride(easedIndexOveride);
		}
	}

	private void UpdatePartReturn()
	{
		if (!MathHelper.IsWholeNumber(_vehicleBuilder._pCurrCarouselIndex))
		{
			return;
		}
		if (_isMovingDown || !_selectionTracker._pIsTracking)
		{
			_isMovingUp = false;
			_hasDoneUp = false;
		}
		else if (!_hasDoneUp)
		{
			if (_selectionTracker._pCurrentDiff.y > 5f)
			{
				_isMovingUp = true;
			}
			if (_selectionTracker._pCurrentDiff.y > 120f)
			{
				_vehicleBuilder.ReturnCurrentPiece();
				_hasDoneUp = true;
			}
		}
	}
}
