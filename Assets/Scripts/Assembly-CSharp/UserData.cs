using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
	private const char SEPERATOR = ',';

	public string _name;

	public string _uniqueId;

	public long _timestamp;

	private Dictionary<string, string> _data = new Dictionary<string, string>();

	public bool hasData(string key)
	{
		return _data.ContainsKey(key);
	}

	public virtual void addData(string key, string value)
	{
		_data[key] = value;
	}

	public virtual string getData(string key)
	{
		return (!_data.ContainsKey(key)) ? null : _data[key];
	}

	public Dictionary<string, string> getAllData()
	{
		return _data;
	}

	public virtual void clone(UserData _dataToClone)
	{
		_uniqueId = _dataToClone._uniqueId;
		_name = _dataToClone._name;
		_timestamp = _dataToClone._timestamp;
		foreach (KeyValuePair<string, string> allDatum in _dataToClone.getAllData())
		{
			addData(allDatum.Key, allDatum.Value);
		}
	}

	public string toCSV()
	{
		string empty = string.Empty;
		empty = empty + _name + ',';
		empty = empty + _uniqueId + ',';
		empty = empty + _timestamp.ToString() + ',';
		foreach (KeyValuePair<string, string> datum in _data)
		{
			string text = empty;
			empty = text + datum.Key + "=" + datum.Value + ',';
		}
		return empty.Substring(0, empty.Length - 1);
	}

	public void LoadFromCSV(string csv)
	{
		string[] array = csv.Split(',');
		if (array.Length < 3)
		{
			return;
		}
		_name = array[0];
		_uniqueId = array[1];
		long.TryParse(array[2], out _timestamp);
		if (array.Length < 4)
		{
			return;
		}
		int i = 3;
		for (int num = array.Length; i < num; i++)
		{
			string[] array2 = array[i].Split('=');
			if (array2.Length == 2)
			{
				addData(array2[0], array2[1]);
			}
		}
	}
}
