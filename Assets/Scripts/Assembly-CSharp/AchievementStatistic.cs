using System;

[Serializable]
public class AchievementStatistic
{
	public string statisticName;

	public bool isConst;

	public virtual object value
	{
		get
		{
			return null;
		}
	}

	public AchievementStatistic(string name)
	{
		statisticName = name;
	}

	public string GetStatisticName()
	{
		return statisticName;
	}

	public void SetStatisticName(string newName)
	{
		statisticName = newName;
	}

	public bool IsConst()
	{
		return isConst;
	}

	public void SetConstFlag(bool newValue)
	{
		isConst = newValue;
	}

	protected void OnChange()
	{
		if (AchievementSystem._pInstance != null)
		{
			AchievementSystem._pInstance.OnStatChanged(statisticName);
		}
	}
}
