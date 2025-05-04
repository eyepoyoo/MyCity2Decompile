using System;
using UnityEngine;

public class FormDataEncryption
{
	private DatabaseRequest request;

	private Action<DownloadRequest> _onComplete;

	private int _changeIndex;

	public FormDataEncryption(DatabaseRequest request, Action<DownloadRequest> onComplete)
	{
		this.request = request;
		_onComplete = onComplete;
		modifyKey(null);
	}

	public void modifyKey(string modifiedValue)
	{
		if (!string.IsNullOrEmpty(modifiedValue))
		{
			request.formData[_changeIndex].value = modifiedValue;
			_changeIndex++;
			while (_changeIndex < request.formData.Count && !request.formData[_changeIndex].doEncrypt)
			{
				_changeIndex++;
			}
			if (_changeIndex == request.formData.Count)
			{
				_onComplete(request);
				return;
			}
		}
		if (request.formData[_changeIndex].key.Length > BasicEncryption.CHARACTER_THRESHOLD)
		{
			BasicEncryption.Instance.encrypt(request.formData[_changeIndex].key, modifyValue);
			return;
		}
		string modifiedKey = BasicEncryption.Instance.encryptImmediate(request.formData[_changeIndex].key);
		modifyValue(modifiedKey);
	}

	public void modifyValue(string modifiedKey)
	{
		if (string.IsNullOrEmpty(modifiedKey))
		{
			Debug.LogError("Uh oh. Encryption failed!");
			return;
		}
		request.formData[_changeIndex].key = modifiedKey;
		if (request.formData[_changeIndex].value.Length > BasicEncryption.CHARACTER_THRESHOLD)
		{
			BasicEncryption.Instance.encrypt(request.formData[_changeIndex].value, modifyKey);
			return;
		}
		string modifiedValue = BasicEncryption.Instance.encryptImmediate(request.formData[_changeIndex].value);
		modifyKey(modifiedValue);
	}
}
