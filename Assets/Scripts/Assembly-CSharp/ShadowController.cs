using UnityEngine;

public class ShadowController : MonoBehaviour
{
	private void Awake()
	{
		if (Facades<FidelityFacade>.Instance.shadows != EShadowQuality.Realtime)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
