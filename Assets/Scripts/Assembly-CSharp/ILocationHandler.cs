using LitJson;

public interface ILocationHandler
{
	int locationChangePriority { get; }

	void ChangeLocation(string previous, ref string current, string linkName, JsonData linkData, JsonData currentLocationData);
}
