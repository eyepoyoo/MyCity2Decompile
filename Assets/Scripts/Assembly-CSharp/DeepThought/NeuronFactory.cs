using System;

namespace DeepThought
{
	public class NeuronFactory<T> : NeuronFactory where T : Neuron
	{
		public override Neuron create(Neuron parent, CreationParameters parameters)
		{
			Neuron neuron = null;
			if (NeuronFactoryStore.applicationFactory != null)
			{
				neuron = NeuronFactoryStore.applicationFactory(typeof(T), parent, parameters);
			}
			if (neuron == null)
			{
				throw new Exception("Type not registered with factory: " + typeof(T).Name);
			}
			return neuron;
		}
	}
	public abstract class NeuronFactory
	{
		public string name;

		public abstract Neuron create(Neuron parent, CreationParameters parameters);
	}
}
