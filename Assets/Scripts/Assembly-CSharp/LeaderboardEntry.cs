using System;

[Serializable]
public class LeaderboardEntry : UserData
{
	public int _position;

	public int _score;

	public override string getData(string key)
	{
		if (key == "position")
		{
			return _position.ToString();
		}
		if (key == "score")
		{
			return _score.ToString();
		}
		return base.getData(key);
	}

	public override void addData(string key, string value)
	{
		if (key == "position")
		{
			_position = int.Parse(value);
		}
		else if (key == "score")
		{
			_score = int.Parse(value);
		}
		base.addData(key, value);
	}
}
