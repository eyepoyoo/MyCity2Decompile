using System;
using UnityEngine;

public abstract class MinigameObjective : MonoBehaviour
{
	public int _studBonus;

	public bool _awardStudsProgressively;

	public float _timeBonus;

	public MinigameObjective _subObjective;

	public bool _playsSoundOnComplete;

	public Transform _spawnStudsFrom;

	public MarkerPoint _markerPoint;

	public bool _alwaysShowDirectorArrow;

	public bool _showDirectorArrowUntilComplete;

	private int _numStudsAwardedProgressively;

	private bool _enabled;

	private Vector3 _initPos;

	private Quaternion _initRot;

	private int _initLayer;

	public bool _pCompleted { get; private set; }

	public abstract float _pNormProgress { get; }

	public Vector3 _pPositionOnEntry { get; protected set; }

	public bool _pEnabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			if (value == _enabled)
			{
				return;
			}
			_enabled = value;
			if (_enabled)
			{
				if (this._onEnabled != null)
				{
					this._onEnabled(this);
				}
				OnObjectiveEnabled();
			}
			else
			{
				if (this._onDisabled != null)
				{
					this._onDisabled(this);
				}
				OnObjectiveDisabled();
			}
		}
	}

	public bool _pIsLockingGame
	{
		get
		{
			return _pLockGameWhileInProgress && _pNormProgress > 0f && _pNormProgress < 1f;
		}
	}

	public virtual bool _pLockGameWhileInProgress
	{
		get
		{
			return false;
		}
	}

	public bool _pForceShowDirectorArrow
	{
		get
		{
			return _alwaysShowDirectorArrow || _showDirectorArrowUntilComplete;
		}
	}

	public event Action<MinigameObjective, float> _onProgress;

	public event Action<MinigameObjective> _onComplete;

	public event Action<MinigameObjective> _onFail;

	public event Action<MinigameObjective, bool> _onReset;

	public event Action<MinigameObjective> _onEnabled;

	public event Action<MinigameObjective> _onDisabled;

	protected virtual void Awake()
	{
		_pPositionOnEntry = base.transform.position;
		if (_markerPoint != null)
		{
			_markerPoint._pRadius = 5f;
		}
		_initPos = base.transform.position;
		_initRot = base.transform.rotation;
		_initLayer = base.gameObject.layer;
	}

	public virtual void Reset(bool toInitialState = false)
	{
		_pCompleted = false;
		base.gameObject.SetActive(true);
		if (toInitialState)
		{
			_numStudsAwardedProgressively = 0;
			_showDirectorArrowUntilComplete = false;
			base.transform.position = _initPos;
			base.transform.rotation = _initRot;
			Rigidbody component = GetComponent<Rigidbody>();
			if ((bool)component)
			{
				component.velocity = Vector3.zero;
				component.angularVelocity = Vector3.zero;
				if ((bool)GetComponent<AutoSleep>())
				{
					GetComponent<AutoSleep>().Invoke("Sleep", 0.1f);
				}
			}
			base.gameObject.layer = _initLayer;
			UnityEngine.Object.Destroy(GetComponent<TimedDestroyer>());
		}
		if (this._onReset != null)
		{
			this._onReset(this, toInitialState);
		}
	}

	protected void Progress()
	{
		if (_awardStudsProgressively)
		{
			int num = Mathf.FloorToInt(_pNormProgress * (float)_studBonus);
			while (_numStudsAwardedProgressively < num)
			{
				MinigameController._pInstance.AwardFloatingStud(((!(_spawnStudsFrom != null)) ? base.transform : _spawnStudsFrom.transform).position, Quaternion.identity);
				_numStudsAwardedProgressively++;
			}
		}
		if (this._onProgress != null)
		{
			this._onProgress(this, _pNormProgress);
		}
	}

	protected void Complete()
	{
		_pCompleted = true;
		_showDirectorArrowUntilComplete = false;
		if (this._onComplete != null)
		{
			this._onComplete(this);
		}
	}

	protected void Fail()
	{
		if (this._onFail != null)
		{
			this._onFail(this);
		}
	}

	protected virtual void OnObjectiveEnabled()
	{
		if (_markerPoint != null)
		{
			_markerPoint._pDoShow = true;
		}
	}

	protected virtual void OnObjectiveDisabled()
	{
		if (_markerPoint != null)
		{
			_markerPoint._pDoShow = false;
		}
	}

	public void CancelSubObjectiveAndReenable()
	{
		Reset();
		if ((bool)_subObjective && _subObjective == MinigameController._pInstance._pMinigame._pOverridingObjective)
		{
			MinigameController._pInstance._pMinigame.CancelOverridingObjective();
		}
		Invoke("AddToMinigame", (!GetComponent<Pickupable>()) ? 0f : (GetComponent<Pickupable>()._dropPickupCooldown - 0.5f));
	}

	private void AddToMinigame()
	{
		MinigameController._pInstance._pMinigame.SetObjectiveEnabled(this, true);
		MinigameController._pInstance._pMinigame._remainingObjectives.Add(this);
	}
}
