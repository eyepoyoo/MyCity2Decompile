using DeepThought;

public sealed class DeepThoughtRoot : Neuron
{
	public delegate void EventHandler();

	private bool initialised;

	public event EventHandler OnInitialise;

	public event EventHandler OnUpdate;

	public event EventHandler OnLateUpdate;

	public DeepThoughtRoot(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
		Facades<DeepThoughtRoot>.Register(this);
	}

	public void Initialise()
	{
		if (this.OnInitialise != null)
		{
			this.OnInitialise();
		}
	}

	public void Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	public void LateUpdate()
	{
		if (this.OnLateUpdate != null)
		{
			this.OnLateUpdate();
		}
	}
}
