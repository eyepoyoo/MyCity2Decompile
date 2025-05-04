using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class XOREncryption : Encryption
{
	private delegate void WorkMethod();

	private const string _b64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

	private bool _guard;

	private Action<string> _onComplete;

	private string _input;

	private string _key;

	private string _output;

	private Thread _thread;

	private static Dictionary<char, int> _b64Lookup;

	public override AmuzoEncryption EncryptionType
	{
		get
		{
			return AmuzoEncryption.XOR;
		}
	}

	private Dictionary<char, int> B64Lookup
	{
		get
		{
			if (_b64Lookup == null)
			{
				_b64Lookup = new Dictionary<char, int>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".Length + 1);
				for (int i = 0; i < "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".Length; i++)
				{
					_b64Lookup.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[i], i);
				}
				_b64Lookup.Add('=', 0);
			}
			return _b64Lookup;
		}
	}

	public override bool Decrypt(string input, string key, Action<string> onComplete, ExecutionMode mode = ExecutionMode.Asynchronous)
	{
		if (_guard)
		{
			return false;
		}
		_onComplete = onComplete;
		if (_onComplete == null)
		{
			return false;
		}
		_guard = true;
		Call(input, key, mode, DecodeBase64);
		return true;
	}

	public override bool Encrypt(string input, string key, Action<string> onComplete, ExecutionMode mode = ExecutionMode.Asynchronous)
	{
		if (_guard)
		{
			return false;
		}
		_onComplete = onComplete;
		if (_onComplete == null)
		{
			return false;
		}
		_guard = true;
		Call(input, key, mode, EncodeBase64);
		return true;
	}

	private void Call(string input, string key, ExecutionMode mode, WorkMethod method)
	{
		_input = input;
		_key = KeyGenerator.Generate(key);
		Debug.Log("The password key we are using for encrypt/decrypt, based on key [" + key + "] is: " + _key);
		_output = string.Empty;
		switch (mode)
		{
		case ExecutionMode.Asynchronous:
			_thread = new Thread(method.Invoke);
			_thread.Start();
			break;
		case ExecutionMode.Synchronous:
			method();
			EndJob();
			break;
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		if (_thread != null && !_thread.IsAlive)
		{
			_thread = null;
			EndJob();
		}
	}

	private void EndJob()
	{
		_guard = false;
		_onComplete(_output);
	}

	private void DecodeBase64()
	{
		_output = string.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		while (num7 < _input.Length)
		{
			string text = null;
			num3 = B64Lookup[_input[num7]];
			num4 = B64Lookup[_input[num7 + 1]];
			num5 = B64Lookup[_input[num7 + 2]];
			num6 = B64Lookup[_input[num7 + 3]];
			num2 = ((num3 << 2) & 0xFF) | (num4 >> 4);
			text += Convert.ToChar(num2 ^ _key[num % _key.Length]);
			num++;
			if (_input[num7 + 2] != '=')
			{
				num2 = ((num4 << 4) & 0xFF) | (num5 >> 2);
				text += Convert.ToChar(num2 ^ _key[num % _key.Length]);
				num++;
			}
			if (_input[num7 + 3] != '=')
			{
				num2 = ((num5 << 6) & 0xFF) | num6;
				text += Convert.ToChar(num2 ^ _key[num % _key.Length]);
				num++;
			}
			num7 += 4;
			_output += text;
		}
		_output = UTF8Decode(_output);
	}

	private void EncodeBase64()
	{
		_output = string.Empty;
		if (_input == null)
		{
			return;
		}
		int length = _input.Length;
		int num = 0;
		int num2 = 0;
		while (num2 < length)
		{
			string text = null;
			num = (_input[num2] ^ _key[num2 % _key.Length]) >> 2;
			text += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[num];
			if (length - num2 == 1)
			{
				num = ((_input[num2] ^ _key[num2 % _key.Length]) << 4) & 0x30;
				text = text + "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[num] + "==";
				num2++;
			}
			else if (length - num2 == 2)
			{
				num = (((_input[num2] ^ _key[num2 % _key.Length]) << 4) & 0x30) | ((_input[num2 + 1] ^ _key[(num2 + 1) % _key.Length]) >> 4);
				text += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[num];
				num = _input[num2 + 1] ^ _key[(num2 + 1) % _key.Length];
				num = (num << 2) & 0x3C;
				text = text + "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[num] + "=";
				num2 += 2;
			}
			else
			{
				num = (((_input[num2] ^ _key[num2 % _key.Length]) << 4) & 0x30) | ((_input[num2 + 1] ^ _key[(num2 + 1) % _key.Length]) >> 4);
				text += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[num];
				num = (((_input[num2 + 1] ^ _key[(num2 + 1) % _key.Length]) << 2) & 0x3C) | ((_input[num2 + 2] ^ _key[(num2 + 2) % _key.Length]) >> 6);
				text += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[num];
				num = (_input[num2 + 2] ^ _key[(num2 + 2) % _key.Length]) & 0x3F;
				text += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[num];
				num2 += 3;
			}
			_output += text;
		}
	}

	private string UTF8Decode(string Utftext)
	{
		string text = string.Empty;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		while (num < Utftext.Length)
		{
			num2 = Utftext[num];
			string text2 = null;
			if (num2 < 128)
			{
				text2 = char.ConvertFromUtf32(num2);
				num++;
			}
			else if (num2 > 193 && num2 <= 223)
			{
				num3 = Utftext[num + 1];
				num2 = (num2 & 0x1F) << 6;
				num3 &= 0x3F;
				text2 = char.ConvertFromUtf32(num2 | num3);
				num += 2;
			}
			else if (num2 >= 224 && num2 <= 239)
			{
				num3 = Utftext[num + 1];
				num4 = Utftext[num + 2];
				num2 = (num2 & 0xF) << 12;
				num3 = (num3 & 0x3F) << 6;
				num4 &= 0x3F;
				text2 = char.ConvertFromUtf32(num2 | num3 | num4);
				num += 3;
			}
			else if (num2 >= 240 && num2 <= 244)
			{
				num3 = Utftext[num + 1];
				num4 = Utftext[num + 2];
				num5 = Utftext[num + 3];
				num2 = (num2 & 7) << 18;
				num3 = (num3 & 0x3F) << 12;
				num4 = (num4 & 0x3F) << 6;
				num5 &= 0x3F;
				text2 = char.ConvertFromUtf32(num2 | num3 | num4 | num5);
				num += 4;
			}
			text += text2;
		}
		return text;
	}
}
