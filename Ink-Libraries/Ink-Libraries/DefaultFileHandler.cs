using System;
using System.IO;

namespace Ink
{
	// Token: 0x02000007 RID: 7
	public class DefaultFileHandler : IFileHandler
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002637 File Offset: 0x00000837
		public string ResolveInkFilename(string includeName)
		{
			return Path.Combine(Directory.GetCurrentDirectory(), includeName);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002644 File Offset: 0x00000844
		public string LoadInkFileContents(string fullFilename)
		{
			return File.ReadAllText(fullFilename);
		}
	}
}
