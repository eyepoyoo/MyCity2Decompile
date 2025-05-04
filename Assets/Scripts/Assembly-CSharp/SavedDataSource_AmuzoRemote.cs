using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedDataSource_AmuzoRemote : SaveDataSource
{
	public override void LoadAllData(Action<SaveDataSource, List<SaveDataGroup>, bool> OnLoadComplete)
	{
		GameObject newObject = new GameObject("CoroutineRunner_Load");
		Action onComplete = delegate
		{
			_pHasInitialised = true;
			UnityEngine.Object.Destroy(newObject);
			OnLoadComplete(this, null, false);
		};
		UpdateForwarder updateForwarder = newObject.AddComponent<UpdateForwarder>();
		updateForwarder.StartCoroutine(Wait(0.25f, onComplete));
	}

	public override void SaveData(List<SaveDataGroup> dataToSave, Action<SaveDataSource, bool> OnSaveComplete)
	{
		GameObject newObject = new GameObject("CoroutineRunner_Save");
		Action onComplete = delegate
		{
			UnityEngine.Object.Destroy(newObject);
			OnSaveComplete(this, false);
		};
		UpdateForwarder updateForwarder = newObject.AddComponent<UpdateForwarder>();
		updateForwarder.StartCoroutine(Wait(2f, onComplete));
	}

	private IEnumerator Wait(float timeToWait, Action onComplete)
	{
		yield return new WaitForSeconds(timeToWait);
		onComplete();
	}
}
