using UnityEngine;

public class SoundSpawnBehaviourIncreasePitch : SoundSpawnBehaviourBase
{
	public override string _pBehaviourName
	{
		get
		{
			return "SoundSpawnBehaviourIncreasePitch";
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
			return false;
		}
	}

	public new static void Register()
	{
		SoundSpawnBehaviourIncreasePitch soundSpawnBehaviourIncreasePitch = new SoundSpawnBehaviourIncreasePitch();
		SoundSpawnBehaviourBase._allBehaviours[soundSpawnBehaviourIncreasePitch._pBehaviourName] = soundSpawnBehaviourIncreasePitch;
	}

	public override void OnSoundPlay(AudioSource source)
	{
	}

	public override void OnSoundContinue(AudioSource source)
	{
		source.pitch += 0.1f;
	}
}
