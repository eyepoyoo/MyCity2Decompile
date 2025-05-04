using System;

[Serializable]
public class StatisticRulePair
{
	public enum RULE
	{
		EQUAL = 0,
		NOTEQUAL = 1,
		GREATER_THAN = 2,
		LESS_THAN = 3,
		GREATER_THAN_EQUALTO = 4,
		LESS_THAN_EQUALTO = 5,
		SUM_GREATER_EQUALTO = 6
	}

	public Statistics.STAT_TYPE valAType;

	public string valAName;

	public RULE rule;

	public Statistics.STAT_TYPE valBType;

	public string valBName;
}
