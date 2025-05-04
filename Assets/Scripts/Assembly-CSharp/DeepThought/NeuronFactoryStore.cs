using System;
using System.Collections.Generic;

namespace DeepThought
{
	public class NeuronFactoryStore
	{
		public delegate Neuron ApplicationFactory(Type t, Neuron parent, CreationParameters parameters);

		private static LinkedList<NeuronFactory> neuronFactories = new LinkedList<NeuronFactory>();

		public static ApplicationFactory applicationFactory = null;

		public static void add<T>(string name) where T : Neuron
		{
			NeuronFactory<T> neuronFactory = new NeuronFactory<T>();
			neuronFactory.name = name;
			neuronFactories.AddLast(neuronFactory);
		}

		public static void getNames(List<string> factoryNames)
		{
			foreach (NeuronFactory neuronFactory in neuronFactories)
			{
				factoryNames.Add(neuronFactory.name);
			}
		}

		public static NeuronFactory get(string name)
		{
			NeuronFactory neuronFactory = null;
			LinkedListNode<NeuronFactory> linkedListNode = neuronFactories.First;
			while (linkedListNode != null && linkedListNode.Value.name != name)
			{
				linkedListNode = linkedListNode.Next;
			}
			if (linkedListNode == null)
			{
				throw new Exception("Failed to obtain factory for type \"" + name + "\"");
			}
			neuronFactory = linkedListNode.Value;
			if (linkedListNode != neuronFactories.First)
			{
				LinkedListNode<NeuronFactory> previous = linkedListNode.Previous;
				neuronFactories.Remove(linkedListNode);
				neuronFactories.AddBefore(previous, linkedListNode);
			}
			return neuronFactory;
		}
	}
}
