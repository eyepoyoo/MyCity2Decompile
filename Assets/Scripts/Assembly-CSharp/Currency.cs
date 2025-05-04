using System;
using CodeStage.AntiCheat.ObscuredTypes;
using GameDefines;
using UnityEngine;

public class Currency
{
	public Action onChangeCallback;

	public CurrencyDefines.CurrencyType _type;

	public int _displayValue;

	public int _displayMax;

	private ObscuredInt _currentValue = 0;

	private ObscuredInt _maxValue = 0;

	public int _pRealValue
	{
		get
		{
			return _currentValue;
		}
	}

	public bool _pIsMaxedOut
	{
		get
		{
			return (int)_maxValue > 0 && _displayValue == (int)_maxValue;
		}
	}

	public string _pLocalisedName
	{
		get
		{
			string text = LocalisationFacade.Instance.GetString("Currencies." + _type);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return _type.ToString();
		}
	}

	public Currency(CurrencyDefines.CurrencyType type)
	{
		_type = type;
		Load();
	}

	public void Load()
	{
		if (ObscuredPrefs.HasKey(_type.ToString()))
		{
			_displayValue = ObscuredPrefs.GetInt(_type.ToString());
			_currentValue = _displayValue;
		}
	}

	public void Save()
	{
		ClearPlayerPrefs();
		ObscuredPrefs.SetInt(_type.ToString(), _currentValue);
	}

	public void ClearPlayerPrefs()
	{
		if (ObscuredPrefs.HasKey(_type.ToString()))
		{
			ObscuredPrefs.DeleteKey(_type.ToString());
		}
	}

	public void AddToCurrency(int currencyToAdd)
	{
		if (currencyToAdd > 0)
		{
			_currentValue = (int)_currentValue + currencyToAdd;
			if ((int)_maxValue > 0)
			{
				_currentValue = Mathf.Min(_maxValue, _currentValue);
			}
			_displayValue = _currentValue;
			if (onChangeCallback != null)
			{
				onChangeCallback();
			}
			Save();
		}
	}

	public bool SubtractFromCurrency(int currencyToSubtract)
	{
		if ((int)_currentValue < currencyToSubtract)
		{
			return false;
		}
		_currentValue = (int)_currentValue - currencyToSubtract;
		_displayValue = _currentValue;
		if (onChangeCallback != null)
		{
			onChangeCallback();
		}
		Save();
		return true;
	}

	public void setLimit(int limit)
	{
		_maxValue = limit;
		_displayMax = limit;
		_currentValue = Mathf.Min(_maxValue, _currentValue);
		_displayValue = _currentValue;
	}
}
