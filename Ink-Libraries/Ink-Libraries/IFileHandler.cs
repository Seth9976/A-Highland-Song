using System;

namespace Ink
{
	// Token: 0x02000006 RID: 6
	public interface IFileHandler
	{
		// Token: 0x06000017 RID: 23
		string ResolveInkFilename(string includeName);

		// Token: 0x06000018 RID: 24
		string LoadInkFileContents(string fullFilename);
	}
}
