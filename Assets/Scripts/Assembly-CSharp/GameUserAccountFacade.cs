using DeepThought;

public class GameUserAccountFacade : Neuron
{
	public delegate void ReturnFunction();

	private ReturnFunction _screenCallbackSucess;

	private ReturnFunction _screenCallbackFail;

	private OnSaveComplete _saveCompleteCallback;

	public GameUserAccountFacade(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
		Facades<GameUserAccountFacade>.Register(this);
	}
}
