using System;
using System.Collections.Generic;

public class SaveDataGroup
{
	public int _savedDataVersion;

	public DateTime _lastSaveTime;

	public List<SaveDataEntry> _saveDataEntries = new List<SaveDataEntry>();

	public string SaveGroupId { get; private set; }

	public SaveDataGroup(string saveId)
	{
		SaveGroupId = saveId;
		_lastSaveTime = TimeManager.GetCurrentTime();
	}

	public void DeleteKey(string key)
	{
		int i = 0;
		for (int count = _saveDataEntries.Count; i < count; i++)
		{
			if (!(_saveDataEntries[i].SaveKey != key))
			{
				AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.DeleteDataEntries(new List<string> { SaveGroupId }, new List<string> { _saveDataEntries[i].SaveKey });
				_saveDataEntries.RemoveAt(i);
				break;
			}
		}
	}

	public bool hasKey(string key)
	{
		int i = 0;
		for (int count = _saveDataEntries.Count; i < count; i++)
		{
			if (!(_saveDataEntries[i].SaveKey != key))
			{
				return true;
			}
		}
		return false;
	}

	public void setBool(string key, bool boolValue)
	{
		SaveDataEntry entry = null;
		getEntry(key, out entry);
		entry._entryType = SaveDataEntryType.Bool;
		if (entry.boolValue != boolValue)
		{
			entry.boolValue = boolValue;
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.OnSaveDataChanged(this, entry);
		}
	}

	public bool getBool(string key, bool defaultBoolValue = false)
	{
		SaveDataEntry entry = null;
		if (!getEntry(key, out entry))
		{
			entry._entryType = SaveDataEntryType.Bool;
			entry.boolValue = defaultBoolValue;
		}
		return entry.boolValue;
	}

	public void setString(string key, string stringValue)
	{
		SaveDataEntry entry = null;
		getEntry(key, out entry);
		entry._entryType = SaveDataEntryType.String;
		if (!(entry.stringValue == stringValue))
		{
			entry.stringValue = stringValue;
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.OnSaveDataChanged(this, entry);
		}
	}

	public string getString(string key, string defaultStringValue = "")
	{
		SaveDataEntry entry = null;
		if (!getEntry(key, out entry))
		{
			entry._entryType = SaveDataEntryType.String;
			entry.stringValue = defaultStringValue;
		}
		return entry.stringValue;
	}

	public void setFloat(string key, float floatValue)
	{
		SaveDataEntry entry = null;
		getEntry(key, out entry);
		entry._entryType = SaveDataEntryType.Float;
		if (entry.floatValue != floatValue)
		{
			entry.floatValue = floatValue;
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.OnSaveDataChanged(this, entry);
		}
	}

	public float getFloat(string key, float defaultFloatValue = 0f)
	{
		SaveDataEntry entry = null;
		if (!getEntry(key, out entry))
		{
			entry._entryType = SaveDataEntryType.Float;
			entry.floatValue = defaultFloatValue;
		}
		return entry.floatValue;
	}

	public void setInt(string key, int intValue)
	{
		SaveDataEntry entry = null;
		getEntry(key, out entry);
		entry._entryType = SaveDataEntryType.Int;
		if (entry.intValue != intValue)
		{
			entry.intValue = intValue;
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.OnSaveDataChanged(this, entry);
		}
	}

	public int getInt(string key, int defaultIntValue = 0)
	{
		SaveDataEntry entry = null;
		if (!getEntry(key, out entry))
		{
			entry._entryType = SaveDataEntryType.Int;
			entry.intValue = defaultIntValue;
		}
		return entry.intValue;
	}

	public void setLong(string key, long longValue)
	{
		SaveDataEntry entry = null;
		getEntry(key, out entry);
		entry._entryType = SaveDataEntryType.Long;
		if (entry.longValue != longValue)
		{
			entry.longValue = longValue;
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.OnSaveDataChanged(this, entry);
		}
	}

	public long getLong(string key, long defaultLongValue = 0)
	{
		SaveDataEntry entry = null;
		if (!getEntry(key, out entry))
		{
			entry._entryType = SaveDataEntryType.Long;
			entry.longValue = defaultLongValue;
		}
		return entry.longValue;
	}

	public void setStringArray(string key, string[] stringArrayValue)
	{
		SaveDataEntry entry = null;
		getEntry(key, out entry);
		entry._entryType = SaveDataEntryType.StringArray;
		if (entry.stringArrayValue != stringArrayValue)
		{
			if (stringArrayValue != null)
			{
				entry.stringArrayValue = new string[stringArrayValue.Length];
				stringArrayValue.CopyTo(entry.stringArrayValue, 0);
			}
			else
			{
				entry.stringArrayValue = null;
			}
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.OnSaveDataChanged(this, entry);
		}
	}

	public string[] getStringArray(string key, string[] defaultStringArrayValue = null)
	{
		SaveDataEntry entry = null;
		if (!getEntry(key, out entry))
		{
			entry._entryType = SaveDataEntryType.StringArray;
			entry.stringArrayValue = defaultStringArrayValue;
		}
		if (entry.stringArrayValue != null)
		{
			string[] array = new string[entry.stringArrayValue.Length];
			entry.stringArrayValue.CopyTo(array, 0);
			return array;
		}
		return null;
	}

	public void setFloatArray(string key, float[] floatArrayValue)
	{
		SaveDataEntry entry = null;
		getEntry(key, out entry);
		entry._entryType = SaveDataEntryType.FloatArray;
		if (entry.floatArrayValue != floatArrayValue)
		{
			if (floatArrayValue != null)
			{
				entry.floatArrayValue = new float[floatArrayValue.Length];
				floatArrayValue.CopyTo(entry.floatArrayValue, 0);
			}
			else
			{
				entry.floatArrayValue = null;
			}
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.OnSaveDataChanged(this, entry);
		}
	}

	public string getFloatArray(string key, float[] defaultFloatArrayValue = null)
	{
		SaveDataEntry entry = null;
		if (!getEntry(key, out entry))
		{
			entry._entryType = SaveDataEntryType.FloatArray;
			entry.floatArrayValue = defaultFloatArrayValue;
		}
		return entry.stringValue;
	}

	public void setIntArray(string key, int[] intArrayValue)
	{
		SaveDataEntry entry = null;
		getEntry(key, out entry);
		entry._entryType = SaveDataEntryType.IntArray;
		if (entry.intArrayValue != intArrayValue)
		{
			if (intArrayValue != null)
			{
				entry.intArrayValue = new int[intArrayValue.Length];
				intArrayValue.CopyTo(entry.intArrayValue, 0);
			}
			else
			{
				entry.intArrayValue = null;
			}
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.OnSaveDataChanged(this, entry);
		}
	}

	public int[] getIntArray(string key, int[] defaultIntArrayValue = null)
	{
		SaveDataEntry entry = null;
		if (!getEntry(key, out entry))
		{
			entry._entryType = SaveDataEntryType.IntArray;
			entry.intArrayValue = defaultIntArrayValue;
		}
		return entry.intArrayValue;
	}

	public void setBoolArray(string key, bool[] boolArrayValue)
	{
		SaveDataEntry entry = null;
		getEntry(key, out entry);
		entry._entryType = SaveDataEntryType.BoolArray;
		if (entry.boolArrayValue != boolArrayValue)
		{
			if (boolArrayValue != null)
			{
				entry.boolArrayValue = new bool[boolArrayValue.Length];
				boolArrayValue.CopyTo(entry.boolArrayValue, 0);
			}
			else
			{
				entry.boolArrayValue = null;
			}
			AmuzoMonoInitSingleton<AmuzoSaveDataFacade>._pInstance.OnSaveDataChanged(this, entry);
		}
	}

	public bool[] getBoolArray(string key, bool[] defaultIntArrayValue = null)
	{
		SaveDataEntry entry = null;
		if (!getEntry(key, out entry))
		{
			entry._entryType = SaveDataEntryType.BoolArray;
			entry.boolArrayValue = defaultIntArrayValue;
		}
		return entry.boolArrayValue;
	}

	public void cloneGroup(SaveDataGroup groupToClone)
	{
		_saveDataEntries.Clear();
		_savedDataVersion = groupToClone._savedDataVersion;
		_lastSaveTime = groupToClone._lastSaveTime;
		SaveDataEntry saveDataEntry = null;
		int i = 0;
		for (int count = groupToClone._saveDataEntries.Count; i < count; i++)
		{
			saveDataEntry = new SaveDataEntry(groupToClone._saveDataEntries[i].SaveKey);
			saveDataEntry.cloneEntry(groupToClone._saveDataEntries[i]);
			_saveDataEntries.Add(saveDataEntry);
		}
	}

	public string getDebugString()
	{
		string text = string.Empty + '\r' + '\n';
		string text2 = "********* Saved Data Group [" + SaveGroupId + "] *************" + text;
		string text3 = text2;
		text2 = text3 + "Saved Data Version [" + _savedDataVersion + "]" + text;
		text3 = text2;
		text2 = text3 + "TimeStamp [" + _lastSaveTime.ToLongTimeString() + " " + _lastSaveTime.ToLongDateString() + "]" + text;
		int i = 0;
		for (int count = _saveDataEntries.Count; i < count; i++)
		{
			text3 = text2;
			text2 = string.Concat(text3, "Entry [", _saveDataEntries[i].SaveKey, "]. Type [", _saveDataEntries[i]._entryType, "]. Value [", _saveDataEntries[i].getValueAsString(), "]", text);
		}
		return text2;
	}

	private bool getEntry(string key, out SaveDataEntry entry)
	{
		int i = 0;
		for (int count = _saveDataEntries.Count; i < count; i++)
		{
			if (!(_saveDataEntries[i].SaveKey != key))
			{
				entry = _saveDataEntries[i];
				return true;
			}
		}
		entry = new SaveDataEntry(key);
		_saveDataEntries.Add(entry);
		return false;
	}
}
