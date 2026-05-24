using System;
using System.Text.RegularExpressions;

// Token: 0x02000097 RID: 151
[Serializable]
public class DebugLogInstruction : BaseContentInstruction
{
	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00028015 File Offset: 0x00026215
	private static Regex parserRegex
	{
		get
		{
			if (DebugLogInstruction._parserRegex == null)
			{
				DebugLogInstruction._parserRegex = new Regex("\\s*>{2,}\\s*(?:LOG)\\s(.*)", RegexOptions.IgnoreCase);
			}
			return DebugLogInstruction._parserRegex;
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00028034 File Offset: 0x00026234
	public static DebugLogInstruction TryParse(string rawContent)
	{
		Match match = DebugLogInstruction.parserRegex.Match(rawContent);
		if (!match.Success)
		{
			return null;
		}
		return new DebugLogInstruction
		{
			log = match.Groups[1].Value
		};
	}

	// Token: 0x040005CD RID: 1485
	private static Regex _parserRegex;

	// Token: 0x040005CE RID: 1486
	public string log;
}
