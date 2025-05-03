using System;
using System.Collections.Generic;

[Serializable]
public class Achievement
{
	private const string _cInvalidString = "INVALID";

	public string achievementName;

	public StatisticRulePair[] rulePairs;

	public bool completed;

	public string completedTimeStamp = string.Empty;

	public string category;

	public AchievementMetaData[] metadata;

	private Dictionary<string, string> _metaDataLookup;

	public Achievement(string name)
	{
		achievementName = name;
	}

	public void NotifyOfChange(string statName)
	{
		if (AchievementSystem._pInstance != null)
		{
			AchievementSystem._pInstance.OnAchievementChanged(achievementName, this);
		}
	}

	public DateTime GetCompletedTimestamp()
	{
		long result;
		return (!long.TryParse(completedTimeStamp, out result)) ? DateTime.MinValue : DateTime.FromBinary(result);
	}

	public void MarkAsComplete()
	{
		if (!completed)
		{
			completed = true;
			completedTimeStamp = DateTime.Now.ToBinary().ToString();
			if (AchievementSystem._pInstance != null)
			{
				AchievementSystem._pInstance.OnAchievementCompleted(achievementName, this);
			}
		}
	}

	public string GetMetaString(string name)
	{
		if (_metaDataLookup == null)
		{
			_metaDataLookup = new Dictionary<string, string>();
			for (int i = 0; i < metadata.Length; i++)
			{
				_metaDataLookup[metadata[i].key] = metadata[i].data;
			}
		}
		if (!_metaDataLookup.ContainsKey(name))
		{
			return "INVALID";
		}
		return _metaDataLookup[name];
	}
}
