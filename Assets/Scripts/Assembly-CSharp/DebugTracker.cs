using System.Collections.Generic;
using DeepThought;
using UnityEngine;

public class DebugTracker : AnalyticsTracker
{
	private bool logInRelease;

	private bool frozen;

	public override bool Frozen
	{
		get
		{
			return frozen;
		}
		set
		{
			if (initialised && frozen != value)
			{
				if (frozen)
				{
					Log("Tracking: UnFreeze");
				}
				else
				{
					Log("Tracking: Freeze");
				}
			}
			frozen = value;
		}
	}

	public DebugTracker(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
		init();
	}

	public override void init()
	{
		base.init();
		Neuron neuron = tryGetNeuron("logInRelease");
		logInRelease = neuron != null && neuron.Value.Bool;
		Debug.Log("Initialise Debug Tracker: release logging? > " + logInRelease);
		initialised = true;
	}

	private void Log(string log)
	{
		if (logInRelease || Debug.isDebugBuild)
		{
			Debug.Log(log);
		}
	}

	private void LogWarning(string log)
	{
		if (logInRelease || !Debug.isDebugBuild)
		{
			Debug.LogWarning(log);
		}
	}

	public override void LogMetric(string metricName, string metricGroup = "", bool timer = false)
	{
		Log(string.Format("Tracking: Metric {0} {1}", metricName, metricGroup));
	}

	public override void LogCustomMetric(Neuron metric)
	{
		string text = metric.Text;
		string text2 = null;
		string text3 = null;
		if (metric.tryGetChild("group") != null)
		{
			text2 = metric.getText("group");
		}
		if (metric.tryGetChild("timer") != null)
		{
			text3 = metric.getText("timer");
		}
		if (name == null)
		{
			LogWarning(string.Format("Custom metric '{0}' has no name, cannot log", text));
		}
		else if (text2 == null)
		{
			Log(string.Format("Tracking: CustomMetric {0}", text));
		}
		else if (text3 == null)
		{
			Log(string.Format("Tracking: CustomMetric {0} {1}", text, text2));
		}
		else
		{
			Log(string.Format("Tracking: CustomMetric {0} {1} timer: {2}", text, text2, text3));
		}
	}

	public override void LogParameterMetric(string metric, Dictionary<string, string> parameters)
	{
		foreach (KeyValuePair<string, string> parameter in parameters)
		{
			Log(string.Format("Tracking: {0} Paramaters {1} : {2}", metric, parameter.Key, parameter.Value));
		}
	}

	public override void LogHeatmapMetric(Neuron metric, long x, long y)
	{
		if (initialised)
		{
			Neuron neuron = metric.tryGetNeuron("name");
			string text = null;
			if (metric.tryGetChild("group") != null)
			{
				text = metric.getText("group");
			}
			if (neuron == null)
			{
				LogWarning(string.Format("Tracking: Heatmap metric '{0}' has no name", metric.getName()));
				return;
			}
			if (text == null)
			{
				LogWarning(string.Format("Tracking: Heatmap metric '{0}' has no group", metric.getName()));
				return;
			}
			Log(string.Format("Tracking: Heatmap '{0}' '{1}' ({2},{3})", neuron.Value.String, text, x, y));
		}
	}

	public override void LogLevelMetric(Neuron metric, LevelMetric metricType, double value)
	{
		if (!initialised)
		{
			return;
		}
		Neuron neuron = metric.tryGetNeuron("level");
		if (neuron == null)
		{
			Debug.LogWarning(string.Format("Tracking: Level metric '{0}' has no level", metric.getName()));
			return;
		}
		string text = neuron.Value.String.Replace(" ", "-");
		if (metricType == LevelMetric.Event)
		{
			Neuron neuron2 = metric.tryGetNeuron("event");
			if (neuron2 != null)
			{
				switch (neuron2.Value.String)
				{
				case "start":
					Log(string.Format("Tracking: Start {0}", text));
					break;
				case "win":
					Log(string.Format("Tracking: Win {0}", text));
					break;
				case "quit":
					Log(string.Format("Tracking: Quit {0}", text));
					break;
				case "retry":
					Log(string.Format("Tracking: Retry {0}", text));
					break;
				default:
					LogWarning(string.Format("Tracking: Level metric '{0}' has unknown event '{1}'", metric.getName(), neuron2.Value.String));
					break;
				}
			}
			else
			{
				LogWarning(string.Format("Tracking: Level metric '{0}' has no event", metric.getName()));
			}
			return;
		}
		Neuron neuron3 = metric.tryGetNeuron("name");
		if (neuron3 == null)
		{
			LogWarning(string.Format("Tracking: Level metric '{0}' has no name", metric.getName()));
			return;
		}
		string text2 = neuron3.Value.String;
		Neuron neuron4 = metric.tryGetNeuron("timer");
		bool flag = neuron4 != null && neuron4.Value.Bool;
		switch (metricType)
		{
		case LevelMetric.Counter:
			Log(string.Format("Tracking: LevelCounterMetric {0} {1} timer: {2}", text2, text, flag));
			break;
		case LevelMetric.Ranged:
			Log(string.Format("Tracking: LevelRangedMetric {0} {1} {2} timer: {3}", text2, text, (int)value, flag));
			break;
		case LevelMetric.Average:
			Log(string.Format("Tracking: LevelAverageMetric {0} {1} {2} timer: {3}", text2, text, value, flag));
			break;
		}
	}
}
