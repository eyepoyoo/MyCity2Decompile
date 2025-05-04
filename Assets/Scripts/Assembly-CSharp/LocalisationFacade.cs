using GameDefines;
using LitJson;
using UnityEngine;

public sealed class LocalisationFacade : LocalisationNode
{
	private static bool DO_DEBUG = true;

	public static bool DO_DOUBLE_TEXT = false;

	public static bool DO_OVERRIDE_TEXT_COLOUR = false;

	public static string OVERRIDE_COLOUR = "C90020";

	public static string OVERRIDE_TEMPLATE = "[{0}]{1}[-]";

	public static bool SHOW_LOCALISATION_NODES = false;

	private static bool _sIsDebugMenuAdded = false;

	public static string defaultLang = GlobalDefines.DEFAULT_LANGUAGE;

	private static JsonData _localisationTable = null;

	private static LocalisationFacade _instance = null;

	private string _language;

	private string[] _availableLanguages;

	public static LocalisationFacade Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new LocalisationFacade();
				Facades<LocalisationFacade>.Register(_instance);
				TextAsset textAsset = Resources.Load("localisation") as TextAsset;
				_instance.Initialise(defaultLang, Extensions.LoadJson(textAsset.text));
			}
			return _instance;
		}
	}

	public string language
	{
		get
		{
			return _language;
		}
		set
		{
			for (int i = 0; i < availableLanguages.Length; i++)
			{
				if (_availableLanguages[i] == value)
				{
					_language = value;
					return;
				}
			}
			Log("Cannot set language to '" + value + "' because it is not in available list of languages");
		}
	}

	public string[] availableLanguages
	{
		get
		{
			if (_localisation == null)
			{
				_availableLanguages = null;
				return null;
			}
			if (_availableLanguages != null)
			{
				return _availableLanguages;
			}
			JsonData jsonData = _localisation.JsonAtPath("Languages/list");
			if (jsonData == null)
			{
				return null;
			}
			_availableLanguages = ((string)jsonData).Split('|');
			return _availableLanguages;
		}
	}

	private LocalisationFacade()
	{
	}

	public void Initialise(string language, JsonData localisationTable)
	{
		_localisationTable = localisationTable;
		if (_localisationTable != null)
		{
			_localisation = _localisationTable.JsonAtPath("localisation");
		}
		else
		{
			_localisation = null;
		}
		Log("Defualt language code [" + language + "].");
		_language = GetLanguageCode(Application.systemLanguage);
		if (_language == null)
		{
			_language = language;
		}
	}

	private void Reinitialize()
	{
		Initialise(_language, _localisationTable);
	}

	public static string GetLanguageCode(string languageString)
	{
		if (string.IsNullOrEmpty(languageString))
		{
			return null;
		}
		Log("Language code received [" + languageString + "]");
		string text;
		switch (languageString.ToLower())
		{
		case "pt":
		case "por":
			text = "PT";
			break;
		case "cs":
			text = "CS";
			break;
		case "dan":
		case "da":
			text = "DA";
			break;
		case "gsw":
		case "ger":
		case "deu":
		case "de":
		case "nds":
		case "gmh":
		case "goh":
		case "gem":
			text = "DE";
			break;
		case "spa":
		case "es":
		case "lad":
			text = "ES";
			break;
		case "fi":
			text = "FI";
			break;
		case "fre":
		case "fra":
		case "fr":
			text = "FR";
			break;
		case "hu":
			text = "HU";
			break;
		case "ita":
		case "it":
			text = "IT";
			break;
		case "ja":
		case "jp":
			text = "JA";
			break;
		case "ko":
			text = "KO";
			break;
		case "dut":
		case "nld":
		case "nl":
		case "dum":
			text = "NL";
			break;
		case "no":
		case "nn":
		case "nb":
			text = "NB";
			break;
		case "pl":
			text = "PL";
			break;
		case "ru":
			text = "RU";
			break;
		case "sv":
			text = "SV";
			break;
		case "zh":
		case "zhsi":
		case "zh-hans":
		case "zh-hant":
			text = "ZH";
			break;
		case "gre":
		case "ell":
		case "el":
			text = "EL";
			break;
		default:
			text = defaultLang;
			break;
		}
		Log("Language code returned [" + text + "]");
		return text;
	}

	public static string GetLanguageCode(SystemLanguage language)
	{
		string empty = string.Empty;
		switch (language)
		{
		case SystemLanguage.Portuguese:
			return "PT";
		case SystemLanguage.Czech:
			return "CS";
		case SystemLanguage.Danish:
			return "DA";
		case SystemLanguage.German:
			return "DE";
		case SystemLanguage.Spanish:
			return "ES";
		case SystemLanguage.Finnish:
			return "FI";
		case SystemLanguage.French:
			return "FR";
		case SystemLanguage.Hungarian:
			return "HU";
		case SystemLanguage.Italian:
			return "IT";
		case SystemLanguage.Japanese:
			return "JA";
		case SystemLanguage.Korean:
			return "KO";
		case SystemLanguage.Dutch:
			return "NL";
		case SystemLanguage.Norwegian:
			return "NB";
		case SystemLanguage.Polish:
			return "PL";
		case SystemLanguage.Russian:
			return "RU";
		case SystemLanguage.Swedish:
			return "SV";
		case SystemLanguage.Chinese:
			return "ZH";
		case SystemLanguage.ChineseSimplified:
			return "ZH";
		case SystemLanguage.ChineseTraditional:
			return "ZH";
		case SystemLanguage.Greek:
			return "EL";
		default:
			return defaultLang;
		}
	}

	public static void Log(string message, Object o = null)
	{
		if (DO_DEBUG)
		{
			Debug.Log("LocalisationFacade: " + message, o);
		}
	}

	private void AddDebugMenu()
	{
		if (!_sIsDebugMenuAdded)
		{
			_sIsDebugMenuAdded = true;
			AmuzoDebugMenu amuzoDebugMenu = new AmuzoDebugMenu("LOCALISATION FACADE");
			amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton(() => (!SHOW_LOCALISATION_NODES) ? "SHOW LOCALISATION NODES" : "SHOW LOCALISED TEXT", delegate
			{
				ToggleLocalisationNodes();
			}));
			amuzoDebugMenu.AddButton(new AmuzoDebugMenuButton(() => (!DO_OVERRIDE_TEXT_COLOUR) ? "OVERRIDE TEXT COLOUR" : "NORMAL TEXT COLOUR", delegate
			{
				ToggleLocalisationColour();
			}));
			AmuzoDebugMenuManager.RegisterRootDebugMenu(amuzoDebugMenu);
		}
	}

	private void ToggleLocalisationNodes()
	{
		SHOW_LOCALISATION_NODES = !SHOW_LOCALISATION_NODES;
	}

	private void ToggleLocalisationColour()
	{
		DO_OVERRIDE_TEXT_COLOUR = !DO_OVERRIDE_TEXT_COLOUR;
	}
}
