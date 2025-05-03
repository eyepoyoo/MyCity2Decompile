using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DownloadRequest
{
	private const char RETURN = '\r';

	private const char NEW_L = '\n';

	private const char TAB = '\t';

	private const char QUOTE = '"';

	private const string DATE_KEY = "DATE";

	private const string CACHE_CONTROL_KEY = "CACHE-CONTROL";

	private const string MAX_AGE_KEY = "max-age";

	private const string FIDELITY_WILDCARD = "{fidelity}";

	private const string LANGUAGE_WILDCARD = "{language}";

	private const string PLATFORM_WILDCARD = "{platform}";

	private const string DEFAULT_FIDELITY = "0";

	private const string PLATFORM_WEB = "WebPlayer";

	private const string PLATFORM_IOS = "iPhone";

	private const string PLATFORM_ANDROID = "Android";

	private const string ASSET_BUNDLE_DEFAULT_FILETYPE = ".unity3d";

	private const string ASSET_BUNDLE_MOBILE_FILETYPE = ".assetbundle";

	private const int MAX_RETRYS = 3;

	private const float DEFAULT_TIMEOUT = 2.5f;

	private const string URL_FILENAME_REPLACEMENT = "_";

	private static string NEW_LINE = string.Empty + '\r' + '\n';

	private static string[] URL_FILENAME_PARTS_TO_REPLACE = new string[6] { "://", ".", "/", "?", "&", "=" };

	private string DEFAULT_LANGUAGE = "EN_UK";

	public string fileName;

	public string requestName;

	public string localFilePath;

	public string streamingFilePath;

	public List<FormDataEntry> formData = new List<FormDataEntry>();

	public Dictionary<string, string> formHeaders = new Dictionary<string, string>();

	public bool doDebug;

	public bool doVerboseDebug;

	public float _requestTimeout = 2.5f;

	private WWW _www;

	private WWW _localHeaderInfoDownload;

	public bool doRemoteDownload = true;

	public bool doLocalDownload = true;

	public bool doLocalHeaderInfo = true;

	public bool doCacheDownload = true;

	public bool doCacheHeaderInfo = true;

	public bool doUseCacheTimeout = true;

	public int downloadSize;

	public int numAttempts;

	private bool hasTriedStreamingAssets;

	private DownloadLocation _downloadLocation;

	private Dictionary<string, string> _responseHeaders;

	private GameObject _updateObject;

	private DateTime _downloadTime;

	private TimeSpan _cacheTimeout;

	private string _dataPath;

	private string _streamingPath;

	private string _rawURL;

	private string _requestURL;

	private string _persistantHeaderInfoFilePath;

	private string _streamingHeaderInfoFilePath;

	private int _lastBytes;

	private float _timeoutTimer;

	private bool _hasBeenDownloaded;

	private bool _isUsingDefaultFidelity;

	private bool _isUsingDefaultLanguage;

	private bool _isUsingDefaultPlatform;

	private bool _hasEncounteredFatalError;

	private bool _hasPreprocessed;

	private bool _hasCheckedForLocalHeaderInfo;

	private bool _hasDLDateBeenSet;

	private bool _hasCacheTimeoutBeenSet;

	private DownloadStatus _currentStatus;

	private EDownloadLibrary _downloadLibrary;

	private EDownloadLibrary _downloadLibraryUsed;

	public bool _pHasEncounteredFatalError
	{
		get
		{
			return _hasEncounteredFatalError;
		}
	}

	public bool _pHasBeenDownloaded
	{
		get
		{
			return _hasBeenDownloaded;
		}
	}

	public DownloadLocation _pDownloadLocation
	{
		get
		{
			return _downloadLocation;
		}
	}

	public DownloadStatus _pCurrentStatus
	{
		get
		{
			return _currentStatus;
		}
	}

	public string _pRequestURLWithFormData
	{
		get
		{
			if (formData == null || formData.Count == 0)
			{
				return _requestURL;
			}
			string text = _requestURL + "?";
			for (int i = 0; i < formData.Count; i++)
			{
				string text2 = text;
				text = text2 + formData[i].key + "=" + formData[i].value + "&";
			}
			return text.Substring(0, text.Length - 1);
		}
	}

	public bool hasLocalFile
	{
		get
		{
			return File.Exists(localFilePath);
		}
	}

	public bool hasLocalHeaderFile
	{
		get
		{
			return File.Exists(_persistantHeaderInfoFilePath);
		}
	}

	public bool hasLocalStreamingFile
	{
		get
		{
			return File.Exists(streamingFilePath);
		}
	}

	public bool hasLocalStreamingHeaderFile
	{
		get
		{
			return File.Exists(_streamingHeaderInfoFilePath);
		}
	}

	public bool doUseDefaultFidelity
	{
		get
		{
			return _isUsingDefaultFidelity;
		}
		set
		{
			if (value != _isUsingDefaultLanguage)
			{
				_isUsingDefaultFidelity = value;
				parseURL();
			}
		}
	}

	public bool doUseDefaultLanguage
	{
		get
		{
			return _isUsingDefaultLanguage;
		}
		set
		{
			if (value != _isUsingDefaultLanguage)
			{
				_isUsingDefaultLanguage = value;
				parseURL();
			}
		}
	}

	public bool doUseDefaultPlatform
	{
		get
		{
			return _isUsingDefaultPlatform;
		}
		set
		{
			if (value != _isUsingDefaultPlatform)
			{
				_isUsingDefaultPlatform = value;
				parseURL();
			}
		}
	}

	public string dataPath
	{
		get
		{
			return _dataPath;
		}
		set
		{
			if (_dataPath != value)
			{
				_dataPath = value;
				parseURL();
			}
		}
	}

	public string requestUrl
	{
		get
		{
			return _requestURL;
		}
		set
		{
			if (_rawURL != value)
			{
				_rawURL = value;
				parseURL();
			}
		}
	}

	public string _pResponseString
	{
		get
		{
			if (_downloadLibrary == EDownloadLibrary.UNITY && _www != null)
			{
				return (!_www.isDone || !string.IsNullOrEmpty(_www.error)) ? string.Empty : _www.text;
			}
			return null;
		}
	}

	public string _pErrorString
	{
		get
		{
			if (_downloadLibrary == EDownloadLibrary.UNITY && _www != null)
			{
				return _www.error;
			}
			return null;
		}
	}

	public AssetBundle _pAssetBundle
	{
		get
		{
			if (_downloadLibrary == EDownloadLibrary.UNITY && _www != null)
			{
				return _www.assetBundle;
			}
			return null;
		}
	}

	public bool _pHasResponse
	{
		get
		{
			if (_downloadLibrary == EDownloadLibrary.UNITY && _www != null)
			{
				return true;
			}
			return false;
		}
	}

	public float _pDecimalProgress
	{
		get
		{
			if (_downloadLibrary == EDownloadLibrary.UNITY && _www != null)
			{
				return _www.progress;
			}
			return 0f;
		}
	}

	public DateTime _pDownloadDate
	{
		get
		{
			if (_hasDLDateBeenSet)
			{
				return _downloadTime;
			}
			parseDownloadTime();
			return _downloadTime;
		}
	}

	public TimeSpan _pCacheTimeout
	{
		get
		{
			if (_hasCacheTimeoutBeenSet)
			{
				return _cacheTimeout;
			}
			parseDownloadTime();
			return _cacheTimeout;
		}
	}

	public bool _pIsCacheInDate
	{
		get
		{
			if (_pCacheTimeout.TotalSeconds <= 0.0)
			{
				return false;
			}
			return DateTime.Now < _pDownloadDate.Add(_pCacheTimeout);
		}
	}

	public event Action<DownloadRequest> onSuccessCallBack;

	public event Action<DownloadRequest> onFailCallback;

	public DownloadRequest()
	{
		_downloadLibrary = EDownloadLibrary.UNITY;
		_isUsingDefaultLanguage = !DLCFacade.Exists || DEFAULT_LANGUAGE == DLCFacade.Instance.getLanguageCode();
		_isUsingDefaultFidelity = !DLCFacade.Exists || "0" == DLCFacade.Instance.getFidelityInteger();
		_isUsingDefaultPlatform = false;
		_dataPath = Application.persistentDataPath;
		if (_dataPath != null && _dataPath.Length > 0 && _dataPath[_dataPath.Length - 1] != '/')
		{
			_dataPath += '/';
		}
		_streamingPath = Application.dataPath + "!/assets/";
	}

	public void ClearDownload()
	{
		destroyUpdateObject();
		if (_www != null)
		{
			_www.Dispose();
		}
		_www = null;
		if (_localHeaderInfoDownload != null)
		{
			_localHeaderInfoDownload.Dispose();
		}
		_localHeaderInfoDownload = null;
		hasTriedStreamingAssets = false;
		_hasEncounteredFatalError = false;
		_hasPreprocessed = false;
		_hasCacheTimeoutBeenSet = false;
		_hasDLDateBeenSet = false;
		_hasBeenDownloaded = false;
	}

	public void success()
	{
		if (doLocalHeaderInfo && !_hasCheckedForLocalHeaderInfo && _downloadLocation != DownloadLocation.REMOTE)
		{
			fetchLocalHeaderInfo();
			return;
		}
		parseDownloadTime();
		if (_downloadLocation == DownloadLocation.LOCAL_STREAMING && doUseCacheTimeout && !_pIsCacheInDate)
		{
			Log("Streaming assets copy was out of date. Checking persistant location");
			ClearDownload();
			hasTriedStreamingAssets = true;
			download();
			return;
		}
		if (doUseCacheTimeout && _downloadLocation != DownloadLocation.REMOTE)
		{
			if (!_pIsCacheInDate)
			{
				Log("Locally cached copy was out of date. Downloading remotely");
				ClearDownload();
				doLocalDownload = false;
				doLocalHeaderInfo = false;
				download();
				return;
			}
			Log("Locally cached copy was in date. Returning local copy.");
		}
		destroyUpdateObject();
		if (this.onSuccessCallBack != null)
		{
			this.onSuccessCallBack(this);
		}
	}

	public void fail()
	{
		if (!hasTriedStreamingAssets && _downloadLocation == DownloadLocation.LOCAL_STREAMING)
		{
			hasTriedStreamingAssets = true;
			download();
			return;
		}
		Log("Request failed [" + _pErrorString + "]");
		destroyUpdateObject();
		if (this.onFailCallback != null)
		{
			this.onFailCallback(this);
		}
	}

	public void onPreProcessComplete()
	{
		_hasPreprocessed = true;
		download();
	}

	public void onPostProcessComplete()
	{
		success();
	}

	public void download()
	{
		if (!_hasPreprocessed)
		{
			preProcessRequest();
			return;
		}
		numAttempts = 0;
		Log("Starting download...");
		if (hasLocalFile && doLocalDownload)
		{
			Log("Downloading local file.");
			startLocalDownload();
			return;
		}
		if (!hasTriedStreamingAssets && doLocalDownload)
		{
			Log("Downloading local file from streaming assets");
			startLocalDownload();
			return;
		}
		if (!doRemoteDownload)
		{
			if (_downloadLocation == DownloadLocation.REMOTE)
			{
				_downloadLocation = DownloadLocation.LOCAL_PERSISTANT;
			}
			Log("Unable to download local file.");
			fail();
			return;
		}
		if (doLocalDownload)
		{
			Log("File locations [" + localFilePath + "] and [" + streamingFilePath + "] were checked for existing asset and none were found.");
			Log("No local copy of asset [" + requestName + "] exists. Starting download from [" + requestUrl + "]");
		}
		else
		{
			Log("Forcing new download from [" + requestUrl + "]");
		}
		startRemoteDownload();
	}

	public void Log(string message, UnityEngine.Object o = null)
	{
		if (doDebug || doVerboseDebug)
		{
			Debug.Log("DownloadRequest: " + message, o);
		}
	}

	private void parseURL()
	{
		_requestURL = _rawURL;
		if (_requestURL.Contains("{fidelity}"))
		{
			_requestURL = _requestURL.Replace("{fidelity}", (!_isUsingDefaultFidelity) ? DLCFacade.Instance.getFidelityInteger() : "0");
		}
		else
		{
			_isUsingDefaultFidelity = true;
		}
		if (_requestURL.Contains("{language}"))
		{
			_requestURL = _requestURL.Replace("{language}", (!_isUsingDefaultLanguage) ? DLCFacade.Instance.getLanguageCode() : DEFAULT_LANGUAGE);
		}
		else
		{
			_isUsingDefaultLanguage = true;
		}
		if (_requestURL.Contains("{platform}"))
		{
			_requestURL = _requestURL.Replace("{platform}", (!_isUsingDefaultPlatform) ? "Android" : "WebPlayer");
		}
		else
		{
			_isUsingDefaultPlatform = true;
		}
		if (_requestURL.Contains(".unity3d"))
		{
			_requestURL = _requestURL.Replace(".unity3d", ".assetbundle");
		}
		fileName = generateFileName(_requestURL);
		localFilePath = dataPath + fileName;
		streamingFilePath = _streamingPath + fileName;
		_persistantHeaderInfoFilePath = makeLocalHeaderFilePath(localFilePath);
		_streamingHeaderInfoFilePath = makeLocalHeaderFilePath(streamingFilePath);
	}

	private string generateFileName(string url)
	{
		string text = url;
		for (int i = 0; i < URL_FILENAME_PARTS_TO_REPLACE.Length; i++)
		{
			text = text.Replace(URL_FILENAME_PARTS_TO_REPLACE[i], "_");
		}
		return text;
	}

	private string makeLocalHeaderFilePath(string downloadPath)
	{
		int num = downloadPath.LastIndexOf("/");
		int num2 = downloadPath.LastIndexOf(".");
		if (num2 != -1 && num2 > num)
		{
			downloadPath = downloadPath.Substring(0, num2);
		}
		downloadPath += "_headerInfo.txt";
		return downloadPath;
	}

	private void startLocalDownload()
	{
		_currentStatus = DownloadStatus.DOWNLOADING;
		_lastBytes = 0;
		_timeoutTimer = 0f;
		_downloadLocation = (hasLocalFile ? DownloadLocation.LOCAL_PERSISTANT : DownloadLocation.LOCAL_STREAMING);
		ensureUpdateObject();
		string text = "file://" + ((!hasLocalFile) ? streamingFilePath : localFilePath);
		if (_downloadLocation == DownloadLocation.LOCAL_STREAMING)
		{
			text = "jar:" + text;
		}
		Log("Downloading from Local file path [" + text + "]");
		_downloadLibraryUsed = EDownloadLibrary.UNITY;
		_www = new WWW(text);
	}

	private void startRemoteDownload()
	{
		_currentStatus = DownloadStatus.DOWNLOADING;
		_lastBytes = 0;
		_timeoutTimer = 0f;
		_downloadLocation = DownloadLocation.REMOTE;
		ensureUpdateObject();
		_downloadLibraryUsed = _downloadLibrary;
		if (formData != null && formData.Count > 0)
		{
			WWWForm wWWForm = new WWWForm();
			for (int i = 0; i < formData.Count; i++)
			{
				wWWForm.AddField(formData[i].key, formData[i].value);
			}
			_www = new WWW(requestUrl, wWWForm.data, formHeaders);
		}
		else
		{
			_www = new WWW(requestUrl, null, formHeaders);
		}
		Log("Request sent [" + requestUrl + "]");
	}

	private void ensureUpdateObject()
	{
		if (_updateObject != null)
		{
			_updateObject.name = "Download [" + fileName + "]";
			return;
		}
		_updateObject = new GameObject("Download [" + fileName + "]");
		UpdateForwarder updateForwarder = _updateObject.AddComponent<UpdateForwarder>();
		updateForwarder.OnUpdateFunction = Update;
	}

	private void destroyUpdateObject()
	{
		if (!(_updateObject == null))
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(_updateObject);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(_updateObject);
			}
			_updateObject = null;
		}
	}

	private void Update()
	{
		if (doVerboseDebug)
		{
			Log(string.Concat("Updating [", _downloadLocation.ToString(), "] download using [", _downloadLibraryUsed, "]. Current status [", _currentStatus, "]"));
		}
		updateLocalHeaderDownload();
		if (_currentStatus == DownloadStatus.DOWNLOADING)
		{
			UpdateUnityDownload();
		}
	}

	private void UpdateUnityDownload()
	{
		if (_downloadLibraryUsed != EDownloadLibrary.UNITY)
		{
			return;
		}
		if (_www == null)
		{
			if (_currentStatus == DownloadStatus.DOWNLOADING)
			{
				_currentStatus = DownloadStatus.FAILED;
				Log("Download became null.");
				fail();
			}
			else
			{
				_currentStatus = DownloadStatus.NOT_DOWNLOADING;
			}
			return;
		}
		bool flag = updateTimeout(Mathf.FloorToInt(_www.progress * 100000f));
		if (!flag && !_www.isDone && _www.error == null)
		{
			_currentStatus = DownloadStatus.DOWNLOADING;
		}
		else if (_www.error == null && !flag)
		{
			_hasBeenDownloaded = true;
			Log("Download complete.");
			processResponseHeaders(_www);
			if (!cacheDownload(_www.bytes) || !cacheHeaderInfo())
			{
				_hasEncounteredFatalError = true;
				_currentStatus = DownloadStatus.ERROR;
				fail();
			}
			else
			{
				_currentStatus = DownloadStatus.SUCCEEDED;
				postProcessResponse();
			}
		}
		else
		{
			Log((!flag) ? ("Error [" + _www.error + "]") : ("Timed out after [" + _requestTimeout + "] seconds."));
			if (numAttempts < 3)
			{
				retry();
				return;
			}
			_currentStatus = DownloadStatus.FAILED;
			fail();
		}
	}

	protected virtual void preProcessRequest()
	{
		onPreProcessComplete();
	}

	protected virtual void postProcessResponse()
	{
		onPostProcessComplete();
	}

	private void retry()
	{
		Log("Retrying download...");
		numAttempts++;
		switch (_downloadLocation)
		{
		case DownloadLocation.REMOTE:
			startRemoteDownload();
			break;
		case DownloadLocation.LOCAL_PERSISTANT:
		case DownloadLocation.LOCAL_STREAMING:
			startLocalDownload();
			break;
		}
	}

	private bool updateTimeout(int currentProgress)
	{
		if (_lastBytes == currentProgress)
		{
			_timeoutTimer += RealTime.deltaTime;
		}
		else
		{
			_timeoutTimer = 0f;
		}
		_lastBytes = currentProgress;
		if (doVerboseDebug)
		{
			Log("Updating timeout [" + (float)Mathf.FloorToInt(_timeoutTimer * 10f) * 0.1f + "/" + _requestTimeout + "]");
		}
		return _timeoutTimer > _requestTimeout;
	}

	private bool cacheDownload(byte[] bytes)
	{
		if (!doCacheDownload || _downloadLocation != DownloadLocation.REMOTE)
		{
			return true;
		}
		Log("Download complete. Writing download to file [" + localFilePath + "]");
		if (!Directory.Exists(dataPath))
		{
			Log("Directory [" + dataPath + "] did not exist, attempting to Create.");
			try
			{
				Directory.CreateDirectory(dataPath);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to Create directory, EXEPTION: " + ex.ToString());
				return false;
			}
		}
		try
		{
			File.WriteAllBytes(localFilePath, bytes);
		}
		catch (Exception ex2)
		{
			Debug.LogError("Failed to write download [" + requestName + "] to file! Error [" + ex2.Message + "]");
			return false;
		}
		return true;
	}

	private void processResponseHeaders(WWW www)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<string, string> responseHeader in www.responseHeaders)
		{
			list.Add(responseHeader.Key);
			list2.Add(responseHeader.Value);
		}
		processResponseHeaders(list.ToArray(), list2.ToArray());
	}

	private void processResponseHeaders(string[] keys, string[] values)
	{
		_responseHeaders = new Dictionary<string, string>();
		if (keys.Length != values.Length)
		{
			Log("Header key-value pair mismatch.");
			return;
		}
		int i = 0;
		for (int num = keys.Length; i < num; i++)
		{
			if (!_responseHeaders.ContainsKey(keys[i]))
			{
				_responseHeaders.Add(keys[i], values[i]);
			}
		}
	}

	private bool cacheHeaderInfo()
	{
		if (!doCacheHeaderInfo || _downloadLocation != DownloadLocation.REMOTE)
		{
			return true;
		}
		string text = "{";
		foreach (KeyValuePair<string, string> responseHeader in _responseHeaders)
		{
			text += NEW_LINE;
			string text2 = text;
			text = text2 + '\t' + string.Empty + '"' + responseHeader.Key + '"' + ":" + '"' + responseHeader.Value + '"' + ",";
		}
		text = text.Substring(0, text.Length - 1);
		text = text + NEW_LINE + "}";
		Log("Header info save location [" + _persistantHeaderInfoFilePath + "]");
		if (!Directory.Exists(dataPath))
		{
			Log("Directory [" + dataPath + "] did not exist, attempting to Create.");
			try
			{
				Directory.CreateDirectory(dataPath);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to Create directory, EXEPTION: " + ex.ToString());
				return false;
			}
		}
		try
		{
			File.WriteAllBytes(_persistantHeaderInfoFilePath, Encoding.UTF8.GetBytes(text));
		}
		catch (Exception ex2)
		{
			Debug.LogError("Failed to write header info to file! Error [" + ex2.Message + "]");
			return false;
		}
		return true;
	}

	private void fetchLocalHeaderInfo()
	{
		_hasCheckedForLocalHeaderInfo = true;
		_lastBytes = 0;
		_timeoutTimer = 0f;
		ensureUpdateObject();
		bool flag = _downloadLocation == DownloadLocation.LOCAL_STREAMING;
		string text = "file://" + ((!flag) ? _persistantHeaderInfoFilePath : _streamingHeaderInfoFilePath);
		if (flag)
		{
			text = "jar:" + text;
		}
		Log("Fetching local header info from [" + text + "]");
		_localHeaderInfoDownload = new WWW(text);
	}

	private void updateLocalHeaderDownload()
	{
		if (_localHeaderInfoDownload != null)
		{
			if (!string.IsNullOrEmpty(_localHeaderInfoDownload.error))
			{
				Log("local header file download failed. Error [" + _localHeaderInfoDownload.error + "]");
				localHeaderInfoDownloadFail();
			}
			else if (!_localHeaderInfoDownload.isDone && updateTimeout(Mathf.FloorToInt(_localHeaderInfoDownload.progress * 100000f)))
			{
				Log("local header file download failed. Timed out.");
				localHeaderInfoDownloadFail();
			}
			else if (_localHeaderInfoDownload.isDone)
			{
				localHeaderInfoDownloadSuccess();
			}
		}
	}

	private void localHeaderInfoDownloadFail()
	{
		if (_localHeaderInfoDownload != null)
		{
			_localHeaderInfoDownload.Dispose();
		}
		_localHeaderInfoDownload = null;
		destroyUpdateObject();
		success();
	}

	private void localHeaderInfoDownloadSuccess()
	{
		if (string.IsNullOrEmpty(_localHeaderInfoDownload.text))
		{
			Log("local header file download succeeded but was empty!");
			localHeaderInfoDownloadFail();
		}
		string text = _localHeaderInfoDownload.text.Replace(NEW_LINE + string.Empty, string.Empty).Replace('\t' + string.Empty, string.Empty).Replace("{", string.Empty)
			.Replace("}", string.Empty);
		Log("Local header file downloaded. [" + text + "]");
		int num = text.IndexOf('"');
		int num2 = -1;
		int num3 = -1;
		string text2 = string.Empty;
		string empty = string.Empty;
		while (num != -1)
		{
			if (num2 == -1)
			{
				num2 = num;
			}
			else if (num3 == -1)
			{
				num3 = num - 1;
			}
			if (num2 != -1 && num3 != -1)
			{
				if (text2 == string.Empty)
				{
					text2 = text.Substring(num2 + 1, num3 - num2);
				}
				else if (empty == string.Empty)
				{
					empty = text.Substring(num2 + 1, num3 - num2);
					if (!_responseHeaders.ContainsKey(text2))
					{
						_responseHeaders.Add(text2, empty);
					}
					text2 = string.Empty;
					empty = string.Empty;
				}
				num2 = -1;
				num3 = -1;
			}
			num = text.IndexOf('"', num + 1);
		}
		destroyUpdateObject();
		success();
	}

	private void parseDownloadTime()
	{
		if (_currentStatus != DownloadStatus.SUCCEEDED)
		{
			return;
		}
		if (_responseHeaders == null || !_responseHeaders.ContainsKey("DATE"))
		{
			_downloadTime = DateTime.Now;
		}
		else
		{
			_downloadTime = DateTime.Now;
			DateTime.TryParse(_responseHeaders["DATE"], out _downloadTime);
		}
		_hasDLDateBeenSet = true;
		if (_responseHeaders != null && _responseHeaders.ContainsKey("CACHE-CONTROL"))
		{
			string[] array = _responseHeaders["CACHE-CONTROL"].Split(',');
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				if (array[i].Contains("max-age"))
				{
					array = array[i].Split('=');
					if (array.Length == 2)
					{
						int result = 0;
						int.TryParse(array[1], out result);
						if (result == 0)
						{
							result = -1;
						}
						_cacheTimeout = new TimeSpan(0, 0, result);
						_hasCacheTimeoutBeenSet = true;
					}
				}
				else if (array[i].Contains("no-cache") || array[i].Contains("no-store"))
				{
					_cacheTimeout = new TimeSpan(0, 0, -1);
					_hasCacheTimeoutBeenSet = true;
				}
			}
			if (!_hasCacheTimeoutBeenSet)
			{
				_cacheTimeout = new TimeSpan(0, 0, -1);
				_hasCacheTimeoutBeenSet = true;
			}
		}
		if (!_hasCacheTimeoutBeenSet)
		{
			_cacheTimeout = default(TimeSpan);
			doUseCacheTimeout = false;
			_hasCacheTimeoutBeenSet = true;
		}
	}
}
