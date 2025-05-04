using System;
using UnityEngine;

public class AESEncryption : Encryption
{
	public override AmuzoEncryption EncryptionType
	{
		get
		{
			return AmuzoEncryption.AES;
		}
	}

	public override bool Decrypt(string input, string key, Action<string> onComplete, ExecutionMode mode)
	{
		Debug.LogWarning("AES Encryption Not Implemented Yet -- not decrypting");
		onComplete(input);
		return true;
	}

	public override bool Encrypt(string input, string key, Action<string> onComplete, ExecutionMode mode)
	{
		Debug.LogWarning("AES Encryption Not Implemented Yet -- not encrypting");
		onComplete(input);
		return true;
	}
}
