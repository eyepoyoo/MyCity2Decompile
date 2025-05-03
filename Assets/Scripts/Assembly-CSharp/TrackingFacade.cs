using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DeepThought;
using LitJson;
using UnityEngine;

public class TrackingFacade : InitialisationObject, ILocationHandler
{
	private const string TRACKING_VAR = "TRACKING_VAR";

	private List<AnalyticsTracker> _trackers = new List<AnalyticsTracker>();

	private Neuron _events;

	private Neuron _metricsRoot;

	private Neuron _metrics;

	private Neuron _metricsData;

	private Neuron _metricsFlow;

	private string _gameId;

	public string _pGameId
	{
		get
		{
			return _gameId;
		}
	}

	public int locationChangePriority
	{
		get
		{
			return -3;
		}
	}

	public override void startInitialising()
	{
		_currentState = InitialisationState.INITIALISING;
		FlowFacade.AddLocationHandler(this);
		OnLoaded();
		_currentState = InitialisationState.FINISHED;
	}

	protected override void Awake()
	{
		base.Awake();
		PersonalLogs.KayLog("TRACKING FACADE: Initialise Tracking Facade");
		Facades<TrackingFacade>.Register(this);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		NeuronFactoryStore.add<Neuron>("Trackers");
		NeuronFactoryStore.add<DebugTracker>("DebugTracker");
		NeuronFactoryStore.add<FlurryTracker>("FlurryTracker");
		NeuronFactoryStore.add<GaTracker>("GaTracker");
		NeuronFactoryStore.add<TrackManTracker>("TrackManTracker");
	}

	private void OnLoaded()
	{
		JsonData jsonData = JsonLoader.data("trackingMetrics");
		_metricsRoot = Neuron.generate(jsonData.ToJson());
		_gameId = _metricsRoot.tryGetNeuron("gameId").Text;
		_events = _metricsRoot.tryGetNeuron("events");
		_metrics = _metricsRoot.tryGetNeuron("metrics");
		_metricsData = _metricsRoot.tryGetNeuron("trackingData");
		_metricsFlow = _metricsRoot.tryGetNeuron("trackingFlow");
		Neuron neuron = _metricsRoot.tryGetNeuron("Trackers");
		if (neuron == null)
		{
			Debug.LogWarning("No trackers defined in Tracking Facade.");
			return;
		}
		foreach (Neuron item in (IEnumerable<Neuron>)neuron)
		{
			AnalyticsTracker analyticsTracker = item as AnalyticsTracker;
			_trackers.Add(analyticsTracker);
			analyticsTracker.init();
		}
	}

	public void setMetricData(string param, string data)
	{
		PersonalLogs.KayLog("TRACKING: SET " + param + " = " + data);
		if (_metricsData != null)
		{
			_metricsData.setValue(param, data);
		}
	}

	private Neuron GetMetric(string metric)
	{
		if (_metrics == null)
		{
			return null;
		}
		Neuron neuron = _metrics.tryGetNeuron(metric);
		if (neuron == null)
		{
			Debug.LogWarning(string.Format("unknown metric '{0}'", metric));
			return null;
		}
		return neuron;
	}

	public List<string> GetEvents()
	{
		if (_events == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		PersonalLogs.KayLog("ALL EVENTS =======================================");
		int num = 0;
		foreach (Neuron item in (IEnumerable<Neuron>)_events)
		{
			num++;
			PersonalLogs.KayLog("EVENT " + num + " " + item.Text);
			list.Add(item.Text);
		}
		return list;
	}

	public void LogProgress(string name)
	{
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.LogProgress(name);
		}
	}

	public void LogMetric(string metricName, string metricGroup = "", bool timer = false)
	{
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.LogMetric(metricName, metricGroup, timer);
		}
	}

	public void LogEvent(string eventName, string eventVar = null)
	{
		Neuron neuron = _events.tryGetNeuron(eventName);
		if (neuron == null)
		{
			return;
		}
		string text;
		if (neuron.Text == "TRACKING_VAR")
		{
			text = eventName;
			if (eventVar != null)
			{
				text += eventVar;
			}
		}
		else
		{
			text = neuron.Text;
		}
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.LogEvent(text);
		}
	}

	public void LogCustomMetric(string metric)
	{
		Neuron metric2 = GetMetric(metric);
		if (metric2 == null)
		{
			return;
		}
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.LogCustomMetric(metric2);
		}
	}

	public void StopTimerMetric(string metricName, string metricGroup = "")
	{
		string metricName2 = metricGroup + ": " + metricName;
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.StopTimerMetric(metricName2);
		}
	}

	public void LogParameterMetric(string metric, Dictionary<string, string> paramaters)
	{
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.LogParameterMetric(metric, paramaters);
		}
	}

	public void LogHeatmapMetric(string metric, long x, long y)
	{
		Neuron metric2 = GetMetric(metric);
		if (metric2 == null)
		{
			return;
		}
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.LogHeatmapMetric(metric2, x, y);
		}
	}

	public void LogLevelMetric(string metric)
	{
		LogLevelMetric(metric, AnalyticsTracker.LevelMetric.Event, 0.0);
	}

	public void LogLevelCounterMetric(string metric)
	{
		LogLevelMetric(metric, AnalyticsTracker.LevelMetric.Counter, 0.0);
	}

	public void LogLevelRangedMetric(string metric, int value)
	{
		LogLevelMetric(metric, AnalyticsTracker.LevelMetric.Ranged, value);
	}

	public void LogLevelAverageMetric(string metric, double value)
	{
		LogLevelMetric(metric, AnalyticsTracker.LevelMetric.Average, value);
	}

	private void LogLevelMetric(string metric, AnalyticsTracker.LevelMetric metricType, double value)
	{
		Neuron metric2 = GetMetric(metric);
		if (metric2 == null)
		{
			return;
		}
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.LogLevelMetric(metric2, metricType, value);
		}
	}

	public void StartSession()
	{
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.init();
		}
	}

	public void EndSession()
	{
		foreach (AnalyticsTracker tracker in _trackers)
		{
			tracker.EndSession();
		}
	}

	private void OnApplicationQuit()
	{
		EndSession();
	}

	private void OnApplicatioPause(bool focus)
	{
		EndSession();
	}

	public string getTimerTier(int seconds)
	{
		string empty = string.Empty;
		double num = 0.0;
		int num2 = 60;
		int num3 = num2 * 60;
		int num4 = num3 * 24;
		int num5 = 30 * num2;
		while (num5 < 5 * num4 && seconds >= num5)
		{
			num5 *= 2;
		}
		if (num5 < 30 * num2)
		{
			return "30 mins";
		}
		if (num5 < num4)
		{
			num = Math.Ceiling((double)num5 / (double)num3);
			return num + " hour" + ((num != 1.0) ? "s" : string.Empty);
		}
		num = Math.Ceiling((double)num5 / (double)num4);
		return num + " day" + ((num != 1.0) ? "s" : string.Empty);
	}

	public string getScoreTier(int score)
	{
		int num = 20;
		int num2 = 10;
		int num3 = 0;
		if (score > num * num2)
		{
			return "Tier " + num + "+";
		}
		for (int i = num3; i <= num && score > i * num2; i++)
		{
			num3 = i;
		}
		Debug.Log("<color=#ff00ff>[TIER]</color> " + score + "= Tier " + num3);
		return "Tier " + num3;
	}

	public void ChangeLocation(string previous, ref string current, string linkName, JsonData linkData, JsonData currentLocationData)
	{
		if (_metricsFlow != null)
		{
			PersonalLogs.KayLog("TRACKING FACADE: try to log <color=#FF00FF>" + Regex.Replace(current, _metricsFlow.Text, string.Empty) + "</color> with exclusion");
			LogMetric(Regex.Replace(current, _metricsFlow.Text, string.Empty), "Flow");
		}
		else
		{
			PersonalLogs.KayLog("TRACKING FACADE: <color=#FF00FF>" + current + "</color>");
			LogMetric(current, "Flow");
		}
		JsonData jsonData = currentLocationData.TryGet("logMetric");
		if (jsonData != null)
		{
			LogCustomMetric((string)jsonData);
		}
	}
}
