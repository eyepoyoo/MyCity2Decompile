public class SignalsInitialiser : InitialisationObject
{
	public SignalSender signals;

	public override void startInitialising()
	{
		signals.SendSignals(this);
		_currentState = InitialisationState.FINISHED;
	}
}
