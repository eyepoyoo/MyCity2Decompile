using System;
using System.Collections.Generic;

[Serializable]
public class SaveDataSource
{
	public bool doUseSaveSource;

	public int SavePriority;

	public virtual bool _pHasInitialised { get; protected set; }

	public virtual void LoadData(string[] saveGroupIds, Action<SaveDataSource, List<SaveDataGroup>, bool> OnLoadComplete)
	{
	}

	public virtual void LoadAllData(Action<SaveDataSource, List<SaveDataGroup>, bool> OnLoadComplete)
	{
	}

	public virtual void SaveData(List<SaveDataGroup> dataToSave, Action<SaveDataSource, bool> OnSaveComplete)
	{
	}

	public virtual void DeleteData(List<string> groupIdsToDelete)
	{
	}

	public virtual void DeleteDataEntries(List<string> groupIdsToDelete, List<string> entryIds)
	{
	}
}
