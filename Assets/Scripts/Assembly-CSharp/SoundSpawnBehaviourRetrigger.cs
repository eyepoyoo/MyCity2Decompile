public class SoundSpawnBehaviourRetrigger : SoundSpawnBehaviourBase
{
	public override string _pBehaviourName
	{
		get
		{
			return "SoundSpawnBehaviourRetrigger";
		}
	}

	public override CREATION_BEHAVIOUR _pCreationBehaviour
	{
		get
		{
			return CREATION_BEHAVIOUR.MODIFY_THEN_CREATE_IF_NONE_EXIST;
		}
	}

	public override bool _pRetrigger
	{
		get
		{
			return true;
		}
	}

	public new static void Register()
	{
		SoundSpawnBehaviourRetrigger soundSpawnBehaviourRetrigger = new SoundSpawnBehaviourRetrigger();
		SoundSpawnBehaviourBase._allBehaviours[soundSpawnBehaviourRetrigger._pBehaviourName] = soundSpawnBehaviourRetrigger;
	}
}
