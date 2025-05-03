using UnityEngine;

public class SoundSpawnBehaviourIncreaseExistingVolume : SoundSpawnBehaviourBase
{
	public override string _pBehaviourName
	{
		get
		{
			return "SoundSpawnBehaviourIncreaseExistingVolume";
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
		SoundSpawnBehaviourIncreaseExistingVolume soundSpawnBehaviourIncreaseExistingVolume = new SoundSpawnBehaviourIncreaseExistingVolume();
		SoundSpawnBehaviourBase._allBehaviours[soundSpawnBehaviourIncreaseExistingVolume._pBehaviourName] = soundSpawnBehaviourIncreaseExistingVolume;
	}

	public override void OnSoundPlay(AudioSource source)
	{
	}

	public override void OnSoundContinue(AudioSource source)
	{
		source.volume += 0.1f;
	}
}
