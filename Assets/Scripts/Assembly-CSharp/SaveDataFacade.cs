using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

[ExecuteInEditMode]
public class SaveDataFacade : MonoBehaviour, IEnumerable, ILocationHandler
{
	public enum SaveMode
	{
		Force = 0,
		Dirty = 1
	}

	private const string _errorMessage = "Still loading or saving: function cannot be called yet";

	public string TITLE_SAVE_KEY = "XXXXXX";

	public Encryption[] _encryptionHandlers;

	public PersistentBehaviour[] persistents;

	public SignalSender onInitialLoadAll;

	public Action<IPersistent, string> onSave;

	private int _slot;

	private Dictionary<string, IPersistent> _persistentObjects = new Dictionary<string, IPersistent>();

	private Dictionary<string, IPersistent>.ValueCollection.Enumerator _pobjIt;

	private bool _working;

	private Action _onComplete;

	private Dictionary<AmuzoEncryption, Encryption> _encryption;

	private SaveMode _saveMode;

	public int slot
	{
		get
		{
			return _slot;
		}
	}

	public int locationChangePriority
	{
		get
		{
			return -2;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public Dictionary<string, IPersistent>.Enumerator GetEnumerator()
	{
		return _persistentObjects.GetEnumerator();
	}

	private void Awake()
	{
		Facades<SaveDataFacade>.Register(this);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		PersistentBehaviour[] array = persistents;
		foreach (PersistentBehaviour pobj in array)
		{
			Register(pobj);
		}
		FlowFacade.AddLocationHandler(this);
		_encryption = new Dictionary<AmuzoEncryption, Encryption>();
		Encryption[] encryptionHandlers = _encryptionHandlers;
		foreach (Encryption encryption in encryptionHandlers)
		{
			_encryption[encryption.EncryptionType] = encryption;
		}
	}

	public void ChangeLocation(string previous, ref string current, string linkName, JsonData linkData, JsonData currentLocationData)
	{
		SaveAll(delegate
		{
		}, Encryption.ExecutionMode.Asynchronous, SaveMode.Dirty);
	}

	public void Register(IPersistent pobj)
	{
		if (_working)
		{
			Debug.LogWarning("Register: Still loading or saving: function cannot be called yet");
			return;
		}
		Debug.Log("Registered: " + pobj.persistenceKey);
		_persistentObjects.Add(pobj.persistenceKey, pobj);
	}

	public void Deregister(IPersistent pobj)
	{
		if (_working)
		{
			Debug.LogWarning("Deregister: Still loading or saving: function cannot be called yet");
			return;
		}
		Debug.Log("No Longer Registered: " + pobj.persistenceKey);
		_persistentObjects.Remove(pobj.persistenceKey);
	}

	public void SetSlot(int index, Action onComplete, Encryption.ExecutionMode mode, bool saveFirst = false)
	{
		if (_working)
		{
			Debug.LogWarning("SetSlot: Still loading or saving: function cannot be called yet");
		}
		else if (saveFirst)
		{
			Debug.Log("Changing save slot: all changes have been saved -- this takes time, are you sure they weren't already saved?");
			SaveAll(delegate
			{
				_slot = index;
				LoadAll(onComplete, mode);
			}, mode, SaveMode.Force);
		}
		else
		{
			Debug.LogWarning("Changing save slot: unsaved changes will be lost!");
			_slot = index;
			LoadAll(onComplete, mode);
		}
	}

	private void Initialise()
	{
		Debug.Log("Save Data Facade Initialise");
		LoadAll(delegate
		{
			onInitialLoadAll.SendSignals(this);
		}, Encryption.ExecutionMode.Asynchronous);
	}

	private string GetSection(IPersistent pobj)
	{
		if (pobj.isGlobal)
		{
			return "global";
		}
		return "slot" + _slot;
	}

	public void SaveAll(Action onComplete, Encryption.ExecutionMode mode, SaveMode saveMode)
	{
		if (_working)
		{
			Debug.LogWarning("SaveAll: Still loading or saving: function cannot be called yet");
			return;
		}
		if (onComplete == null)
		{
			Debug.LogError("You must supply a completion action");
			return;
		}
		_working = true;
		_onComplete = onComplete;
		_saveMode = saveMode;
		_pobjIt = _persistentObjects.Values.GetEnumerator();
		_pobjIt.MoveNext();
		SaveAllIterate(mode);
	}

	private void SaveAllIterate(Encryption.ExecutionMode mode)
	{
		do
		{
			IPersistent pobj = _pobjIt.Current;
			if (_saveMode == SaveMode.Dirty && !pobj.markedForSave)
			{
				SaveAllMoveNext(mode);
				return;
			}
			string text = pobj.Save();
			if (text == null || _encryption[pobj.encryption].Encrypt(text, pobj.persistenceKey, delegate(string encrypted)
			{
				Save(pobj, encrypted);
				SaveAllMoveNext(mode);
			}, mode))
			{
				return;
			}
		}
		while (_pobjIt.MoveNext());
		_working = false;
		_onComplete();
	}

	private void SaveAllMoveNext(Encryption.ExecutionMode mode)
	{
		if (_pobjIt.MoveNext())
		{
			SaveAllIterate(mode);
			return;
		}
		_working = false;
		_onComplete();
	}

	public void Save(string key, Action onComplete, Encryption.ExecutionMode mode, SaveMode saveMode)
	{
		if (_working)
		{
			Debug.LogWarning("Save: Still loading or saving: function cannot be called yet");
			return;
		}
		_onComplete = onComplete;
		_saveMode = saveMode;
		IPersistent pobj = null;
		if (!_persistentObjects.TryGetValue(key, out pobj))
		{
			Debug.LogWarning("Unknown persistence object [" + key + "] -- no IPersistent with this key was registered.");
			return;
		}
		if (_saveMode == SaveMode.Dirty && !pobj.markedForSave)
		{
			_onComplete();
			return;
		}
		_working = true;
		string input = pobj.Save();
		_encryption[pobj.encryption].Encrypt(input, pobj.persistenceKey, delegate(string encrypted)
		{
			Save(pobj, encrypted);
			_working = false;
			_onComplete();
		}, mode);
	}

	private void Save(IPersistent pobj, string data)
	{
		if (onSave != null)
		{
			onSave(pobj, data);
		}
		SetLocalSaveData(GetSection(pobj), pobj.persistenceKey, data);
	}

	public void LoadAll(Action onComplete, Encryption.ExecutionMode mode)
	{
		if (_working)
		{
			Debug.LogWarning("LoadAll: Still loading or saving: function cannot be called yet");
			return;
		}
		if (onComplete == null)
		{
			Debug.LogError("You must supply a completion action");
			return;
		}
		_working = true;
		_onComplete = onComplete;
		_pobjIt = _persistentObjects.Values.GetEnumerator();
		_pobjIt.MoveNext();
		LoadAllIterate(mode);
	}

	private void LoadAllIterate(Encryption.ExecutionMode mode)
	{
		do
		{
			IPersistent pobj = _pobjIt.Current;
			string input = LoadFromLocal(pobj);
			if (_encryption[pobj.encryption].Decrypt(input, pobj.persistenceKey, delegate(string decrypted)
			{
				pobj.Load(Extensions.LoadJson(decrypted));
				LoadAllMoveNext(mode);
			}, mode))
			{
				return;
			}
		}
		while (_pobjIt.MoveNext());
		_working = false;
		_onComplete();
	}

	private void LoadAllMoveNext(Encryption.ExecutionMode mode)
	{
		if (_pobjIt.MoveNext())
		{
			LoadAllIterate(mode);
			return;
		}
		_working = false;
		_onComplete();
	}

	public void Load(string key, Action onComplete, Encryption.ExecutionMode mode, string data = null)
	{
		if (_working)
		{
			Debug.LogWarning("Load: Still loading or saving: function cannot be called yet");
			return;
		}
		IPersistent pobj = null;
		if (!_persistentObjects.TryGetValue(key, out pobj))
		{
			Debug.LogWarning("Unknown persistence object [" + key + "] -- no IPersistent with this key was registered.");
			return;
		}
		string input = data ?? LoadFromLocal(pobj);
		_encryption[pobj.encryption].Decrypt(input, pobj.persistenceKey, delegate(string decrypted)
		{
			pobj.Load(Extensions.LoadJson(decrypted));
		}, mode);
	}

	private string LoadFromLocal(IPersistent pobj)
	{
		return GetLocalSaveData(GetSection(pobj), pobj.persistenceKey);
	}

	public string GetLocalSaveData(string section, string node)
	{
		string localKey = GetLocalKey(section, node);
		if (PlayerPrefs.HasKey(localKey))
		{
			return PlayerPrefs.GetString(localKey);
		}
		return string.Empty;
	}

	public void SetLocalSaveData(string section, string node, string data)
	{
		string localKey = GetLocalKey(section, node);
		UpdateTitleSaveKeys_Add(localKey);
		PlayerPrefs.SetString(localKey, data);
		PlayerPrefs.Save();
	}

	public string[] GetTitleSaveKeys()
	{
		string titleSaveKeysKey = GetTitleSaveKeysKey();
		if (titleSaveKeysKey == string.Empty)
		{
			return null;
		}
		string text = string.Empty;
		if (PlayerPrefs.HasKey(titleSaveKeysKey))
		{
			text = PlayerPrefs.GetString(titleSaveKeysKey);
		}
		if (text != string.Empty)
		{
			return text.Split(';');
		}
		return null;
	}

	public void ClearLocalSaveData(string section, string node)
	{
		ClearLocalSaveData(GetLocalKey(section, node));
	}

	public void ClearLocalSaveData(string playerPrefsKey)
	{
		if (PlayerPrefs.HasKey(playerPrefsKey))
		{
			PlayerPrefs.DeleteKey(playerPrefsKey);
			UpdateTitleSaveKeys_Remove(playerPrefsKey);
		}
	}

	private string GetKey(string section, string node)
	{
		return node + "_" + section;
	}

	private string GetLocalKey(string section, string node)
	{
		return TITLE_SAVE_KEY + "_" + GetKey(section, node);
	}

	private void UpdateTitleSaveKeys_Add(string titlekey)
	{
		UpdateTitleSaveKeys_Remove(titlekey);
		string[] titleSaveKeys = GetTitleSaveKeys();
		if (titleSaveKeys == null)
		{
			PlayerPrefs.SetString(GetTitleSaveKeysKey(), titlekey);
		}
		else
		{
			PlayerPrefs.SetString(GetTitleSaveKeysKey(), string.Join(";", titleSaveKeys) + ";" + titlekey);
		}
	}

	private void UpdateTitleSaveKeys_Remove(string titlekey)
	{
		string[] titleSaveKeys = GetTitleSaveKeys();
		if (titleSaveKeys == null)
		{
			return;
		}
		string text = string.Empty;
		string[] array = titleSaveKeys;
		foreach (string text2 in array)
		{
			if (text2 != null && text2 != string.Empty && text2 != titlekey)
			{
				text = ((!(text == string.Empty)) ? (text + ";" + text2) : text2);
			}
		}
		PlayerPrefs.SetString(GetTitleSaveKeysKey(), text);
	}

	private string GetTitleSaveKeysKey()
	{
		return TITLE_SAVE_KEY + "_keys";
	}
}
