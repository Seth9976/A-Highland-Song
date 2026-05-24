using System;
using System.IO;

// Token: 0x020001F5 RID: 501
public static class DirectoryX
{
	// Token: 0x0600129D RID: 4765 RVA: 0x000856A0 File Offset: 0x000838A0
	public static void DeleteAllContents(this DirectoryInfo directoryInfo, bool alsoDeleteFolder = true)
	{
		if (!directoryInfo.Exists)
		{
			return;
		}
		FileInfo[] files = directoryInfo.GetFiles();
		for (int i = 0; i < files.Length; i++)
		{
			files[i].Delete();
		}
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		for (int i = 0; i < directories.Length; i++)
		{
			directories[i].Delete(true);
		}
		if (alsoDeleteFolder)
		{
			directoryInfo.Delete(true);
		}
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x000856FC File Offset: 0x000838FC
	public static string GetRelativePath(string filespec, string folder)
	{
		Uri uri = new Uri(filespec);
		if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
		{
			folder += Path.DirectorySeparatorChar.ToString();
		}
		return Uri.UnescapeDataString(new Uri(folder).MakeRelativeUri(uri).ToString().Replace('/', Path.DirectorySeparatorChar));
	}
}
