using System;
using System.Collections.Generic;

[Serializable]
public class SavedDataSource_Local : SaveDataSource
{
	private const string GROUP_NAMES_ARRAY_KEY = "SavedDataSource_Local_KnownGroups";

	private const string GROUP_TIME_STAMP_SUFFIX_ARRAY_KEY = "_TIME";

	private const string GROUP_VERSION_SUFFIX_ARRAY_KEY = "_VERSION";

	private const char ARRAY_ENTRY_SEPERATOR = ',';

	private const char ENTRY_NAME_PART_SEPERATOR = '~';

	public override void DeleteData(List<string> groupIdsToDelete)
	{
		string text = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString("SavedDataSource_Local_KnownGroups", string.Empty);
		List<string> list = new List<string>(text.Split(','));
		string empty = string.Empty;
		string[] array = null;
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (groupIdsToDelete.Contains(list[num]))
			{
				empty = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString(list[num], string.Empty);
				array = empty.Split(',');
				int i = 0;
				for (int num2 = array.Length; i < num2; i++)
				{
					AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.DeleteKey(array[i]);
				}
				AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.DeleteKey(list[num]);
				list.RemoveAt(num);
			}
		}
		AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString("SavedDataSource_Local_KnownGroups", Utils.arrayToCSV(list.ToArray()));
	}

	public override void DeleteDataEntries(List<string> groupIds, List<string> entryIds)
	{
		string text = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString("SavedDataSource_Local_KnownGroups", string.Empty);
		List<string> list = new List<string>(text.Split(','));
		string empty = string.Empty;
		List<string> list2 = null;
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (groupIds.Contains(list[num]))
			{
				empty = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString(list[num], string.Empty);
				list2 = new List<string>(empty.Split(','));
				for (int num2 = list2.Count - 1; num2 >= 0; num2--)
				{
					if (entryIds.Contains(list2[num2]))
					{
						AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.DeleteKey(list2[num2]);
						list2.RemoveAt(num2);
					}
				}
				AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString(list[num], Utils.arrayToCSV(list2.ToArray()));
			}
		}
	}

	public override void SaveData(List<SaveDataGroup> dataToSave, Action<SaveDataSource, bool> OnSaveComplete)
	{
		List<string> list = new List<string>();
		List<string> keyList = new List<string>();
		int i = 0;
		for (int count = dataToSave.Count; i < count; i++)
		{
			if (!string.IsNullOrEmpty(dataToSave[i].SaveGroupId))
			{
				list.Add(dataToSave[i].SaveGroupId);
			}
		}
		DeleteData(list);
		int j = 0;
		for (int count2 = dataToSave.Count; j < count2; j++)
		{
			if (!string.IsNullOrEmpty(dataToSave[j].SaveGroupId))
			{
				keyList.Clear();
				int k = 0;
				for (int count3 = dataToSave[j]._saveDataEntries.Count; k < count3; k++)
				{
					SaveEntry(dataToSave[j], dataToSave[j]._saveDataEntries[k], ref keyList);
				}
				AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString(dataToSave[j].SaveGroupId, Utils.arrayToCSV(keyList.ToArray()));
				AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetLong(dataToSave[j].SaveGroupId + "_TIME", TimeManager.GetCurrentTime().Ticks);
				AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetInt(dataToSave[j].SaveGroupId + "_VERSION", dataToSave[j]._savedDataVersion);
			}
		}
		string text = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString("SavedDataSource_Local_KnownGroups", string.Empty);
		string[] array = text.Split(',');
		int l = 0;
		for (int num = array.Length; l < num; l++)
		{
			if (!list.Contains(array[l]))
			{
				list.Add(array[l]);
			}
		}
		AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString("SavedDataSource_Local_KnownGroups", Utils.arrayToCSV(list.ToArray()));
		OnSaveComplete(this, true);
	}

	public override void LoadData(string[] saveGroupIds, Action<SaveDataSource, List<SaveDataGroup>, bool> OnLoadComplete)
	{
		List<string> list = new List<string>(saveGroupIds);
		List<SaveDataGroup> list2 = new List<SaveDataGroup>();
		string text = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString("SavedDataSource_Local_KnownGroups", string.Empty);
		string[] array = text.Split(',');
		string empty = string.Empty;
		string[] array2 = null;
		SaveDataEntry saveDataEntry = null;
		SaveDataGroup saveDataGroup = null;
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (!list.Contains(array[i]))
			{
				continue;
			}
			saveDataGroup = new SaveDataGroup(array[i]);
			saveDataGroup._lastSaveTime = new DateTime(AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetLong(array[i] + "_TIME", 0L));
			saveDataGroup._savedDataVersion = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetInt(array[i] + "_VERSION", 0);
			empty = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString(array[i], string.Empty);
			array2 = empty.Split(',');
			int j = 0;
			for (int num2 = array2.Length; j < num2; j++)
			{
				saveDataEntry = LoadEntry(array2[j]);
				if (saveDataEntry != null)
				{
					saveDataGroup._saveDataEntries.Add(saveDataEntry);
				}
			}
			list2.Add(saveDataGroup);
		}
		_pHasInitialised = true;
		OnLoadComplete(this, list2, true);
	}

	public override void LoadAllData(Action<SaveDataSource, List<SaveDataGroup>, bool> OnLoadComplete)
	{
		string text = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString("SavedDataSource_Local_KnownGroups", string.Empty);
		string[] saveGroupIds = text.Split(',');
		LoadData(saveGroupIds, OnLoadComplete);
	}

	private string GetSaveKey(string parentGroupId, SaveDataEntryType type, string entrySaveKey)
	{
		return string.Concat(parentGroupId, '~', type, '~', entrySaveKey);
	}

	private void SaveEntry(SaveDataGroup parentGroup, SaveDataEntry entryToSave, ref List<string> keyList)
	{
		string saveKey = GetSaveKey(parentGroup.SaveGroupId, entryToSave._entryType, entryToSave.SaveKey);
		switch (entryToSave._entryType)
		{
		case SaveDataEntryType.Bool:
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetInt(saveKey, entryToSave.boolValue ? 1 : 0);
			break;
		case SaveDataEntryType.BoolArray:
			if (entryToSave.boolArrayValue == null)
			{
				return;
			}
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString(saveKey, Utils.arrayToCSV(entryToSave.boolArrayValue));
			break;
		case SaveDataEntryType.Float:
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetFloat(saveKey, entryToSave.floatValue);
			break;
		case SaveDataEntryType.FloatArray:
			if (entryToSave.floatArrayValue == null)
			{
				return;
			}
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString(saveKey, Utils.arrayToCSV(entryToSave.floatArrayValue));
			break;
		case SaveDataEntryType.Int:
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetInt(saveKey, entryToSave.intValue);
			break;
		case SaveDataEntryType.Long:
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetLong(saveKey, entryToSave.longValue);
			break;
		case SaveDataEntryType.IntArray:
			if (entryToSave.intArrayValue == null)
			{
				return;
			}
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString(saveKey, Utils.arrayToCSV(entryToSave.intArrayValue));
			break;
		case SaveDataEntryType.String:
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString(saveKey, entryToSave.stringValue);
			break;
		case SaveDataEntryType.StringArray:
		{
			if (entryToSave.stringArrayValue == null)
			{
				return;
			}
			string value = Utils.arrayToCSV(entryToSave.stringArrayValue);
			AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.SetString(saveKey, value);
			break;
		}
		}
		keyList.Add(saveKey);
	}

	private SaveDataEntry LoadEntry(string saveKey)
	{
		if (string.IsNullOrEmpty(saveKey))
		{
			return null;
		}
		string[] array = saveKey.Split('~');
		if (array.Length != 3)
		{
			return null;
		}
		SaveDataEntry saveDataEntry = new SaveDataEntry(array[2]);
		saveDataEntry._entryType = (SaveDataEntryType)(int)Enum.Parse(typeof(SaveDataEntryType), array[1]);
		switch (saveDataEntry._entryType)
		{
		case SaveDataEntryType.Bool:
			saveDataEntry.boolValue = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetInt(saveKey) == 1;
			break;
		case SaveDataEntryType.BoolArray:
		{
			string text3 = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString(saveKey);
			string[] array3 = text3.Split(',');
			saveDataEntry.boolArrayValue = new bool[array3.Length];
			int j = 0;
			for (int num2 = array3.Length; j < num2; j++)
			{
				bool.TryParse(array3[j], out saveDataEntry.boolArrayValue[j]);
			}
			break;
		}
		case SaveDataEntryType.Float:
			saveDataEntry.floatValue = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetFloat(saveKey);
			break;
		case SaveDataEntryType.FloatArray:
		{
			string text4 = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString(saveKey);
			string[] array4 = text4.Split(',');
			saveDataEntry.floatArrayValue = new float[array4.Length];
			int k = 0;
			for (int num3 = array4.Length; k < num3; k++)
			{
				float.TryParse(array4[k], out saveDataEntry.floatArrayValue[k]);
			}
			break;
		}
		case SaveDataEntryType.Int:
			saveDataEntry.intValue = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetInt(saveKey);
			break;
		case SaveDataEntryType.Long:
			saveDataEntry.longValue = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetLong(saveKey);
			break;
		case SaveDataEntryType.IntArray:
		{
			string text2 = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString(saveKey);
			string[] array2 = text2.Split(',');
			saveDataEntry.intArrayValue = new int[array2.Length];
			int i = 0;
			for (int num = array2.Length; i < num; i++)
			{
				int.TryParse(array2[i], out saveDataEntry.intArrayValue[i]);
			}
			break;
		}
		case SaveDataEntryType.String:
			saveDataEntry.stringValue = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString(saveKey);
			break;
		case SaveDataEntryType.StringArray:
		{
			string text = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetString(saveKey);
			saveDataEntry.stringArrayValue = text.Split(',');
			break;
		}
		}
		return saveDataEntry;
	}
}
