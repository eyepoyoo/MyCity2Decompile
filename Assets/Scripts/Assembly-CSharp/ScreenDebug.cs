using UnityEngine;

public class ScreenDebug : MonoBehaviour
{
	public Rect debugAreaRelRect = new Rect(0f, 0.5f, 1f, 0.5f);

	public Color textColor = Color.black;

	public int maxEntries = 10000;

	private string log = string.Empty;

	private string previous = string.Empty;

	private GUIStyle style = new GUIStyle();

	private int entriesLeft;

	public string LogContent
	{
		get
		{
			return log;
		}
	}

	private void Start()
	{
		style.normal.textColor = textColor;
		entriesLeft = maxEntries;
	}

	public void Log(string text, bool miniCollapse = false)
	{
		if (!miniCollapse || !(text == previous))
		{
			previous = text;
			if (entriesLeft == 0)
			{
				log = string.Empty;
				entriesLeft = maxEntries;
			}
			else
			{
				entriesLeft--;
			}
			Debug.Log("Logging : " + text);
			log = log + "\n" + text;
		}
	}

	public void ClearLog()
	{
		previous = string.Empty;
		log = string.Empty;
	}

	public void ShowLog()
	{
		GUI.TextArea(new Rect(debugAreaRelRect.xMin * (float)Screen.width, debugAreaRelRect.yMin * (float)Screen.height, (float)Screen.width * debugAreaRelRect.width, (float)Screen.height * debugAreaRelRect.height - 50f), log, maxEntries, style);
	}
}
