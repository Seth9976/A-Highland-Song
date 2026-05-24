using System;
using System.IO;

// Token: 0x020001F6 RID: 502
public static class PathX
{
	// Token: 0x0600129F RID: 4767 RVA: 0x00085758 File Offset: 0x00083958
	public static string GetFullPathWithNewFileName(string fullPath, string newFileName)
	{
		string extension = Path.GetExtension(fullPath);
		return Path.Combine(Path.GetDirectoryName(fullPath), newFileName) + extension;
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x0008577E File Offset: 0x0008397E
	public static string GetFullPathWithoutExtension(string path)
	{
		return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x00085791 File Offset: 0x00083991
	public static bool PathIsDirectory(string absolutePath)
	{
		return (File.GetAttributes(absolutePath) & FileAttributes.Directory) == FileAttributes.Directory;
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x000857A4 File Offset: 0x000839A4
	public static bool Compare(string pathA, string pathB)
	{
		string fullPath = Path.GetFullPath(pathA);
		string fullPath2 = Path.GetFullPath(pathB);
		return fullPath == fullPath2;
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x000857C4 File Offset: 0x000839C4
	public static string ReplaceIllegalCharacters(string toCleanPath, string replaceWith = "_")
	{
		string[] array = toCleanPath.Split(new char[] { '\\' });
		string text = array[array.Length - 1];
		string text2 = toCleanPath.Substring(0, toCleanPath.Length - text.Length);
		foreach (char c in Path.GetInvalidPathChars())
		{
			text2 = text2.Replace(c.ToString(), replaceWith);
		}
		foreach (char c2 in Path.GetInvalidFileNameChars())
		{
			text = text.Replace(c2.ToString(), replaceWith);
		}
		if (!string.IsNullOrWhiteSpace(replaceWith))
		{
			text2 = text2.Replace(replaceWith.ToString() + replaceWith.ToString(), replaceWith.ToString());
			text = text.Replace(replaceWith.ToString() + replaceWith.ToString(), replaceWith.ToString());
		}
		return text2 + text;
	}
}
