using UnityEngine;

public class ChildRandomizer : MonoBehaviour
{
	private void OnEnable()
	{
		int childCount = base.transform.childCount;
		int num = Random.Range(0, childCount);
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			child.gameObject.SetActive(i == num);
		}
	}
}
