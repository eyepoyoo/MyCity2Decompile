using System.Collections.Generic;
using DeepThought;

public class AnalyticsTracker : Neuron
{
	public enum LevelMetric
	{
		Event = 0,
		Counter = 1,
		Ranged = 2,
		Average = 3
	}

	protected bool initialised;

	public bool isInitialised
	{
		get
		{
			return initialised;
		}
	}

	public virtual bool Frozen { get; set; }

	public AnalyticsTracker(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
	}

	public virtual void init()
	{
		initialised = true;
	}

	public virtual void LogEvent(string eventItem)
	{
	}

	public virtual void LogMetric(string metricName, string metricGroup, bool timer)
	{
	}

	public virtual void StopTimerMetric(string metricName)
	{
	}

	public virtual void LogHeatmapMetric(Neuron metricItem, long x, long y)
	{
	}

	public virtual void LogLevelMetric(Neuron metric, LevelMetric metricType, double value)
	{
	}

	public virtual void LogCustomMetric(Neuron metric)
	{
	}

	public virtual void LogParameterMetric(string metric, Dictionary<string, string> paramaters)
	{
	}

	public virtual void LogProgress(string name)
	{
	}

	public virtual void StartSession()
	{
	}

	public virtual void EndSession()
	{
	}
}
