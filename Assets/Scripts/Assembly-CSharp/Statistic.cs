using System.Collections.Generic;
using DeepThought;
using UnityEngine;

public class Statistic
{
	public Neuron valueNueron;

	public EStatisticsRule statRule;

	public List<string> tags;

	public string name;

	public string playerName;

	public string category;

	private bool dirty;

	public float StatValue
	{
		get
		{
			return valueNueron.Number;
		}
		set
		{
			dirty = true;
			valueNueron.Number = value;
		}
	}

	public Statistic(Neuron definitionRootNode, string playerName, string category)
	{
		this.playerName = playerName;
		this.category = category;
		name = definitionRootNode.getName();
		tags = new List<string>();
		tags.Add(category);
		if (definitionRootNode != null)
		{
			Neuron neuron = definitionRootNode.tryGetChild("tags");
			if (neuron != null)
			{
				List<Neuron> list = new List<Neuron>();
				neuron.getChildren(list);
				foreach (Neuron item in list)
				{
					tags.Add(item.getName());
				}
			}
		}
		Neuron neuron2 = null;
		if (definitionRootNode != null)
		{
			neuron2 = definitionRootNode.tryGetChild("statRule");
		}
		if (neuron2 == null)
		{
			statRule = EStatisticsRule.INCREMENT;
			return;
		}
		switch (neuron2.getValue().getText())
		{
		case "KeepHigher":
			statRule = EStatisticsRule.KEEP_HIGHEST;
			break;
		case "KeepLower":
			statRule = EStatisticsRule.KEEP_LOWEST;
			break;
		case "Multiply":
			statRule = EStatisticsRule.MULTIPLY;
			break;
		case "Dirty":
			statRule = EStatisticsRule.KEEP_IF_DIRTY;
			break;
		default:
			statRule = EStatisticsRule.INCREMENT;
			break;
		}
	}

	public void reset()
	{
		valueNueron.Number = 0f;
		dirty = false;
	}

	public void modifyStat(float valueToUse)
	{
		switch (statRule)
		{
		case EStatisticsRule.INCREMENT:
			StatValue += valueToUse;
			break;
		case EStatisticsRule.KEEP_HIGHEST:
			StatValue = Mathf.Max(StatValue, valueToUse);
			break;
		case EStatisticsRule.KEEP_LOWEST:
			StatValue = ((StatValue != 0f) ? Mathf.Min(StatValue, valueToUse) : valueToUse);
			break;
		case EStatisticsRule.MULTIPLY:
			StatValue = ((StatValue != 0f) ? (StatValue * valueToUse) : valueToUse);
			break;
		case EStatisticsRule.KEEP_IF_DIRTY:
			if (!dirty)
			{
				StatValue = valueToUse;
				dirty = false;
			}
			break;
		}
	}
}
