using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class Database
{
	public bool _doEnable;

	public DatabaseType _type;

	public ObscuredString _userUniqueId;

	public virtual void initialise(Action<bool> onFinishCallback)
	{
	}

	public virtual void getUserPosition(string leaderboardName, string userId, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
	}

	public virtual void getHighScores(string leaderboardName, int currentIndex, int length, bool doFriendsOnly, Action<DownloadRequest> onSuccessCallback, Action<DownloadRequest> onFailCallback)
	{
	}

	public virtual void saveUserData(UserData saveData, Action<DownloadRequest> onSuccess, Action<DownloadRequest> onFail)
	{
	}

	public virtual void addDebugMenu(AmuzoDebugMenu databaseManeu)
	{
	}
}
