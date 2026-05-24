using System;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public static class SystemX
{
	// Token: 0x060012A4 RID: 4772 RVA: 0x00085899 File Offset: 0x00083A99
	public static bool OpenInFileBrowser(string path)
	{
		if (SystemInfoX.IsWinOS)
		{
			return SystemX.OpenInWinFileBrowser(path);
		}
		if (SystemInfoX.IsMacOS)
		{
			return SystemX.OpenInMacFileBrowser(path);
		}
		Debug.LogError("Could not open in file browser because OS is unrecognized. OS is " + SystemInfo.operatingSystem);
		return false;
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x000858CC File Offset: 0x00083ACC
	private static bool OpenInMacFileBrowser(string path)
	{
		bool flag = false;
		string text = path.Replace("\\", "/");
		if (!text.StartsWith("\""))
		{
			text = "\"" + text;
		}
		if (!text.EndsWith("\""))
		{
			text += "\"";
		}
		string text2 = (flag ? "" : "-R ") + text;
		try
		{
			Process.Start("open", text2);
		}
		catch (Win32Exception ex)
		{
			ex.HelpLink = "";
			return false;
		}
		return true;
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x00085964 File Offset: 0x00083B64
	private static bool OpenInWinFileBrowser(string path)
	{
		bool flag = false;
		string text = path.Replace("/", "\\");
		try
		{
			Process.Start("explorer.exe", (flag ? "" : "/select, \"") + text + "\"");
		}
		catch (Win32Exception ex)
		{
			ex.HelpLink = "";
			return false;
		}
		return true;
	}
}
