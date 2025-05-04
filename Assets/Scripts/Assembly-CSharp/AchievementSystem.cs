using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
	public interface IAchievementListener
	{
		bool HasGameObject();

		void OnStatChanged(string statName);

		void OnAchievementChanged(string achievementName, Achievement achi);

		void OnAchievementCompleted(string achievementName, Achievement achi);
	}

	private static AchievementSystem _instance;

	public AchievementCategory[] categories;

	public Achievement[] achievements;

	public Statistics stats;

	private Dictionary<string, int> _achievementLookup;

	private Dictionary<string, List<int>> _achievementStatUsageLookup;

	private Dictionary<string, List<int>> _achievementCategoryLookup;

	private bool _isInitialising;

	private bool _hasInitialised;

	private string _loadingString;

	private List<IAchievementListener> _listeners = new List<IAchievementListener>();

	[Obsolete("Instance is deprecated, please use _pInstance instead.")]
	public static AchievementSystem Instance
	{
		get
		{
			return _instance;
		}
	}

	public static AchievementSystem _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public bool _pHasInitialised
	{
		get
		{
			return _hasInitialised;
		}
	}

	public void Load(JsonData data)
	{
		if (_achievementLookup == null)
		{
			Initialise();
		}
		stats._pLoadingStats = true;
		SetToDefaults();
		if (data == null)
		{
			Debug.Log("Resetting Achievement System");
			stats._pLoadingStats = false;
			return;
		}
		if (data.Contains("stats"))
		{
			JsonData jsonData = data["stats"];
			int count = jsonData.Count;
			for (int i = 0; i < count; i++)
			{
				string text = (string)jsonData[i]["name"];
				if (stats.HasStat(text))
				{
					Statistics.STAT_TYPE statType = stats.GetStatType(text);
					JsonData jsonData2 = jsonData[i]["value"];
					switch (statType)
					{
					case Statistics.STAT_TYPE.BOOLEAN:
						if (!jsonData2.IsBoolean)
						{
							Debug.LogWarning("[" + text + "] load failed, expecting BOOLEAN, found " + jsonData2.GetJsonType());
						}
						else
						{
							stats.SetStatBoolean(text, (bool)jsonData2);
						}
						break;
					case Statistics.STAT_TYPE.FLOAT:
						if (!jsonData2.IsDouble)
						{
							Debug.LogWarning("[" + text + "] load failed, expecting FLOAT, found " + jsonData2.GetJsonType());
						}
						else
						{
							stats.SetStatFloat(text, (float)(double)jsonData2);
						}
						break;
					case Statistics.STAT_TYPE.INTEGER:
						if (!jsonData2.IsInt)
						{
							Debug.LogWarning("[" + text + "] load failed, expecting INTEGER, found " + jsonData2.GetJsonType());
						}
						else
						{
							stats.SetStatInteger(text, (int)jsonData2);
						}
						break;
					case Statistics.STAT_TYPE.STRING:
						if (!jsonData2.IsString)
						{
							Debug.LogWarning("[" + text + "] load failed, expecting STRING, found " + jsonData2.GetJsonType());
						}
						else
						{
							stats.SetStatString(text, (string)jsonData2);
						}
						break;
					}
				}
				else
				{
					Debug.LogWarning("Stat '" + text + "' does not exist in the achievement system, did not load this save data stat");
				}
			}
		}
		if (data.Contains("achievements"))
		{
			JsonData jsonData3 = data["achievements"];
			if (jsonData3 != null)
			{
				int count2 = jsonData3.Count;
				Debug.Log("Loading " + count2 + " achievements...");
				for (int j = 0; j < count2; j++)
				{
					string text2 = (string)jsonData3[j]["name"];
					if (HasAchievement(text2))
					{
						Achievement achievement = GetAchievement(text2);
						string completedTimeStamp = (string)jsonData3[j]["value"];
						achievement.completed = true;
						achievement.completedTimeStamp = completedTimeStamp;
					}
					else
					{
						Debug.LogWarning("Achievement '" + text2 + "' does not exist in the achievement system, did not load save data for this achievement");
					}
				}
			}
		}
		stats._pLoadingStats = false;
		Debug.Log("AchievementSystem Loading Complete");
	}

	public string GetSaveString()
	{
		JsonData jsonData = new JsonData();
		int num = 0;
		num += CountStatisticArray(stats.booleans);
		num += CountStatisticArray(stats.integers);
		num += CountStatisticArray(stats.floats);
		num += CountStatisticArray(stats.strings);
		if (num > 0)
		{
			jsonData["stats"] = new JsonData();
			AddStatisticArray(jsonData["stats"], stats.booleans);
			AddStatisticArray(jsonData["stats"], stats.integers);
			AddStatisticArray(jsonData["stats"], stats.floats);
			AddStatisticArray(jsonData["stats"], stats.strings);
		}
		if (achievements != null)
		{
			int num2 = achievements.Length;
			bool flag = false;
			for (int i = 0; i < num2; i++)
			{
				if (achievements[i].completed)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				jsonData["achievements"] = new JsonData();
				for (int j = 0; j < num2; j++)
				{
					Achievement achievement = achievements[j];
					if (achievement.completed)
					{
						JsonData jsonData2 = new JsonData();
						jsonData2["name"] = achievement.achievementName;
						jsonData2["value"] = achievement.completedTimeStamp;
						jsonData["achievements"].Add(jsonData2);
					}
				}
			}
		}
		return jsonData.ToJson(true, true);
	}

	private int CountStatisticArray<T>(T[] array) where T : AchievementStatistic
	{
		int num = 0;
		if (array != null)
		{
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				T val = array[i];
				if (!val.IsConst())
				{
					num++;
				}
			}
		}
		return num;
	}

	private void AddStatisticArray<T>(JsonData statRoot, T[] array) where T : AchievementStatistic
	{
		if (array == null)
		{
			return;
		}
		Type typeFromHandle = typeof(AchievementStatisticBoolean);
		Type typeFromHandle2 = typeof(AchievementStatisticInteger);
		Type typeFromHandle3 = typeof(AchievementStatisticString);
		Type typeFromHandle4 = typeof(AchievementStatisticFloat);
		Type typeFromHandle5 = typeof(T);
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			T val = array[i];
			if (!val.IsConst())
			{
				JsonData jsonData = new JsonData();
				jsonData["name"] = val.statisticName;
				if (typeFromHandle5 == typeFromHandle)
				{
					jsonData["value"] = (bool)val.value;
				}
				else if (typeFromHandle5 == typeFromHandle2)
				{
					jsonData["value"] = (int)val.value;
				}
				else if (typeFromHandle5 == typeFromHandle3)
				{
					jsonData["value"] = (string)val.value;
				}
				else if (typeFromHandle5 == typeFromHandle4)
				{
					jsonData["value"] = (float)val.value;
				}
				statRoot.Add(jsonData);
			}
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	public void Initialise()
	{
		_isInitialising = true;
		stats.Initialise();
		_achievementLookup = new Dictionary<string, int>();
		_achievementStatUsageLookup = new Dictionary<string, List<int>>();
		_achievementCategoryLookup = new Dictionary<string, List<int>>();
		for (int i = 0; i < achievements.Length; i++)
		{
			_achievementLookup[achievements[i].achievementName] = i;
			List<string> list = new List<string>();
			if (achievements[i].rulePairs != null)
			{
				for (int j = 0; j < achievements[i].rulePairs.Length; j++)
				{
					if (!list.Contains(achievements[i].rulePairs[j].valAName))
					{
						list.Add(achievements[i].rulePairs[j].valAName);
					}
					if (!list.Contains(achievements[i].rulePairs[j].valBName))
					{
						list.Add(achievements[i].rulePairs[j].valBName);
					}
				}
			}
			if (achievements[i].category != null)
			{
				if (!_achievementCategoryLookup.ContainsKey(achievements[i].category))
				{
					_achievementCategoryLookup[achievements[i].category] = new List<int>();
				}
				_achievementCategoryLookup[achievements[i].category].Add(i);
			}
			for (int k = 0; k < list.Count; k++)
			{
				if (!_achievementStatUsageLookup.ContainsKey(list[k]))
				{
					_achievementStatUsageLookup[list[k]] = new List<int>();
				}
				_achievementStatUsageLookup[list[k]].Add(i);
			}
		}
		if (Application.isPlaying)
		{
			for (int l = 0; l < achievements.Length; l++)
			{
				if (!achievements[l].completed && EvaluateAchievement(achievements[l].achievementName))
				{
					achievements[l].MarkAsComplete();
				}
			}
		}
		_isInitialising = false;
		_hasInitialised = true;
	}

	public void SetToDefaults()
	{
		if (stats.booleans != null)
		{
			for (int i = 0; i < stats.booleans.Length; i++)
			{
				stats.booleans[i].SetCurrentStatValue(stats.booleans[i].GetStatDefaultValue());
			}
		}
		if (stats.floats != null)
		{
			for (int j = 0; j < stats.floats.Length; j++)
			{
				stats.floats[j].SetCurrentStatValue(stats.floats[j].GetStatDefaultValue());
			}
		}
		if (stats.integers != null)
		{
			for (int k = 0; k < stats.integers.Length; k++)
			{
				stats.integers[k].SetCurrentStatValue(stats.integers[k].GetStatDefaultValue());
			}
		}
		if (stats.strings != null)
		{
			for (int l = 0; l < stats.strings.Length; l++)
			{
				stats.strings[l].SetCurrentStatValue(stats.strings[l].GetStatDefaultValue());
			}
		}
		for (int m = 0; m < achievements.Length; m++)
		{
			achievements[m].completed = false;
			achievements[m].completedTimeStamp = string.Empty;
		}
	}

	public bool HasAchievement(string name)
	{
		if (_achievementLookup == null)
		{
			return false;
		}
		return _achievementLookup.ContainsKey(name);
	}

	public Achievement GetAchievement(string name)
	{
		return achievements[_achievementLookup[name]];
	}

	public bool EvaluateAchievement(string name, bool forceRecheck = false)
	{
		if (_achievementLookup == null)
		{
			Initialise();
		}
		if (!_achievementLookup.ContainsKey(name))
		{
			Debug.LogWarning("Tried to evaluate Achievement: " + name + " but no such achievement exists");
			return false;
		}
		Achievement achievement = achievements[_achievementLookup[name]];
		if (achievement.completed && !forceRecheck)
		{
			return true;
		}
		if (achievement.rulePairs == null)
		{
			return false;
		}
		if (achievement.rulePairs.Length == 0)
		{
			return false;
		}
		bool flag = false;
		float num = 0f;
		for (int i = 0; i < achievement.rulePairs.Length; i++)
		{
			if (achievement.rulePairs[i].rule == StatisticRulePair.RULE.SUM_GREATER_EQUALTO)
			{
				flag = true;
				switch (achievement.rulePairs[i].valBType)
				{
				case Statistics.STAT_TYPE.BOOLEAN:
					num = ((!stats.GetStatBoolean(achievement.rulePairs[i].valBName)) ? 0f : 1f);
					break;
				case Statistics.STAT_TYPE.INTEGER:
					num = stats.GetStatInteger(achievement.rulePairs[i].valBName);
					break;
				case Statistics.STAT_TYPE.FLOAT:
					num = stats.GetStatFloat(achievement.rulePairs[i].valBName);
					break;
				}
			}
		}
		if (flag)
		{
			float num2 = 0f;
			for (int j = 0; j < achievement.rulePairs.Length; j++)
			{
				if (achievement.rulePairs[j].rule != StatisticRulePair.RULE.SUM_GREATER_EQUALTO)
				{
					num2 += stats.GetSumValue(achievement.rulePairs[j]);
				}
			}
			bool flag2 = num2 >= num;
			if (forceRecheck && achievement.completed)
			{
				achievement.completed = flag2;
			}
			return flag2;
		}
		for (int k = 0; k < achievement.rulePairs.Length; k++)
		{
			if (!stats.HasStatRuleBeenMet(achievement.rulePairs[k]))
			{
				if (forceRecheck && achievement.completed)
				{
					achievement.completed = false;
					Debug.Log("Achi: " + achievement.achievementName + " has become 'uncompleted' after a forced recheck");
				}
				return false;
			}
		}
		return true;
	}

	public bool HasCategory(string name)
	{
		return _achievementCategoryLookup.ContainsKey(name);
	}

	public int[] GetAchievementIndexesInCategory(string name)
	{
		if (_achievementCategoryLookup == null)
		{
			return null;
		}
		if (!_achievementCategoryLookup.ContainsKey(name))
		{
			return null;
		}
		return _achievementCategoryLookup[name].ToArray();
	}

	public bool HasAchievementCompleted(string name)
	{
		if (_achievementLookup == null)
		{
			Initialise();
		}
		if (!_achievementLookup.ContainsKey(name))
		{
			Debug.LogWarning("Achievement: " + name + " does not exist, cannot check if it has been completed.");
			return false;
		}
		return achievements[_achievementLookup[name]].completed;
	}

	public void RegisterForNotifications(IAchievementListener otherObject)
	{
		if (!_listeners.Contains(otherObject))
		{
			_listeners.Add(otherObject);
		}
	}

	public void UnregisterForNotifications(IAchievementListener otherObject)
	{
		if (_listeners.Contains(otherObject))
		{
			_listeners.Remove(otherObject);
		}
	}

	public void OnStatChanged(string statName)
	{
		if (stats._pLoadingStats)
		{
			return;
		}
		if (_achievementStatUsageLookup != null && _achievementStatUsageLookup.ContainsKey(statName))
		{
			int count = _achievementStatUsageLookup[statName].Count;
			for (int i = 0; i < count; i++)
			{
				achievements[_achievementStatUsageLookup[statName][i]].NotifyOfChange(statName);
			}
		}
		_listeners.RemoveAll((IAchievementListener item) => item == null);
		int count2 = _listeners.Count;
		for (int num = 0; num < count2; num++)
		{
			if (_listeners[num] != null && _listeners[num].HasGameObject())
			{
				_listeners[num].OnStatChanged(statName);
			}
		}
	}

	public void OnAchievementChanged(string achievementName, Achievement achi, bool force = false)
	{
		if (_isInitialising || stats._pLoadingStats || !Application.isPlaying || achi.completed || (!force && !EvaluateAchievement(achievementName)))
		{
			return;
		}
		Debug.Log("Achi " + achievementName + " was completed");
		achi.MarkAsComplete();
		_listeners.RemoveAll((IAchievementListener item) => item == null);
		int count = _listeners.Count;
		for (int num = 0; num < count; num++)
		{
			if (_listeners[num] != null && _listeners[num].HasGameObject())
			{
				_listeners[num].OnAchievementChanged(achievementName, achi);
			}
		}
	}

	public int GetNumAchieved()
	{
		int num = achievements.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (!achievements[i].completed && EvaluateAchievement(achievements[i].achievementName))
			{
				achievements[i].MarkAsComplete();
			}
			if (achievements[i].completed)
			{
				num2++;
			}
		}
		return num2;
	}

	public void OnAchievementCompleted(string achievementName, Achievement achi)
	{
		if (_isInitialising || stats._pLoadingStats)
		{
			return;
		}
		_listeners.RemoveAll((IAchievementListener item) => item == null);
		int count = _listeners.Count;
		Debug.Log("Achievement completed: " + achievementName + " listeners=" + count);
		for (int num = 0; num < count; num++)
		{
			if (_listeners[num] != null && _listeners[num].HasGameObject())
			{
				_listeners[num].OnAchievementCompleted(achievementName, achi);
			}
		}
	}
}
