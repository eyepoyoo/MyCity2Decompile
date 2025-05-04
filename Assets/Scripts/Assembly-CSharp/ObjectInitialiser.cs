using System.Collections.Generic;
using UnityEngine;

public class ObjectInitialiser : MonoBehaviour
{
	public GameObject toEnable;

	private List<GameObject> _enableList;

	private int _enableIndex;

	private void Start()
	{
		if (toEnable == null)
		{
			Debug.Log("Initialiser found nothing to initialise");
			return;
		}
		_enableIndex = 0;
		_enableList = new List<GameObject>();
		AddToList(toEnable);
	}

	private void AddToList(GameObject obj)
	{
		_enableList.Add(obj);
		int childCount = obj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			AddToList(obj.transform.GetChild(i).gameObject);
		}
	}

	private void Update()
	{
		if (_enableIndex != _enableList.Count)
		{
			_enableList[_enableIndex++].SetActive(true);
		}
	}
}
