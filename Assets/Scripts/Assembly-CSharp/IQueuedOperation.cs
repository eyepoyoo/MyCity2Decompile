public interface IQueuedOperation
{
	EQueuedOperationStage operationStage { get; set; }

	void StartOperation();
}
