using System;
using System.Text.RegularExpressions;

// Token: 0x02000093 RID: 147
[Serializable]
public class AudioInstruction : BaseContentInstruction
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060004EA RID: 1258 RVA: 0x00027E64 File Offset: 0x00026064
	private static Regex parserRegex
	{
		get
		{
			if (AudioInstruction._parserRegex == null)
			{
				AudioInstruction._parserRegex = new Regex(InkInstructionParserUtility.BuildInstructionPrefixRegex("AUDIO"), RegexOptions.IgnoreCase);
			}
			return AudioInstruction._parserRegex;
		}
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00027E88 File Offset: 0x00026088
	public static AudioInstruction TryParse(string rawContent)
	{
		Match match = AudioInstruction.parserRegex.Match(rawContent);
		if (!match.Success)
		{
			return null;
		}
		AudioInstruction audioInstruction = new AudioInstruction();
		InkInstructionParserUtility.ParsedArgumentCollection parsedArgumentCollection = InkInstructionParserUtility.ParseArguments(match.Groups[1].Value);
		parsedArgumentCollection.TryGetValue<bool>("waitForFinish", ref audioInstruction.waitForFinish);
		parsedArgumentCollection.TryGetValue<float>("customAdvanceTime", ref audioInstruction.customAdvanceTime);
		audioInstruction.audioPath = parsedArgumentCollection.textBeforeArguments.Trim();
		return audioInstruction;
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060004EC RID: 1260 RVA: 0x00027EFE File Offset: 0x000260FE
	public bool advanceImmediately
	{
		get
		{
			return !this.waitForFinish && this.customAdvanceTime == -1f;
		}
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00027F17 File Offset: 0x00026117
	public override string ToString()
	{
		return string.Format("[" + base.GetType().Name + "] audioPath={0} waitForFinish={1} customAdvanceTime={2}", this.audioPath, this.waitForFinish, this.customAdvanceTime);
	}

	// Token: 0x040005C2 RID: 1474
	private static Regex _parserRegex;

	// Token: 0x040005C3 RID: 1475
	public string audioPath;

	// Token: 0x040005C4 RID: 1476
	private bool waitForFinish;

	// Token: 0x040005C5 RID: 1477
	public float customAdvanceTime = -1f;

	// Token: 0x040005C6 RID: 1478
	public MediaPlayInstruction mode;
}
