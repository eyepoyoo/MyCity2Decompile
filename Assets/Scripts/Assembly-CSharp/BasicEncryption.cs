using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using UnityEngine;

[Obfuscation(Exclude = true, ApplyToMembers = true)]
public class BasicEncryption : MonoBehaviour
{
	public delegate void ReturnFunction(string returnString);

	private static string key = "emjrwtbadozhykng";

	private static string bases = "0123456789ABCDEF";

	private ReturnFunction returnOnEncryptionComplete;

	private ReturnFunction returnOnDecryptionComplete;

	private static BasicEncryption instance;

	public static int CHARACTER_THRESHOLD = 200;

	public static BasicEncryption Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject = new GameObject("BasicEncryption");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				instance = gameObject.AddComponent<BasicEncryption>();
			}
			return instance;
		}
	}

	public string encryptImmediate(string input)
	{
		string text = string.Empty;
		for (int i = 0; i < input.Length; i++)
		{
			text += GetEncryptedHexString(input, i);
		}
		System.Random random = new System.Random();
		string text2 = string.Empty;
		int num = 0;
		for (int j = 0; j < text.Length; j++)
		{
			num++;
			if (num++ > 3)
			{
				text2 += random.Next(16).ToString("X");
				text2 += text[j];
				num = 0;
			}
			else
			{
				text2 += text[j];
			}
		}
		string text3 = string.Empty;
		for (int k = 0; k < text2.Length; k++)
		{
			text3 += text2[text2.Length - k - 1];
		}
		string text4 = string.Empty;
		for (int l = 0; l < text3.Length; l++)
		{
			for (int m = 0; m < bases.Length; m++)
			{
				if (bases[m] == text3[l])
				{
					text4 += key[m];
				}
			}
		}
		return text4;
	}

	private static IEnumerator Encrypt(string input)
	{
		string eS = string.Empty;
		for (int idx = 0; idx < input.Length; idx++)
		{
			eS += GetEncryptedHexString(input, idx);
			if (idx % CHARACTER_THRESHOLD == 0)
			{
				yield return new WaitForSeconds(0f);
			}
		}
		System.Random random = new System.Random();
		string eS2 = string.Empty;
		int c = 0;
		for (int i = 0; i < eS.Length; i++)
		{
			c++;
			if (c++ > 3)
			{
				eS2 += random.Next(16).ToString("X");
				eS2 += eS[i];
				c = 0;
			}
			else
			{
				eS2 += eS[i];
			}
			if (i % CHARACTER_THRESHOLD == 0)
			{
				yield return new WaitForSeconds(0f);
			}
		}
		string eS3 = string.Empty;
		for (int j = 0; j < eS2.Length; j++)
		{
			eS3 += eS2[eS2.Length - j - 1];
			if (j % CHARACTER_THRESHOLD == 0)
			{
				yield return new WaitForSeconds(0f);
			}
		}
		string eS4 = string.Empty;
		for (int k = 0; k < eS3.Length; k++)
		{
			for (int y = 0; y < bases.Length; y++)
			{
				if (bases[y] == eS3[k])
				{
					eS4 += key[y];
				}
			}
			if (k % CHARACTER_THRESHOLD == 0)
			{
				yield return new WaitForSeconds(0f);
			}
		}
		Instance.encrpytionComplete(eS4);
	}

	private void encrpytionComplete(string returnString)
	{
		if (returnOnEncryptionComplete != null)
		{
			returnOnEncryptionComplete(returnString);
		}
	}

	private static IEnumerator Decrypt(string input)
	{
		string eS4 = string.Empty;
		for (int idx = 0; idx < input.Length; idx++)
		{
			for (int y = 0; y < key.Length; y++)
			{
				if (key[y] == input[idx])
				{
					eS4 += bases[y];
				}
			}
			if (idx % CHARACTER_THRESHOLD == 0)
			{
				yield return new WaitForSeconds(0f);
			}
		}
		string eS5 = string.Empty;
		for (int i = 0; i < eS4.Length; i++)
		{
			eS5 += eS4[eS4.Length - i - 1];
			if (i % CHARACTER_THRESHOLD == 0)
			{
				yield return new WaitForSeconds(0f);
			}
		}
		string eS6 = string.Empty;
		int c = 1;
		for (int j = 0; j < eS5.Length; j++)
		{
			c++;
			if (c > 3)
			{
				c = 0;
			}
			else
			{
				eS6 += eS5[j];
			}
			if (j % CHARACTER_THRESHOLD == 0)
			{
				yield return new WaitForSeconds(0f);
			}
		}
		string eS7 = string.Empty;
		string batch = string.Empty;
		for (int k = 0; k < eS6.Length; k += 4)
		{
			batch = eS6.Substring(k, 4);
			eS7 = GetDecryptedHexString(batch, k) + eS7;
			if (k % CHARACTER_THRESHOLD == 0)
			{
				yield return new WaitForSeconds(0f);
			}
		}
		Instance.decrpytionComplete(eS7);
	}

	private void decrpytionComplete(string returnString)
	{
		if (returnOnDecryptionComplete != null)
		{
			returnOnDecryptionComplete(returnString);
		}
	}

	private static string GetEncryptedHexString(string input, int idx)
	{
		return (input[input.Length - idx - 1] + idx + 476).ToString("X4");
	}

	private static string GetDecryptedHexString(string input, int idx)
	{
		return string.Empty + (char)(int.Parse(input, NumberStyles.HexNumber) - idx / 4 - 476);
	}

	public void encrypt(string input, ReturnFunction returnOnEncryptionComplete)
	{
		this.returnOnEncryptionComplete = returnOnEncryptionComplete;
		StartCoroutine(Encrypt(input));
	}

	public void decrypt(string input, ReturnFunction returnOnDecryptionComplete)
	{
		this.returnOnDecryptionComplete = returnOnDecryptionComplete;
		StartCoroutine(Decrypt(input));
	}
}
