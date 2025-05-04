using System;
using System.Collections.Generic;
using UnityEngine;

public class DLCCMSPreviewer : MonoBehaviour
{
	private static Type[] acceptableTypes = new Type[4]
	{
		typeof(Texture2D),
		typeof(GameObject),
		typeof(TextAsset),
		typeof(AudioClip)
	};

	private static Dictionary<string, PreviewType> extensionToTypeIndex = new Dictionary<string, PreviewType>
	{
		{
			".jpg",
			PreviewType.TEXTURE
		},
		{
			".jpeg",
			PreviewType.TEXTURE
		},
		{
			".gif",
			PreviewType.TEXTURE
		},
		{
			".png",
			PreviewType.TEXTURE
		},
		{
			".bmp",
			PreviewType.TEXTURE
		},
		{
			".ogg",
			PreviewType.AUDIO
		},
		{
			".wav",
			PreviewType.AUDIO
		},
		{
			".mp4",
			PreviewType.VIDEO
		},
		{
			".ogv",
			PreviewType.VIDEO
		},
		{
			".unity3d",
			PreviewType.ASSET_BUNDLE
		},
		{
			".assetbundle",
			PreviewType.ASSET_BUNDLE
		},
		{
			".txt",
			PreviewType.TEXT
		},
		{
			".xml",
			PreviewType.TEXT
		},
		{
			".json",
			PreviewType.TEXT
		}
	};

	public GameObject objectViewer;

	public ObjectViewer objectViewerScript;

	private bool isDownloading;

	private WWW download;

	private string errorMessage = string.Empty;

	private GUISkin viewerSkin;

	private List<PreviewObject> newObjects;

	private void Start()
	{
		viewerSkin = (GUISkin)ScriptableObject.CreateInstance(typeof(GUISkin));
		viewerSkin.label.alignment = TextAnchor.MiddleCenter;
		Application.ExternalCall("OnUnityPlayerLoaded");
	}

	private void reset()
	{
		if (isDownloading)
		{
			download.Dispose();
		}
		if (objectViewer.activeInHierarchy && objectViewerScript != null)
		{
			objectViewerScript.clear();
			objectViewerScript.objectArray = null;
			objectViewer.SetActive(false);
		}
		newObjects = new List<PreviewObject>();
		errorMessage = string.Empty;
		base.gameObject.GetComponent<Camera>().enabled = true;
		isDownloading = false;
	}

	public void downloadAndDisplay(string contentURL)
	{
		reset();
		try
		{
			download = new WWW(contentURL);
			isDownloading = true;
		}
		catch (Exception ex)
		{
			errorMessage = ex.Message;
			isDownloading = false;
		}
	}

	private void Update()
	{
		if (isDownloading && (download.isDone || (download.error != null && download.error.Length != 0)))
		{
			if (download.error != null && download.error.Length > 0)
			{
				downloadFailed();
			}
			else
			{
				downloadSucceeded();
			}
		}
	}

	private void downloadFailed()
	{
		errorMessage = download.error;
		isDownloading = false;
	}

	private void downloadSucceeded()
	{
		isDownloading = false;
		string text = download.url.Substring(download.url.LastIndexOf('/') + 1);
		bool flag = false;
		flag = checkAgainstExtensionList(download);
		if (!flag)
		{
			flag = hardCheckDownload(download);
		}
		if (!flag)
		{
			return;
		}
		for (int i = 0; i < newObjects.Count; i++)
		{
			if (newObjects[i] != null && newObjects[i].isPopulated())
			{
				string text2 = "[" + i + "]_" + ((!(newObjects[i].previewObject == null) && !string.IsNullOrEmpty(newObjects[i].previewObject.name)) ? newObjects[i].previewObject.name : text);
				if (text2.IndexOf('?') != -1)
				{
					text2 = text2.Substring(0, text2.IndexOf('?'));
				}
				newObjects[i].name = text2;
				Debug.Log(string.Concat("Object [", i + 1, "] defined [", newObjects[i].name, "] as type [", newObjects[i].type, "]"));
			}
		}
		base.gameObject.GetComponent<Camera>().enabled = false;
		objectViewer.SetActive(true);
		objectViewerScript.objectArray = newObjects.ToArray();
		objectViewerScript.init();
	}

	private bool parseAssetBundle(AssetBundle newBundle)
	{
		if (newBundle == null)
		{
			return false;
		}
		UnityEngine.Object[] array = newBundle.LoadAllAssets();
		for (int i = 0; i < array.Length; i++)
		{
			if (!checkObjectAgainstExtensionList(array[i]))
			{
				parseObject(array[i]);
			}
		}
		return true;
	}

	private bool parseObject(UnityEngine.Object newObject)
	{
		if (newObject == null)
		{
			return false;
		}
		Type type = newObject.GetType();
		for (int i = 0; i < acceptableTypes.Length; i++)
		{
			if (type == acceptableTypes[i])
			{
				PreviewObject previewObject = new PreviewObject();
				previewObject.previewObject = newObject;
				previewObject.type = (PreviewType)i;
				newObjects.Add(previewObject);
				return true;
			}
		}
		return false;
	}

	private bool parseText(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		PreviewObject previewObject = new PreviewObject();
		previewObject.type = PreviewType.TEXT;
		previewObject.downloadedText = text;
		newObjects.Add(previewObject);
		return true;
	}

	private void OnGUI()
	{
		GUISkin skin = GUI.skin;
		GUI.skin = viewerSkin;
		Rect position = new Rect(0f, (float)Screen.height * 0.45f, Screen.width, (float)Screen.height * 0.1f);
		if (isDownloading && download != null)
		{
			GUI.Label(position, Mathf.FloorToInt(download.progress * 100f).ToString("D2") + "%", "Label");
		}
		else if (!string.IsNullOrEmpty(errorMessage))
		{
			GUI.Label(position, "ERROR: " + errorMessage, "Label");
		}
		GUI.skin = skin;
	}

	private bool checkObjectAgainstExtensionList(UnityEngine.Object newObject)
	{
		string extension = getExtension(newObject.name);
		Debug.Log("Checking extension [" + extension + "]");
		if (!extensionToTypeIndex.ContainsKey(extension))
		{
			return false;
		}
		PreviewObject previewObject = new PreviewObject();
		previewObject.type = extensionToTypeIndex[extension];
		PreviewType type = previewObject.type;
		if (type != PreviewType.TEXT)
		{
			previewObject.previewObject = newObject;
		}
		if (!previewObject.isPopulated())
		{
			return false;
		}
		newObjects.Add(previewObject);
		return true;
	}

	private bool checkAgainstExtensionList(WWW download)
	{
		string extension = getExtension(download.url);
		Debug.Log("Checking extension [" + extension + "]");
		if (!extensionToTypeIndex.ContainsKey(extension))
		{
			return false;
		}
		PreviewObject previewObject = new PreviewObject();
		previewObject.type = extensionToTypeIndex[extension];
		string text = download.url.Substring(download.url.LastIndexOf('/') + 1);
		if (text.IndexOf('?') != -1)
		{
			text = text.Substring(0, text.IndexOf('?'));
		}
		switch (previewObject.type)
		{
		case PreviewType.TEXTURE:
			previewObject.previewObject = download.textureNonReadable;
			break;
		case PreviewType.AUDIO:
			previewObject.previewObject = download.GetAudioClip();
			break;
		case PreviewType.TEXT:
			previewObject.downloadedText = download.text;
			previewObject.name = text;
			break;
		case PreviewType.ASSET_BUNDLE:
			previewObject.previewObject = download.assetBundle;
			break;
		}
		if (!previewObject.isPopulated())
		{
			return false;
		}
		newObjects.Add(previewObject);
		return true;
	}

	private string getExtension(string unknownString)
	{
		int num = unknownString.LastIndexOf('.');
		if (num == -1)
		{
			return string.Empty;
		}
		string text = unknownString.Substring(num);
		int num2 = text.IndexOf('?');
		if (num2 != -1)
		{
			text = text.Substring(0, num2);
		}
		return text;
	}

	private bool hardCheckDownload(WWW download)
	{
		bool flag = false;
		try
		{
			flag = parseObject(download.GetAudioClip());
		}
		catch (Exception ex)
		{
			if (ex.Message == null)
			{
				Debug.Log("How odd.");
			}
		}
		if (!flag)
		{
			try
			{
				flag = parseAssetBundle(download.assetBundle);
			}
			catch (Exception ex2)
			{
				if (ex2.Message == null)
				{
					Debug.Log("How odd.");
				}
			}
		}
		if (!flag)
		{
			try
			{
				flag = parseObject(download.textureNonReadable);
			}
			catch (Exception ex3)
			{
				if (ex3.Message == null)
				{
					Debug.Log("How odd.");
				}
			}
		}
		if (!flag)
		{
			try
			{
				flag = parseText(download.text);
			}
			catch (Exception ex4)
			{
				if (ex4.Message == null)
				{
					Debug.Log("How odd.");
				}
			}
		}
		return flag;
	}
}
