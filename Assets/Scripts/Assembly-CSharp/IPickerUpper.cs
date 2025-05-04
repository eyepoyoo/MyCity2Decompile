public interface IPickerUpper
{
	Pickupable _pCurrentObject { get; }

	float _pTimeSinceLastDrop { get; }

	void Drop();
}
