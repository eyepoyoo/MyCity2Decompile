using System;
using System.Collections.Generic;
using System.IO;
using GameDefines;
using LitJson;
using UnityEngine;

public class DLCFacade : MonoBehaviour
{
	private const string ASSET_NAME_KEY = "name";

	private const string ASSET_TYPE_KEY = "type";

	private const string ASSET_DOWNLOAD_KEY = "downloadURL";

	private const string ASSET_TAGS_KEY = "tags";

	private const string ASSET_DOWNLOAD_SIZE_KEY = "size";

	private const string DATE_TIME_SAVE_KEY = "lastQueryDateTime";

	private const string QUERY_SAVE_KEY = "lastQuery";

	public static bool DO_DEBUG;

	private static DLCFacade _instance;

	protected List<DLCAsset> dlcAssets = new List<DLCAsset>();

	private List<DLCAsset> oldDLCAssets = new List<DLCAsset>();

	private List<DownloadRequest> activeDownloads = new List<DownloadRequest>();

	private DateTime lastQueryTimeStamp;

	public static DLCFacade Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("DLCFacade");
				_instance = gameObject.AddComponent<DLCFacade>();
			}
			return _instance;
		}
	}

	public static bool Exists
	{
		get
		{
			return _instance != null;
		}
	}

	protected virtual void HandleFatalError()
	{
		Debug.LogError("DLCFacade::Update - FATAL ERROR HANDLE, Handled at base DLVFacade level... quitting game.");
		Application.Quit();
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(this);
		_instance = this;
		if (PlayerPrefs.HasKey("lastQueryDateTime"))
		{
			lastQueryTimeStamp = new DateTime(long.Parse(PlayerPrefs.GetString("lastQueryDateTime")));
		}
		else
		{
			lastQueryTimeStamp = new DateTime(1984, 5, 19);
		}
		Log("Last query time is [" + lastQueryTimeStamp.ToString() + "]");
		if (PlayerPrefs.HasKey("lastQuery"))
		{
			parseFullQueryResponse(PlayerPrefs.GetString("lastQuery"), false);
			for (int i = 0; i < dlcAssets.Count; i++)
			{
				oldDLCAssets.Add(dlcAssets[i]);
			}
			dlcAssets.Clear();
			Log("Old assets found. Quantity [" + oldDLCAssets.Count + "]");
		}
		Log("Initialising");
		initialise();
	}

	private void parseFullQueryResponse(string jsonResponse, bool doCallComplete = true)
	{
		Log("Parsing json response [" + jsonResponse + "]");
		JsonReader reader = new JsonReader(jsonResponse);
		JsonData jsonData = JsonMapper.ToObject(reader);
		string stringOut = string.Empty;
		string stringOut2 = string.Empty;
		string stringOut3 = string.Empty;
		string stringOut4 = string.Empty;
		string stringOut5 = string.Empty;
		jsonData.SetJsonType(JsonType.Array);
		if (!jsonData.IsArray)
		{
			Log("Failed to parse json file! Aborting...");
			queryParseComplete();
		}
		int count = jsonData.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData dataToParse = jsonData[i];
			if (!tryParseJson("name", ref stringOut, dataToParse) || !tryParseJson("downloadURL", ref stringOut2, dataToParse) || !tryParseJson("type", ref stringOut3, dataToParse) || !tryParseJson("tags", ref stringOut4, dataToParse) || !tryParseJson("size", ref stringOut5, dataToParse))
			{
				Log("Failed to parse json entry! Skipping...");
				continue;
			}
			Log("New DLC Asset defined. Name [" + stringOut + "], Type [" + stringOut3 + "], Tags [" + stringOut4 + "], URL [" + stringOut2 + "]");
			DLCAsset dLCAsset = new DLCAsset(stringOut, stringOut2, stringOut4, stringOut3, stringOut5);
			dLCAsset.onSuccessCallBack += assetParser;
			dLCAsset.onFailCallback += assetDownloadFailed;
			dlcAssets.Add(dLCAsset);
		}
		if (doCallComplete)
		{
			PlayerPrefs.SetString("lastQuery", jsonResponse);
			lastQueryTimeStamp = DateTime.Now;
			PlayerPrefs.SetString("lastQueryDateTime", lastQueryTimeStamp.Ticks.ToString());
			Debug.LogWarning("DLCFacade: Last query time set to [" + lastQueryTimeStamp.ToString() + "]");
			deleteAllDeprecatedAssets();
			queryParseComplete();
		}
	}

	private bool tryParseJson(string elementKey, ref string stringOut, JsonData dataToParse)
	{
		try
		{
			stringOut = dataToParse[elementKey].ToString();
			return true;
		}
		catch (Exception ex)
		{
			Log("Exception caught [" + ex.Message + "]");
			return false;
		}
	}

	protected void onInternetCheckFinished(bool hasInternet)
	{
		Log("Has internet? [" + hasInternet + "]");
		makeQueryRequest(hasInternet);
	}

	private void Update()
	{
		if (activeDownloads.Count == 0)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < activeDownloads.Count; i++)
		{
			if (activeDownloads[i]._pCurrentStatus == DownloadStatus.ERROR)
			{
				flag = true;
			}
			if (activeDownloads[i]._pCurrentStatus != DownloadStatus.DOWNLOADING)
			{
				activeDownloads.RemoveAt(i);
				i--;
			}
		}
		if (!flag)
		{
			return;
		}
		Debug.LogError("DLCFacade::Update - FATAL ERROR FOUND, stopping all downloads and handling error.");
		for (int num = activeDownloads.Count; num >= 0; num--)
		{
			if (activeDownloads[num] != null)
			{
				activeDownloads[num].ClearDownload();
				activeDownloads.RemoveAt(num);
			}
		}
		HandleFatalError();
	}

	private void downloadFinished()
	{
	}

	private void onQueryDownloadSuccess(DownloadRequest finishedRequest)
	{
		parseFullQueryResponse(finishedRequest._pResponseString);
	}

	private void onQueryDownloadFailed(DownloadRequest finishedRequest)
	{
		if (!finishedRequest.doLocalDownload)
		{
			makeQueryRequest(false);
		}
	}

	private void deleteAllDeprecatedAssets()
	{
		if (oldDLCAssets == null || oldDLCAssets.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < oldDLCAssets.Count; i++)
		{
			Log("Checking if file [" + oldDLCAssets[i].fileName + "] is still in use...");
			bool flag = false;
			for (int j = 0; j < dlcAssets.Count; j++)
			{
				if (!(oldDLCAssets[i].fileName != dlcAssets[j].fileName))
				{
					Log("...File [" + oldDLCAssets[i].fileName + "] is still in use.");
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				deleteFile(oldDLCAssets[i].localFilePath);
				deleteFile(oldDLCAssets[i].streamingFilePath);
			}
		}
		oldDLCAssets.Clear();
	}

	private void deleteFile(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return;
		}
		try
		{
			File.Delete(filePath);
			Log("Deleted depricated file [" + filePath + "].");
		}
		catch (Exception ex)
		{
			Log("Unable to delete depricated file [" + filePath + "]! Error message [" + ex.Message + "]");
		}
	}

	public void makeQueryRequest(bool doForceNew)
	{
		if (activeDownloads.Count > 0)
		{
			Log("Download in progress. Ignoring new query request.");
			return;
		}
		DownloadRequest downloadRequest = new DownloadRequest();
		downloadRequest.requestName = "InitialQuery";
		downloadRequest.requestUrl = getQueryUrl();
		downloadRequest.doLocalDownload = !doForceNew;
		downloadRequest.onSuccessCallBack += onQueryDownloadSuccess;
		downloadRequest.onFailCallback += onQueryDownloadFailed;
		downloadRequest.formData.Add(new FormDataEntry("LastQueryTimestamp", lastQueryTimeStamp.ToString()));
		downloadRequest.formData.Add(new FormDataEntry("GameKey", getGameKey()));
		downloadRequest.formData.Add(new FormDataEntry("BuildNumber", getBuildNumber()));
		downloadRequest.formData.Add(new FormDataEntry("FidelityInteger", getFidelityInteger()));
		downloadRequest.formData.Add(new FormDataEntry("LanguageCode", getLanguageCode()));
		downloadRequest.formData.Add(new FormDataEntry("TestBuild", getTestBuild()));
		downloadRequest.formData.Add(new FormDataEntry("Platform", "Android"));
		startDownload(downloadRequest);
	}

	public void downloadAll()
	{
		if (dlcAssets != null && dlcAssets.Count > 0)
		{
			int count = dlcAssets.Count;
			for (int i = 0; i < count; i++)
			{
				startDownload(dlcAssets[i]);
			}
		}
	}

	public void startDownload(DownloadRequest newRequest)
	{
		Log("Request to start download [" + newRequest.requestName + "] was recieved.");
		if (newRequest._pHasBeenDownloaded)
		{
			Log("Download already finished. Ignoring request.");
			return;
		}
		newRequest.download();
		activeDownloads.Add(newRequest);
	}

	public void startTagGroupDownload(List<string> tags)
	{
		for (int i = 0; i < tags.Count; i++)
		{
			startTagGroupDownload(tags[i]);
		}
	}

	public void startTagGroupDownload(string tag)
	{
		if (dlcAssets == null)
		{
			return;
		}
		for (int i = 0; i < dlcAssets.Count; i++)
		{
			for (int j = 0; j < dlcAssets[i].assetTags.Count; j++)
			{
				if (!(dlcAssets[i].assetTags[j] != tag))
				{
					startDownload(dlcAssets[i]);
					break;
				}
			}
		}
	}

	public void startTypeDownload(List<AssetType> typesToDownload)
	{
		for (int i = 0; i < typesToDownload.Count; i++)
		{
			startTypeDownload(typesToDownload[i]);
		}
	}

	public void startTypeDownload(AssetType typeToDownload)
	{
		if (dlcAssets == null || dlcAssets.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < dlcAssets.Count; i++)
		{
			if (dlcAssets[i].assetType == typeToDownload)
			{
				startDownload(dlcAssets[i]);
			}
		}
	}

	public void downloadAsset(string assetName, AssetType assetType, Action<DownloadRequest> onSucessCallback, Action<DownloadRequest> onFailCallback)
	{
		Log("Received download request for specific asset [" + assetName + "]");
		for (int i = 0; i < dlcAssets.Count; i++)
		{
			if (dlcAssets[i].assetType != assetType || !(dlcAssets[i].requestName == assetName))
			{
				continue;
			}
			if (dlcAssets[i]._pHasBeenDownloaded)
			{
				Log("Download was not needed. File already downloaded.");
				if (onSucessCallback != null)
				{
					onSucessCallback(dlcAssets[i]);
				}
				return;
			}
			if (onSucessCallback != null)
			{
				dlcAssets[i].onSuccessCallBack += onSucessCallback;
			}
			if (onFailCallback != null)
			{
				dlcAssets[i].onFailCallback += onFailCallback;
			}
			Log("Download is required.");
			startDownload(dlcAssets[i]);
			return;
		}
		Log("Asset not found.");
		if (onFailCallback != null)
		{
			onFailCallback(null);
		}
	}

	public virtual void initialise()
	{
		if (InternetCheckFacade._pIsReadyForInternetChecks)
		{
			onInternetCheckFinished(InternetCheckFacade._pIsOnline);
		}
		else
		{
			InternetCheckFacade.RequestInternetCheck(onInternetCheckFinished);
		}
	}

	protected virtual string getQueryUrl()
	{
		return "http://demos.4t2.co.uk/DLCTest/exampleResponse.json";
	}

	public virtual string getGameKey()
	{
		return "OURTEC";
	}

	public virtual string getBuildNumber()
	{
		return "1";
	}

	public virtual string getFidelityInteger()
	{
		return "0";
	}

	public virtual string getLanguageCode()
	{
		return GlobalDefines.DEFAULT_LANGUAGE;
	}

	public virtual string getTestBuild()
	{
		return "true";
	}

	protected virtual void queryParseComplete()
	{
		Log("Parsing of json response complete!");
	}

	protected virtual void assetParser(DownloadRequest finishedRequest)
	{
		Log("Parsing asset [" + finishedRequest.requestName + "]");
		DLCAsset dLCAsset = (DLCAsset)finishedRequest;
		if (dLCAsset == null)
		{
			Log("Parsing non-DLC asset");
			parseNonDLCAsset(finishedRequest);
		}
		Log("Parsing " + dLCAsset.assetType);
		switch (dLCAsset.assetType)
		{
		case AssetType.TEXT:
			parseText(dLCAsset);
			break;
		case AssetType.IMAGE:
			parseImage(dLCAsset);
			break;
		case AssetType.VIDEO:
			parseVideo(dLCAsset);
			break;
		case AssetType.ITEM_BUNDLE:
			parseItemBundle(dLCAsset);
			break;
		case AssetType.LEVEL_BUNDLE:
			parseLevelBundle(dLCAsset);
			break;
		case AssetType.TEXTURE_ATLAS:
			parseTextureAtlas(dLCAsset);
			break;
		case AssetType.TEXTURE_ATLAS_TEXTURE:
			parseTextureAtlasTexture(dLCAsset);
			break;
		}
	}

	protected virtual void parseTextureAtlasTexture(DLCAsset finishedRequest)
	{
	}

	protected virtual void parseTextureAtlas(DLCAsset finishedRequest)
	{
	}

	protected virtual void parseNonDLCAsset(DownloadRequest finishedRequest)
	{
	}

	protected virtual void parseText(DLCAsset dlcTextAsset)
	{
	}

	protected virtual void parseImage(DLCAsset dlcImageAsset)
	{
	}

	protected virtual void parseVideo(DLCAsset dlcVideoAsset)
	{
	}

	protected virtual void parseLevelBundle(DLCAsset dlcLevelBundle)
	{
	}

	protected virtual void parseItemBundle(DLCAsset dlcItemBundle)
	{
	}

	protected virtual void assetDownloadFailed(DownloadRequest finishedRequest)
	{
		finishedRequest.ClearDownload();
		if (!finishedRequest.doUseDefaultFidelity)
		{
			Log("Asset download failed. Attempting to download using default fidelity.");
			finishedRequest.doUseDefaultFidelity = true;
			startDownload(finishedRequest);
		}
		else if (!finishedRequest.doUseDefaultLanguage)
		{
			Log("Asset download failed. Attempting to download using default language.");
			finishedRequest.doUseDefaultLanguage = true;
			startDownload(finishedRequest);
		}
		else if (!finishedRequest.doUseDefaultPlatform)
		{
			Log("Asset download failed. Attempting to download using default platform.");
			finishedRequest.doUseDefaultPlatform = true;
			startDownload(finishedRequest);
		}
		else
		{
			Log("Asset download failed using all default parameters.");
		}
	}

	public int getDownloadQueueLength()
	{
		return activeDownloads.Count;
	}

	public int getTotalDownloadQueueFileSize()
	{
		int num = 0;
		for (int i = 0; i < activeDownloads.Count; i++)
		{
			num += activeDownloads[i].downloadSize;
		}
		return num;
	}

	public float getCurrentDownloadProgress()
	{
		if (activeDownloads.Count == 0)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < activeDownloads.Count; i++)
		{
			if (activeDownloads[i]._pHasResponse)
			{
				num *= activeDownloads[i]._pDecimalProgress;
			}
		}
		return num;
	}

	public int getCurrentDownloadBytes()
	{
		if (activeDownloads.Count == 0)
		{
			return 0;
		}
		float num = 0f;
		for (int i = 0; i < activeDownloads.Count; i++)
		{
			if (activeDownloads[i]._pHasResponse)
			{
				num += activeDownloads[i]._pDecimalProgress * (float)activeDownloads[i].downloadSize;
			}
		}
		return (int)num;
	}

	public int getCurrentUndownloadedBytesInDownloadQueue()
	{
		if (activeDownloads.Count == 0)
		{
			return 0;
		}
		return getTotalDownloadQueueFileSize() - getCurrentDownloadBytes();
	}

	public static void Log(string message, UnityEngine.Object o = null)
	{
		if (DO_DEBUG)
		{
			Debug.Log("DLC: " + message, o);
		}
	}

	public int GetNumAssetsOfType(AssetType type)
	{
		int num = 0;
		for (int i = 0; i < dlcAssets.Count; i++)
		{
			if (dlcAssets[i].assetType == type)
			{
				num++;
			}
		}
		return num;
	}

	public int GetNumAssetsOfTypeDownloaded(AssetType type)
	{
		int num = 0;
		for (int i = 0; i < dlcAssets.Count; i++)
		{
			if (dlcAssets[i].assetType == type && dlcAssets[i]._pHasBeenDownloaded)
			{
				num++;
			}
		}
		return num;
	}

	public float GetAssetTypeDownloadProgress(AssetType type)
	{
		int numAssetsOfType = GetNumAssetsOfType(type);
		if (numAssetsOfType == 0)
		{
			return 1f;
		}
		return (float)GetNumAssetsOfTypeDownloaded(type) / (float)numAssetsOfType;
	}

	public DLCAsset GetAsset(AssetType type, string name)
	{
		foreach (DLCAsset dlcAsset in dlcAssets)
		{
			if (dlcAsset.requestName == name && dlcAsset.assetType == type)
			{
				return dlcAsset;
			}
		}
		return null;
	}

	public DLCAsset[] GetAssetsOfType(AssetType type)
	{
		List<DLCAsset> list = new List<DLCAsset>();
		foreach (DLCAsset dlcAsset in dlcAssets)
		{
			if (dlcAsset.assetType == type)
			{
				list.Add(dlcAsset);
			}
		}
		return list.ToArray();
	}
}
