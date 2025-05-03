using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using LitJson;
using UnityEngine;

public class FlowChart
{
	public const string BACK_LINK = "<<";

	public const string EXIT_FLOW = "^";

	private const string LOCATION_IGNORE_LINK = "!";

	private const string LINK_PASS_THRU = "PASS_THRU";

	private const string LOG_TAG = "[FlowChart] ";

	private const string platform = "mobile";

	private string _flowId;

	private MonoBehaviour _coroutineHandler;

	private string _flowVarPrefix = "[";

	private string _flowVarSuffix = "]";

	private string _startLocation = string.Empty;

	private List<ILocationHandler> _locationChangeHandlers = new List<ILocationHandler>();

	private bool _isNotifyingHandlers;

	private Stack<string> _previousLocations = new Stack<string>();

	private Stack<int> _rootLocations = new Stack<int>();

	private JsonData _flowData;

	private JsonData _globalLocationData;

	private string _currentLocation;

	private string _cachedLocation;

	private JsonData _cachedLocationData;

	private bool _isForceCacheCurrentLocation;

	private string _bookmark;

	private bool _isOldGlobalLinkBehaviour;

	private Dictionary<string, Func<bool>> _flowConditions = new Dictionary<string, Func<bool>>();

	private Dictionary<string, JsonData> _flowVariables = new Dictionary<string, JsonData>();

	private Action _onFlowControlQuit;

	private IFlowManager _flowManager;

	public string _pFlowId
	{
		get
		{
			return _flowId;
		}
	}

	public MonoBehaviour _pCoroutineHandler
	{
		set
		{
			_coroutineHandler = value;
		}
	}

	public string _pFlowVarPrefix
	{
		set
		{
			_flowVarPrefix = value;
		}
	}

	public string _pFlowVarSuffix
	{
		set
		{
			_flowVarSuffix = value;
		}
	}

	public bool _pIsOldGlobalLinkBehaviour
	{
		set
		{
			_isOldGlobalLinkBehaviour = value;
		}
	}

	public Action _pOnFlowControlQuit
	{
		set
		{
			_onFlowControlQuit = value;
		}
	}

	private string _pLogTag
	{
		get
		{
			return "[FlowChart:" + _flowId + "] ";
		}
	}

	public JsonData _pJson
	{
		get
		{
			return _flowData;
		}
	}

	public string _pCurrentLocation
	{
		get
		{
			return _currentLocation;
		}
		set
		{
			if (!(_currentLocation == value) && !AddLocation(value, _flowId + ".CurrentLocation", null))
			{
				Debug.LogWarning(_pLogTag + "Cannot change to unknown location: " + value);
			}
		}
	}

	private string _pPreviousLocation
	{
		get
		{
			return (_previousLocations.Count <= 0) ? null : _previousLocations.Peek();
		}
	}

	private int _pCurrentRootLocationIndex
	{
		get
		{
			return (_rootLocations != null && _rootLocations.Count > 0) ? _rootLocations.Peek() : 0;
		}
	}

	private JsonData _pCurrentLocationLinks
	{
		get
		{
			JsonData pCurrentLocationData = _pCurrentLocationData;
			if (pCurrentLocationData == null)
			{
				Debug.LogError(_pLogTag + "No data for " + _currentLocation);
				return null;
			}
			return pCurrentLocationData.TryGet("links");
		}
	}

	private JsonData _pCurrentLocationData
	{
		get
		{
			if (_cachedLocation != _currentLocation || _isForceCacheCurrentLocation)
			{
				_isForceCacheCurrentLocation = false;
				CacheLocationData(_currentLocation);
			}
			return _cachedLocationData;
		}
	}

	public FlowChart(string flowId, JsonData flowData, IFlowManager flowManager)
	{
		if (flowId == null || flowId.Length == 0)
		{
			Debug.LogError("[FlowChart] Must specify an ID for flow chart");
		}
		_flowId = flowId;
		_flowData = flowData;
		_globalLocationData = _flowData.TryGet("Global");
		_startLocation = GetStartLocation(_flowData);
		_flowManager = flowManager;
	}

	public void ReplaceLocationChangeHandlersList(List<ILocationHandler> list)
	{
		_locationChangeHandlers = list;
	}

	private string GetStartLocation(JsonData flowData)
	{
		JsonData jsonData = flowData.TryGet("StartLocation");
		return (jsonData == null || !jsonData.IsString) ? string.Empty : ((string)jsonData);
	}

	public void SetStartLocation()
	{
		ClearHistory();
		_pCurrentLocation = _startLocation;
	}

	public void SetFlowCondition(string name, bool value)
	{
		_flowConditions[name] = () => value;
	}

	public void SetFlowCondition(string name, Func<bool> eval)
	{
		_flowConditions[name] = eval;
	}

	public bool GetFlowCondition(string name)
	{
		if (_flowConditions.ContainsKey(name) && _flowConditions[name] != null)
		{
			return _flowConditions[name]();
		}
		return false;
	}

	public void SetFlowVariable(string name, JsonData value)
	{
		_flowVariables[name] = Extensions.DeepCopyJson(value);
		_isForceCacheCurrentLocation = true;
	}

	private JsonData GetFlowVariable(string name)
	{
		return (!_flowVariables.ContainsKey(name)) ? null : _flowVariables[name];
	}

	public void AddLocationChangeHandler(ILocationHandler target)
	{
		if (_locationChangeHandlers == null || _locationChangeHandlers.Contains(target))
		{
			return;
		}
		foreach (ILocationHandler locationChangeHandler in _locationChangeHandlers)
		{
			if (locationChangeHandler.locationChangePriority == target.locationChangePriority)
			{
				Debug.LogError("Cannot add handler with priority " + target.locationChangePriority + " as one already exists at that priority");
				return;
			}
		}
		_locationChangeHandlers.Add(target);
	}

	public void RemoveLocationChangeHandler(ILocationHandler target)
	{
		if (_locationChangeHandlers != null && _locationChangeHandlers.Contains(target))
		{
			_locationChangeHandlers.Remove(target);
		}
	}

	private void OutputHandlerDebug()
	{
		foreach (ILocationHandler locationChangeHandler in _locationChangeHandlers)
		{
			UnityEngine.Object obj = locationChangeHandler as UnityEngine.Object;
			Debug.Log("Handler [" + ((!(obj != null)) ? "unknown" : obj.name) + "] priority " + locationChangeHandler.locationChangePriority);
		}
	}

	private void NotifyLocationChangeHandlers(string previous, string linkName, JsonData linkData)
	{
		if (_locationChangeHandlers == null)
		{
			return;
		}
		if (_isNotifyingHandlers)
		{
			Debug.LogError("[FlowChart] Nested location change detected! Aborting!");
			return;
		}
		_isNotifyingHandlers = true;
		int num = int.MaxValue;
		int num2 = int.MinValue;
		int num3 = 0;
		while (num3 < _locationChangeHandlers.Count)
		{
			ILocationHandler locationHandler = null;
			foreach (ILocationHandler locationChangeHandler in _locationChangeHandlers)
			{
				if (locationChangeHandler.locationChangePriority > num2 && locationChangeHandler.locationChangePriority < num)
				{
					num2 = locationChangeHandler.locationChangePriority;
					locationHandler = locationChangeHandler;
				}
			}
			num = num2;
			num2 = int.MinValue;
			if (locationHandler == null)
			{
				Debug.LogWarning("Location Handler NULL -- skipping");
			}
			else
			{
				string currentLocation = _currentLocation;
				locationHandler.ChangeLocation(previous, ref _currentLocation, linkName, linkData, _pCurrentLocationData);
				if (_currentLocation != currentLocation)
				{
					if (_pCurrentLocationData != null)
					{
						num3 = 0;
						num = int.MaxValue;
						num2 = int.MinValue;
						continue;
					}
					Debug.LogError(_pLogTag + "Aborted location change to unknown location [" + _currentLocation + "], reverting to [" + currentLocation + "]");
					_currentLocation = currentLocation;
				}
			}
			num3++;
		}
		_isNotifyingHandlers = false;
	}

	private void CacheLocationData(string location)
	{
		_cachedLocation = location;
		if (_flowData != null && location != null && location.Length > 0)
		{
			_cachedLocationData = Extensions.DeepCopyJson(_flowData.TryGet(location));
			ReplaceFlowVariables(_cachedLocationData, null);
		}
		else
		{
			_cachedLocationData = null;
		}
	}

	public bool ManualChangeLocation(string newLocation, string linkName, JsonData linkData)
	{
		return AddLocation(newLocation, linkName, linkData);
	}

	private bool AddLocation(string location, string linkName, JsonData linkData)
	{
		string pPreviousLocation = _pPreviousLocation;
		JsonData jsonData = linkData.TryGet("flow");
		if (jsonData != null)
		{
			HandleFlowControl(jsonData, false);
		}
		_currentLocation = location;
		if (_isNotifyingHandlers)
		{
			return true;
		}
		NotifyLocationChangeHandlers(pPreviousLocation, linkName, linkData);
		JsonData pCurrentLocationData = _pCurrentLocationData;
		if (pCurrentLocationData == null)
		{
			return false;
		}
		_previousLocations.Push(_currentLocation);
		jsonData = pCurrentLocationData.TryGet("flow");
		if (jsonData != null)
		{
			HandleFlowControl(jsonData, true);
		}
		if (!HandleFlowControlFollowLink(jsonData, linkName))
		{
			HandleFlowControlSubFlow(jsonData, linkName, linkData);
		}
		return true;
	}

	private IEnumerator DelayFollowLink(string link, bool isSearchGlobalLinks, float seconds)
	{
		if (link != null)
		{
			yield return new WaitForSeconds(seconds);
			FollowLink(link, null, false, isSearchGlobalLinks);
		}
	}

	private bool HandleFlowControlFollowLink(JsonData flowControl, string sourceLink)
	{
		bool result = false;
		if (flowControl != null)
		{
			JsonData jsonData = flowControl.TryGet("followLink");
			if (jsonData != null)
			{
				JsonData jsonData2 = null;
				JsonData jsonData3 = null;
				JsonData jsonData4 = null;
				JsonData jsonData5 = flowControl.TryGet("frameDelay");
				if (jsonData.IsString)
				{
					jsonData2 = jsonData;
				}
				else if (jsonData.IsObject)
				{
					jsonData2 = jsonData.TryGet("link", JsonType.String);
					jsonData3 = jsonData.TryGet("ignoreGlobalLinks", JsonType.Boolean);
					jsonData4 = jsonData.TryGet("forgetThisLocation", JsonType.Boolean);
					jsonData5 = jsonData.TryGet("delay");
				}
				if (jsonData2 != null)
				{
					string text = (string)jsonData2;
					if (text == "*")
					{
						text = sourceLink;
					}
					bool flag = ((jsonData3 == null) ? _isOldGlobalLinkBehaviour : ((bool)jsonData3));
					if (jsonData4 != null && (bool)jsonData4)
					{
						ForgetCurrentLocation();
					}
					if (jsonData5 != null)
					{
						if (_coroutineHandler != null)
						{
							_coroutineHandler.StartCoroutine(DelayFollowLink(text, !flag, (float)(double)jsonData5));
							result = true;
						}
						else
						{
							Debug.LogError(_pLogTag + "No co-routine handler set");
						}
					}
					else
					{
						FollowLink(text, null, false, !flag);
						result = true;
					}
				}
			}
			else
			{
				result = HandleFlowControlPassThru(flowControl);
			}
		}
		return result;
	}

	private bool HandleFlowControlPassThru(JsonData flowControl)
	{
		bool result = false;
		if (flowControl != null)
		{
			string text = flowControl.TryGetString("passThru", string.Empty);
			if (text != null && text.Length > 0)
			{
				ForgetCurrentLocation();
				string parentLocation = GetParentLocation(_currentLocation);
				result = GotoLocation(text, parentLocation, "PASS_THRU", null, false);
			}
		}
		return result;
	}

	private void HandleFlowControl(JsonData flowControl, bool isNextLocationAdded)
	{
		if (flowControl == null)
		{
			return;
		}
		JsonData jsonData = flowControl.TryGet("bookmark");
		if (jsonData != null)
		{
			if (jsonData.IsString)
			{
				SetBookmark((string)jsonData);
			}
			else if (jsonData.IsBoolean && (bool)jsonData)
			{
				SetBookmark(_currentLocation);
			}
		}
		JsonData jsonData2 = flowControl.TryGet("clearHistory");
		if (jsonData2 != null)
		{
			bool flag = false;
			string[] array = null;
			string parentLocation = GetParentLocation(_currentLocation);
			if (jsonData2.IsBoolean)
			{
				flag = (bool)jsonData2;
			}
			else if (jsonData2.IsString)
			{
				flag = true;
				array = new string[1] { GetGlobalLocation((string)jsonData2, parentLocation) };
			}
			else if (jsonData2.IsArray)
			{
				flag = true;
				array = new string[jsonData2.Count];
				for (int i = 0; i < jsonData2.Count; i++)
				{
					if (jsonData2[i] != null && jsonData2[i].IsString)
					{
						array[i] = GetGlobalLocation((string)jsonData2[i], parentLocation);
					}
				}
			}
			if (flag)
			{
				ClearHistory(array, isNextLocationAdded);
			}
		}
		JsonData jsonData3 = flowControl.TryGet("forgetThisLocation", JsonType.Boolean);
		if (jsonData3 == null)
		{
			jsonData3 = flowControl.TryGet("removeUsFromStack", JsonType.Boolean);
		}
		if (jsonData3 != null && (bool)jsonData3)
		{
			ForgetCurrentLocation();
		}
		JsonData jsonData4 = flowControl.TryGet("setFlowCondition");
		if (jsonData4 != null)
		{
			SetFlowCondition((string)jsonData4["key"], (bool)jsonData4["value"]);
		}
		JsonData jsonData5 = flowControl.TryGet("setFlowVariable");
		if (jsonData5 != null)
		{
			Action<JsonData> action = delegate(JsonData d)
			{
				if (d != null && d.IsObject)
				{
					string text = d.TryGetString("key", string.Empty);
					if (text.Length != 0)
					{
						SetFlowVariable(text, d.TryGet("value"));
					}
				}
			};
			if (jsonData5.IsArray)
			{
				for (int num = 0; num < jsonData5.Count; num++)
				{
					action(jsonData5[num]);
				}
			}
			else
			{
				action(jsonData5);
			}
		}
		JsonData jsonData6 = flowControl.TryGet("quit");
		if (jsonData6 != null && (bool)jsonData6 && _onFlowControlQuit != null)
		{
			_onFlowControlQuit();
		}
		JsonData jsonData7 = flowControl.TryGet("doRecheckInternet");
		if (jsonData7 != null && (bool)jsonData7)
		{
			InternetCheckFacade.DoRecheckInternetFromFlow();
		}
	}

	private void HandleFlowControlSubFlow(JsonData flowControl, string link, JsonData linkData)
	{
		if (flowControl == null || !flowControl.IsObject)
		{
			return;
		}
		JsonData jsonData = flowControl.TryGet("subflow");
		if (jsonData == null)
		{
			return;
		}
		JsonData jsonData2 = null;
		JsonData startLocationData = null;
		if (jsonData.IsString)
		{
			jsonData2 = jsonData;
		}
		else if (jsonData.IsObject)
		{
			jsonData2 = jsonData.TryGet("name", JsonType.String);
			if (jsonData2 == null)
			{
				jsonData2 = jsonData.TryGet("id", JsonType.String);
			}
			startLocationData = jsonData.TryGet("start", JsonType.String);
		}
		string text = (string)jsonData2;
		if (text == null || text.Length == 0)
		{
			return;
		}
		if (_flowManager == null)
		{
			Debug.LogError(_pLogTag + "Cannot look up sub-flow data '" + text + "'");
			return;
		}
		jsonData = (flowControl["subflow"] = _flowManager.FindFlowData(text));
		if (jsonData == null || !jsonData.IsObject)
		{
			Debug.LogError(_pLogTag + "Bad or missing sub-flow data '" + text + "'");
			return;
		}
		LoadSubFlowLocations(jsonData, _currentLocation);
		jsonData = Extensions.DeepCopyJson(jsonData);
		ReplaceFlowVariables(jsonData, null);
		string text2 = GetStartLocation(jsonData);
		if (startLocationData != null && startLocationData.IsString)
		{
			ReplaceFlowVariables(startLocationData, delegate(JsonData varData)
			{
				if (!varData.IsString)
				{
					Debug.LogError(_pLogTag + "Expected string flow variable for sub-flow start location");
				}
				else
				{
					startLocationData = new JsonData((string)varData);
				}
			});
			text2 = (string)startLocationData;
		}
		if (text2 == null || text2.Length == 0)
		{
			Debug.LogError(_pLogTag + "Sub-flow has no start location");
			return;
		}
		string location = _currentLocation + "." + text2;
		ForgetCurrentLocation();
		_rootLocations.Push(_previousLocations.Count);
		AddLocation(location, link, linkData);
	}

	private void LoadSubFlowLocations(JsonData subFlowData, string parentLocation)
	{
		if (subFlowData == null || !subFlowData.IsObject)
		{
			Debug.LogError(_pLogTag + "Bad or missing sub-flow data");
			return;
		}
		if (parentLocation == null || parentLocation.Length == 0)
		{
			Debug.LogError(_pLogTag + "Cannot load sub-flow into null parent-location");
			return;
		}
		foreach (DictionaryEntry item in (IOrderedDictionary)subFlowData)
		{
			string text = (string)item.Key;
			JsonData jsonData = (JsonData)item.Value;
			if (text == "Global")
			{
			}
			if (jsonData.IsObject)
			{
				jsonData = Extensions.DeepCopyJson(jsonData);
				_flowData[parentLocation + "." + text] = jsonData;
			}
		}
	}

	public void HandleExternalFlowControl(JsonData flowControl)
	{
		HandleFlowControl(flowControl, true);
		if (!HandleFlowControlFollowLink(flowControl, string.Empty))
		{
			HandleFlowControlSubFlow(flowControl, string.Empty, null);
		}
	}

	public bool GoBack()
	{
		return FollowLink("<<");
	}

	private bool GoBack(string linkName, JsonData linkData)
	{
		if (_previousLocations.Count < 2)
		{
			Debug.LogError(_pLogTag + "No previous location to go back to!");
			return false;
		}
		int pCurrentRootLocationIndex = _pCurrentRootLocationIndex;
		int num = _previousLocations.Count - 1;
		_previousLocations.Pop();
		if (pCurrentRootLocationIndex > 0 && num == pCurrentRootLocationIndex)
		{
			_rootLocations.Pop();
		}
		string location = _previousLocations.Pop();
		return AddLocation(location, linkName, linkData);
	}

	public void ForgetCurrentLocation()
	{
		if (_previousLocations != null && _previousLocations.Count > 0)
		{
			_previousLocations.Pop();
		}
	}

	public void ClearHistory(string initLocation = null)
	{
		ClearHistory(new string[1] { initLocation }, false);
	}

	private void ClearHistory(string[] initLocations, bool isAddCurrentLocation)
	{
		int pCurrentRootLocationIndex = _pCurrentRootLocationIndex;
		while (_previousLocations.Count > pCurrentRootLocationIndex)
		{
			_previousLocations.Pop();
		}
		if (initLocations != null)
		{
			for (int i = 0; i < initLocations.Length; i++)
			{
				if (initLocations[i] != null && initLocations[i].Length > 0)
				{
					_previousLocations.Push(initLocations[i]);
				}
			}
		}
		if (isAddCurrentLocation)
		{
			_previousLocations.Push(_currentLocation);
		}
	}

	public bool FollowGlobalLink(string name)
	{
		if (!_isOldGlobalLinkBehaviour)
		{
			Debug.LogError(_pLogTag + "FollowGlobalLink is unsupported");
			return false;
		}
		if (_globalLocationData == null)
		{
			Debug.LogWarning(_pLogTag + "No global links");
			return false;
		}
		JsonData jsonData = _globalLocationData.TryGet("links");
		bool flag = false;
		JsonData jsonData2 = null;
		for (int i = 0; i < jsonData.Count; i++)
		{
			JsonData jsonData3 = jsonData[i];
			JsonData jsonData4 = jsonData3.TryGet("link");
			if (jsonData4 != null && (string)jsonData4 == name)
			{
				jsonData2 = jsonData3;
				JsonData jsonData5 = jsonData2.TryGet("preferLocal");
				if (jsonData5 != null)
				{
					flag = (bool)jsonData5;
				}
				break;
			}
		}
		if (!flag && jsonData2 == null)
		{
			Debug.LogWarning(_pLogTag + "Cannot follow unknown global link: " + name + ", and no local version in current location");
			return false;
		}
		bool flag2 = false;
		if (!((!flag) ? FollowLink(name, jsonData2, true, false) : FollowLink(name, null, false, false)))
		{
			Debug.LogWarning(_pLogTag + "Cannot follow unknown global link: " + name);
			return false;
		}
		return true;
	}

	public bool FollowLink(string link)
	{
		if (link == "<<")
		{
			JsonData jsonData = FindLinkData(link, _pCurrentLocationLinks, null);
			if (jsonData != null && FollowLink(link, jsonData, false, false))
			{
				return true;
			}
			return GoBack(link, jsonData);
		}
		return FollowLink(link, null, false, !_isOldGlobalLinkBehaviour);
	}

	private bool FollowLink(string link, JsonData linkDataOverride, bool isForceRootGlobalLink, bool isSearchGlobalLinks)
	{
		JsonData jsonData = null;
		string linkSpace = GetParentLocation(_currentLocation);
		if (linkDataOverride != null)
		{
			if (linkDataOverride.IsObject)
			{
				jsonData = linkDataOverride;
			}
			else if (linkDataOverride.IsArray)
			{
				jsonData = FindLinkData(link, linkDataOverride, (JsonData defaultLink) => defaultLink);
			}
		}
		else
		{
			jsonData = FindLinkData(link, _pCurrentLocationLinks, delegate(JsonData defaultLink)
			{
				if (isSearchGlobalLinks)
				{
					string parentLocation = linkSpace;
					JsonData jsonData4 = FindGlobalLink(link, ref parentLocation, true);
					if (jsonData4 != null)
					{
						linkSpace = parentLocation;
						return jsonData4;
					}
				}
				return defaultLink;
			});
		}
		if (jsonData == null)
		{
			Debug.LogWarning(_pLogTag + "From location '" + _currentLocation + "', cannot follow unknown link: " + link);
			return false;
		}
		string text = null;
		JsonData jsonData2 = null;
		jsonData2 = jsonData.TryGet("mobile");
		if (jsonData2 != null)
		{
			text = (string)jsonData2;
			if (text == string.Empty)
			{
				return false;
			}
		}
		else
		{
			if (text == null)
			{
				text = ResolveConditionalLink(jsonData);
			}
			if (text == null)
			{
				text = ResolveSwitchLink(jsonData);
			}
			if (text == null)
			{
				JsonData jsonData3 = jsonData.TryGet("location");
				text = ((jsonData3 == null || !jsonData3.IsString) ? null : ((string)jsonData3));
			}
		}
		return GotoLocation(text, linkSpace, link, jsonData, isForceRootGlobalLink);
	}

	private bool GotoLocation(string location, string linkSpace, string link, JsonData linkData, bool isForceRootGlobalLink)
	{
		if (location == null || location == string.Empty || location == "!")
		{
			return false;
		}
		bool flag = false;
		if (location == "*")
		{
			return FollowBookmark(link, linkData);
		}
		if (location == "..")
		{
			return GoBack(link, linkData);
		}
		if (location.StartsWith("^"))
		{
			if (location.Length > 1)
			{
				link = location.Substring(1);
			}
			return ExitSubFlow(link);
		}
		if (!isForceRootGlobalLink)
		{
			location = GetGlobalLocation(location, linkSpace);
		}
		return AddLocation(location, link, linkData);
	}

	private bool ExitSubFlow(string link)
	{
		string parentLocation = GetParentLocation(_currentLocation);
		if (parentLocation == null || parentLocation.Length == 0)
		{
			return false;
		}
		_currentLocation = parentLocation;
		ClearHistory(null, true);
		_rootLocations.Pop();
		return FollowLink(link, null, false, !_isOldGlobalLinkBehaviour);
	}

	private JsonData FindLinkData(string link, JsonData linkDataList, Func<JsonData, JsonData> onNotFound)
	{
		if (linkDataList == null || !linkDataList.IsArray)
		{
			if (onNotFound != null)
			{
				return onNotFound(null);
			}
			return null;
		}
		JsonData arg = null;
		for (int i = 0; i < linkDataList.Count; i++)
		{
			if (linkDataList[i] == null || !linkDataList[i].IsObject)
			{
				continue;
			}
			JsonData jsonData = linkDataList[i].TryGet("link");
			if (jsonData == null || !jsonData.IsString)
			{
				continue;
			}
			string text = (string)jsonData;
			if (text != null && text.Length != 0)
			{
				if (text == link)
				{
					return linkDataList[i];
				}
				if (text == "*")
				{
					arg = linkDataList[i];
				}
			}
		}
		if (onNotFound != null)
		{
			return onNotFound(arg);
		}
		return null;
	}

	private JsonData FindGlobalLink(string link, ref string parentLocation, bool isSearchParent)
	{
		JsonData jsonData = null;
		string globalLocation = GetGlobalLocation("Global", parentLocation);
		JsonData jsonData2 = _flowData.TryGet(globalLocation, JsonType.Object);
		if (jsonData2 != null)
		{
			JsonData jsonData3 = jsonData2.TryGet("links", JsonType.Array);
			if (jsonData3 != null)
			{
				jsonData = FindLinkData(link, jsonData3, (JsonData defaultLink) => defaultLink);
			}
		}
		if (jsonData == null && isSearchParent && parentLocation != null && parentLocation.Length > 0)
		{
			parentLocation = GetParentLocation(parentLocation);
			jsonData = FindGlobalLink(link, ref parentLocation, true);
		}
		return jsonData;
	}

	private string GetParentLocation(string location)
	{
		string[] array = location.Split('.');
		return string.Join(".", array, 0, array.Length - 1);
	}

	private string GetGlobalLocation(string location, string parentLocation)
	{
		return (parentLocation == null || parentLocation.Length <= 0) ? location : (parentLocation + "." + location);
	}

	public string ResolveConditionalLink(JsonData link)
	{
		string result = null;
		JsonData jsonData = link.TryGet("conditionalLink");
		if (jsonData != null)
		{
			JsonData jsonData2 = jsonData.TryGet("condition");
			if (jsonData2 != null)
			{
				JsonData jsonData3 = jsonData.TryGet("locationTrue");
				JsonData jsonData4 = jsonData.TryGet("locationFalse");
				result = (GetFlowCondition(jsonData2.ToString()) ? ((jsonData3 == null) ? null : ((!jsonData3.IsObject || jsonData3.TryGet("mobile") == null) ? jsonData3.ToString() : jsonData3.TryGet("mobile").ToString())) : ((jsonData4 == null) ? null : ((!jsonData4.IsObject || jsonData4.TryGet("mobile") == null) ? jsonData4.ToString() : jsonData4.TryGet("mobile").ToString())));
			}
		}
		return result;
	}

	private string ResolveSwitchLink(JsonData linkData)
	{
		if (linkData == null)
		{
			return null;
		}
		if (linkData.GetJsonType() != JsonType.Object)
		{
			return null;
		}
		JsonData jsonData = linkData.TryGet("switchLink", JsonType.Object);
		if (jsonData == null)
		{
			return null;
		}
		JsonData jsonData2 = jsonData.TryGet("switchVar", JsonType.String);
		if (jsonData2 == null)
		{
			Debug.LogError(_pLogTag + "Missing or invalid switch variable");
			return null;
		}
		string text = (string)jsonData2;
		JsonData jsonData3 = GetFlowVariable(text);
		if (jsonData3 == null)
		{
			Debug.LogWarning(_pLogTag + "Switch variable '" + text + "' cannot be found");
		}
		else if (jsonData3.GetJsonType() != JsonType.String && jsonData3.GetJsonType() != JsonType.Int)
		{
			Debug.LogWarning(_pLogTag + "Switch variable '" + text + "' cannot be used, due to type '" + jsonData3.GetJsonType().ToString() + "'");
			jsonData3 = null;
		}
		JsonData jsonData4 = null;
		if (jsonData3 != null)
		{
			jsonData4 = jsonData.TryGet("locationCase_" + jsonData3.ToString(), JsonType.String);
		}
		if (jsonData4 == null)
		{
			jsonData4 = jsonData.TryGet("locationDefault", JsonType.String);
		}
		return (jsonData4 == null) ? null : ((string)jsonData4);
	}

	public void SetBookmark(string location)
	{
		_bookmark = location;
	}

	private bool FollowBookmark(string linkName, JsonData linkData)
	{
		if (_bookmark == null)
		{
			Debug.LogError(_pLogTag + "Attempt to pop from " + _currentLocation + " failed because there is no pushed location");
			return false;
		}
		string bookmark = _bookmark;
		_bookmark = null;
		return AddLocation(bookmark, linkName, linkData);
	}

	public Stack<string> GetLocationStack()
	{
		return _previousLocations;
	}

	private string ParseFlowVariableId(string text)
	{
		string result = null;
		int num = text.IndexOf(_flowVarPrefix);
		if (0 <= num && num < text.Length - _flowVarPrefix.Length)
		{
			num += _flowVarPrefix.Length;
			if (_flowVarSuffix.Length > 0)
			{
				int num2 = text.IndexOf(_flowVarSuffix, num);
				if (num <= num2 && num2 < text.Length)
				{
					result = text.Substring(num, num2 - num).Trim();
				}
			}
			else
			{
				result = text.Substring(num).Trim();
			}
		}
		return result;
	}

	private void ReplaceFlowVariables(JsonData data, Action<JsonData> replaceDataAction)
	{
		switch (data.GetJsonType())
		{
		case JsonType.String:
		{
			string text = ParseFlowVariableId((string)data);
			if (text == null || text.Length <= 0)
			{
				break;
			}
			if (_flowVariables.ContainsKey(text))
			{
				JsonData jsonData = _flowVariables[text];
				if (jsonData != null)
				{
					replaceDataAction(jsonData);
				}
				else
				{
					replaceDataAction(new JsonData("null variable '" + text + "'"));
				}
			}
			else
			{
				replaceDataAction(new JsonData("missing variable '" + text + "'"));
			}
			break;
		}
		case JsonType.Array:
		{
			for (int i = 0; i < data.Count; i++)
			{
				ReplaceFlowVariables(data[i], delegate(JsonData newData)
				{
					data[i] = Extensions.DeepCopyJson(newData);
				});
			}
			break;
		}
		case JsonType.Object:
		{
			foreach (DictionaryEntry kvp in (IOrderedDictionary)data)
			{
				ReplaceFlowVariables(data[(string)kvp.Key], delegate(JsonData newData)
				{
					data[(string)kvp.Key] = Extensions.DeepCopyJson(newData);
				});
			}
			break;
		}
		}
	}
}
