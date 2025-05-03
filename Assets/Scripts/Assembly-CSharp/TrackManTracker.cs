using System.Collections.Generic;
using DeepThought;
using UnityEngine;

public class TrackManTracker : AnalyticsTracker
{
	private List<string> _allEvents = new List<string>();

	public TrackManTracker(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
	}

	public override void init()
	{
		Application.ExternalEval("if ( typeof gameTracking != 'undefined' ) gameTracking.Progress.startGame('" + Facades<TrackingFacade>.Instance._pGameId + "');");
		_allEvents = Facades<TrackingFacade>.Instance.GetEvents();
		if (_allEvents != null)
		{
			int count = _allEvents.Count;
			for (int i = 0; i < count; i++)
			{
				Application.ExternalEval("if ( typeof gameTracking != 'undefined' ) gameTracking.CustomEvents.createReportingEvent('" + _allEvents[i] + "');");
			}
		}
		initialised = true;
	}

	public override void LogEvent(string eventItem)
	{
		if (initialised)
		{
			PersonalLogs.KayLog("<color=red>Log Event: " + eventItem + "</color>");
			Application.ExternalEval("if ( typeof gameTracking != 'undefined' ) gameTracking.CustomEvents.raise('" + eventItem + "', 1);");
		}
	}

	public override void LogProgress(string name)
	{
		if (initialised)
		{
			PersonalLogs.KayLog("<color=red>Log Progress: " + name + "</color>");
			Application.ExternalEval("if ( typeof gameTracking != 'undefined' ) gameTracking.Progress.registerGameProgress('" + name + "');");
		}
	}

	public void startLevel(string level)
	{
		Application.ExternalEval("if ( typeof gameTracking != 'undefined' ) gameTracking.Progress.registerGameProgress('" + level + "-start', 1);");
	}

	public void endLevel(string level)
	{
		Application.ExternalEval("if ( typeof gameTracking != 'undefined' ) gameTracking.Progress.registerGameProgress('" + level + "-end', 1);");
	}

	public override void EndSession()
	{
		if (initialised)
		{
			Application.ExternalEval("if ( typeof gameTracking != 'undefined' ) gameTracking.Progress.endGame();");
		}
	}
}
