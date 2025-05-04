using UnityEngine;

public class Follower
{
	public float upTime;

	public float downTime;

	public float value;

	public float target;

	public Follower(float upTime = 1f, float downTime = 1f)
	{
		this.upTime = upTime;
		this.downTime = downTime;
	}

	public void Update(float deltaTime)
	{
		if (value < target)
		{
			if (upTime == 0f)
			{
				value = target;
			}
			else
			{
				value = Mathf.Min(value += deltaTime / upTime, target);
			}
		}
		else if (value > target)
		{
			if (downTime == 0f)
			{
				value = target;
			}
			else
			{
				value = Mathf.Max(value -= deltaTime / downTime, target);
			}
		}
	}
}
