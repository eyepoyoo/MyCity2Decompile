using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadBunker : MonoBehaviour
{
	private static DontDestroyOnLoadBunker _Instance;

	private readonly List<Transform> _toRemoveOnNextSceneLoad = new List<Transform>();

	public static DontDestroyOnLoadBunker _pInstance
	{
		get
		{
			if (!_Instance)
			{
				GameObject gameObject = new GameObject("DontDestroyOnLoadBunker");
				Object.DontDestroyOnLoad(gameObject);
				_Instance = gameObject.AddComponent<DontDestroyOnLoadBunker>();
			}
			return _Instance;
		}
	}

	public void Add(Transform item, bool onceOnly = false)
	{
		item.parent = base.transform;
		if (onceOnly)
		{
			_toRemoveOnNextSceneLoad.Add(item);
		}
	}

	public void Remove(Transform item)
	{
		if (item.parent == base.transform)
		{
			item.parent = null;
		}
		if (_toRemoveOnNextSceneLoad.Contains(item))
		{
			_toRemoveOnNextSceneLoad.Remove(item);
		}
		if (base.transform.childCount == 0)
		{
			_Instance = null;
			Object.Destroy(base.gameObject);
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		StartCoroutine("OnLevelWasLoaded_NextFrame");
	}

	private IEnumerator OnLevelWasLoaded_NextFrame()
	{
		yield return new WaitForEndOfFrame();
		for (int i = _toRemoveOnNextSceneLoad.Count - 1; i >= 0; i--)
		{
			_toRemoveOnNextSceneLoad[i].SetDoPersistBetweenSceneLoads(false);
		}
	}
}
