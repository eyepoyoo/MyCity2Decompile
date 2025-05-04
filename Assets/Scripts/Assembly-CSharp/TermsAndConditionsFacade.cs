using UnityEngine;

public class TermsAndConditionsFacade : MonoBehaviour
{
	public const string _cTermsNode = "TermsAndConditions";

	public const string _cEntryBody = "TermsOfUse";

	private const float BUTTON_HEIGHT_DECIMAL = 0.1f;

	private const float BUTTON_WIDTH_DECIMAL = 0.2f;

	private const float GAP_DECIMAL = 0.05f;

	private GameObject onGUIForwarderObject;

	private GUIStyle buttonStyle;

	private GUIStyle displayStyle;

	private Texture2D blackTexture;

	private string localisedTextToDisplay = string.Empty;

	private string localisedBackButtonText = string.Empty;

	private Rect buttonRect = new Rect(1f, 1f, 1f, 1f);

	private Rect scrollAreaRect = new Rect(1f, 1f, 1f, 1f);

	private Rect backgroundRect = new Rect(1f, 1f, 1f, 1f);

	private Vector2 scrollPosition = Vector2.zero;

	private float screenWidth;

	private float screenHeight;

	private void Awake()
	{
		Facades<TermsAndConditionsFacade>.Register(this);
	}

	public void showPage()
	{
		hidePage();
		LocalisationNode node = Facades<LocalisationFacade>.Instance.GetNode("TermsAndConditions");
		if (node == null)
		{
			Debug.LogWarning("TermsAndConditionsFacade: Unable to find localisation target neuron or target path was blank. Cannot display T&C's page.");
			return;
		}
		if (node._numChildren > 1)
		{
			string text = string.Empty;
			localisedTextToDisplay = string.Empty;
			int num = 0;
			do
			{
				num++;
				if (text.Length > 0)
				{
					if (localisedTextToDisplay.Length > 0)
					{
						string text2 = localisedTextToDisplay;
						localisedTextToDisplay = text2 + " " + '\n' + '\n';
					}
					localisedTextToDisplay += text;
				}
				text = node.GetString("TermsOfUse_" + num.ToString("D2"));
			}
			while (!string.IsNullOrEmpty(text));
		}
		else
		{
			localisedTextToDisplay = getLocalisedText("TermsAndConditionsTermsOfUse");
		}
		if (string.IsNullOrEmpty(localisedTextToDisplay))
		{
			Debug.LogWarning("TermsAndConditionsFacade: Unable to find localised text neuron or there was no text. Cannot display T&C's page.");
			return;
		}
		localisedBackButtonText = node.GetString("Back");
		screenWidth = -1f;
		onGUIForwarderObject = new GameObject("TermsAndConditionsPage");
		OnGUIForwarder onGUIForwarder = onGUIForwarderObject.AddComponent<OnGUIForwarder>();
		onGUIForwarder.onGUIFunctionToForwardTo = onGUI;
	}

	private string getLocalisedText(string localisationTarget)
	{
		return Facades<LocalisationFacade>.Instance.GetString(localisationTarget);
	}

	public void hidePage()
	{
		if (!(onGUIForwarderObject == null))
		{
			Object.Destroy(onGUIForwarderObject);
		}
	}

	private void setRects()
	{
		buttonStyle = new GUIStyle();
		displayStyle = new GUIStyle();
		blackTexture = new Texture2D(1, 1);
		backgroundRect.x = 0f;
		backgroundRect.y = 0f;
		backgroundRect.width = Screen.width;
		backgroundRect.height = Screen.height;
		buttonRect.width = (float)Screen.width * 0.2f;
		buttonRect.x = ((float)Screen.width - buttonRect.width) * 0.5f;
		buttonRect.height = (float)Screen.height * 0.1f;
		buttonRect.y = (float)Screen.height - (float)Screen.height * 0.05f - buttonRect.height;
		scrollAreaRect.x = (float)Screen.width * 0.05f;
		scrollAreaRect.width = (float)Screen.width - (float)Screen.width * 0.05f * 2f;
		scrollAreaRect.y = (float)Screen.height * 0.05f;
		scrollAreaRect.height = (float)Screen.height - (buttonRect.height + (float)Screen.height * 0.05f * 3f);
		GUI.skin.verticalScrollbar.fixedWidth = (float)Screen.width * 0.05f;
		GUI.skin.verticalScrollbarThumb.fixedWidth = (float)Screen.width * 0.05f;
		GUI.skin.verticalScrollbarDownButton.fixedWidth = (float)Screen.width * 0.05f;
		GUI.skin.verticalScrollbarUpButton.fixedWidth = (float)Screen.width * 0.05f;
		displayStyle.fontSize = Mathf.RoundToInt(0.017578125f * (float)Screen.width);
		buttonStyle.fontSize = Mathf.RoundToInt(0.017578125f * (float)Screen.width);
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		blackTexture.SetPixel(0, 0, Color.black);
		blackTexture.Apply();
		displayStyle.wordWrap = true;
		displayStyle.normal.textColor = Color.white;
		buttonStyle.normal.textColor = Color.white;
		buttonStyle.alignment = TextAnchor.MiddleCenter;
	}

	private void onGUI()
	{
		if (screenWidth != (float)Screen.width || screenHeight != (float)Screen.height)
		{
			setRects();
		}
		GUI.DrawTexture(backgroundRect, blackTexture);
		GUILayout.BeginArea(scrollAreaRect);
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, displayStyle);
		GUILayout.Box(localisedTextToDisplay, displayStyle);
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		if (GUI.Button(buttonRect, string.Empty))
		{
			hidePage();
		}
		GUI.Label(buttonRect, localisedBackButtonText, buttonStyle);
	}
}
