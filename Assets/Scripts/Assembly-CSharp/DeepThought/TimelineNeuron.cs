using System.Collections.Generic;

namespace DeepThought
{
	public class TimelineNeuron : Neuron
	{
		public TimelineNeuron(Neuron parent, CreationParameters parameters)
			: base(parent, parameters)
		{
			merge("<Neuron><Neuron name=\"currentTime\" value=\"0\" /><Neuron name=\"startTime\" value=\"-1\" /><Neuron name=\"started\" value=\"false\" /><Neuron name=\"length\" value=\"-1\" /><Neuron name=\"type\" value=\"stop\" INSPECTOR_VALUES=\"repeat;stop\" /><Neuron name=\"timelineUnique\" value=\"someval\" /><Neuron name=\"events\" /></Neuron>");
		}

		public override bool onChange(string path)
		{
			if (path == "started.Boolean" && getChild("started").getValue().getBoolean())
			{
				started();
			}
			base.onChange(path);
			return true;
		}

		private void started()
		{
			Neuron child = getChild("events");
			for (int i = 0; i < child.numChildren; i++)
			{
				Neuron child2 = child.getChild(i).getChild("targets");
				Neuron child3 = child.getChild(i).getChild("messages");
				for (int j = 0; j < child2.numChildren; j++)
				{
					for (int k = 0; k < child3.numChildren; k++)
					{
						child2.getChild(j).getValue().getNeuron()
							.message(child3.getChild(k));
					}
				}
			}
		}

		public override void onMessage(Neuron messageNeuron, Neuron resultNeuron)
		{
			base.onMessage(messageNeuron, resultNeuron);
			if (messageNeuron.getName() == "tick")
			{
				tick(messageNeuron.getChild("deltaTime").getValue().getNumber());
			}
			else if (messageNeuron.getName() == "reset")
			{
				reset();
			}
			else if (messageNeuron.getName() == "start")
			{
				start();
			}
		}

		public void reset()
		{
			List<Neuron> list = null;
			TimelineNeuron timelineNeuron = null;
			list = new List<Neuron>();
			getChildren("Timeline", list);
			getChild("currentTime").getValue().setNumber(0f);
			getChild("started").getValue().setBoolean(false);
			for (int i = 0; i < list.Count; i++)
			{
				timelineNeuron = (TimelineNeuron)list[i];
				timelineNeuron.reset();
			}
		}

		public void start()
		{
			getChild("currentTime").getValue().setNumber(0f);
			getChild("started").getValue().setBoolean(false);
			getChild("startTime").getValue().setNumber(0f);
		}

		private void propagate(float timeDelta)
		{
			Value value = null;
			List<Neuron> list = null;
			TimelineNeuron timelineNeuron = null;
			if (getChild("startTime").getValue().getNumber() == -1f || (getChild("type").getValue().getText() == "stop" && getChild("length").getValue().getNumber() != -1f && getChild("currentTime").getValue().getNumber() >= getChild("length").getValue().getNumber()))
			{
				return;
			}
			if (getChild("type").getValue().getText() == "repeat" && getChild("length").getValue().getNumber() != -1f && getChild("currentTime").getValue().getNumber() >= getChild("length").getValue().getNumber())
			{
				reset();
				return;
			}
			value = getChild("currentTime").getValue();
			value.setNumber(value.getNumber() + timeDelta);
			if (getChild("startTime").getValue().getNumber() <= getChild("currentTime").getValue().getNumber() && !getChild("started").getValue().getBoolean())
			{
				getChild("started").getValue().setBoolean(true);
			}
			if (getChild("started").getValue().getBoolean())
			{
				list = new List<Neuron>();
				getChildren("Timeline", list);
				for (int i = 0; i < list.Count; i++)
				{
					timelineNeuron = (TimelineNeuron)list[i];
					timelineNeuron.propagate(timeDelta);
				}
			}
		}

		private void tick(float timeDelta)
		{
			if (parent.tryGetChild("timelineUnique") == null)
			{
				propagate(timeDelta);
			}
		}
	}
}
