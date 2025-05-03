using System.Collections.Generic;
using DeepThought;

public class PlayerStatistics
{
	public string playerName;

	public List<Statistic> playerStatistics;

	public Neuron playerStatsRootNuron;

	public PlayerStatistics(Neuron playerStatsRootNuron, List<Neuron> allStatisticsDefinitions)
	{
		this.playerStatsRootNuron = playerStatsRootNuron;
		playerName = playerStatsRootNuron.getName();
		playerStatistics = new List<Statistic>();
		foreach (Neuron allStatisticsDefinition in allStatisticsDefinitions)
		{
			Neuron child = allStatisticsDefinition.getChild("categories");
			List<Neuron> list = new List<Neuron>();
			child.getChildren(list);
			foreach (Neuron item in list)
			{
				Statistic statistic = new Statistic(allStatisticsDefinition, playerName, item.getName());
				Neuron neuron = playerStatsRootNuron.VerifyProperty(item.getName(), string.Empty);
				statistic.valueNueron = neuron.VerifyProperty(allStatisticsDefinition.getName(), string.Empty);
				playerStatistics.Add(statistic);
			}
		}
	}
}
