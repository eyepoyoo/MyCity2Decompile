public class SaveDataEntry
{
	public SaveDataEntryType _entryType;

	public string[] _batchSaveTags;

	public bool boolValue;

	public string stringValue;

	public float floatValue;

	public int intValue;

	public long longValue;

	public string[] stringArrayValue;

	public float[] floatArrayValue;

	public int[] intArrayValue;

	public bool[] boolArrayValue;

	public string SaveKey { get; private set; }

	public SaveDataEntry(string saveKey)
	{
		SaveKey = saveKey;
	}

	public string getValueAsString()
	{
		switch (_entryType)
		{
		case SaveDataEntryType.Bool:
			return boolValue.ToString();
		case SaveDataEntryType.Float:
			return floatValue.ToString();
		case SaveDataEntryType.Int:
			return intValue.ToString();
		case SaveDataEntryType.Long:
			return longValue.ToString();
		case SaveDataEntryType.StringArray:
			return Utils.arrayToCSV(stringArrayValue);
		case SaveDataEntryType.FloatArray:
			return Utils.arrayToCSV(floatArrayValue);
		case SaveDataEntryType.IntArray:
			return Utils.arrayToCSV(intArrayValue);
		case SaveDataEntryType.BoolArray:
			return Utils.arrayToCSV(boolArrayValue);
		default:
			return stringValue;
		}
	}

	public void cloneEntry(SaveDataEntry otherEntry)
	{
		_batchSaveTags = otherEntry._batchSaveTags;
		_entryType = otherEntry._entryType;
		boolValue = otherEntry.boolValue;
		stringValue = otherEntry.stringValue;
		floatValue = otherEntry.floatValue;
		intValue = otherEntry.intValue;
		longValue = otherEntry.longValue;
		stringArrayValue = otherEntry.stringArrayValue;
		floatArrayValue = otherEntry.floatArrayValue;
		intArrayValue = otherEntry.intArrayValue;
		boolArrayValue = otherEntry.boolArrayValue;
	}
}
