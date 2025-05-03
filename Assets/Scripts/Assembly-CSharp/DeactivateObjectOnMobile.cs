using UnityEngine;

public class DeactivateObjectOnMobile : MonoBehaviour
{
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}
}
