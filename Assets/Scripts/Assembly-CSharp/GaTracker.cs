using System.Collections.Generic;
using DeepThought;
using UnityEngine;

public class GaTracker : AnalyticsTracker
{
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

	public GaTracker(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
		init();
	}

	public override void init()
	{
		base.init();
		initialised = true;
	}

	private string SafeGetName(string metricItem)
	{
		if (metricItem == null)
		{
			return null;
		}
		return metricItem.Replace(":", " ");
	}

	public override void LogMetric(string metricName, string metricGroup = "", bool timer = false)
	{
		if (initialised)
		{
			string text = SafeGetName(metricName);
			if (text != null)
			{
				Application.ExternalEval("_gaq.push(['_trackPageview', '/" + text + "']);");
				Application.ExternalEval("_gaq.push(['_trackEvent', '" + metricGroup + "', '" + text + "']);");
			}
		}
	}

	public override void LogCustomMetric(Neuron metric)
	{
		if (!initialised)
		{
			return;
		}
		string text = SafeGetName(metric.getName());
		if (text == null)
		{
			return;
		}
		if (metric.tryGetChild("type") != null && metric.getText("type") == "pageview")
		{
			Application.ExternalEval("_gaq.push(['_trackPageview', '/" + text + "']);");
		}
		if (metric.tryGetChild("group") != null)
		{
			string text2 = metric.getText("group");
			if (tryGetNeuron(text) != null)
			{
				text = getText(text);
			}
			Application.ExternalEval("_gaq.push(['_trackEvent', '" + text2 + "', '" + text + "']);");
			return;
		}
		foreach (Neuron item in (IEnumerable<Neuron>)metric)
		{
			if (item.tryGetNeuron("type") != null && item.getText("type") == "parameter")
			{
				string text3 = item.getName();
				string text4 = metric.getText(text3);
				if (tryGetNeuron(text4) != null)
				{
					text4 = getText(text4);
				}
				Application.ExternalEval("_gaq.push(['_trackEvent', '" + text + "', '" + text3 + "', '" + text4 + "']);");
			}
		}
		Application.ExternalEval("_gaq.push(['_trackEvent', '" + text + "']);");
	}

	public override void LogParameterMetric(string metric, Dictionary<string, string> parameters)
	{
		if (!initialised)
		{
			return;
		}
		foreach (KeyValuePair<string, string> parameter in parameters)
		{
			Application.ExternalEval("_gaq.push(['_trackEvent', '" + metric + "', '" + parameter.Key + "', '" + parameter.Value + "']);");
		}
	}

	public override void LogHeatmapMetric(Neuron metricItem, long x, long y)
	{
	}

	public override void LogLevelMetric(Neuron metric, LevelMetric metricType, double value)
	{
	}
}
