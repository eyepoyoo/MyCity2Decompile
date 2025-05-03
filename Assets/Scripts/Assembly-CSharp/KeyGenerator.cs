using System.Collections.Generic;
using UnityEngine;

public static class KeyGenerator
{
	private const string _defaultKey = "ayzCsAEL6cx7HeJBnaEW9KtlYK1IMJq3ar639ypS";

	private const string _keyCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

	private static string[] _keys = new string[62]
	{
		"EigxqnVvMfR8DQpRVX0KIYRKlC48MhQmuLJZ4vGo", "CabMtjr9pHdPOABofhlqx53sBlaya9dAmwZp4L97", "2h3JggBoVfnQKUhr8JXGcy4a598IXsPcqqtemWPX", "QaRhhHe1nDpQknjko9Tc8UHOS53BUdiyaRmV1zrE", "nxZ4DwHW7KShfuzMZdFup1MOd9waCezv1qUQoShF", "IITPD4Ilbi6StLukSjXwazQNhJNL77VFxu82OtQq", "Ccr95faTdvmzrV3MmqAg8S7ya6epc12dU8GUKyTk", "EOmhc9Us3rLCrlDbICC9cz1zc9A5Yuukj3hLT88k", "7qQGFJKd3e3QYUDN0f4Y78UFRZlY0d9ywWoKR8Yk", "GWnBT45uktM8RYzF3TpmtRTfr01sn7HOEUDdEya3",
		"ZLtsgF0sQJTtEmWmIX0RcHonKU7zLsI0lgeJvBDW", "IxdcJb5Jy8EI1omrV8QOdCeMeYzpcPapbmqMnEyi", "J6u6gJ2b1IB2cVIf5GjnAAMCvEf0RkyKiSbmpa0P", "wzKIUCFolweYjtnDmTqn8gVMOHo6lI6cunedimRq", "xA6KzYWIA83xRDMFcNDU7kqEtRTDEKlfuBMo9KxO", "m4hReLfBZGvHbFJUE6N7hh84OBjBj341TvKDv2Sm", "GwbcX0pOL9ZuhFt6lgRAiwFkFb2F1knMv63agwbb", "gLFApiTHpoyNqG7Nhj3hGfbRbUC96cQOOl1PPS0h", "pxzXkia3ggK68ixFKHHqj8axrEiZV2SBsHVHnyvs", "6s0OiPPRZDUdSMN72EYHJNUwfHCLO0NG3LwxMhOP",
		"NuMvPZxlWjYt6WVyRir5Sr9jIvPSb91p8R3ZFe6o", "qjGLTa3WiKY1xgdfM317uN977ZfLWkwqfwkk0w0a", "fW6GwPiQZS6KPZTTq7CUNZU52okVMfY6BTTH4kLE", "1RE8LjpMjG4IKBNEQf6IkntTR3957sRbQrQ9g0Ff", "MZpoL0QAAwbIUekLds7VJ7wU3lrfFlRIIvoLfmrR", "xGUUGaCISvr0eC2RMCCHjIVUFjV8sb5V3QfDpcyV", "1IUH1gXO4U0eMycQTdFEmFv58nhxnUMGjlICXiMv", "ZTDJ1psi0KxRqLNq1RSnehCFVzidSMzYBftu9r0H", "uSEIu9xmDrMSg5ILDhlDmZmiAoFTPf8tTquHWKwe", "epBgJaib4LD0LapZ4PUSXr9Mk8zdXnJkNyzeLibH",
		"LsOCUhyOKUNEHqO57ijQx8Zst08T2HmpMvN5m45m", "oy20CSxOAwoFX8z4XzGn5dy7u5Fkd6x7oLTj6aD3", "ayzCsAEL6cx7HeJBnaEW9KtlYK1IMJq3ar639ypS", "qBeoKj6OdASOt9aXtwSDFKrEFSCmkabNEbxwltCd", "ioptDTbvhTCOZITTX17tSAIGe5beYHsW7abH95bL", "mXYXYyckiRLpvGW9CLxL598UdPoDTwoSD7lz5J9z", "MsVhcIE2WtPaDZSisGRskKAYsyhXirwpJxPh7mQN", "S0ELpeGowv8myzPI5HS1r9rvnSBjOfOvWDzKDDjK", "WLizinznyXSBFrEyVnzEfuX48veggpwzlhp1P3Tv", "j5QoXCjNNak54anD9HjXW1nDqarTBuBDuuoKKh9N",
		"mmsjXzyFIG5G3ZYSPBnLxz8RkGIl83xUcldxSN2o", "mp0kK1HgfQoCBc8kSwLKIoRcZ4vX7XncxaxcyCUa", "XT9FwHAYLTfgIa2bCqzTzCFkMSp1aun2vKqllY5P", "AHyDSew6WQSz9vsXbQiBlkQpl2AeCmu73JgxJwUC", "Oo4lFAMdUdpL8a0AMYr5sE0ejHDoORpUekoGbZSt", "HgWqhxdlj1jyJO8hmfg4oB7EvVmfXMp3Fi3CliTY", "1w6CKUkpC5TUyruMtPqAZqxHgy3aZu9boU0k4Ci7", "QYAsacQ63dSO1yqYHnhy0wLkqQxLdG80RniLuXNE", "MykL9YWzWzuDea3jYvoq8hrjPcvTuCCsqNEfVqk5", "aAcbhoCg2iOMVJ74bQ4ooLAOXbby53q5LOYo38Ri",
		"XygxYfJALuO3z1BxBe1D84OTb97EU5TVE9OfBEjt", "XtOgBCyKYnPSnvdYBOr51DHaAe2rDcD44KeIWgU7", "JpJatmwye6cAdxxX6pYvGeL762BsTUpSzXShQgH4", "SGc0nc7l01l9NBEmSH3zfAkXlSyXUt1yXokwTsP0", "oZLr8Afv8mbM6ZVbzdHiOmTvaDMokt1BuS9uoyBU", "0UuUzYGZ3DGhYHAGOj3B87SVdFVyDSFT9pK40SI9", "y8dCN6g8yxtjqWcfaHPqjhfKo4SGUM3EE6lp4Put", "1gV4S3vmz2FMWbMGLc4WiTBLX5XrN13CWgwoKJR4", "Ku9HmESLRWuzlO1M1SfOd2eVSn5pg5lY2pKXbe2U", "dTK6awlb3qqGvmK5Vpa9wLqMZhVyCuEGQvvQM8EO",
		"aJ6kfh6NEY0yb0kmx3VMDji4i4hLfJdQqdx7PuSB", "PhpZPWVS1rtljN4WdKMYzLTj9UoZPNMGyb33Mr8Q"
	};

	private static Dictionary<char, string> _keyDictionary = null;

	private static bool _initialised = false;

	private static void EnsureInitialised()
	{
		if (_initialised)
		{
			return;
		}
		int num = _keys.Length;
		if ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length != num)
		{
			Debug.LogError("Code error initialising KeyGen");
			return;
		}
		_keyDictionary = new Dictionary<char, string>(num);
		for (int i = 0; i < num; i++)
		{
			_keyDictionary.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[i], _keys[i]);
		}
		_initialised = true;
	}

	public static string Generate(string key)
	{
		if (key == null || key == string.Empty)
		{
			return string.Empty;
		}
		EnsureInitialised();
		string text = string.Empty;
		int length = key.Length;
		for (int i = 0; i < length; i++)
		{
			string value;
			if (!_keyDictionary.TryGetValue(key[i], out value))
			{
				value = "ayzCsAEL6cx7HeJBnaEW9KtlYK1IMJq3ar639ypS";
			}
			text += value;
		}
		return text;
	}
}
