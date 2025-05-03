public class SceneLoadInitialiser : InitialisationObject
{
	public SceneLoader loader;

	public override void startInitialising()
	{
		if (loader == null)
		{
			_currentState = InitialisationState.FINISHED;
			return;
		}
		_currentState = InitialisationState.INITIALISING;
		if (!loader.AttemptLoadLevel(onComplete))
		{
			_currentState = InitialisationState.FINISHED;
		}
	}

	private void onComplete(bool boolOfQuestionablePurpouse)
	{
		_currentState = InitialisationState.FINISHED;
	}
}
