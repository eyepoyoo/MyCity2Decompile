using UnityEngine;

public class InitialisationObject : MonoBehaviour, JobManager.IJob
{
	public InitialisationObject[] _initDepends;

	protected InitialisationState _currentState;

	private float _finishedTime;

	string JobManager.IJob._pName
	{
		get
		{
			return base.name;
		}
	}

	bool JobManager.IJob._pIsFinished
	{
		get
		{
			return _currentState == InitialisationState.FINISHED;
		}
	}

	public float _pFinishedTime
	{
		get
		{
			return _finishedTime;
		}
		set
		{
			_finishedTime = value;
		}
	}

	bool JobManager.IJob.Start()
	{
		startInitialising();
		return _currentState != InitialisationState.WAITING_TO_START;
	}

	void JobManager.IJob.Update()
	{
		_currentState = updateInitialisation();
	}

	protected virtual void Awake()
	{
		InitialisationFacade.Instance.addToQueue(this);
		_currentState = InitialisationState.WAITING_TO_START;
	}

	protected virtual void OnDestroy()
	{
		_currentState = InitialisationState.FINISHED;
		if (InitialisationFacade._pExists)
		{
			InitialisationFacade.Instance.removeFromQueue(this);
		}
	}

	public virtual void startInitialising()
	{
	}

	public virtual InitialisationState updateInitialisation()
	{
		return _currentState;
	}
}
