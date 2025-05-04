using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class QuestSystem : MonoBehaviour, AchievementSystem.IAchievementListener
{
	public delegate void OnQuestCompleted(Achievement achievement);

	private static QuestSystem _instance;

	public int _numQuestsToPresentSimultaneously = 3;

	public bool _findNewQuestsOnAwake;

	public int _numQuestsToFindOnAwake = -1;

	private string[] _currentList;

	private string[] _achievementNameList;

	private int[] _ticketList;

	private bool[] _completedQuests;

	private bool[] _dummyBools;

	private int[] _dummyInts;

	private int _numCompletedQuests;

	private Dictionary<string, int> _ticketLookup = new Dictionary<string, int>();

	private Dictionary<string, int> _questIndexLookup = new Dictionary<string, int>();

	private int _totalTicketSize;

	private List<string> _dummyStringList = new List<string>();

	private List<string> _currentStatNames = new List<string>();

	private QuestAvailabilityTable _availableLUT = new QuestAvailabilityTable();

	public static QuestSystem _pInstance
	{
		get
		{
			return _instance;
		}
	}

	public string[] _pCurrentList
	{
		get
		{
			return _currentList;
		}
	}

	private event OnQuestCompleted _onQuestCompletedEvent;

	private void Awake()
	{
		_instance = this;
		_currentList = new string[_numQuestsToPresentSimultaneously];
		_availableLUT.Initialise();
		for (int i = 0; i < _numQuestsToPresentSimultaneously; i++)
		{
			_currentList[i] = null;
		}
		if (AchievementSystem._pInstance == null)
		{
			Debug.LogError("QuestSystem relies on Achievement System, if QuestSystem is in the same scene as the Achievement System, please ensure the execution order of the systems are enforced");
			base.enabled = false;
			return;
		}
		if (!AchievementSystem._pInstance._pHasInitialised)
		{
			AchievementSystem._pInstance.Initialise();
		}
		AchievementSystem._pInstance.RegisterForNotifications(this);
		if (_findNewQuestsOnAwake)
		{
			FindNewQuests(_numQuestsToFindOnAwake);
		}
	}

	public string GetQuestName(int element)
	{
		int num = 0;
		for (int i = 0; i < _numQuestsToPresentSimultaneously; i++)
		{
			if (_currentList[i] != null)
			{
				if (num == element)
				{
					return _currentList[i];
				}
				num++;
			}
		}
		return null;
	}

	public string GetSaveString()
	{
		JsonData jsonData = new JsonData();
		JsonData jsonData2 = new JsonData();
		for (int i = 0; i < _numQuestsToPresentSimultaneously; i++)
		{
			if (_currentList[i] != null)
			{
				jsonData2.Add(_currentList[i]);
			}
			else
			{
				jsonData2.Add("NULL");
			}
		}
		JsonData jsonData3 = new JsonData();
		bool flag = false;
		int num = _achievementNameList.Length;
		for (int j = 0; j < num; j++)
		{
			if (_completedQuests[j])
			{
				flag = true;
				jsonData3.Add(_achievementNameList[j]);
			}
		}
		jsonData["currentQuests"] = jsonData2;
		if (flag)
		{
			jsonData["completedQuests"] = jsonData3;
		}
		return jsonData.ToJson();
	}

	public void Load(JsonData data)
	{
		if (data == null)
		{
			Debug.Log("Quest System recieved null data to load, skipping");
			return;
		}
		Initialise();
		if (data.Contains("currentQuests"))
		{
			int count = data["currentQuests"].Count;
			for (int i = 0; i < count && i < _numQuestsToPresentSimultaneously; i++)
			{
				if ((string)data["currentQuests"][i] != "NULL")
				{
					_currentList[i] = (string)data["currentQuests"][i];
				}
			}
		}
		if (data.Contains("completedQuests"))
		{
			int count2 = data["completedQuests"].Count;
			int num = _completedQuests.Length;
			for (int j = 0; j < num; j++)
			{
				_completedQuests[j] = false;
			}
			int num2 = 0;
			for (int k = 0; k < count2; k++)
			{
				string key = (string)data["completedQuests"][k];
				if (_questIndexLookup.ContainsKey(key))
				{
					_completedQuests[_questIndexLookup[key]] = true;
					num2++;
				}
			}
			_numCompletedQuests = num2;
		}
		GenerateCurrentStatNameList();
	}

	public void ResetAll()
	{
		int num = _currentList.Length;
		for (int i = 0; i < num; i++)
		{
			_currentList[i] = null;
			_completedQuests[i] = false;
		}
		_numCompletedQuests = 0;
		AchievementSystem._pInstance.SetToDefaults();
	}

	public int GetNumActiveQuests()
	{
		int num = 0;
		for (int i = 0; i < _numQuestsToPresentSimultaneously; i++)
		{
			if (_currentList[i] != null)
			{
				num++;
			}
		}
		return num;
	}

	public void RegisterForQuestCompleteNotifications(OnQuestCompleted handler)
	{
		this._onQuestCompletedEvent = (OnQuestCompleted)Delegate.Combine(this._onQuestCompletedEvent, handler);
	}

	public void UnRegisterForQuestCompleteNotifications(OnQuestCompleted handler)
	{
		this._onQuestCompletedEvent = (OnQuestCompleted)Delegate.Remove(this._onQuestCompletedEvent, handler);
	}

	public void SetStatBoolean(string statName, bool newValue)
	{
		if (ListContains(_currentStatNames, statName))
		{
			AchievementSystem._pInstance.stats.SetStatBoolean(statName, newValue);
		}
		else
		{
			Debug.Log("Stat: " + statName + " is not in use by an active quest, skipping");
		}
	}

	public void SetStatInteger(string statName, int newValue)
	{
		if (ListContains(_currentStatNames, statName))
		{
			AchievementSystem._pInstance.stats.SetStatInteger(statName, newValue);
		}
		else
		{
			Debug.Log("Stat: " + statName + " is not in use by an active quest, skipping");
		}
	}

	public void IncrementStatInteger(string statName, int amount)
	{
		if (ListContains(_currentStatNames, statName))
		{
			AchievementSystem._pInstance.stats.IncrementStatInteger(statName, amount);
		}
		else
		{
			Debug.Log("Stat: " + statName + " is not in use by an active quest, skipping");
		}
	}

	public void SetStatFloat(string statName, float newValue)
	{
		if (ListContains(_currentStatNames, statName))
		{
			AchievementSystem._pInstance.stats.SetStatFloat(statName, newValue);
		}
		else
		{
			Debug.Log("Stat: " + statName + " is not in use by an active quest, skipping");
		}
	}

	public void SetStatString(string statName, string newValue)
	{
		if (ListContains(_currentStatNames, statName))
		{
			AchievementSystem._pInstance.stats.SetStatString(statName, newValue);
		}
		else
		{
			Debug.Log("Stat: " + statName + " is not in use by an active quest, skipping");
		}
	}

	public int FindNewQuests(int numToFind = -1)
	{
		if (numToFind == 0)
		{
			return 0;
		}
		int num = AchievementSystem._pInstance.achievements.Length;
		Initialise();
		int num2 = 0;
		for (int i = 0; i < _numQuestsToPresentSimultaneously; i++)
		{
			if (_currentList[i] == null)
			{
				num2++;
			}
		}
		int num3 = _numQuestsToPresentSimultaneously - num2;
		int num4 = num - _numCompletedQuests - num3;
		Debug.Log("Need to find: " + num2 + " quests out of " + num4 + " available quests [" + num3 + " already slotted]");
		if (num4 < num2)
		{
			Debug.Log("Need to start recycling quests!");
			FindQuestsToRecycle();
		}
		int num5 = 0;
		int num6 = 0;
		for (int j = 0; j < num; j++)
		{
			if (_completedQuests[j])
			{
				_dummyBools[j] = false;
				num6 += _ticketList[j];
			}
			else if (_ticketList[j] > 0 && _availableLUT.IsAvailable(_achievementNameList[j]))
			{
				_dummyBools[j] = true;
			}
			else
			{
				_dummyBools[j] = false;
				num5 += _ticketList[j];
			}
		}
		int num7 = 0;
		for (int k = 0; k < _numQuestsToPresentSimultaneously; k++)
		{
			if (_currentList[k] != null)
			{
				continue;
			}
			if (num7 == numToFind)
			{
				break;
			}
			int num8 = _totalTicketSize - num6 - num5;
			for (int l = 0; l < _numQuestsToPresentSimultaneously; l++)
			{
				if (_currentList[l] != null)
				{
					num8 -= _ticketLookup[_currentList[l]];
				}
			}
			int num9 = UnityEngine.Random.Range(0, num8);
			Debug.Log("Diceroll = " + num9 + " of " + num8);
			string text = RetrieveAchiFromTicketPool(num9);
			Debug.Log("Assigned " + text + " to quest slot " + k);
			_currentList[k] = text;
			num7++;
		}
		GenerateCurrentStatNameList();
		return num7;
	}

	private void Initialise()
	{
		int num = AchievementSystem._pInstance.achievements.Length;
		if (_achievementNameList != null)
		{
			return;
		}
		_achievementNameList = new string[num];
		_ticketList = new int[num];
		_completedQuests = new bool[num];
		_dummyBools = new bool[num];
		_dummyInts = new int[num];
		_numCompletedQuests = 0;
		for (int i = 0; i < num; i++)
		{
			string achievementName = AchievementSystem._pInstance.achievements[i].achievementName;
			_achievementNameList[i] = achievementName;
			_ticketList[i] = 1;
			_completedQuests[i] = false;
			string metaString = AchievementSystem._pInstance.achievements[i].GetMetaString("NOQUEST");
			string metaString2 = AchievementSystem._pInstance.achievements[i].GetMetaString("QUESTWEIGHT");
			int result = 0;
			if (int.TryParse(metaString2, out result))
			{
				_ticketList[i] = result;
			}
			if (metaString != "INVALID")
			{
				_ticketList[i] = 0;
			}
			_totalTicketSize += _ticketList[i];
			_ticketLookup[achievementName] = _ticketList[i];
			_questIndexLookup[achievementName] = i;
		}
	}

	private void GenerateCurrentStatNameList()
	{
		_currentStatNames.Clear();
		for (int i = 0; i < _numQuestsToPresentSimultaneously; i++)
		{
			if (_currentList[i] == null)
			{
				continue;
			}
			Achievement achievement = AchievementSystem._pInstance.GetAchievement(_currentList[i]);
			int num = achievement.rulePairs.Length;
			for (int j = 0; j < num; j++)
			{
				if (!AchievementSystem._pInstance.stats.IsConst(achievement.rulePairs[j].valAName) && !ListContains(_currentStatNames, achievement.rulePairs[j].valAName))
				{
					_currentStatNames.Add(achievement.rulePairs[j].valAName);
				}
				if (!AchievementSystem._pInstance.stats.IsConst(achievement.rulePairs[j].valBName) && !ListContains(_currentStatNames, achievement.rulePairs[j].valBName))
				{
					_currentStatNames.Add(achievement.rulePairs[j].valBName);
				}
			}
		}
	}

	private bool DoesAchiUseStatFromList(Achievement achi, List<string> statList)
	{
		int num = achi.rulePairs.Length;
		for (int i = 0; i < num; i++)
		{
			if (!AchievementSystem._pInstance.stats.IsConst(achi.rulePairs[i].valAName) && ListContains(statList, achi.rulePairs[i].valAName))
			{
				return true;
			}
			if (!AchievementSystem._pInstance.stats.IsConst(achi.rulePairs[i].valBName) && ListContains(statList, achi.rulePairs[i].valBName))
			{
				return true;
			}
		}
		return false;
	}

	private void FindQuestsToRecycle(int numToFind = -1)
	{
		Debug.Log("Checking for possible recyclable slots, only checking items that don't share stats with current quests");
		GenerateCurrentStatNameList();
		int num = 0;
		for (int i = 0; i < _achievementNameList.Length; i++)
		{
			_dummyBools[i] = _completedQuests[i];
			if (_ticketList[i] == 0)
			{
				_dummyBools[i] = false;
			}
			if (!_dummyBools[i])
			{
				continue;
			}
			if (!AchievementSystem._pInstance.EvaluateAchievement(_achievementNameList[i], true))
			{
				Achievement achievement = AchievementSystem._pInstance.GetAchievement(_achievementNameList[i]);
				if (!DoesAchiUseStatFromList(achievement, _currentStatNames))
				{
					if (_availableLUT.IsAvailable(_achievementNameList[i]))
					{
						num += _ticketList[i];
						continue;
					}
					_dummyBools[i] = false;
					Debug.Log("Would've considered " + _achievementNameList[i] + " but it is currently unavailable");
				}
				else
				{
					_dummyBools[i] = false;
					Debug.Log("Would've considered " + _achievementNameList[i] + " but it has stats that are in use by other quests, so potentially already has some progress, disregarding (for now)");
				}
			}
			else
			{
				Debug.Log("Would've considered " + _achievementNameList[i] + " but it's fully completed disregarding (for now)");
				_dummyBools[i] = false;
			}
		}
		int num2 = 0;
		int num3 = 0;
		for (int j = 0; j < _numQuestsToPresentSimultaneously; j++)
		{
			if (_currentList[j] != null)
			{
				continue;
			}
			if (num3 == numToFind)
			{
				break;
			}
			int num4 = UnityEngine.Random.Range(0, num);
			int num5 = 0;
			int num6 = _ticketList.Length;
			for (int k = 0; k < num6; k++)
			{
				if (_dummyBools[k])
				{
					if (num5 >= num4 && _ticketList[k] != 0)
					{
						Debug.Log("Freeing completed quest: " + _achievementNameList[k] + " as a free quest");
						num -= _ticketList[k];
						_completedQuests[k] = false;
						_dummyBools[k] = false;
						_numCompletedQuests--;
						break;
					}
					num5 += _ticketList[k];
				}
			}
			if (_currentList[j] == null)
			{
				num2++;
			}
			else
			{
				num3++;
			}
		}
		if (num2 <= 0)
		{
			return;
		}
		Debug.Log("Checking for possible recyclable slots, ALLOWING items that share stats with current quests, but not those already complete");
		num = 0;
		for (int l = 0; l < _achievementNameList.Length; l++)
		{
			_dummyBools[l] = _completedQuests[l];
			if (_ticketList[l] == 0)
			{
				_dummyBools[l] = false;
			}
			if (!_dummyBools[l])
			{
				continue;
			}
			if (!AchievementSystem._pInstance.EvaluateAchievement(_achievementNameList[l], true))
			{
				if (_availableLUT.IsAvailable(_achievementNameList[l]))
				{
					num += _ticketList[l];
					continue;
				}
				_dummyBools[l] = false;
				Debug.Log("Would've considered " + _achievementNameList[l] + " but it is currently unavailable");
			}
			else
			{
				Debug.Log("Would've considered " + _achievementNameList[l] + " but it's fully completed in the achievement system still");
				_dummyBools[l] = false;
			}
		}
		for (int m = 0; m < _numQuestsToPresentSimultaneously; m++)
		{
			if (_currentList[m] != null)
			{
				continue;
			}
			int num7 = UnityEngine.Random.Range(0, num);
			int num8 = 0;
			int num9 = _ticketList.Length;
			for (int n = 0; n < num9; n++)
			{
				if (_dummyBools[n])
				{
					if (num8 >= num7 && _ticketList[n] != 0)
					{
						Debug.Log("Freeing completed quest: " + _achievementNameList[n] + " as a free quest");
						num -= _ticketList[n];
						_completedQuests[n] = false;
						_dummyBools[n] = false;
						_numCompletedQuests--;
						break;
					}
					num8 += _ticketList[n];
				}
			}
		}
	}

	private string RetrieveAchiFromTicketPool(int winningTicket)
	{
		int num = 0;
		int num2 = _ticketList.Length;
		for (int i = 0; i < num2; i++)
		{
			bool flag = IsActiveQuest(_achievementNameList[i]);
			bool flag2 = _completedQuests[i];
			bool flag3 = _dummyBools[i];
			if (!flag && !flag2)
			{
				if (num >= winningTicket && _ticketList[i] != 0 && flag3)
				{
					return _achievementNameList[i];
				}
				if (!flag && flag3)
				{
					num += _ticketList[i];
				}
			}
		}
		return null;
	}

	public bool IsActiveQuest(string achiName)
	{
		if (_currentList == null)
		{
			return false;
		}
		for (int i = 0; i < _numQuestsToPresentSimultaneously; i++)
		{
			if (_currentList[i] == achiName)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasGameObject()
	{
		return true;
	}

	public void OnStatChanged(string statName)
	{
	}

	public void OnAchievementChanged(string achievementName, Achievement achi)
	{
	}

	public void OnAchievementCompleted(string achievementName, Achievement achi)
	{
		Debug.Log("Quest System -- Achievement " + achievementName + " Completed!");
		if (Facades<TrackingFacade>.Instance != null)
		{
			Facades<TrackingFacade>.Instance.LogParameterMetric("Daily Task: Completed", new Dictionary<string, string> { { "Completed", achievementName } });
			Facades<TrackingFacade>.Instance.LogProgress("Task_" + achievementName + "_completed");
		}
		_completedQuests[_ticketLookup[achievementName]] = true;
		_numCompletedQuests++;
		_dummyStringList.Clear();
		for (int i = 0; i < _numQuestsToPresentSimultaneously; i++)
		{
			if (_currentList[i] == null)
			{
				continue;
			}
			Achievement achievement = AchievementSystem._pInstance.GetAchievement(_currentList[i]);
			if (achievementName == achievement.achievementName)
			{
				continue;
			}
			int num = achievement.rulePairs.Length;
			for (int j = 0; j < num; j++)
			{
				if (!ListContains(_dummyStringList, achievement.rulePairs[j].valAName))
				{
					_dummyStringList.Add(achievement.rulePairs[j].valAName);
				}
				if (!ListContains(_dummyStringList, achievement.rulePairs[j].valBName))
				{
					_dummyStringList.Add(achievement.rulePairs[j].valBName);
				}
			}
		}
		int num2 = achi.rulePairs.Length;
		bool flag = false;
		for (int k = 0; k < num2; k++)
		{
			string valAName = achi.rulePairs[k].valAName;
			string valBName = achi.rulePairs[k].valBName;
			Statistics.STAT_TYPE valAType = achi.rulePairs[k].valAType;
			Statistics.STAT_TYPE valBType = achi.rulePairs[k].valBType;
			if (!ListContains(_dummyStringList, valAName))
			{
				ResetStat(valAName, valAType);
				flag = true;
			}
			else
			{
				Debug.Log(valAName + " is still in use by another quest");
			}
			if (!ListContains(_dummyStringList, valBName))
			{
				ResetStat(valBName, valBType);
				flag = true;
			}
			else
			{
				Debug.Log(valBName + " is still in use by another quest");
			}
		}
		if (flag)
		{
			ForceRefreshAchis();
		}
		for (int l = 0; l < _numQuestsToPresentSimultaneously; l++)
		{
			if (_currentList[l] == achievementName)
			{
				_currentList[l] = null;
			}
		}
		if (this._onQuestCompletedEvent != null)
		{
			Debug.Log("Calling quest completed event");
			this._onQuestCompletedEvent(achi);
		}
		GenerateCurrentStatNameList();
	}

	private void ForceRefreshAchis()
	{
		Debug.Log("Force re-eval of achievements");
		int num = _achievementNameList.Length;
		for (int i = 0; i < num; i++)
		{
			AchievementSystem._pInstance.EvaluateAchievement(_achievementNameList[i], true);
		}
	}

	public void ResetStat(string statName, Statistics.STAT_TYPE type)
	{
		switch (type)
		{
		case Statistics.STAT_TYPE.BOOLEAN:
			if (!AchievementSystem._pInstance.stats.IsBooleanConst(statName))
			{
				bool defaultBoolean = AchievementSystem._pInstance.stats.GetDefaultBoolean(statName);
				Debug.Log("Resetting bool: " + statName + " to " + defaultBoolean);
				AchievementSystem._pInstance.stats.SetStatBoolean(statName, defaultBoolean);
			}
			break;
		case Statistics.STAT_TYPE.FLOAT:
			if (!AchievementSystem._pInstance.stats.IsFloatConst(statName))
			{
				float defaultFloat = AchievementSystem._pInstance.stats.GetDefaultFloat(statName);
				Debug.Log("Resetting float: " + statName + " to " + defaultFloat);
				AchievementSystem._pInstance.stats.SetStatFloat(statName, defaultFloat);
			}
			break;
		case Statistics.STAT_TYPE.INTEGER:
			if (!AchievementSystem._pInstance.stats.IsIntegerConst(statName))
			{
				int defaultInteger = AchievementSystem._pInstance.stats.GetDefaultInteger(statName);
				Debug.Log("Resetting int: " + statName + " to " + defaultInteger);
				AchievementSystem._pInstance.stats.SetStatInteger(statName, defaultInteger);
			}
			break;
		case Statistics.STAT_TYPE.STRING:
			if (!AchievementSystem._pInstance.stats.IsStringConst(statName))
			{
				string defaultString = AchievementSystem._pInstance.stats.GetDefaultString(statName);
				Debug.Log("Resetting string: " + statName + " to " + defaultString);
				AchievementSystem._pInstance.stats.SetStatString(statName, defaultString);
			}
			break;
		}
	}

	private bool ListContains<T>(List<T> list, T value)
	{
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (value.Equals(list[i]))
			{
				return true;
			}
		}
		return false;
	}
}
