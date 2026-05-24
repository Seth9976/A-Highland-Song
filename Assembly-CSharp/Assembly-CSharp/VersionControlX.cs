using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000214 RID: 532
public static class VersionControlX
{
	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x0600137B RID: 4987 RVA: 0x00089030 File Offset: 0x00087230
	public static string gitDirectory
	{
		get
		{
			string text = Directory.GetCurrentDirectory();
			bool flag = false;
			while (!flag)
			{
				flag = Directory.Exists(Path.Combine(text, ".git"));
				if (!flag)
				{
					text = Path.GetDirectoryName(text);
					if (text == "" || text == null)
					{
						Debug.LogError("no .git folder could be found.");
						return null;
					}
				}
			}
			return Path.Combine(text, ".git");
		}
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x0008908C File Offset: 0x0008728C
	public static string GetGitBranch()
	{
		string text = Path.Combine(VersionControlX.gitDirectory, "HEAD");
		if (!File.Exists(text))
		{
			Debug.LogError("Tried to get git branch but failed to find " + text);
			return "???";
		}
		string text2 = File.ReadAllText(text).Trim();
		if (text2.StartsWith("ref: "))
		{
			int num = text2.LastIndexOf("/") + 1;
			return text2.Substring(num, text2.Length - num);
		}
		return "???";
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x00089104 File Offset: 0x00087304
	public static string GetGitSHA()
	{
		string gitDirectory = VersionControlX.gitDirectory;
		string text = Path.Combine(gitDirectory, "HEAD");
		if (!File.Exists(text))
		{
			return VersionControlX.<GetGitSHA>g__Error|3_0("failed to find " + text);
		}
		string text2 = File.ReadAllText(text).Trim();
		string text4;
		if (text2.StartsWith("ref: "))
		{
			string text3 = text2.Substring("ref: ".Length);
			text3 = Path.Combine(gitDirectory, text3);
			if (!File.Exists(text3))
			{
				return VersionControlX.<GetGitSHA>g__Error|3_0("path of ref file could not be found: " + text3);
			}
			text4 = File.ReadAllText(text3).Trim();
		}
		else
		{
			text4 = text2;
		}
		if (text4.Length < 6 || text4.Length > 42 || text4.Contains(" "))
		{
			return VersionControlX.<GetGitSHA>g__Error|3_0("got unexpected output: " + text4);
		}
		return text4.Substring(0, 6);
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x000891D6 File Offset: 0x000873D6
	[CompilerGenerated]
	internal static string <GetGitSHA>g__Error|3_0(string msg)
	{
		Debug.LogError("Tried to get git SHA to put in Version object, but " + msg);
		return null;
	}
}
