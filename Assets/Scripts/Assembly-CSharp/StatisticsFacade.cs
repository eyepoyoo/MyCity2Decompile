using System.Collections.Generic;
using DeepThought;
using UnityEngine;

public class StatisticsFacade : Neuron
{
	public const string STAT_RULE_NODE_NAME = "statRule";

	public const string STAT_RULE_INCREMENT_KEY = "Increment";

	public const string STAT_RULE_KEEP_HIGHER_KEY = "KeepHigher";

	public const string STAT_RULE_KEEP_LOWER_KEY = "KeepLower";

	public const string STAT_RULE_KEEP_MULTIPLY_KEY = "Multiply";

	public const string STAT_RULE_KEEP_IF_DIRTY_KEY = "Dirty";

	public const string STAT_RULE_OVERRIDE_KEY = "Override";

	public const string CATEGORY_NODE_NAME = "categories";

	public const string STATISTICS_XML_NAME = "Statistics";

	public const string STATISTICS_DEFINITION_NODE_NAME = "statisticDefinitions";

	public const string CURRENT_PLAYER_NAME = "currentPlayer";

	private Dictionary<string, PlayerStatistics> playersStatistics;

	private List<Neuron> allStatisticsDefinitions;

	public StatisticsFacade(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
		Facades<StatisticsFacade>.Register(this);
		LoadDefinitionsXMLFromResources();
	}

	private void LoadDefinitionsXMLFromResources()
	{
		TextAsset textAsset = Resources.Load("Statistics", typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogError("StatisticsFacade: Could not find Statistics XML asset.");
			return;
		}
		merge(textAsset.text);
		playersStatistics = new Dictionary<string, PlayerStatistics>();
		Neuron child = getChild("statisticDefinitions");
		allStatisticsDefinitions = new List<Neuron>();
		child.getChildren(allStatisticsDefinitions);
		Neuron playerStatsRootNuron = this.VerifyProperty("currentPlayer", string.Empty);
		PlayerStatistics playerStatistics = new PlayerStatistics(playerStatsRootNuron, allStatisticsDefinitions);
		playersStatistics.Add("currentPlayer", playerStatistics);
	}

	public Neuron GetChangeLogRoot(string xmlString)
	{
		if (xmlString == null || xmlString == string.Empty || !xmlString.StartsWith("<"))
		{
			Debug.LogWarning("StatisticsFacade: No valid save data: cannot generate change log");
			return null;
		}
		Neuron.cheapMode = true;
		Neuron neuron = Neuron.generate(xmlString);
		Neuron.cheapMode = false;
		if (neuron.getName() != "currentPlayer")
		{
			neuron = neuron.tryGetChild("currentPlayer");
		}
		return neuron;
	}

	public void mergeCurrentPlayerStatistics(string xmlString, bool overwrite = false)
	{
		Neuron changeLogRoot = GetChangeLogRoot(xmlString);
		if (changeLogRoot == null)
		{
			Debug.LogError("Statistics Facade: Merge failed. Unable to find root.");
			return;
		}
		foreach (Neuron item in (IEnumerable<Neuron>)changeLogRoot)
		{
			foreach (Neuron item2 in (IEnumerable<Neuron>)item)
			{
				Statistic statisticForCurrentPlayer = getStatisticForCurrentPlayer(item.getName(), item2.getName());
				if (statisticForCurrentPlayer != null)
				{
					if (overwrite)
					{
						statisticForCurrentPlayer.StatValue = item2.Number;
					}
					else
					{
						statisticForCurrentPlayer.modifyStat(item2.Number);
					}
				}
			}
		}
		Debug.Log("Statistics Facade: Merge complete");
	}

	public string getCurrentPlayerStatisticsAsString()
	{
		return playersStatistics["currentPlayer"].playerStatsRootNuron.serialize();
	}

	public void addNewPlayerStats(string xmlString, string newPlayerName)
	{
		Neuron neuron = this.VerifyProperty(newPlayerName, string.Empty);
		neuron.merge(xmlString);
		PlayerStatistics playerStatistics = new PlayerStatistics(neuron, allStatisticsDefinitions);
		playersStatistics.Add(newPlayerName, playerStatistics);
	}

	public List<Statistic> getStatisticsWithTag(string tag)
	{
		return getStatisticsWithTags(new string[1] { tag });
	}

	public List<Statistic> getStatisticsWithTags(string[] tags)
	{
		List<Statistic> list = new List<Statistic>();
		PlayerStatistics playerStatistics = playersStatistics["currentPlayer"];
		foreach (Statistic playerStatistic in playerStatistics.playerStatistics)
		{
			bool flag = false;
			foreach (string tag in playerStatistic.tags)
			{
				foreach (string text in tags)
				{
					if (!flag && tag == text)
					{
						list.Add(playerStatistic);
						flag = true;
					}
				}
			}
		}
		return list;
	}

	public void resetAllStatsWithTag(string tag)
	{
		resetAllStatsWithTags(new string[1] { tag });
	}

	public void resetAllStatsWithTags(string[] tags)
	{
		List<Statistic> statisticsWithTags = getStatisticsWithTags(tags);
		foreach (Statistic item in statisticsWithTags)
		{
			item.reset();
		}
	}

	public void modifyStat(string category, string statName, float modifyValue)
	{
		Statistic statisticForCurrentPlayer = getStatisticForCurrentPlayer(category, statName);
		if (statisticForCurrentPlayer == null)
		{
			Debug.LogWarning("StatisticsFacade: Unable to find [" + statName + "] in category [" + category + "]. Cannot modify!");
		}
		else
		{
			statisticForCurrentPlayer.modifyStat(modifyValue);
		}
	}

	public Statistic getStatisticForCurrentPlayer(string category, string statName)
	{
		return getStatisticForPlayer(category, statName, "currentPlayer");
	}

	public Statistic getStatisticForPlayer(string category, string statName, string playerName)
	{
		if (!playersStatistics.ContainsKey(playerName))
		{
			return null;
		}
		foreach (Statistic playerStatistic in playersStatistics[playerName].playerStatistics)
		{
			if (playerStatistic.category == category && playerStatistic.name == statName)
			{
				return playerStatistic;
			}
		}
		return null;
	}

	public int getCurrentPlayerPositionForStat(string category, string statKey)
	{
		Statistic statisticForCurrentPlayer = getStatisticForCurrentPlayer(category, statKey);
		if (statisticForCurrentPlayer == null)
		{
			Debug.LogWarning("StatisticsFacade: Unable to find [" + statKey + "] in category [" + category + "]. Cannot get position!");
			return 0;
		}
		if (playersStatistics.Count <= 1)
		{
			Debug.LogWarning("StatisticsFacade: There is only one player. Get position will always return 0!");
			return 0;
		}
		List<Statistic> sortedStatisticListForPlayers = getSortedStatisticListForPlayers(category, statKey);
		for (int i = 0; i < sortedStatisticListForPlayers.Count; i++)
		{
			if (sortedStatisticListForPlayers[i] == statisticForCurrentPlayer)
			{
				return i;
			}
		}
		return 0;
	}

	public List<Statistic> getSortedStatisticListForPlayers(string category, string statKey)
	{
		List<Statistic> list = new List<Statistic>();
		foreach (KeyValuePair<string, PlayerStatistics> playersStatistic in playersStatistics)
		{
			Statistic statisticForPlayer = getStatisticForPlayer(category, statKey, playersStatistic.Value.playerName);
			if (statisticForPlayer != null)
			{
				list.Add(statisticForPlayer);
			}
		}
		if (list.Count <= 1)
		{
			return list;
		}
		bool flag = true;
		do
		{
			flag = true;
			for (int i = 0; i < list.Count - 1; i++)
			{
				if (list[i + 1].StatValue != 0f && ((list[0].statRule != EStatisticsRule.KEEP_LOWEST) ? (list[i + 1].StatValue > list[i].StatValue) : (list[i + 1].StatValue < list[i].StatValue)))
				{
					Statistic statistic = list[i + 1];
					list[i + 1] = list[i];
					list[i] = statistic;
					flag = false;
				}
			}
		}
		while (!flag);
		return list;
	}
}
