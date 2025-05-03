using System;
using System.Collections.Generic;
using UnityEngine;

public class AmuzoSaveDataFacade : AmuzoMonoInitSingleton<AmuzoSaveDataFacade>
{
	private const string LOG_PREFIX = "AmuzoSaveDataFacade: ";

	private const string DEFAULT_PLAYER_SAVE_ID = "DefaultPlayer";

	private const string DEFAULT_SETTINGS_SAVE_ID = "DefaultSettings";

	private static bool DO_DEBUG;

	public int _currentSavedDataVersion = 1;

	public SavedDataSource_Local _localSaveSource;

	public SavedDataSource_AmuzoRemote _amuzoSaveSource;

	public SavedDataSource_LEGORemote _legoSaveSource;

	private List<SaveDataSource> _allActiveSaveSoruces;

	private List<SaveDataGroup> _allSavedData = new List<SaveDataGroup>();

	private string _currentPlayerId = "DefaultPlayer";

	private SaveDataGroup _currentPlayerData;

	private string _currentSystemSettingsId = "DefaultSettings";

	private SaveDataGroup _currentSystemSettings;

	private float _initFinishedTime;

	public SaveDataGroup _pCurrentPlayerData
	{
		get
		{
			if (_currentPlayerData == null)
			{
				_currentPlayerData = GetSaveGroup(_currentPlayerId);
			}
			return _currentPlayerData;
		}
	}

	public SaveDataGroup _pCurrentSystemSettings
	{
		get
		{
			if (_currentSystemSettings == null)
			{
				_currentSystemSettings = GetSaveGroup(_currentSystemSettingsId);
			}
			return _currentSystemSettings;
		}
	}

	public override void startInitialising()
	{
		if (DO_DEBUG)
		{
			Debug.Log("AmuzoSaveDataFacade: Initialising...");
		}
		_currentState = InitialisationState.INITIALISING;
		SetUpActiveSaveSources();
		LoadAllSavedData();
	}

	public void ChangeCurrentPlayerSaveId(string newSaveId, bool doCopyCurrentDataOver = false, bool doLoadSaveDataIfItExists = true)
	{
		if (doLoadSaveDataIfItExists)
		{
			int i = 0;
			for (int count = _allActiveSaveSoruces.Count; i < count; i++)
			{
				_allActiveSaveSoruces[i].LoadData(new string[1] { newSaveId }, OnSourceLoadComplete);
			}
		}
		SaveDataGroup pCurrentPlayerData = _pCurrentPlayerData;
		_currentPlayerData = null;
		_currentPlayerId = newSaveId;
		if (doCopyCurrentDataOver)
		{
			_pCurrentPlayerData.cloneGroup(pCurrentPlayerData);
		}
	}

	public void ChangeCurrentSettingsSaveId(string newSettingsId, bool doCopyCurrentDataOver = false, bool doLoadSaveDataIfItExists = true)
	{
		if (doLoadSaveDataIfItExists)
		{
			int i = 0;
			for (int count = _allActiveSaveSoruces.Count; i < count; i++)
			{
				_allActiveSaveSoruces[i].LoadData(new string[1] { newSettingsId }, OnSourceLoadComplete);
			}
		}
		SaveDataGroup pCurrentSystemSettings = _pCurrentSystemSettings;
		_currentSystemSettings = null;
		_currentSystemSettingsId = newSettingsId;
		if (doCopyCurrentDataOver)
		{
			_pCurrentSystemSettings.cloneGroup(pCurrentSystemSettings);
		}
	}

	public void DeleteAll()
	{
		for (int num = _allSavedData.Count - 1; num >= 0; num--)
		{
			DeleteSaveGroup(_allSavedData[num].SaveGroupId);
		}
	}

	public void SaveAll()
	{
		int i = 0;
		for (int count = _allActiveSaveSoruces.Count; i < count; i++)
		{
			_allActiveSaveSoruces[i].SaveData(_allSavedData, OnSaveComplete);
		}
	}

	public void DebugLogCurrentPlayerSavedData()
	{
		Debug.Log(_pCurrentPlayerData.getDebugString());
	}

	public void DebugLogSavedData()
	{
		for (int num = _allSavedData.Count - 1; num >= 0; num--)
		{
			Debug.Log(_allSavedData[num].getDebugString());
		}
	}

	public void DebugDumpSavedData()
	{
	}

	protected virtual int CompareSavedDataGroups(SaveDataGroup groupA, SaveDataGroup groupB)
	{
		return groupA._lastSaveTime.CompareTo(groupB._lastSaveTime);
	}

	protected virtual SaveDataGroup MergeSavedDataGroups(SaveDataGroup groupA, SaveDataGroup groupB)
	{
		SaveDataGroup saveDataGroup = ((CompareSavedDataGroups(groupA, groupB) != -1) ? groupA : groupB);
		if (DO_DEBUG)
		{
			Debug.Log("AmuzoSaveDataFacade: Comparing :");
			Debug.Log("AmuzoSaveDataFacade: SaveGroupA:" + groupA.getDebugString());
			Debug.Log("AmuzoSaveDataFacade: SaveGroupB:" + groupB.getDebugString());
			Debug.Log("AmuzoSaveDataFacade: SaveGroup" + ((saveDataGroup != groupA) ? "B" : "A") + " was chosen as better.");
		}
		return saveDataGroup;
	}

	protected virtual SaveDataGroup UpdateSavedDataGroup(SaveDataGroup oldGroup)
	{
		if (DO_DEBUG)
		{
			Debug.Log("AmuzoSaveDataFacade: Save data found with old version number! Save version [" + oldGroup._savedDataVersion + "]. Current Version [" + _currentSavedDataVersion + "]");
		}
		oldGroup._savedDataVersion = _currentSavedDataVersion;
		return oldGroup;
	}

	public virtual void OnSaveDataChanged(SaveDataGroup group, SaveDataEntry entry)
	{
		SaveAll();
	}

	private void OnSaveComplete(SaveDataSource sourceThatSaved, bool didSaveSuccessfully)
	{
		if (DO_DEBUG)
		{
			Debug.Log("AmuzoSaveDataFacade: SaveDataSource [" + sourceThatSaved.GetType().ToString() + "] " + ((!didSaveSuccessfully) ? "failed to save." : "saved successfully."));
		}
	}

	private void SetUpActiveSaveSources()
	{
		_allActiveSaveSoruces = new List<SaveDataSource>();
		if (_localSaveSource.doUseSaveSource)
		{
			_allActiveSaveSoruces.Add(_localSaveSource);
		}
		if (_amuzoSaveSource.doUseSaveSource)
		{
			_allActiveSaveSoruces.Add(_amuzoSaveSource);
		}
		if (_legoSaveSource.doUseSaveSource)
		{
			_allActiveSaveSoruces.Add(_legoSaveSource);
		}
		if (_allActiveSaveSoruces.Count == 0)
		{
			if (DO_DEBUG)
			{
				Debug.LogError("AmuzoSaveDataFacade: No save sources marked as being used. Nowhere to save data to!");
			}
		}
		else
		{
			_allActiveSaveSoruces.Sort(CompareSavedDataSources);
		}
	}

	private int CompareSavedDataSources(SaveDataSource sourceA, SaveDataSource sourceB)
	{
		return sourceA.SavePriority.CompareTo(sourceB.SavePriority);
	}

	private void LoadAllSavedData()
	{
		if (_allActiveSaveSoruces.Count != 0)
		{
			int i = 0;
			for (int count = _allActiveSaveSoruces.Count; i < count; i++)
			{
				_allActiveSaveSoruces[i].LoadAllData(OnSourceLoadComplete);
			}
		}
	}

	private void OnSourceLoadComplete(SaveDataSource source, List<SaveDataGroup> loadedData, bool didLoadSuccessfully)
	{
		if (!didLoadSuccessfully)
		{
			if (DO_DEBUG)
			{
				Debug.LogError("AmuzoSaveDataFacade: SaveDataSource [" + source.GetType().ToString() + "] did not load correctly!");
			}
		}
		else
		{
			SaveDataGroup saveDataGroup = null;
			int i = 0;
			for (int count = loadedData.Count; i < count; i++)
			{
				if (!string.IsNullOrEmpty(loadedData[i].SaveGroupId))
				{
					if (loadedData[i] != null && loadedData[i]._savedDataVersion < _currentSavedDataVersion)
					{
						loadedData[i] = UpdateSavedDataGroup(loadedData[i]);
					}
					saveDataGroup = GetSaveGroup(loadedData[i].SaveGroupId, false);
					if (DO_DEBUG)
					{
						Debug.Log("AmuzoSaveDataFacade: SaveDataSource [" + source.GetType().ToString() + "] loaded save data group [" + loadedData[i].SaveGroupId + "].");
					}
					if (saveDataGroup == null)
					{
						_allSavedData.Add(loadedData[i]);
						continue;
					}
					DeleteSaveGroup(loadedData[i].SaveGroupId);
					_allSavedData.Add(MergeSavedDataGroups(saveDataGroup, loadedData[i]));
				}
			}
		}
		int j = 0;
		for (int count2 = _allActiveSaveSoruces.Count; j < count2; j++)
		{
			if (!_allActiveSaveSoruces[j]._pHasInitialised)
			{
				return;
			}
		}
		AddDebugMenu();
		_currentState = InitialisationState.FINISHED;
	}

	private SaveDataGroup GetSaveGroup(string groupId, bool doCreateIfNotFound = true)
	{
		int i = 0;
		for (int count = _allSavedData.Count; i < count; i++)
		{
			if (!(_allSavedData[i].SaveGroupId != groupId))
			{
				return _allSavedData[i];
			}
		}
		if (doCreateIfNotFound)
		{
			if (DO_DEBUG)
			{
				Debug.Log("AmuzoSaveDataFacade: Creating new save data group [" + groupId + "]");
			}
			SaveDataGroup saveDataGroup = new SaveDataGroup(groupId);
			saveDataGroup._lastSaveTime = new DateTime(0L);
			saveDataGroup._savedDataVersion = _currentSavedDataVersion;
			_allSavedData.Add(saveDataGroup);
			return saveDataGroup;
		}
		return null;
	}

	private void DeleteSaveGroup(string groupId)
	{
		if (_currentPlayerData != null && _currentPlayerData.SaveGroupId == groupId)
		{
			_currentPlayerData = null;
		}
		if (_currentSystemSettings != null && _currentSystemSettings.SaveGroupId == groupId)
		{
			_currentSystemSettings = null;
		}
		int i = 0;
		for (int count = _allSavedData.Count; i < count; i++)
		{
			if (!(_allSavedData[i].SaveGroupId != groupId))
			{
				int j = 0;
				for (int count2 = _allActiveSaveSoruces.Count; j < count2; j++)
				{
					_allActiveSaveSoruces[j].DeleteData(new List<string> { _allSavedData[i].SaveGroupId });
				}
				_allSavedData.RemoveAt(i);
				break;
			}
		}
	}

	public void DeleteDataEntries(List<string> groupIds, List<string> entryIds)
	{
		int i = 0;
		for (int count = _allActiveSaveSoruces.Count; i < count; i++)
		{
			_allActiveSaveSoruces[i].DeleteDataEntries(groupIds, entryIds);
		}
	}

	private void AddDebugMenu()
	{
		AmuzoDebugMenu menu = new AmuzoDebugMenu("SAVED DATA FACADE");
		Func<string> textAreaFunction = () => "SAVED DATA FACADE" + AmuzoDebugMenu.NEW_LINE + "Save Data Version: " + _currentSavedDataVersion + AmuzoDebugMenu.NEW_LINE + "LEGO Source active: " + (_legoSaveSource != null && _legoSaveSource.doUseSaveSource) + AmuzoDebugMenu.NEW_LINE + "Amuzo Source active: " + (_amuzoSaveSource != null && _amuzoSaveSource.doUseSaveSource) + AmuzoDebugMenu.NEW_LINE + "Local Source active: " + (_localSaveSource != null && _localSaveSource.doUseSaveSource) + AmuzoDebugMenu.NEW_LINE;
		menu.AddInfoTextFunction(textAreaFunction);
		int num = 0;
		for (int count = _allSavedData.Count; num < count; num++)
		{
			addDebugMenuForSavedData(_allSavedData[num].SaveGroupId, ref menu);
		}
		AmuzoDebugMenuManager.RegisterRootDebugMenu(menu);
	}

	private void addDebugMenuForSavedData(string saveId, ref AmuzoDebugMenu menu)
	{
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu(saveId);
		Func<string> textAreaFunction = delegate
		{
			SaveDataGroup saveGroup = GetSaveGroup(saveId);
			return (saveGroup == null) ? ("GROUP [" + saveId + "] NOT FOUND!") : saveGroup.getDebugString();
		};
		amuzoDebugMenu.AddInfoTextFunction(textAreaFunction);
		menu.AddButton(new AmuzoDebugMenuButton(amuzoDebugMenu));
	}
}
