using UnityEngine;

public class MakeAudioListenerFollowThisOnAwake : MonoBehaviour
{
	private void Awake()
	{
		if ((bool)SoundFacade._pInstance)
		{
			SoundFacade._pInstance.MakeAudioListenerTrackTransform(base.transform);
		}
	}
}
