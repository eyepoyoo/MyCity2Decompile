using System;
using LitJson;
using UnityEngine;

public class PersistentBehaviour : MonoBehaviour, IPersistent
{
	public AmuzoEncryption _encryption;

	protected bool _markedForSave;

	public AmuzoEncryption encryption
	{
		get
		{
			return _encryption;
		}
	}

	public virtual bool isGlobal
	{
		get
		{
			return false;
		}
	}

	public virtual string persistenceKey
	{
		get
		{
			throw new Exception("Must Be Overridden");
		}
	}

	public bool markedForSave
	{
		get
		{
			return _markedForSave;
		}
		set
		{
			_markedForSave = value;
		}
	}

	public virtual void Load(JsonData data)
	{
	}

	public virtual string Save()
	{
		return null;
	}

	public override string ToString()
	{
		return Extensions.PrettyPrint(Save());
	}

	public void FromString(string saveData)
	{
		Load(Extensions.LoadJson(saveData));
	}
}
