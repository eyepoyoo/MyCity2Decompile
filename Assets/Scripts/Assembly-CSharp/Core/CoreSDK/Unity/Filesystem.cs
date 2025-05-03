using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace LEGO.CoreSDK.Unity
{
	// Token: 0x02000059 RID: 89
	public class Filesystem : IFilesystem
	{
		// Token: 0x06000155 RID: 341 RVA: 0x000071A8 File Offset: 0x000053A8
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000071B0 File Offset: 0x000053B0
		public string FileReadAllText(string path)
		{
			return File.ReadAllText(path);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000071B8 File Offset: 0x000053B8
		public void FileWriteAllText(string path, string contents)
		{
			File.WriteAllText(path, contents);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000071C4 File Offset: 0x000053C4
		public void FileDelete(string path)
		{
			File.Delete(path);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000071CC File Offset: 0x000053CC
		public string ReadAllTextOfBundledFile(string bundledFilePath)
		{
			string text = Path.Combine(Path.GetDirectoryName(bundledFilePath), Path.GetFileNameWithoutExtension(bundledFilePath));
			TextAsset textAsset = Resources.Load(text) as TextAsset;
			return (!(textAsset != null)) ? null : textAsset.text;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007210 File Offset: 0x00005410
		public IEnumerable<TextFile> ContentsOfBundledDirectory(string directoryPath)
		{
			return from TextAsset x in from x in Resources.LoadAll(directoryPath)
					where x is TextAsset
					select x
				select new TextFile(x.name, x.text);
		}
	}
}
