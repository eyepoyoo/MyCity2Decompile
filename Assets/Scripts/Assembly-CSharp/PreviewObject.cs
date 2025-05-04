using System;
using UnityEngine;

[Serializable]
public class PreviewObject
{
	public string name;

	public UnityEngine.Object previewObject;

	public string downloadedText = string.Empty;

	public PreviewType type;

	public bool isPopulated()
	{
		return previewObject != null || !string.IsNullOrEmpty(downloadedText);
	}
}
