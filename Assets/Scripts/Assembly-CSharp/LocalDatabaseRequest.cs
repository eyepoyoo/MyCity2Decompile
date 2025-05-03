using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalDatabaseRequest : MonoBehaviour
{
	public float _requestTime;

	public float _decimalFailureChance;

	public Dictionary<string, string> _metaData = new Dictionary<string, string>();

	public float _requestStartTime = -1f;

	public Action<DownloadRequest> _onFinalSuccess;

	public Action<DownloadRequest> _onFinalFail;

	private Action<LocalDatabaseRequest> _onSuccess;

	private Action<LocalDatabaseRequest> _onFail;

	private void Update()
	{
		if (!(_requestStartTime <= 0f) && !(_requestStartTime + _requestTime > Time.realtimeSinceStartup))
		{
			if (_decimalFailureChance > UnityEngine.Random.value)
			{
				failure();
			}
			else
			{
				success();
			}
		}
	}

	public void startRequest(float requestTime, Action<LocalDatabaseRequest> onSuccess, Action<LocalDatabaseRequest> onFail, float decimalFailureChance = 0f)
	{
		_requestStartTime = Time.realtimeSinceStartup;
		_requestTime = requestTime;
		_onFail = onFail;
		_onSuccess = onSuccess;
	}

	private void success()
	{
		_requestStartTime = 0f;
		if (_onSuccess != null)
		{
			_onSuccess(this);
		}
		dispose();
	}

	private void failure()
	{
		_requestStartTime = 0f;
		if (_onFail != null)
		{
			_onFail(this);
		}
		dispose();
	}

	private void dispose()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
