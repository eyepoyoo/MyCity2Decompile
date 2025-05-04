using System.Collections.Generic;
using UnityEngine;

public class TrackableIcon : MonoBehaviour
{
	public bool _showInFirstFTUE;

	private static List<TrackableIcon> _allKnownIcons = new List<TrackableIcon>();

	private static List<TrackableIcon> _allIcons = new List<TrackableIcon>();

	private static int _numTrackables;

	public string trackableIconName;

	public string trackableIconBacking = "OffScreenMarker";

	public CityManager.REGIONS region;

	protected MinigameIconFX _iconFX;

	private int _numInteractions;

	public static List<TrackableIcon> _pAllIcons
	{
		get
		{
			return _allIcons;
		}
	}

	public static int _pNumTrackables
	{
		get
		{
			return _numTrackables;
		}
	}

	public virtual Vector3 _pRootPos
	{
		get
		{
			return base.transform.position;
		}
	}

	public virtual string _pUniqueId
	{
		get
		{
			return base.gameObject.name;
		}
	}

	public int _pNumInteractions
	{
		get
		{
			return _numInteractions;
		}
		set
		{
			_numInteractions = value;
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.setInt(_pUniqueId, _numInteractions);
			if (_iconFX != null)
			{
				_iconFX.gameObject.SetActive(_numInteractions < 1);
			}
		}
	}

	public static void RefreshAllVisibility()
	{
		_allIcons.RemoveAll((TrackableIcon item) => item == null);
		_numTrackables = _allIcons.Count;
		_allKnownIcons.RemoveAll((TrackableIcon item) => item == null);
		int count = _allKnownIcons.Count;
		for (int num = 0; num < count; num++)
		{
			_allKnownIcons[num].RefreshVisibility();
		}
	}

	public void RefreshVisibility()
	{
		RefreshFX();
		_allIcons.RemoveAll((TrackableIcon item) => item == null);
		_numTrackables = _allIcons.Count;
		bool flag = true;
		if (!GlobalInGameData._pHasDoneFirstTimeVisit)
		{
			flag = _showInFirstFTUE;
		}
		else if (!GlobalInGameData._pHasSeenGarageTutorial)
		{
			if (this is SpecialActionIcon)
			{
				SpecialActionIcon specialActionIcon = this as SpecialActionIcon;
				flag = specialActionIcon.actionType == SpecialActionIcon.EACTION_TYPE.GARAGE;
			}
			else
			{
				flag = false;
			}
		}
		if (flag)
		{
			_allIcons.Add(this);
			_numTrackables = _allIcons.Count;
			base.gameObject.SetActive(true);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	protected virtual void Awake()
	{
		_allKnownIcons.RemoveAll((TrackableIcon item) => item == null);
		_allKnownIcons.Add(this);
		RefreshVisibility();
		_iconFX = base.gameObject.GetComponentInChildren<MinigameIconFX>();
		RefreshFX();
	}

	private void OnDestroy()
	{
		if (_allIcons.Contains(this))
		{
			_allIcons.Remove(this);
			_numTrackables = _allIcons.Count;
		}
		if (_allKnownIcons.Contains(this))
		{
			_allKnownIcons.Remove(this);
		}
	}

	protected virtual void RefreshFX()
	{
		_numInteractions = AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance._pCurrentPlayerData.getInt(_pUniqueId);
		if (_iconFX != null)
		{
			_iconFX.gameObject.SetActive(_numInteractions < 1);
		}
	}
}
