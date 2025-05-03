using System;

[Serializable]
public class AchievementStatisticFloat : AchievementStatistic
{
	public float statValue;

	public float defaultStatValue;

	public override object value
	{
		get
		{
			return statValue;
		}
	}

	public AchievementStatisticFloat(string name)
		: base(name)
	{
	}

	public float GetStatDefaultValue()
	{
		return defaultStatValue;
	}

	public void SetDefaultStatValue(float newValue)
	{
		defaultStatValue = newValue;
	}

	public float GetStatCurrentValue()
	{
		return statValue;
	}

	public void SetCurrentStatValue(float newValue)
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
