using System;
using LitJson;
using UnityEngine;

public class ConfigFiles : MonoBehaviour, ConfigData.ISchemas
{
	private const string LOG_TAG = "[ConfigFiles] ";

	public TextAsset _defaultConfig;

	public TextAsset _mainSchema;

	public TextAsset[] _subSchemas;

	public bool _isUseJsonLoader;

	private JsonData _defaultConfigData;

	private JsonData _mainSchemaData;

	private JsonData[] _subSchemaData;

	JsonData ConfigData.ISchemas._pMainSchema
	{
		get
		{
			return _mainSchemaData;
		}
	}

	public string _pDefaultConfigName
	{
		get
		{
			return (!(_defaultConfig != null)) ? null : _defaultConfig.name;
		}
	}

	public JsonData _pDefaultConfigData
	{
		get
		{
			return _defaultConfigData;
		}
	}

	public string _pMainSchemaName
	{
		get
		{
			return (!(_mainSchema != null)) ? null : _mainSchema.name;
		}
	}

	public JsonData _pMainSchemaData
	{
		get
		{
			return _mainSchemaData;
		}
	}

	JsonData ConfigData.ISchemas.GetSubSchema(string specId)
	{
		return FindSchemaData(specId);
	}

	public void Initialise()
	{
		if (_defaultConfig != null)
		{
			_defaultConfigData = LoadJsonFile(_defaultConfig);
		}
		if (_mainSchema != null)
		{
			_mainSchemaData = LoadJsonFile(_mainSchema);
		}
		if (_subSchemas == null)
		{
			return;
		}
		_subSchemaData = new JsonData[_subSchemas.Length];
		for (int i = 0; i < _subSchemas.Length; i++)
		{
			if (!(_subSchemas[i] == null))
			{
				_subSchemaData[i] = LoadJsonFile(_subSchemas[i]);
			}
		}
	}

	private JsonData LoadJsonFile(TextAsset file)
	{
		JsonData jsonData = null;
		if (_isUseJsonLoader)
		{
			return JsonLoader.data(file.name);
		}
		return Extensions.LoadJson(file.text);
	}

	private JsonData FindSchemaData(string schemaName)
	{
		if (_mainSchema != null && string.Equals(schemaName, _mainSchema.name, StringComparison.CurrentCultureIgnoreCase))
		{
			return _mainSchemaData;
		}
		if (_subSchemas != null)
		{
			int num = Array.FindIndex(_subSchemas, (TextAsset s) => s != null && string.Equals(schemaName, s.name, StringComparison.CurrentCultureIgnoreCase));
			if (_subSchemaData != null && num >= 0 && num < _subSchemaData.Length)
			{
				return _subSchemaData[num];
			}
		}
		return null;
	}
}
