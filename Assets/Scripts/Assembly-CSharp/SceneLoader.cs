using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	private const int DELAY_FRAMES = 2;

	private static bool DO_DEBUG = true;

	public static Action<string, string> _OnWillLoadLevel;

	public string levelName;

	public bool assetbundle;

	public bool forceLoadIfAlreadyLoaded;

	public SignalSender onLoadComplete;

	public bool loadAdditively;

	private AsyncOperation levelLoader;

	private int _delayFrames = 2;

	private Action<bool> _onLoadCompleteAction;

	public bool isLoading
	{
		get
		{
			return levelLoader != null && !levelLoader.isDone;
		}
	}

	public float _pLoadingProgress
	{
		get
		{
			return (levelLoader == null) ? 0f : levelLoader.progress;
		}
	}

	private string _pCurrentLevelName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	private void OnSignal()
	{
		if (!AttemptLoadLevel())
		{
			Log("Not switching to scene: " + levelName + " (already loaded)");
		}
	}

	public bool AttemptLoadLevel(Action<bool> onLoadComplete = null)
	{
		Log("Attempting to load level [" + levelName + "] from level [" + _pCurrentLevelName + "]");
		bool result = false;
		_delayFrames = 2;
		if (_OnWillLoadLevel != null)
		{
			_OnWillLoadLevel(levelName, _pCurrentLevelName);
		}
		if (forceLoadIfAlreadyLoaded || _pCurrentLevelName != levelName)
		{
			SafeCallOnLoadCompleteAction(false);
			_onLoadCompleteAction = onLoadComplete;
			if (assetbundle)
			{
				if (!File.Exists(streamingFilePath()) && !File.Exists(persistantFilePath()))
				{
					Log("Unable to find scene bundle at path [" + streamingFilePath() + "]");
					if (!(DLCFacade.Instance != null))
					{
						Log("Could not load level bundle");
						return false;
					}
					DLCFacade.Instance.downloadAsset(levelName, AssetType.LEVEL_BUNDLE, downloadComplete, downloadFailed);
				}
				else
				{
					if (_OnWillLoadLevel != null)
					{
						_OnWillLoadLevel(levelName, _pCurrentLevelName);
					}
					Log("Loading scene from file.");
					StartCoroutine(LoadSceneBundle());
				}
			}
			else
			{
				if (_OnWillLoadLevel != null)
				{
					_OnWillLoadLevel(levelName, _pCurrentLevelName);
				}
				Log("Loading level.");
				StartCoroutine(LoadLevel(null));
			}
			result = true;
		}
		else
		{
			Log("Level loading not required.");
		}
		return result;
	}

	private void downloadComplete(DownloadRequest assetBundleRequest)
	{
		if (assetBundleRequest._pHasResponse)
		{
			StartCoroutine(LoadLevel(assetBundleRequest._pAssetBundle));
		}
		else
		{
			LoadSceneBundle();
		}
		assetBundleRequest.ClearDownload();
	}

	private void downloadFailed(DownloadRequest assetBundleRequest)
	{
		if (assetBundleRequest._pHasResponse)
		{
			assetBundleRequest.ClearDownload();
		}
		Log("Could not load level bundle");
	}

	private IEnumerator LoadLevel(AssetBundle bundle)
	{
		while (_delayFrames > 0)
		{
			_delayFrames--;
			yield return new WaitForEndOfFrame();
		}
		if (loadAdditively)
		{
			Log("Loading scene additively: " + levelName);
			levelLoader = Application.LoadLevelAdditiveAsync(levelName);
		}
		else
		{
			Log("Loading scene now: " + levelName);
			levelLoader = Application.LoadLevelAsync(levelName);
		}
		yield return levelLoader;
		if (bundle != null)
		{
			yield return new WaitForEndOfFrame();
			bundle.Unload(false);
		}
		Log("Scene loading complete. Current level name [" + _pCurrentLevelName + "]");
		SafeCallOnLoadCompleteAction(true);
		if (onLoadComplete != null)
		{
			onLoadComplete.SendSignals(this);
		}
		levelLoader = null;
	}

	private void SafeCallOnLoadCompleteAction(bool success)
	{
		if (_onLoadCompleteAction != null)
		{
			Action<bool> onLoadCompleteAction = _onLoadCompleteAction;
			_onLoadCompleteAction = null;
			onLoadCompleteAction(true);
		}
	}

	private void UpdateLoadingFeedback()
	{
	}

	private IEnumerator LoadSceneBundle()
	{
		string path = streamingFilePath();
		if (!File.Exists(streamingFilePath()))
		{
			path = persistantFilePath();
		}
		path = "file://" + path;
		Log("Retrieving [" + path + "]");
		WWW download = new WWW(path);
		yield return download;
		if (download.isDone && download.assetBundle != null)
		{
			AssetBundle bundle = download.assetBundle;
			if (bundle != null)
			{
				Log("SceneLoader: Level loaded from file. Loading scene");
				StartCoroutine(LoadLevel(bundle));
			}
			else
			{
				Log("Could not load level bundle");
			}
		}
		else
		{
			Log("Unable to download: " + path);
		}
	}

	private string GetPlatformFolder()
	{
		return "Android";
	}

	private string streamingFilePath()
	{
		return Application.streamingAssetsPath + "/" + GetPlatformFolder() + "/" + levelName + ".assetbundle";
	}

	private string persistantFilePath()
	{
		return Application.persistentDataPath + "/" + GetPlatformFolder() + "/" + levelName + ".assetbundle";
	}

	public static void Log(string message, UnityEngine.Object o = null)
	{
		if (DO_DEBUG)
		{
		}
	}
}
