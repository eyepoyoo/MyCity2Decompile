using UnityEngine;

public class FloatingNewIcon : MonoBehaviour
{
	private float Y_POS = 490f;

	private UIWidget _newIconWidget;

	private VehiclePart _partToTrack;

	private Transform _transform;

	private Vector3 _tempPos;

	public bool _pIsTracking
	{
		get
		{
			return _partToTrack != null;
		}
	}

	public void TrackPart(VehiclePart partToTrack)
	{
		if (_transform == null)
		{
			_transform = base.gameObject.transform;
			_newIconWidget = base.gameObject.GetComponent<UIWidget>();
		}
		_partToTrack = partToTrack;
		base.gameObject.SetActive(true);
	}

	public void StopTrackingTransform()
	{
		_partToTrack = null;
		base.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (!(_partToTrack == null))
		{
			_newIconWidget.alpha = ((!(_partToTrack.transform.localScale.z > 0.01f)) ? 0.005f : 1f);
			_transform.OverlayPosition(_partToTrack.transform.position, Camera.main, ScreenRoot._pInstance._pUiCam);
			_tempPos = _transform.localPosition;
			_tempPos.z = 0f;
			_tempPos.y = Y_POS;
			_transform.localPosition = _tempPos;
		}
	}
}
