public class ScreenSignIn : ScreenBase
{
	public void OnSigninCancelled()
	{
		Navigate("OnSigninCancelled");
	}

	public void OnSigninFailed()
	{
		Navigate("OnSigninFailed");
	}

	public void OnSigninSuccess()
	{
		Navigate("OnSigninSuccess");
	}
}
