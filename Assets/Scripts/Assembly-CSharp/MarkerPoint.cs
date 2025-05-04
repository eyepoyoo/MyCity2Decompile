using System;
using UnityEngine;

public class MarkerPoint : MonoBehaviour
{
	private const float SHOW_HIDE_DURATION = 0.5f;

	private const float ALPHA_SHOWN = 0.5f;

	private const float ACTIVE_DIST_FROM_PLAYER = 100f;

	private const float ACTIVE_DIST_FROM_PLAYER_SQRD = 10000f;

	public Color _colorNormal;

	public Color _colorDanger;

	public Transform _visualContainer;

	public Renderer _renderer;

	public Transform _follow;

	public bool _ignoreFollowY;

	public bool _doFlicker;

	[NonSerialized]
	public Color _tintColor;

	private float _normShown;

	private MaterialPropertyBlock _propertyBlock;

	private bool _isDanger;

	private bool _isActive;

	public bool _pIsActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			if (value != _isActive)
			{
				_isActive = value;
				_visualContainer.gameObject.SetActive(value);
				_normShown = 0f;
				_pAlpha = 0f;
			}
		}
	}

	public bool _pDoShow { get; set; }

	public float _pRadius
	{
		get
		{
			return _visualContainer.localScale.x;
		}
		set
		{
			_visualContainer.localScale = new Vector3(value, 1f, value);
		}
	}

	public bool _pIsDanger
	{
		get
		{
			return _isDanger;
		}
		set
		{
			_isDanger = value;
			_tintColor = ((!value) ? _colorNormal : _colorDanger);
		}
	}

	private float _pAlpha
	{
		set
		{
			_tintColor.a = value;
			_propertyBlock.SetColor("_TintColor", _tintColor);
			_renderer.SetPropertyBlock(_propertyBlock);
		}
	}

	private void Awake()
	{
		_pIsDanger = false;
		_propertyBlock = new MaterialPropertyBlock();
		HideInstant();
	}

	private void Update()
	{
		if (_follow != null)
		{
			Vector3 position = _follow.position;
			if (_ignoreFollowY)
			{
				position.y = base.transform.position.y;
			}
			base.transform.position = position;
		}
		if (_pIsActive = MathHelper.DistXZSqrd(base.transform.position, VehicleController_Player._pInstance.transform.position) < 10000f)
		{
			float num = (_pDoShow ? 1 : 0);
			if (_normShown != num)
			{
				_normShown = Mathf.MoveTowards(_normShown, num, Time.deltaTime / 0.5f);
				_visualContainer.gameObject.SetActive(_normShown > 0f);
				_pAlpha = 0.5f * _normShown;
			}
			if (_doFlicker && _normShown > 0f)
			{
				_pAlpha = 0.5f * _normShown * ((!(Time.time * 10f % 1f < 0.5f)) ? 1f : 0.5f);
			}
		}
	}

	public void HideInstant()
	{
		_pDoShow = false;
		_normShown = 0f;
		_visualContainer.gameObject.SetActive(false);
	}
}
