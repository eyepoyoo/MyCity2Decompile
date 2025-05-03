using System;
using UnityEngine;

public class Encryption : MonoBehaviour
{
	public enum ExecutionMode
	{
		Synchronous = 0,
		Asynchronous = 1
	}

	public virtual AmuzoEncryption EncryptionType
	{
		get
		{
			return AmuzoEncryption.None;
		}
	}

	public virtual bool Decrypt(string input, string key, Action<string> onComplete, ExecutionMode mode)
	{
		onComplete(input);
		return true;
	}

	public virtual bool Encrypt(string input, string key, Action<string> onComplete, ExecutionMode mode)
	{
		onComplete(input);
		return true;
	}
}
