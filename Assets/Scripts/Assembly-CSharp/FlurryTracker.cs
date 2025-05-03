using System.Collections.Generic;
using DeepThought;
using LitJson;
using UnityEngine;

public class FlurryTracker : AnalyticsTracker
{
	private string _parameter = "parameter";

	public override bool Frozen
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public FlurryTracker(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
	}

	public override void init()
	{
		base.init();
		JsonData jsonData = JsonLoader.data(base.Text);
		JsonData jsonData2 = null;
		JsonData jsonData3 = null;
		if (jsonData == null)
		{
			Debug.LogError("TRACKING FACADE: No flurry data!");
			return;
		}
		jsonData = jsonData["googlePlay"];
		if (jsonData != null)
		{
			if (jsonData.Contains("default"))
			{
				jsonData2 = jsonData["default"];
			}
			if (jsonData.Contains(AnvilBuildInfo._pBuildId))
			{
				jsonData3 = jsonData[AnvilBuildInfo._pBuildId];
			}
			if (jsonData3 != null)
			{
				jsonData = jsonData3;
			}
			else if (jsonData2 != null)
			{
				jsonData = jsonData2;
			}
			string apiKey = (string)jsonData["API_Key"];
			FlurryAndroid.onStartSession(apiKey, false, false);
			initialised = true;
		}
	}

	public override void LogMetric(string metricName, string metricGroup = "", bool timer = false)
	{
		if (initialised)
		{
			if (timer)
			{
				PersonalLogs.KayLog("FLURRY: START TIMER <color=#00ff00> " + metricGroup + ": " + metricName + "</color>");
			}
			string text = metricGroup + ": " + metricName;
			FlurryAndroid.logEvent(text, timer);
			PersonalLogs.KayLog("FLURRY: LOGGED " + text);
		}
	}

	public override void StopTimerMetric(string metricName)
	{
		if (initialised)
		{
			PersonalLogs.KayLog("FLURRY: STOP TIMER <color=#00ff00> " + metricName + "</color>");
			FlurryAndroid.endTimedEvent(metricName);
		}
	}

	public override void LogCustomMetric(Neuron metric)
	{
		if (!initialised)
		{
			return;
		}
		string text = metric.Text;
		bool isTimed = false;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		PersonalLogs.KayLog("FLURRY: LogCustomMetric " + text);
		if (metric.tryGetChild("group") != null)
		{
			text = metric.getText("group");
			string key = text;
			if (tryGetNeuron("type") != null && getText("type") == _parameter)
			{
				key = metric.getName();
			}
			string text2 = metric.Text;
			if (tryGetNeuron(text2) != null)
			{
				text2 = getText(text2);
			}
			dictionary.Add(key, text2);
		}
		if (metric.tryGetChild("timer") != null)
		{
			string text3 = metric.getText("timer");
			if (!(text3 == "Start"))
			{
				FlurryAndroid.endTimedEvent(text3);
				return;
			}
			isTimed = true;
		}
		foreach (Neuron item in (IEnumerable<Neuron>)metric)
		{
			Neuron neuron = item.tryGetNeuron("type");
			if (neuron == null || !(neuron.getValue().getText() == _parameter))
			{
				continue;
			}
			string key = item.getName();
			Neuron neuron2 = item.tryGetNeuron("label");
			if (neuron2 != null)
			{
				key = neuron2.getValue().getText();
				Neuron neuron3 = tryGetNeuron(key);
				if (neuron3 != null)
				{
					key = neuron3.getValue().getText();
				}
			}
			string text2 = item.getValue().getText();
			Neuron neuron4 = tryGetNeuron(text2);
			if (neuron4 != null)
			{
				text2 = neuron4.getValue().getText();
			}
			dictionary.Add(key, text2);
		}
		foreach (KeyValuePair<string, string> item2 in dictionary)
		{
			PersonalLogs.KayLog("FLURRY: " + item2.Key + " = " + item2.Value);
		}
		FlurryAndroid.logEvent(text, dictionary, isTimed);
	}

	public override void LogParameterMetric(string metric, Dictionary<string, string> parameters)
	{
		if (!initialised)
		{
			return;
		}
		PersonalLogs.KayLog("FLURRY: LogParamaterMetric " + metric);
		foreach (KeyValuePair<string, string> parameter in parameters)
		{
			PersonalLogs.KayLog("FLURRY: " + parameter.Key + " = " + parameter.Value);
		}
		FlurryAndroid.logEvent(metric, parameters, false);
	}

	public override void LogHeatmapMetric(Neuron metric, long x, long y)
	{
		if (initialised)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("x", x.ToString());
			dictionary.Add("y", y.ToString());
			FlurryAndroid.logEvent(metric.Text, dictionary, false);
		}
	}

	public override void LogLevelMetric(Neuron metric, LevelMetric metricType, double value)
	{
		if (initialised)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(metricType.ToString(), value.ToString());
			FlurryAndroid.logEvent(metric.Text, dictionary, false);
		}
	}

	public override void EndSession()
	{
		FlurryAndroid.onEndSession();
	}
}
