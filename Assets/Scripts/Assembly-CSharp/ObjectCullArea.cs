using UnityEngine;

public class ObjectCullArea : MonoBehaviour
{
	public ObjectCullList objectCullList;

	public bool enableState = true;

	private void OnTriggerEnter(Collider other)
	{
		GameObject[] objectList = objectCullList.objectList;
		foreach (GameObject gameObject in objectList)
		{
			gameObject.SetActive(enableState);
		}
	}
}
