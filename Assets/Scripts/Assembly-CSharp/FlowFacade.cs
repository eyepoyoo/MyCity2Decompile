using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class FlowFacade : InitialisationObject, IFlowManager
{
	[Serializable]
	public class SubFlowSource
	{
		public string _flowId;

		public TextAsset _flowSrc;
	}

	public bool ShowFlowDebug = true;

	public bool _isOldGlobalLinkBehaviour;

	public bool _isHandleBackButton;

	public TextAsset _flowSourceData;

	public string _flowVarPrefix = "[";

	public string _flowVarSuffix = "]";

	private static List<ILocationHandler> _pendingLocationChangeTargets;

	private FlowChart _flowChart;

	public SubFlowSource[] _subFlowSources;

	private Dictionary<string, JsonData> _subFlowData;

	private static Dictionary<string, JsonData> _pendingFlowVariables = new Dictionary<string, JsonData>();

	public string CurrentLocation
	{
		get
		{
			return (_flowChart == null) ? null : _flowChart._pCurrentLocation;
		}
		set
		{
			if (_flowChart != null)
			{
				_flowChart._pCurrentLocation = value;
			}
		}
	}

	JsonData IFlowManager.FindFlowData(string flowId)
	{
		return (_subFlowData == null || !_subFlowData.ContainsKey(flowId)) ? null : _subFlowData[flowId];
	}

	protected override void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!InitialisationFacade._pExists)
		{
			Initialize1();
			return;
		}
		InitialisationFacade.Instance.addToQueue(this);
		_currentState = InitialisationState.WAITING_TO_START;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Facades<FlowFacade>.Register(null);
	}

	private void Start()
	{
		if (!InitialisationFacade._pExists)
		{
			Initialize2();
		}
	}

	private void Update()
	{
		if (_isHandleBackButton)
		{
			HandleBackButton();
		}
	}

	public override void startInitialising()
	{
		Initialize1();
		Initialize2();
		_currentState = InitialisationState.FINISHED;
	}

	private void Initialize1()
	{
		JsonData flowData = null;
		if (_flowSourceData != null && _flowSourceData.text != null)
		{
			flowData = Extensions.LoadJson(_flowSourceData.text);
		}
		else
		{
			Debug.LogWarning("[FlowFacade] No flow source data!");
		}
		_flowChart = new FlowChart("FlowFacade", flowData, this);
		_flowChart._pOnFlowControlQuit = delegate
		{
			Debug.Log("Quitting Application");
			Application.Quit();
		};
		EnsureHandlerList();
		_flowChart.ReplaceLocationChangeHandlersList(_pendingLocationChangeTargets);
		_flowChart._pCoroutineHandler = this;
		_flowChart._pFlowVarPrefix = _flowVarPrefix;
		_flowChart._pFlowVarSuffix = _flowVarSuffix;
		_flowChart._pIsOldGlobalLinkBehaviour = _isOldGlobalLinkBehaviour;
		if (Facades<FlowFacade>.Instance == null || Facades<FlowFacade>.Instance != this)
		{
			Facades<FlowFacade>.Register(this);
		}
		foreach (KeyValuePair<string, JsonData> pendingFlowVariable in _pendingFlowVariables)
		{
			SetFlowVariable(pendingFlowVariable.Key, pendingFlowVariable.Value);
		}
		_pendingFlowVariables.Clear();
	}

	private void Initialize2()
	{
		InitSubFlowData();
		SetStartLocation();
	}

	private void HandleBackButton()
	{
		bool flag = false;
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GoBack();
		}
	}

	private void SetStartLocation()
	{
		if (_flowChart != null)
		{
			_flowChart.SetStartLocation();
		}
	}

	public void SetFlowCondition(string name, bool value)
	{
		if (_flowChart != null)
		{
			_flowChart.SetFlowCondition(name, value);
		}
	}

	public void SetFlowCondition(string name, Func<bool> eval)
	{
		if (_flowChart != null)
		{
			_flowChart.SetFlowCondition(name, eval);
		}
	}

	public bool GetFlowCondition(string name)
	{
		return _flowChart != null && _flowChart.GetFlowCondition(name);
	}

	public static void SetFlowVariable(string name, JsonData value)
	{
		if (Facades<FlowFacade>.Instance != null)
		{
			if (Facades<FlowFacade>.Instance._flowChart != null)
			{
				Facades<FlowFacade>.Instance._flowChart.SetFlowVariable(name, value);
			}
		}
		else if (_pendingFlowVariables.ContainsKey(name))
		{
			_pendingFlowVariables[name] = value;
		}
		else
		{
			_pendingFlowVariables.Add(name, value);
		}
	}

	public static void AddLocationHandler(ILocationHandler handler)
	{
		if (Facades<FlowFacade>.Instance != null)
		{
			Facades<FlowFacade>.Instance.AddLocationChangeTarget(handler);
			return;
		}
		EnsureHandlerList();
		_pendingLocationChangeTargets.Add(handler);
	}

	private static void EnsureHandlerList()
	{
		if (_pendingLocationChangeTargets == null)
		{
			_pendingLocationChangeTargets = new List<ILocationHandler>();
		}
	}

	public void AddLocationChangeTarget(ILocationHandler target)
	{
		if (_flowChart != null)
		{
			_flowChart.AddLocationChangeHandler(target);
		}
	}

	public void RemoveLocationChangeTarget(ILocationHandler target)
	{
		if (_flowChart != null)
		{
			_flowChart.RemoveLocationChangeHandler(target);
		}
	}

	public bool ManualChangeLocation(string newLocation, string linkName, JsonData linkData)
	{
		return _flowChart != null && _flowChart.ManualChangeLocation(newLocation, linkName, linkData);
	}

	public void GoBack()
	{
		if (_flowChart != null)
		{
			_flowChart.GoBack();
		}
	}

	public void ForgetCurrentLocation()
	{
		if (_flowChart != null)
		{
			_flowChart.ForgetCurrentLocation();
		}
	}

	public void ClearHistory(string initLocation = null)
	{
		if (_flowChart != null)
		{
			_flowChart.ClearHistory(initLocation);
		}
	}

	public bool FollowGlobalLink(string name)
	{
		return _flowChart != null && _flowChart.FollowGlobalLink(name);
	}

	public bool FollowLink(string name)
	{
		return _flowChart != null && _flowChart.FollowLink(name);
	}

	public Stack<string> GetLocationStack()
	{
		return (_flowChart == null) ? null : _flowChart.GetLocationStack();
	}

	private void InitSubFlowData()
	{
		if (_subFlowSources == null || _subFlowSources.Length == 0)
		{
			_subFlowData = new Dictionary<string, JsonData>();
			return;
		}
		_subFlowData = new Dictionary<string, JsonData>(_subFlowSources.Length);
		for (int i = 0; i < _subFlowSources.Length; i++)
		{
			if (_subFlowSources[i] != null && _subFlowSources[i]._flowId != null && _subFlowSources[i]._flowId.Length != 0 && !(_subFlowSources[i]._flowSrc == null))
			{
				JsonData value = Extensions.LoadJson(_subFlowSources[i]._flowSrc.text);
				_subFlowData[_subFlowSources[i]._flowId] = value;
			}
		}
	}
}
