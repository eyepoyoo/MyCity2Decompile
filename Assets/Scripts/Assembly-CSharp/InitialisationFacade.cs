using System;
using System.Collections.Generic;
using UnityEngine;

public class InitialisationFacade : MonoBehaviour, JobManager.IOwner
{
	private const bool DO_DEBUG = false;

	private static InitialisationFacade _instance;

	private float _queueStartTime;

	private bool _hasInitialised;

	private JobManager _jobManager;

	private List<InitialisationObject> _objects;

	private Action _onFinished;

	string JobManager.IOwner._pName
	{
		get
		{
			return "InitialisationFacade";
		}
	}

	bool JobManager.IOwner._pCanStartJobs
	{
		get
		{
			return _queueStartTime > 0f;
		}
	}

	public static InitialisationFacade Instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}
			_instance = (InitialisationFacade)UnityEngine.Object.FindObjectOfType(typeof(InitialisationFacade));
			if (_instance != null)
			{
				return _instance;
			}
			Log("New facade created.");
			GameObject gameObject = new GameObject("InitialisationFacade");
			_instance = gameObject.AddComponent<InitialisationFacade>();
			return _instance;
		}
	}

	public static bool _pExists
	{
		get
		{
			return _instance != null;
		}
	}

	public bool _pHasFinished
	{
		get
		{
			return _jobManager == null || _jobManager._pHasAllJobsFinished;
		}
	}

	public event Action _pOnFinished
	{
		add
		{
			_onFinished = (Action)Delegate.Combine(_onFinished, value);
		}
		remove
		{
			_onFinished = (Action)Delegate.Remove(_onFinished, value);
		}
	}

	private void Awake()
	{
		init();
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_queueStartTime = Time.realtimeSinceStartup;
		Log("Checking for un-assigned children.");
		for (int i = 0; i < base.transform.childCount; i++)
		{
			InitialisationObject component = base.transform.GetChild(i).gameObject.GetComponent<InitialisationObject>();
			if (!(component == null))
			{
				addToQueue(component);
			}
		}
	}

	private void Update()
	{
		if (_jobManager != null)
		{
			_jobManager.Update();
		}
	}

	public void addToQueue(InitialisationObject obj)
	{
		init();
		if (obj == null)
		{
			return;
		}
		if (_objects != null && !_objects.Contains(obj))
		{
			_objects.Add(obj);
		}
		if (_jobManager == null)
		{
			return;
		}
		Log("Added new init job [" + obj.name + "]");
		_jobManager.RegisterJob(obj, false);
		if (obj._initDepends != null)
		{
			InitialisationObject[] initDepends = obj._initDepends;
			foreach (InitialisationObject initialisationObject in initDepends)
			{
				if (initialisationObject != null)
				{
					_jobManager.AddDependency(obj, initialisationObject);
				}
			}
		}
		_jobManager.EnableJob(obj);
	}

	public void removeFromQueue(InitialisationObject obj)
	{
		if (!(obj == null))
		{
			if (_jobManager != null)
			{
				_jobManager.UnregisterJob(obj);
			}
			if (_objects != null && _objects.Contains(obj))
			{
				_objects.Remove(obj);
			}
		}
	}

	private void init()
	{
		if (_hasInitialised)
		{
			return;
		}
		_hasInitialised = true;
		_instance = this;
		_jobManager = new JobManager(this);
		_jobManager._pOnJobStarting += delegate(JobManager.IJob job)
		{
			Log("Starting job '" + job._pName + "'");
		};
		_jobManager._pOnJobStarted += delegate(JobManager.IJob job)
		{
			Log("Job '" + job._pName + "' started");
		};
		_jobManager._pOnJobFinished += delegate(JobManager.IJob job)
		{
			Log("Job '" + job._pName + "' finished");
		};
		_jobManager._pOnAllJobsFinished += delegate
		{
			Log("All jobs finished. Time taken: [" + (Time.realtimeSinceStartup - _queueStartTime) + "] secs.");
			if (_onFinished != null)
			{
				_onFinished();
				_onFinished = null;
			}
		};
		_objects = new List<InitialisationObject>();
	}

	public static void Log(string message, UnityEngine.Object o = null)
	{
	}
}
