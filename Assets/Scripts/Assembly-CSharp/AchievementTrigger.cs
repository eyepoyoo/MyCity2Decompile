using System;

public class AchievementTrigger : AchievementTriggerableBase
{
	[Serializable]
	public class AchievementTriggerData
	{
		public enum TRIGGER_RULE
		{
			SET = 0,
			INCREMENT = 1,
			DECREMENT = 2
		}

		public string statisticName;

		public bool boolVal;

		public float floatVal;

		public int intVal;

		public string stringVal;

		public TRIGGER_RULE rule;

		public Statistics.STAT_TYPE statType;
	}

	public AchievementTriggerData[] triggerData;

	public bool test;

	private void Update()
	{
		if (test)
		{
			test = false;
			Trigger();
		}
	}

	public override void Trigger()
	{
		int num = triggerData.Length;
		for (int i = 0; i < num; i++)
		{
			switch (triggerData[i].statType)
			{
			case Statistics.STAT_TYPE.BOOLEAN:
				HandleBool(triggerData[i]);
				break;
			case Statistics.STAT_TYPE.INTEGER:
				HandleInt(triggerData[i]);
				break;
			case Statistics.STAT_TYPE.FLOAT:
				HandleFloat(triggerData[i]);
				break;
			case Statistics.STAT_TYPE.STRING:
				HandleString(triggerData[i]);
				break;
			}
		}
		base.Trigger();
	}

	private void HandleBool(AchievementTriggerData data)
	{
		switch (data.rule)
		{
		case AchievementTriggerData.TRIGGER_RULE.INCREMENT:
			if (!AchievementSystem._pInstance.stats.GetStatBoolean(data.statisticName))
			{
				AchievementSystem._pInstance.stats.SetStatBoolean(data.statisticName, data.boolVal);
			}
			break;
		case AchievementTriggerData.TRIGGER_RULE.DECREMENT:
			if (AchievementSystem._pInstance.stats.GetStatBoolean(data.statisticName))
			{
				AchievementSystem._pInstance.stats.SetStatBoolean(data.statisticName, data.boolVal);
			}
			break;
		default:
			AchievementSystem._pInstance.stats.SetStatBoolean(data.statisticName, data.boolVal);
			break;
		}
	}

	private void HandleInt(AchievementTriggerData data)
	{
		switch (data.rule)
		{
		case AchievementTriggerData.TRIGGER_RULE.INCREMENT:
			AchievementSystem._pInstance.stats.IncrementStatInteger(data.statisticName, data.intVal);
			break;
		case AchievementTriggerData.TRIGGER_RULE.DECREMENT:
		{
			int statInteger = AchievementSystem._pInstance.stats.GetStatInteger(data.statisticName);
			AchievementSystem._pInstance.stats.SetStatInteger(data.statisticName, statInteger - data.intVal);
			break;
		}
		default:
			AchievementSystem._pInstance.stats.SetStatInteger(data.statisticName, data.intVal);
			break;
		}
	}

	private void HandleFloat(AchievementTriggerData data)
	{
		switch (data.rule)
		{
		case AchievementTriggerData.TRIGGER_RULE.INCREMENT:
		{
			float statFloat2 = AchievementSystem._pInstance.stats.GetStatFloat(data.statisticName);
			AchievementSystem._pInstance.stats.SetStatFloat(data.statisticName, statFloat2 + data.floatVal);
			break;
		}
		case AchievementTriggerData.TRIGGER_RULE.DECREMENT:
		{
			float statFloat = AchievementSystem._pInstance.stats.GetStatFloat(data.statisticName);
			AchievementSystem._pInstance.stats.SetStatFloat(data.statisticName, statFloat - data.floatVal);
			break;
		}
		default:
			AchievementSystem._pInstance.stats.SetStatFloat(data.statisticName, data.floatVal);
			break;
		}
	}

	private void HandleString(AchievementTriggerData data)
	{
		AchievementTriggerData.TRIGGER_RULE rule = data.rule;
		if (rule == AchievementTriggerData.TRIGGER_RULE.INCREMENT)
		{
			string statString = AchievementSystem._pInstance.stats.GetStatString(data.statisticName);
			AchievementSystem._pInstance.stats.SetStatString(data.statisticName, statString + data.stringVal);
		}
		else
		{
			AchievementSystem._pInstance.stats.SetStatString(data.statisticName, data.stringVal);
		}
	}
}
