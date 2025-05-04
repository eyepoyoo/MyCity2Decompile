public class LeaderboardFacade_LEGCTY : LeaderboardFacade
{
	public const string SAVE_KEY_NAME = "HighScore";

	public const string PLAYER_DEFAULT_NAME_KEY = "General.DefaultName";

	public const string PLAYER_DEFAULT_NAME_SUB = "No Default Name";

	public override void startInitialising()
	{
		DatabaseFacade.Instance._userData._name = LocalisationFacade.Instance.GetString("General.DefaultName");
		if (string.IsNullOrEmpty(DatabaseFacade.Instance._userData._name))
		{
			DatabaseFacade.Instance._userData._name = "No Default Name";
		}
		int num = 0;
		if (AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.HasKey("HighScore"))
		{
			num = AmuzoScriptableSingleton<PlayerPrefsFacade>._pInstance.GetInt("HighScore");
		}
		UserData userData = new UserData();
		userData._name = DatabaseFacade.Instance._userData._name;
		userData.addData("leaderboardName", "CumulativeStuds");
		userData.addData("score", num.ToString());
		DatabaseFacade.Instance.saveUser(DatabaseType.LOCAL, userData, null, null);
		base.startInitialising();
	}
}
