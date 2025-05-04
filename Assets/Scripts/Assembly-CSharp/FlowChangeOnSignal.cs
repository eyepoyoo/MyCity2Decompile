using UnityEngine;

public class FlowChangeOnSignal : MonoBehaviour
{
	public string _link;

	private void OnSignal()
	{
		if (_link != null && _link != string.Empty)
		{
			Facades<FlowFacade>.Instance.FollowLink(_link);
		}
	}
}
