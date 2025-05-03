using System;
using System.Collections.Generic;

namespace LEGO.CoreSDK
{
	// Token: 0x02000058 RID: 88
	public interface IFilesystem
	{
		// Token: 0x0600014E RID: 334
		bool FileExists(string path);

		// Token: 0x0600014F RID: 335
		string FileReadAllText(string path);

		// Token: 0x06000150 RID: 336
		void FileWriteAllText(string path, string contents);

		// Token: 0x06000151 RID: 337
		void FileDelete(string path);

		// Token: 0x06000152 RID: 338
		string ReadAllTextOfBundledFile(string bundledFilePath);

		// Token: 0x06000153 RID: 339
		IEnumerable<TextFile> ContentsOfBundledDirectory(string directoryPath);
	}
}
