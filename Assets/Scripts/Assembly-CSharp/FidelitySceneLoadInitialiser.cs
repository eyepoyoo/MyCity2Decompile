public class FidelitySceneLoadInitialiser : InitialisationObject
{
	public SceneLoader lowFidelityLoader;

	public SceneLoader mediumFidelityLoader;

	public SceneLoader highFidelityLoader;

	protected override void Awake()
	{
		base.Awake();
	}

	public override void startInitialising()
	{
		if (highFidelityLoader == null || mediumFidelityLoader == null || lowFidelityLoader == null)
		{
			_currentState = InitialisationState.FINISHED;
			return;
		}
		_currentState = InitialisationState.INITIALISING;
		bool flag = false;
		bool flag2 = false;
		if (Facades<FidelityFacade>.Instance.fidelity == Fidelity.High && highFidelityLoader != null)
		{
			flag = highFidelityLoader.AttemptLoadLevel(onComplete);
			flag2 = true;
		}
		if (!flag2 && (Facades<FidelityFacade>.Instance.fidelity == Fidelity.High || Facades<FidelityFacade>.Instance.fidelity == Fidelity.Normal) && mediumFidelityLoader != null)
		{
			flag = mediumFidelityLoader.AttemptLoadLevel(onComplete);
			flag2 = true;
		}
		if (!flag2 && lowFidelityLoader != null)
		{
			flag = lowFidelityLoader.AttemptLoadLevel(onComplete);
		}
		if (!flag)
		{
			_currentState = InitialisationState.FINISHED;
		}
	}

	private void onComplete(bool boolOfQuestionablePurpouse)
	{
		_currentState = InitialisationState.FINISHED;
	}
}
