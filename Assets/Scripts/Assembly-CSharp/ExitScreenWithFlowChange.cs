using UnityEngine;

public class ExitScreenWithFlowChange : MonoBehaviour
{
	public string linkToFollow;

	public void OnSignal()
	{
		if (linkToFollow == string.Empty)
		{
			linkToFollow = null;
		}
		Facades<ScreenFacade>.Instance.ExitCurrentScreenWithFlowChange(linkToFollow);
	}
}
