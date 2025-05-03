using System;

[Serializable]
public class AchievementStatisticInteger : AchievementStatistic
{
	public int statValue;

	public int defaultStatValue;

	public override object value
	{
		get
		{
			return statValue;
		}
	}

	public AchievementStatisticInteger(string name)
		: base(name)
	{
	}

	public int GetStatDefaultValue()
	{
		return defaultStatValue;
	}

	public void SetDefaultStatValue(int newValue)
	{
		defaultStatValue = newValue;
	}

	public int GetStatCurrentValue()
	{
		return statValue;
	}

	public void SetCurrentStatValue(int newValue)
	{
		if (newValue != statValue)
		{
			if (IsConst())
			{
				statValue = defaultStatValue;
				return;
			}
			statValue = newValue;
			OnChange();
		}
	}
}
