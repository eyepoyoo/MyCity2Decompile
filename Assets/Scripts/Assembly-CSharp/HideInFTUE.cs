using UnityEngine;

public class HideInFTUE : MonoBehaviour
{
	private MeshRenderer[] _renderers;

	private void Awake()
	{
		_renderers = GetComponentsInChildren<MeshRenderer>();
	}

	private void Update()
	{
		if (_renderers != null)
		{
			int num = _renderers.Length;
			for (int i = 0; i < num; i++)
			{
				_renderers[i].enabled = GlobalInGameData._pHasSeenGarageTutorial;
			}
		}
	}
}
