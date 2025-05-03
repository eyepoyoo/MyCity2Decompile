using System;
using System.Collections.Generic;
using AmuzoEngine;
using LitJson;
using UnityEngine;

public class FlowSceneLoader : MonoBehaviour, ILocationHandler
{
	public interface ILoadingScreen
	{
		int _pLoadingStage { get; set; }

		void SetLoadingProgress(float progress);
	}

	private class LoadSceneBlocker
	{
		public string _name;

		public int _numRequests;

		public LoadSceneBlocker(string name)
		{
			_name = name;
			_numRequests = 1;
		}
	}

	private class SceneArgs
	{
		public string _sceneName;

		public bool _isAssetBundle;

		public bool _isAdditive;

		public bool _isForceLoad;

		public SceneArgs(string sceneName, bool isAssetBundle, bool isAdditive, bool isForceLoad)
		{
			_sceneName = sceneName;
			_isAssetBundle = isAssetBundle;
			_isAdditive = isAdditive;
			_isForceLoad = isForceLoad;
		}
	}

	private class FlowData
	{
		public List<SceneArgs> _scenes;

		public bool _isIgnore;

		public bool _isClearOnEnter;

		public bool _isClearOnExit;

		public string _loadingLocation;

		public string _onLoadedLink;

		public int _pSceneCount
		{
			get
			{
				return (_scenes != null) ? _scenes.Count : 0;
			}
		}

		public FlowData(string loadingLocation)
		{
			_scenes = new List<SceneArgs>();
			_isClearOnExit = false;
			_loadingLocation = loadingLocation;
		}

		public void AddScene(string sceneName, bool isAssetBundle, bool isAdditive, bool isForceLoad)
		{
			if (sceneName != null && sceneName.Length != 0)
			{
				_scenes.Add(new SceneArgs(sceneName, isAssetBundle, isAdditive, isForceLoad));
			}
		}

		public void AddEmptyScene(string sceneName)
		{
			if (sceneName != null && sceneName.Length != 0)
			{
				_scenes.Insert(0, new SceneArgs(sceneName, false, false, true));
			}
		}

		public void ReadFromJson(JsonData srcData, bool isReadGlobalArgs)
		{
			if (srcData == null)
			{
				return;
			}
			if (srcData.IsArray)
			{
				for (int i = 0; i < srcData.Count; i++)
				{
					ReadFromJson(srcData[i], false);
				}
			}
			else if (srcData.IsString)
			{
				AddScene((string)srcData, false, false, false);
			}
			else
			{
				if (!srcData.IsObject)
				{
					return;
				}
				JsonData jsonData = srcData.TryGet("scenes", JsonType.Array);
				if (jsonData != null)
				{
					ReadFromJson(jsonData, false);
				}
				else
				{
					jsonData = srcData.TryGet("name", JsonType.String);
					if (jsonData == null)
					{
						jsonData = srcData.TryGet("sceneName", JsonType.String);
					}
					if (jsonData != null)
					{
						string sceneName = (string)jsonData;
						bool isAssetBundle = false;
						bool isAdditive = false;
						bool isForceLoad = false;
						jsonData = srcData.TryGet("assetBundle", JsonType.Boolean);
						if (jsonData != null)
						{
							isAssetBundle = (bool)jsonData;
						}
						jsonData = srcData.TryGet("additive", JsonType.Boolean);
						if (jsonData != null)
						{
							isAdditive = (bool)jsonData;
						}
						jsonData = srcData.TryGet("forceLoad", JsonType.Boolean);
						if (jsonData != null)
						{
							isForceLoad = (bool)jsonData;
						}
						AddScene(sceneName, isAssetBundle, isAdditive, isForceLoad);
					}
				}
				if (isReadGlobalArgs)
				{
					jsonData = srcData.TryGet("ignore", JsonType.Boolean);
					if (jsonData != null)
					{
						_isIgnore = (bool)jsonData;
					}
					jsonData = srcData.TryGet("clearSceneOnEnter", JsonType.Boolean);
					if (jsonData != null)
					{
						_isClearOnEnter = (bool)jsonData;
					}
					jsonData = srcData.TryGet("clearSceneOnExit", JsonType.Boolean);
					if (jsonData != null)
					{
						_isClearOnExit = (bool)jsonData;
					}
					jsonData = srcData.TryGet("loadingLocation", JsonType.String);
					if (jsonData != null)
					{
						_loadingLocation = (string)jsonData;
					}
					jsonData = srcData.TryGet("onLoadedLink", JsonType.String);
					if (jsonData != null)
					{
						_onLoadedLink = (string)jsonData;
					}
				}
			}
		}

		public JsonData ToJson()
		{
			JsonData jsonData = new JsonData();
			JsonData jsonData2 = new JsonData();
			for (int i = 0; i < _pSceneCount; i++)
			{
				JsonData jsonData3 = new JsonData();
				jsonData3["sceneName"] = new JsonData(_scenes[i]._sceneName);
				jsonData3["assetBundle"] = new JsonData(_scenes[i]._isAssetBundle);
				jsonData3["additive"] = new JsonData(_scenes[i]._isAdditive);
				jsonData3["forceLoad"] = new JsonData(_scenes[i]._isForceLoad);
				jsonData2.Add(jsonData3);
			}
			jsonData["scenes"] = jsonData2;
			jsonData["ignore"] = new JsonData(_isIgnore);
			jsonData["clearSceneOnEnter"] = new JsonData(_isClearOnEnter);
			jsonData["clearSceneOnExit"] = new JsonData(_isClearOnExit);
			jsonData["loadingLocation"] = new JsonData(_loadingLocation);
			jsonData["onLoadedLink"] = new JsonData(_onLoadedLink);
			return jsonData;
		}
	}

	private const string SCENE_SECTION = "scene";

	private const string LOG_TAG = "[FlowSceneLoader] ";

	public string _defaultLoadingLocation;

	public string _emptyScene;

	public bool _checkSceneInitialization;

	public bool _isDebugSceneLoadBlocking;

	private SceneLoader _sceneLoader;

	private bool _wasClearSceneOnExit;

	private Dictionary<int, LoadSceneBlocker> _loadSceneBlockers;

	private Utils.SimpleIntegerIdGenerator _loadSceneBlockerHandles;

	private bool _isSceneLoadBlockersDirty;

	private Action _deferredLoadScene;

	private Action _deferredOnScenesLoaded;

	private ILoadingScreen _loadingScreen;

	int ILocationHandler.locationChangePriority
	{
		get
		{
			return 1;
		}
	}

	private bool _pIsLoadSceneBlocked
	{
		get
		{
			return _loadSceneBlockers != null && _loadSceneBlockers.Count > 0;
		}
	}

	void ILocationHandler.ChangeLocation(string previous, ref string current, string linkName, JsonData linkData, JsonData currentLocationData)
	{
		OnChangeLocation(previous, ref current, currentLocationData, linkName, linkData);
	}

	private void Awake()
	{
		_sceneLoader = base.gameObject.AddComponent<SceneLoader>();
		_loadSceneBlockers = new Dictionary<int, LoadSceneBlocker>();
		_loadSceneBlockerHandles = new Utils.SimpleIntegerIdGenerator();
		FlowFacade.AddLocationHandler(this);
		Singleton<FlowSceneLoader>.Set(this);
		ScreenFacade._onFlowChangeComplete = (Action)Delegate.Combine(ScreenFacade._onFlowChangeComplete, new Action(OnScreenChanged));
	}

	private void OnDestroy()
	{
		Singleton<FlowSceneLoader>.Unset(this);
	}

	private void Update()
	{
		if (_deferredLoadScene != null)
		{
			if (_pIsLoadSceneBlocked)
			{
				if (_isSceneLoadBlockersDirty)
				{
					if (_isDebugSceneLoadBlocking)
					{
						string text = "Scene load blocked by:\n";
						foreach (KeyValuePair<int, LoadSceneBlocker> loadSceneBlocker in _loadSceneBlockers)
						{
							text = text + "{" + loadSceneBlocker.Key + "," + loadSceneBlocker.Value._name + "(" + loadSceneBlocker.Value._numRequests + ")}\n";
						}
					}
					_isSceneLoadBlockersDirty = false;
				}
			}
			else
			{
				_deferredLoadScene();
				_deferredLoadScene = null;
			}
		}
		if (_deferredOnScenesLoaded != null)
		{
			_deferredOnScenesLoaded();
			_deferredOnScenesLoaded = null;
		}
		if (_sceneLoader != null && _sceneLoader.isLoading && _loadingScreen != null)
		{
			_loadingScreen.SetLoadingProgress(_sceneLoader._pLoadingProgress);
		}
	}

	public bool RequestSceneLoadBlocker(ref int? blockerHandle, string context)
	{
		if (_loadSceneBlockers == null || _loadSceneBlockerHandles == null)
		{
			Debug.LogError("[FlowSceneLoader] Scene-load blocker system not initialized", base.gameObject);
			return false;
		}
		if (blockerHandle.HasValue)
		{
			if (!_loadSceneBlockers.ContainsKey(blockerHandle.Value))
			{
				Debug.LogError("[FlowSceneLoader] Scene-load blocker not found, handle=" + blockerHandle.Value, base.gameObject);
				return false;
			}
			LoadSceneBlocker loadSceneBlocker = _loadSceneBlockers[blockerHandle.Value];
			if (loadSceneBlocker == null)
			{
				Debug.LogError("[FlowSceneLoader] Null scene-load blocker, handle=" + blockerHandle.Value, base.gameObject);
				return false;
			}
			loadSceneBlocker._numRequests++;
		}
		else
		{
			if (context == null || context.Length == 0)
			{
				Debug.LogError("[FlowSceneLoader] No context provided for scene-load blocker", base.gameObject);
				return false;
			}
			blockerHandle = _loadSceneBlockerHandles._pNew;
			_loadSceneBlockers.Add(blockerHandle.Value, new LoadSceneBlocker(context));
		}
		_isSceneLoadBlockersDirty = true;
		return true;
	}

	public bool ReleaseSceneLoadBlocker(ref int? blockerHandle)
	{
		if (!blockerHandle.HasValue)
		{
			Debug.LogWarning("[FlowSceneLoader] No scene-load blocker to release", base.gameObject);
			return false;
		}
		if (_loadSceneBlockers == null || _loadSceneBlockerHandles == null)
		{
			Debug.LogError("[FlowSceneLoader] Scene-load blocker system not initialized", base.gameObject);
			return false;
		}
		if (!_loadSceneBlockers.ContainsKey(blockerHandle.Value))
		{
			Debug.LogError("[FlowSceneLoader] Scene-load blocker not found, handle=" + blockerHandle.Value, base.gameObject);
			return false;
		}
		LoadSceneBlocker loadSceneBlocker = _loadSceneBlockers[blockerHandle.Value];
		if (loadSceneBlocker == null)
		{
			Debug.LogError("[FlowSceneLoader] Null scene-load blocker, handle=" + blockerHandle.Value, base.gameObject);
			return false;
		}
		loadSceneBlocker._numRequests--;
		if (loadSceneBlocker._numRequests <= 0)
		{
			_loadSceneBlockers.Remove(blockerHandle.Value);
			blockerHandle = null;
		}
		_isSceneLoadBlockersDirty = true;
		return true;
	}

	private void OnChangeLocation(string previousLocation, ref string currentLocation, JsonData currentLocationData, string linkName, JsonData linkData)
	{
		FlowData flowData = new FlowData(_defaultLoadingLocation);
		if (currentLocationData != null && currentLocationData.IsObject)
		{
			flowData.ReadFromJson(currentLocationData.TryGet("scene"), true);
		}
		if (linkData != null && linkData.IsObject)
		{
			flowData.ReadFromJson(linkData.TryGet("scene"), true);
		}
		if (flowData._isIgnore)
		{
			return;
		}
		if (flowData._isClearOnEnter || _wasClearSceneOnExit)
		{
			flowData.AddEmptyScene(_emptyScene);
		}
		_wasClearSceneOnExit = flowData._isClearOnExit;
		string targetLocation = currentLocation;
		if (LoadScenes(flowData, targetLocation, linkName, linkData))
		{
			if (flowData._loadingLocation != null && flowData._loadingLocation.Length > 0)
			{
				currentLocation = flowData._loadingLocation;
			}
			else
			{
				Debug.LogWarning("[FlowSceneLoader] No loading location", base.gameObject);
			}
		}
	}

	private bool LoadScenes(FlowData args, string targetLocation, string linkName, JsonData linkData)
	{
		if (args == null)
		{
			return false;
		}
		if (args._pSceneCount == 0)
		{
			return OnScenesLoaded(args, targetLocation, linkName, linkData);
		}
		_deferredLoadScene = delegate
		{
			LoadScene(0, args, targetLocation, linkName, linkData);
		};
		return true;
	}

	private bool LoadScene(int sceneIndex, FlowData args, string targetLocation, string linkName, JsonData linkData)
	{
		bool flag = false;
		if (_sceneLoader == null)
		{
			Debug.LogError("[FlowSceneLoader] No scene loader", base.gameObject);
			flag = true;
		}
		if (_sceneLoader.isLoading)
		{
			Debug.LogError("[FlowSceneLoader] Scene loader busy", base.gameObject);
			flag = true;
		}
		if (sceneIndex >= args._scenes.Count)
		{
			flag = true;
		}
		if (flag)
		{
			_deferredOnScenesLoaded = (Action)Delegate.Combine(_deferredOnScenesLoaded, (Action)delegate
			{
				OnScenesLoaded(args, targetLocation, linkName, linkData);
			});
			return false;
		}
		SceneArgs sceneArgs = args._scenes[sceneIndex];
		bool flag2 = false;
		if (sceneArgs == null || sceneArgs._sceneName == null || sceneArgs._sceneName.Length == 0)
		{
			Debug.LogWarning("[FlowSceneLoader] Bad scene args", base.gameObject);
			flag2 = true;
		}
		if (Application.loadedLevelName == sceneArgs._sceneName && !sceneArgs._isForceLoad && !sceneArgs._isAdditive)
		{
			flag2 = true;
		}
		if (flag2)
		{
			return LoadScene(sceneIndex + 1, args, targetLocation, linkName, linkData);
		}
		_sceneLoader.levelName = sceneArgs._sceneName;
		_sceneLoader.assetbundle = sceneArgs._isAssetBundle;
		_sceneLoader.loadAdditively = sceneArgs._isAdditive;
		_sceneLoader.forceLoadIfAlreadyLoaded = sceneArgs._isForceLoad;
		if (!_sceneLoader.AttemptLoadLevel(delegate(bool isSuccess)
		{
			if (!isSuccess)
			{
				Debug.LogWarning("[FlowSceneLoader] Failed loading scene '" + sceneArgs._sceneName + "'", base.gameObject);
			}
			LoadScene(sceneIndex + 1, args, targetLocation, linkName, linkData);
		}))
		{
			return LoadScene(sceneIndex + 1, args, targetLocation, linkName, linkData);
		}
		return true;
	}

	private bool OnScenesLoaded(FlowData args, string targetLocation, string linkName, JsonData linkData)
	{
		bool result = false;
		if (_checkSceneInitialization && InitialisationFacade._pExists && !InitialisationFacade.Instance._pHasFinished)
		{
			result = true;
			InitialisationFacade.Instance._pOnFinished += delegate
			{
				OnFinished(args, targetLocation, linkName, linkData);
			};
			if (_loadingScreen != null)
			{
				_loadingScreen._pLoadingStage++;
			}
		}
		else
		{
			OnFinished(args, targetLocation, linkName, linkData);
		}
		return result;
	}

	private void OnFinished(FlowData args, string targetLocation, string linkName, JsonData linkData)
	{
		if (Facades<FlowFacade>.Instance != null)
		{
			if (Facades<FlowFacade>.Instance.CurrentLocation == args._loadingLocation)
			{
				Facades<FlowFacade>.Instance.ForgetCurrentLocation();
			}
			if (Facades<FlowFacade>.Instance.CurrentLocation != targetLocation)
			{
				linkData = AddIgnoreFlag(linkData);
				Facades<FlowFacade>.Instance.ManualChangeLocation(targetLocation, linkName, linkData);
			}
			if (args._onLoadedLink != null && args._onLoadedLink.Length > 0)
			{
				Facades<FlowFacade>.Instance.FollowLink(args._onLoadedLink);
			}
		}
	}

	private JsonData AddIgnoreFlag(JsonData origData)
	{
		FlowData flowData = new FlowData(_defaultLoadingLocation);
		flowData.ReadFromJson(origData, true);
		flowData._isIgnore = true;
		JsonData jsonData = Extensions.DeepCopyJson(origData);
		if (jsonData == null)
		{
			jsonData = new JsonData();
		}
		jsonData["scene"] = flowData.ToJson();
		return jsonData;
	}

	private void OnScreenChanged()
	{
		if (!(Facades<ScreenFacade>.Instance == null))
		{
			_loadingScreen = Facades<ScreenFacade>.Instance.FindActiveScreenOfType<ILoadingScreen>();
		}
	}
}
