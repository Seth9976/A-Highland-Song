using System;
using System.Globalization;
using System.Text.RegularExpressions;

// Token: 0x02000099 RID: 153
public class PauseInstruction : BaseContentInstruction
{
	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060004FB RID: 1275 RVA: 0x0002814D File Offset: 0x0002634D
	private static Regex parserRegex
	{
		get
		{
			if (PauseInstruction._parserRegex == null)
			{
				PauseInstruction._parserRegex = new Regex(InkInstructionParserUtility.BuildInstructionPrefixRegex("PAUSE"), RegexOptions.IgnoreCase);
			}
			return PauseInstruction._parserRegex;
		}
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00028170 File Offset: 0x00026370
	public static PauseInstruction TryParse(string rawContent)
	{
		Match match = PauseInstruction.parserRegex.Match(rawContent);
		if (!match.Success)
		{
			return null;
		}
		PauseInstruction pauseInstruction = new PauseInstruction();
		if (!float.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out pauseInstruction.pauseLength))
		{
			return null;
		}
		return pauseInstruction;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x000281C4 File Offset: 0x000263C4
	public override string ToString()
	{
		return string.Format("[" + base.GetType().Name + "] pauseLength {0}", this.pauseLength);
	}

	// Token: 0x040005D3 RID: 1491
	private static Regex _parserRegex;

	// Token: 0x040005D4 RID: 1492
	public float pauseLength;
}
