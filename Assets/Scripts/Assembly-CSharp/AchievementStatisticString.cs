using System;

[Serializable]
public class AchievementStatisticString : AchievementStatistic
{
	public string statValue = string.Empty;

	public string defaultStatValue = string.Empty;

	public override object value
	{
		get
		{
			return statValue;
		}
	}

	public AchievementStatisticString(string name)
		: base(name)
	{
	}

	public string GetStatDefaultValue()
	{
		return defaultStatValue;
	}

	public void SetDefaultStatValue(string newValue)
	{
		defaultStatValue = newValue;
	}

	public string GetStatCurrentValue()
	{
		return statValue;
	}

	public void SetCurrentStatValue(string newValue)
	{
		if (!(newValue == statValue))
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
