using System;
using System.IO;
using UnityEngine;

// Token: 0x02000009 RID: 9
public static class FileHelper
{
	// Token: 0x06000040 RID: 64 RVA: 0x00002C48 File Offset: 0x00000E48
	public static void WriteTextFile(string filePath, string fileData)
	{
		string directoryName = Path.GetDirectoryName(filePath);
		if (directoryName != null && directoryName != string.Empty)
		{
			try
			{
				Directory.CreateDirectory(directoryName);
			}
			catch (Exception)
			{
				Debug.LogWarning("Failed to create directory: " + directoryName.ToString());
			}
		}
		try
		{
			File.WriteAllText(filePath, fileData);
		}
		catch (Exception)
		{
			Debug.LogWarning("Failed to write file:" + filePath.ToString());
		}
	}
}
