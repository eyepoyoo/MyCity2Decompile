using LitJson;

public interface IPersistent
{
	AmuzoEncryption encryption { get; }

	bool isGlobal { get; }

	string persistenceKey { get; }

	bool markedForSave { get; set; }

	void Load(JsonData data);

	string Save();
}
