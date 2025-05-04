using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Statistics
{
	public enum STAT_TYPE
	{
		BOOLEAN = 0,
		INTEGER = 1,
		FLOAT = 2,
		STRING = 3
	}

	public AchievementStatisticBoolean[] booleans;

	public AchievementStatisticFloat[] floats;

	public AchievementStatisticInteger[] integers;

	public AchievementStatisticString[] strings;

	private Dictionary<string, int> _indexLookup;

	private Dictionary<string, STAT_TYPE> _statTypeLookup;

	private bool _loadingData;

	public bool _pLoadingStats
	{
		get
		{
			return _loadingData;
		}
		set
		{
			_loadingData = value;
		}
	}

	public void Initialise()
	{
		_indexLookup = new Dictionary<string, int>();
		_statTypeLookup = new Dictionary<string, STAT_TYPE>();
		if (booleans != null)
		{
			for (int i = 0; i < booleans.Length; i++)
			{
				_indexLookup[booleans[i].statisticName] = i;
				_statTypeLookup[booleans[i].statisticName] = STAT_TYPE.BOOLEAN;
			}
		}
		if (floats != null)
		{
			for (int j = 0; j < floats.Length; j++)
			{
				_indexLookup[floats[j].statisticName] = j;
				_statTypeLookup[floats[j].statisticName] = STAT_TYPE.FLOAT;
			}
		}
		if (integers != null)
		{
			for (int k = 0; k < integers.Length; k++)
			{
				_indexLookup[integers[k].statisticName] = k;
				_statTypeLookup[integers[k].statisticName] = STAT_TYPE.INTEGER;
			}
		}
		if (strings != null)
		{
			for (int l = 0; l < strings.Length; l++)
			{
				_indexLookup[strings[l].statisticName] = l;
				_statTypeLookup[strings[l].statisticName] = STAT_TYPE.STRING;
			}
		}
	}

	public bool HasStat(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		return _indexLookup.ContainsKey(name);
	}

	public STAT_TYPE GetStatType(string name)
	{
		return _statTypeLookup[name];
	}

	public bool IsConst(string name)
	{
		switch (GetStatType(name))
		{
		case STAT_TYPE.BOOLEAN:
			return IsBooleanConst(name);
		case STAT_TYPE.INTEGER:
			return IsIntegerConst(name);
		case STAT_TYPE.STRING:
			return IsStringConst(name);
		case STAT_TYPE.FLOAT:
			return IsFloatConst(name);
		default:
			return false;
		}
	}

	public bool IsBooleanConst(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > booleans.Length || booleans[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return false;
		}
		return booleans[_indexLookup[name]].IsConst();
	}

	public void SetStatBoolean(string name, bool newValue)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > booleans.Length || booleans[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
		}
		else
		{
			booleans[_indexLookup[name]].SetCurrentStatValue(newValue);
		}
	}

	public bool GetStatBoolean(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > booleans.Length || booleans[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return false;
		}
		return booleans[_indexLookup[name]].GetStatCurrentValue();
	}

	public bool GetDefaultBoolean(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > booleans.Length || booleans[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return false;
		}
		return booleans[_indexLookup[name]].defaultStatValue;
	}

	public bool IsIntegerConst(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > integers.Length || integers[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return false;
		}
		return integers[_indexLookup[name]].IsConst();
	}

	public void SetStatInteger(string name, int newValue)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > integers.Length || integers[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
		}
		else
		{
			integers[_indexLookup[name]].SetCurrentStatValue(newValue);
		}
	}

	public void IncrementStatInteger(string name, int amount)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > integers.Length || integers[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return;
		}
		int statCurrentValue = integers[_indexLookup[name]].GetStatCurrentValue();
		integers[_indexLookup[name]].SetCurrentStatValue(statCurrentValue + amount);
	}

	public int GetStatInteger(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > integers.Length || integers[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return 0;
		}
		return integers[_indexLookup[name]].GetStatCurrentValue();
	}

	public int GetDefaultInteger(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > integers.Length || integers[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return 0;
		}
		return integers[_indexLookup[name]].defaultStatValue;
	}

	public bool IsFloatConst(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > floats.Length || floats[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return false;
		}
		return floats[_indexLookup[name]].IsConst();
	}

	public void SetStatFloat(string name, float newValue)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > floats.Length || floats[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
		}
		else
		{
			floats[_indexLookup[name]].SetCurrentStatValue(newValue);
		}
	}

	public float GetStatFloat(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > floats.Length || floats[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return 0f;
		}
		return floats[_indexLookup[name]].GetStatCurrentValue();
	}

	public float GetDefaultFloat(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > floats.Length || floats[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return 0f;
		}
		return floats[_indexLookup[name]].defaultStatValue;
	}

	public bool IsStringConst(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > strings.Length || strings[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return false;
		}
		return strings[_indexLookup[name]].IsConst();
	}

	public void SetStatString(string name, string newValue)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > strings.Length || strings[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
		}
		else
		{
			strings[_indexLookup[name]].SetCurrentStatValue(newValue);
		}
	}

	public string GetStatString(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > strings.Length || strings[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return null;
		}
		return strings[_indexLookup[name]].GetStatCurrentValue();
	}

	public string GetDefaultString(string name)
	{
		if (_indexLookup == null)
		{
			Initialise();
		}
		if (!_indexLookup.ContainsKey(name) || _indexLookup[name] > strings.Length || strings[_indexLookup[name]] == null)
		{
			Debug.LogWarning("Could not find Statistic: " + name);
			return null;
		}
		return strings[_indexLookup[name]].defaultStatValue;
	}

	public float GetSumValue(StatisticRulePair pair)
	{
		switch (pair.valAType)
		{
		case STAT_TYPE.BOOLEAN:
			return (!GetStatBoolean(pair.valAName)) ? 0f : 1f;
		case STAT_TYPE.FLOAT:
			return GetStatFloat(pair.valAName);
		case STAT_TYPE.INTEGER:
			return GetStatInteger(pair.valAName);
		default:
			return 0f;
		}
	}

	public bool HasStatRuleBeenMet(StatisticRulePair pair)
	{
		if (pair.valAType == pair.valBType)
		{
			switch (pair.valAType)
			{
			case STAT_TYPE.BOOLEAN:
			{
				bool statBoolean = GetStatBoolean(pair.valAName);
				bool statBoolean2 = GetStatBoolean(pair.valBName);
				return EvaluateBool(statBoolean, statBoolean2, pair.rule);
			}
			case STAT_TYPE.FLOAT:
			{
				float statFloat = GetStatFloat(pair.valAName);
				float statFloat2 = GetStatFloat(pair.valBName);
				return EvaluateFloat(statFloat, statFloat2, pair.rule);
			}
			case STAT_TYPE.INTEGER:
			{
				int statInteger = GetStatInteger(pair.valAName);
				int statInteger2 = GetStatInteger(pair.valBName);
				return EvaluateInt(statInteger, statInteger2, pair.rule);
			}
			case STAT_TYPE.STRING:
			{
				string statString = GetStatString(pair.valAName);
				string statString2 = GetStatString(pair.valBName);
				return EvaluateString(statString, statString2, pair.rule);
			}
			default:
				return false;
			}
		}
		string a = string.Empty;
		string b = string.Empty;
		switch (pair.valAType)
		{
		case STAT_TYPE.BOOLEAN:
			a = GetStatBoolean(pair.valAName).ToString();
			break;
		case STAT_TYPE.FLOAT:
			a = GetStatFloat(pair.valAName).ToString();
			break;
		case STAT_TYPE.INTEGER:
			a = GetStatInteger(pair.valAName).ToString();
			break;
		case STAT_TYPE.STRING:
			a = GetStatString(pair.valAName);
			break;
		}
		switch (pair.valBType)
		{
		case STAT_TYPE.BOOLEAN:
			b = GetStatBoolean(pair.valBName).ToString();
			break;
		case STAT_TYPE.FLOAT:
			b = GetStatFloat(pair.valBName).ToString();
			break;
		case STAT_TYPE.INTEGER:
			b = GetStatInteger(pair.valBName).ToString();
			break;
		case STAT_TYPE.STRING:
			b = GetStatString(pair.valBName);
			break;
		}
		return EvaluateString(a, b, pair.rule);
	}

	private bool EvaluateBool(bool A, bool B, StatisticRulePair.RULE rule)
	{
		switch (rule)
		{
		case StatisticRulePair.RULE.EQUAL:
			return A == B;
		case StatisticRulePair.RULE.GREATER_THAN:
			return false;
		case StatisticRulePair.RULE.GREATER_THAN_EQUALTO:
			return false;
		case StatisticRulePair.RULE.LESS_THAN:
			return false;
		case StatisticRulePair.RULE.LESS_THAN_EQUALTO:
			return false;
		case StatisticRulePair.RULE.NOTEQUAL:
			return A != B;
		default:
			return false;
		}
	}

	private bool EvaluateInt(int A, int B, StatisticRulePair.RULE rule)
	{
		switch (rule)
		{
		case StatisticRulePair.RULE.EQUAL:
			return A == B;
		case StatisticRulePair.RULE.GREATER_THAN:
			return A > B;
		case StatisticRulePair.RULE.GREATER_THAN_EQUALTO:
			return A >= B;
		case StatisticRulePair.RULE.LESS_THAN:
			return A < B;
		case StatisticRulePair.RULE.LESS_THAN_EQUALTO:
			return A <= B;
		case StatisticRulePair.RULE.NOTEQUAL:
			return A != B;
		default:
			return false;
		}
	}

	private bool EvaluateFloat(float A, float B, StatisticRulePair.RULE rule)
	{
		switch (rule)
		{
		case StatisticRulePair.RULE.EQUAL:
			return A == B;
		case StatisticRulePair.RULE.GREATER_THAN:
			return A > B;
		case StatisticRulePair.RULE.GREATER_THAN_EQUALTO:
			return A >= B;
		case StatisticRulePair.RULE.LESS_THAN:
			return A < B;
		case StatisticRulePair.RULE.LESS_THAN_EQUALTO:
			return A <= B;
		case StatisticRulePair.RULE.NOTEQUAL:
			return A != B;
		default:
			return false;
		}
	}

	private bool EvaluateString(string A, string B, StatisticRulePair.RULE rule)
	{
		switch (rule)
		{
		case StatisticRulePair.RULE.EQUAL:
			return A == B;
		case StatisticRulePair.RULE.GREATER_THAN:
			return A.CompareTo(B) > 0;
		case StatisticRulePair.RULE.GREATER_THAN_EQUALTO:
			return A.CompareTo(B) >= 0;
		case StatisticRulePair.RULE.LESS_THAN:
			return A.CompareTo(B) < 0;
		case StatisticRulePair.RULE.LESS_THAN_EQUALTO:
			return A.CompareTo(B) <= 0;
		case StatisticRulePair.RULE.NOTEQUAL:
			return A != B;
		default:
			return false;
		}
	}
}
