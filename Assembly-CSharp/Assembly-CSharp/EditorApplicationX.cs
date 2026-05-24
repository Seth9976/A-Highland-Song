using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001F9 RID: 505
public static class EditorApplicationX
{
	// Token: 0x060012B1 RID: 4785 RVA: 0x00086008 File Offset: 0x00084208
	public static bool IsRetina()
	{
		if (EditorApplicationX._isRetina == null)
		{
			EditorApplicationX._isRetina = new bool?(Application.platform == RuntimePlatform.OSXEditor && (double)float.Parse(Application.unityVersion.Substring(0, 3)) >= 5.4);
		}
		return EditorApplicationX._isRetina.Value;
	}

	// Token: 0x060012B2 RID: 4786 RVA: 0x0008605F File Offset: 0x0008425F
	public static string SanitizePathString(string path)
	{
		if (path == null)
		{
			return null;
		}
		return path.Replace('\\', '/');
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x00086070 File Offset: 0x00084270
	public static string CombinePaths(string firstPath, string secondPath)
	{
		return EditorApplicationX.SanitizePathString(Path.Combine(firstPath, secondPath));
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x0008607E File Offset: 0x0008427E
	public static string AbsoluteToUnityRelativePath(string absolutePath)
	{
		return EditorApplicationX.SanitizePathString(absolutePath.Substring(Application.dataPath.Length - 6));
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x00086097 File Offset: 0x00084297
	public static string UnityRelativeToAbsolutePath(string localPath)
	{
		if (localPath.Length < 7)
		{
			return Application.dataPath;
		}
		return EditorApplicationX.CombinePaths(Application.dataPath, localPath.Substring(7));
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x000860B9 File Offset: 0x000842B9
	public static string AbsoluteToProjectPath(string absolutePath)
	{
		return EditorApplicationX.SanitizePathString(absolutePath.Substring(Application.dataPath.Length + 1));
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x000860D2 File Offset: 0x000842D2
	public static string ProjectToAbsolutePath(string localPath)
	{
		return EditorApplicationX.CombinePaths(Application.dataPath, localPath);
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x000860E0 File Offset: 0x000842E0
	public static string AbsoluteToResourcesPath(string absolutePath)
	{
		if (!absolutePath.Contains("Resources/"))
		{
			return "";
		}
		string text = EditorApplicationX.<AbsoluteToResourcesPath>g__After|8_0(absolutePath, "Resources/");
		return Path.Combine(Path.GetDirectoryName(text), Path.GetFileNameWithoutExtension(text));
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0008611F File Offset: 0x0008431F
	public static string AbsoluteToPersistentDataPath(string absolutePath)
	{
		return absolutePath.Substring(Application.persistentDataPath.Length + 1);
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x00086133 File Offset: 0x00084333
	public static string PersistentDataPathToAbsolutePath(string localPath)
	{
		return Path.Combine(Application.persistentDataPath, localPath);
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x00086140 File Offset: 0x00084340
	public static bool IsAbsolutePath(string filePath)
	{
		return Path.IsPathRooted(filePath) && !Path.GetPathRoot(filePath).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x00086165 File Offset: 0x00084365
	[CompilerGenerated]
	internal static string <AbsoluteToResourcesPath>g__After|8_0(string value, string a)
	{
		return value.Substring(value.LastIndexOf(a) + a.Length);
	}

	// Token: 0x0400129F RID: 4767
	private static bool? _isRetina;
}
