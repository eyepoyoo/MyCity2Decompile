using UnityEngine;

public class AnimateOnSignal : MonoBehaviour
{
	public string _animName;

	public PlayMode _playMode;

	private void OnSignal()
	{
		if (GetComponent<Animation>() != null)
		{
			bool flag = false;
			if (!((_animName == null || !(_animName != string.Empty)) ? GetComponent<Animation>().Play(_playMode) : GetComponent<Animation>().Play(_animName, _playMode)))
			{
				Debug.LogWarning("[AnimateOnSignal] Failed to play animation " + _animName);
			}
		}
	}
}
