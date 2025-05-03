using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using CodeStage.AntiCheat.ObscuredTypes;
using GameDefines;
using LitJson;
using UnityEngine;

public class PlayerPrefsFacade : AmuzoScriptableSingleton<PlayerPrefsFacade>
{
	public enum EValueType
	{
		NULL = 0,
		STRING = 1,
		FLOAT = 2,
		INT = 3,
		LONG = 4
	}

	private const string LOG_TAG = "[PlayerPrefs] ";

	private const string KEY_DATA_KEY = "ppkd";

	private const string DUMP_FILE_NAME = "PlayerPrefsOutput.txt";

	[SerializeField]
	private bool _useEncryption;

	[SerializeField]
	private string _encryptionKey = string.Empty;

	[NonSerialized]
	private EnumUtils.FromStringMap<EValueType> _valueTypeFromStringMap = new EnumUtils.FromStringMap<EValueType>(EValueType.NULL);

	[NonSerialized]
	private Dictionary<string, EValueType> _keyData;

	[NonSerialized]
	private bool hasInitialised;

	protected override void Initialise()
	{
		if (hasInitialised)
		{
			return;
		}
		if (_useEncryption)
		{
			if (string.IsNullOrEmpty(_encryptionKey))
			{
				ObscuredPrefs.SetNewCryptoKey(GlobalDefines._pAmuzoDeviceUniqueId.ToString());
			}
			else
			{
				ObscuredPrefs.SetNewCryptoKey(_encryptionKey);
			}
		}
		CheckInitializeKeyData();
		AddDebugMenu();
		hasInitialised = true;
	}

	public void DeleteKey(string key)
	{
		if (_useEncryption)
		{
			ObscuredPrefs.DeleteKey(key);
		}
		else
		{
			PlayerPrefs.DeleteKey(key);
		}
		OnDeleteKey(key);
	}

	public void DeleteAll()
	{
		if (_useEncryption)
		{
			ObscuredPrefs.DeleteAll();
		}
		else
		{
			PlayerPrefs.DeleteAll();
		}
		OnDeleteAllKeys();
	}

	public bool HasKey(string key)
	{
		return (_useEncryption && ObscuredPrefs.HasKey(key)) || PlayerPrefs.HasKey(key);
	}

	public void Save()
	{
		if (_useEncryption)
		{
			ObscuredPrefs.Save();
		}
		else
		{
			PlayerPrefs.Save();
		}
	}

	public string GetString(string key)
	{
		return (!_useEncryption) ? PlayerPrefs.GetString(key) : ObscuredPrefs.GetString(key);
	}

	public float GetFloat(string key)
	{
		return (!_useEncryption) ? PlayerPrefs.GetFloat(key) : ObscuredPrefs.GetFloat(key);
	}

	public int GetInt(string key)
	{
		return (!_useEncryption) ? PlayerPrefs.GetInt(key) : ObscuredPrefs.GetInt(key);
	}

	public long GetLong(string key)
	{
		return (!_useEncryption) ? PlayerPrefs.GetInt(key) : ObscuredPrefs.GetLong(key);
	}

	public string GetString(string key, string defaultValue)
	{
		return (!_useEncryption) ? PlayerPrefs.GetString(key, defaultValue) : ObscuredPrefs.GetString(key, defaultValue);
	}

	public float GetFloat(string key, float defaultValue)
	{
		return (!_useEncryption) ? PlayerPrefs.GetFloat(key, defaultValue) : ObscuredPrefs.GetFloat(key, defaultValue);
	}

	public int GetInt(string key, int defaultValue)
	{
		return (!_useEncryption) ? PlayerPrefs.GetInt(key, defaultValue) : ObscuredPrefs.GetInt(key, defaultValue);
	}

	public long GetLong(string key, long defaultValue)
	{
		return (!_useEncryption) ? PlayerPrefs.GetInt(key, (int)defaultValue) : ObscuredPrefs.GetLong(key, defaultValue);
	}

	public void SetString(string key, string value)
	{
		if (_useEncryption)
		{
			ObscuredPrefs.SetString(key, value);
		}
		else
		{
			PlayerPrefs.SetString(key, value);
		}
		OnSetKey(key, EValueType.STRING);
	}

	public void SetFloat(string key, float value)
	{
		if (_useEncryption)
		{
			ObscuredPrefs.SetFloat(key, value);
		}
		else
		{
			PlayerPrefs.SetFloat(key, value);
		}
		OnSetKey(key, EValueType.FLOAT);
	}

	public void SetInt(string key, int value)
	{
		if (_useEncryption)
		{
			ObscuredPrefs.SetInt(key, value);
		}
		else
		{
			PlayerPrefs.SetInt(key, value);
		}
		OnSetKey(key, EValueType.INT);
	}

	public void SetLong(string key, long value)
	{
		if (_useEncryption)
		{
			ObscuredPrefs.SetLong(key, value);
		}
		else
		{
			PlayerPrefs.SetInt(key, (int)value);
		}
		OnSetKey(key, EValueType.LONG);
	}

	public void DebugLogPlayerPrefs()
	{
		Debug.Log("[PlayerPrefs] Player prefs:");
		Debug.Log(SerializePlayerPrefs(true));
	}

	public void DebugDumpPlayerPrefs()
	{
		string text = SerializePlayerPrefs(true);
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogError("[PlayerPrefs] Can not dump player prefs: failed to serialize prefs");
			return;
		}
		string persistentDataPath = Application.persistentDataPath;
		if (!Directory.Exists(persistentDataPath))
		{
			try
			{
				Directory.CreateDirectory(persistentDataPath);
			}
			catch (Exception ex)
			{
				Debug.LogError("[PlayerPrefs] Can not dump player prefs: failed to create directory '" + persistentDataPath + "': " + ex.ToString());
				return;
			}
		}
		string text2 = Path.Combine(persistentDataPath, "PlayerPrefsOutput.txt");
		Debug.Log("[PlayerPrefs] Dumping player prefs to '" + text2 + "'");
		try
		{
			File.WriteAllBytes(text2, Encoding.UTF8.GetBytes(text));
		}
		catch (Exception ex2)
		{
			Debug.LogError("[PlayerPrefs] Can not dump player prefs: failed to write file '" + text2 + "': " + ex2.ToString());
		}
	}

	private void CheckInitializeKeyData()
	{
		if (_keyData == null)
		{
			_keyData = new Dictionary<string, EValueType>();
			string text = GetString("ppkd");
			if (!string.IsNullOrEmpty(text))
			{
				DeserializeKeyData(text);
			}
		}
	}

	private void OnSetKey(string key, EValueType valueType)
	{
		CheckInitializeKeyData();
		if (_keyData.ContainsKey(key))
		{
			if (_keyData[key] != valueType)
			{
				BeginKeyDataChange();
				_keyData[key] = valueType;
				EndKeyDataChange();
			}
		}
		else
		{
			BeginKeyDataChange();
			_keyData.Add(key, valueType);
			EndKeyDataChange();
		}
	}

	private void OnDeleteKey(string key)
	{
		CheckInitializeKeyData();
		if (_keyData.ContainsKey(key))
		{
			BeginKeyDataChange();
			_keyData.Remove(key);
			EndKeyDataChange();
		}
	}

	private void OnDeleteAllKeys()
	{
		CheckInitializeKeyData();
		if (_keyData == null || _keyData.Count > 0)
		{
			BeginKeyDataChange();
			_keyData = new Dictionary<string, EValueType>();
			EndKeyDataChange();
		}
	}

	private void BeginKeyDataChange()
	{
	}

	private void EndKeyDataChange()
	{
		OnKeyDataChanged();
	}

	private void OnKeyDataChanged()
	{
		string value = SerializeKeyData();
		SetString("ppkd", value);
	}

	private string SerializeKeyData()
	{
		JsonWriter jsonWriter = Extensions.BeginJson();
		if (jsonWriter == null)
		{
			Debug.LogError("[PlayerPrefs] Can not serialize key data: failed to begin json writing");
			return null;
		}
		foreach (KeyValuePair<string, EValueType> keyDatum in _keyData)
		{
			jsonWriter.WriteValue(keyDatum.Key, keyDatum.Value.ToString());
		}
		return Extensions.EndJson();
	}

	private void DeserializeKeyData(string serData)
	{
		JsonData jsonData = Extensions.LoadJson(serData);
		if (jsonData == null)
		{
			Debug.LogError("[PlayerPrefs] Can not deserialize key data: failed to load json string: " + serData);
			return;
		}
		if (!jsonData.IsObject)
		{
			Debug.LogError(string.Concat("[PlayerPrefs] Can not deserialize key data: unexpected json type: type=", jsonData.GetJsonType(), ", json=", serData));
			return;
		}
		_keyData = new Dictionary<string, EValueType>();
		foreach (DictionaryEntry item in (IOrderedDictionary)jsonData)
		{
			JsonData jsonData2 = (JsonData)item.Value;
			if (jsonData2.IsString)
			{
				_keyData.Add((string)item.Key, _valueTypeFromStringMap.Lookup((string)jsonData2));
			}
		}
	}

	private string SerializePlayerPrefs(bool pretty = false)
	{
		JsonWriter jsonWriter = Extensions.BeginJson();
		if (jsonWriter == null)
		{
			Debug.LogError("[PlayerPrefs] Can not get player prefs string: failed to begin json writing");
			return null;
		}
		jsonWriter.PrettyPrint = pretty;
		CheckInitializeKeyData();
		foreach (KeyValuePair<string, EValueType> keyDatum in _keyData)
		{
			switch (keyDatum.Value)
			{
			case EValueType.STRING:
				jsonWriter.WriteValue(keyDatum.Key, GetString(keyDatum.Key));
				break;
			case EValueType.FLOAT:
				jsonWriter.WriteValue(keyDatum.Key, GetFloat(keyDatum.Key));
				break;
			case EValueType.INT:
				jsonWriter.WriteValue(keyDatum.Key, GetInt(keyDatum.Key));
				break;
			case EValueType.LONG:
				jsonWriter.WriteValue(keyDatum.Key, GetLong(keyDatum.Key));
				break;
			}
		}
		return Extensions.EndJson();
	}

	private void AddDebugMenu()
	{
		Func<string> textAreaFunction = () => SerializePlayerPrefs(true);
		AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("PLAYER PREFS FACADE");
		amuzoDebugMenu.AddInfoTextFunction(textAreaFunction);
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("DUMP TO FILE", delegate
		{
			DebugDumpPlayerPrefs();
		}));
		amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton("DELETE ALL", delegate
		{
			DeleteAll();
		}));
		AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu);
	}
}
