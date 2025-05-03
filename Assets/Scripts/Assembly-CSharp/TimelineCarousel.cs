using System.Collections.Generic;
using DeepThought;

public class TimelineCarousel : Neuron
{
	private List<Neuron> _timelines = new List<Neuron>();

	private int _currentTimeline;

	public TimelineCarousel(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
		_currentTimeline = 0;
		getChildren("Timeline", _timelines);
	}

	public override void onMessage(Neuron messageItem, Neuron resultItem)
	{
		base.onMessage(messageItem, resultItem);
		switch (messageItem.getName())
		{
		case "reset":
			_currentTimeline = 0;
			ExecuteCurrentTimeline();
			_currentTimeline++;
			_currentTimeline %= _timelines.Count;
			break;
		case "skipToEnd":
			_currentTimeline = _timelines.Count - 1;
			ExecuteCurrentTimeline();
			break;
		case "next":
			ExecuteCurrentTimeline();
			_currentTimeline++;
			_currentTimeline %= _timelines.Count;
			break;
		case "prev":
			_currentTimeline += _timelines.Count - 1;
			_currentTimeline %= _timelines.Count;
			ExecuteCurrentTimeline();
			break;
		case "activate":
			if (getBoolean("resetOnActive"))
			{
				_currentTimeline = 0;
				ExecuteCurrentTimeline();
				_currentTimeline++;
				_currentTimeline %= _timelines.Count;
			}
			break;
		}
	}

	private void ExecuteCurrentTimeline()
	{
		TimelineNeuron timelineNeuron = _timelines[_currentTimeline] as TimelineNeuron;
		timelineNeuron.reset();
		timelineNeuron.start();
	}
}
