using DeepThought;

public class DeepThoughtApplicationBinder
{
	public static void BindApplicationNeurons()
	{
		NeuronFactoryStore.add<Neuron>("node");
		NeuronFactoryStore.add<Neuron>("Neuron");
		NeuronFactoryStore.add<DeepThoughtRoot>("root");
	}
}
