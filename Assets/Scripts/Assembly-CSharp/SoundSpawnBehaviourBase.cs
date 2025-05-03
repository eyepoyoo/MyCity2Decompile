using System.Collections.Generic;
using UnityEngine;

public class SoundSpawnBehaviourBase
{
	public enum CREATION_BEHAVIOUR
	{
		CREATE_NEW = 0,
		MODIFY_THEN_CREATE_IF_NONE_EXIST = 1,
		ALWAYS_MODIFY = 2
	}

	public static Dictionary<string, SoundSpawnBehaviourBase> _allBehaviours = new Dictionary<string, SoundSpawnBehaviourBase>();

	public virtual string _pBehaviourName
	{
		get
		{
			return "SoundSpawnBehaviourBase";
		}
	}

	public virtual CREATION_BEHAVIOUR _pCreationBehaviour
	{
		get
		{
			return CREATION_BEHAVIOUR.CREATE_NEW;
		}
	}

	public virtual bool _pRetrigger
	{
		get
		{
			return true;
		}
	}

	public static void Register()
	{
		SoundSpawnBehaviourBase soundSpawnBehaviourBase = new SoundSpawnBehaviourBase();
		_allBehaviours[soundSpawnBehaviourBase._pBehaviourName] = soundSpawnBehaviourBase;
	}

	public virtual void OnSoundPlay(AudioSource source)
	{
	}

	public virtual void OnSoundContinue(AudioSource source)
	{
	}
}
