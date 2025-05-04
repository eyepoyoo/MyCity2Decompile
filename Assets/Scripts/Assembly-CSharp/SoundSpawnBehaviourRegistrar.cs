public class SoundSpawnBehaviourRegistrar
{
	public static void RegisterProjectSoundSpawnBehaviours()
	{
	}

	public static void Register()
	{
		SoundSpawnBehaviourBase.Register();
		SoundSpawnBehaviourRetrigger.Register();
		SoundSpawnBehaviourIncreaseExistingVolume.Register();
		SoundSpawnBehaviourIncreasePitch.Register();
		RegisterProjectSoundSpawnBehaviours();
	}
}
