using UnityEngine;

public class InitialiseDestroy : InitialisationObject
{
	[SerializeField]
	private GameObject _objectToDestroy;

	public override void startInitialising()
	{
		base.startInitialising();
		Object.Destroy(_objectToDestroy);
		_currentState = InitialisationState.FINISHED;
	}
}
