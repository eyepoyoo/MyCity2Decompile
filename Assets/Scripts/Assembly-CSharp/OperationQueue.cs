using System.Collections.Generic;
using UnityEngine;

public class OperationQueue : MonoBehaviour
{
	public GameObject[] _operations;

	private List<IQueuedOperation> _operationComponents = new List<IQueuedOperation>();

	private int _operationIndex;

	public int _pOperationQueueLength
	{
		get
		{
			return _operationComponents.Count;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		GameObject[] operations = _operations;
		foreach (GameObject gameObject in operations)
		{
			Component[] components = gameObject.GetComponents<Component>();
			foreach (Component component in components)
			{
				if (component is IQueuedOperation)
				{
					_operationComponents.Add((IQueuedOperation)component);
				}
			}
		}
	}

	private void Update()
	{
		if (_operationIndex == _operationComponents.Count - 1)
		{
			Log("...all operations complete.");
			return;
		}
		if (_operationComponents[_operationIndex] == null)
		{
			_operationComponents.RemoveAt(0);
		}
		switch (_operationComponents[0].operationStage)
		{
		case EQueuedOperationStage.NOT_STARTED:
			Log(string.Format("starting operation [{0}/{1}] \"{2}\"...", _operationIndex, _operationComponents.Count, ((Component)_operationComponents[_operationIndex]).name));
			_operationComponents[0].operationStage = EQueuedOperationStage.IN_PROGRESS;
			_operationComponents[0].StartOperation();
			break;
		case EQueuedOperationStage.IN_PROGRESS:
			break;
		case EQueuedOperationStage.COMPLETE:
			Log(string.Format("...operation complete [{0}/{1}] \"{2}\"", _operationIndex, _operationComponents.Count, ((Component)_operationComponents[_operationIndex]).name));
			_operationIndex++;
			break;
		}
	}

	public void AddOperation(IQueuedOperation operation)
	{
		_operationComponents.Add(operation);
	}

	private void Log(string message)
	{
		Debug.Log("<b>OPERATION QUEUE:</b> " + message);
	}
}
