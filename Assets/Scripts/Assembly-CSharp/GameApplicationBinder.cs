using System;
using DeepThought;

public class GameApplicationBinder
{
	public static void BindApplicationNeurons()
	{
		NeuronFactoryStore.applicationFactory = ApplicationFactory;
		DeepThoughtApplicationBinder.BindApplicationNeurons();
		NeuronFactoryStore.add<UserAccountFacade>("UserAccountFacade");
		NeuronFactoryStore.add<GameUserAccountFacade>("GameUserAccountFacade");
	}

	public static Neuron ApplicationFactory(Type t, Neuron parent, CreationParameters parameters)
	{
		string name = t.Name;
		switch (name)
		{
		case "Neuron":
			return new Neuron(parent, parameters);
		case "GameUserAccountFacade":
			return new GameUserAccountFacade(parent, parameters);
		case "DeepThoughtRoot":
			return new DeepThoughtRoot(parent, parameters);
		case "AnalyticsTracker":
			return new AnalyticsTracker(parent, parameters);
		case "DebugTracker":
			return new DebugTracker(parent, parameters);
		case "FlurryTracker":
			return new FlurryTracker(parent, parameters);
		case "GaTracker":
			return new GaTracker(parent, parameters);
		case "TrackManTracker":
			return new TrackManTracker(parent, parameters);
		default:
			Console.WriteLine("Warning: Could not find specific creator for type " + name);
			return null;
		}
	}
}
