using System;

[Serializable]
public class AchievementStatisticBoolean : AchievementStatistic
{
	public bool statValue;

	public bool defaultStatValue;

	public override object value
	{
		get
		{
			return statValue;
		}
	}

	public AchievementStatisticBoolean(string name)
		: base(name)
	{
	}

	public bool GetStatDefaultValue()
	{
		return defaultStatValue;
	}

	public void SetDefaultStatValue(bool newValue)
	{
		defaultStatValue = newValue;
	}

	public bool GetStatCurrentValue()
	{
		return statValue;
	}

	public void SetCurrentStatValue(bool newValue)
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
