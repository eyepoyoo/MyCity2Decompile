using LitJson;
using UnityEngine;

public class FlowDebug : MonoBehaviour, ILocationHandler
{
	public Color labelColor;

	private string _previous = "?";

	private string _current = "?";

	private string _link = "?";

	private string _output = string.Empty;

	public int locationChangePriority
	{
		get
		{
			return 2;
		}
	}

	private void Awake()
	{
	}

	public void ChangeLocation(string previous, ref string current, string linkName, JsonData linkData, JsonData currentLocationData)
	{
		_previous = previous;
		_current = current;
		_link = linkName;
		_output = "[" + _previous + "] -> " + _link + " -> [" + _current + "]";
	}
}
