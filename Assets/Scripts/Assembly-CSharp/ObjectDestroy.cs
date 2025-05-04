using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
	public string[] objects;

	private List<GameObject> _gameObjects;

	private void OnSignal()
	{
		if (_gameObjects == null)
		{
			_gameObjects = new List<GameObject>();
			string[] array = objects;
			foreach (string text in array)
			{
				GameObject gameObject = GameObject.Find(text);
				if (gameObject == null)
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("Object Destroy could not find " + text + " (will not be able to destroy)");
					}
				}
				else
				{
					_gameObjects.Add(gameObject);
				}
			}
		}
		foreach (GameObject gameObject2 in _gameObjects)
		{
			Object.Destroy(gameObject2);
		}
	}
}
