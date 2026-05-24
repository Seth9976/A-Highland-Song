using System;

// Token: 0x02000096 RID: 150
public class DebuggingInstruction : BaseContentInstruction
{
	// Token: 0x060004F2 RID: 1266 RVA: 0x00027FA4 File Offset: 0x000261A4
	public static DebuggingInstruction TryParse(string rawContent)
	{
		string text = rawContent.Trim();
		if (!text.StartsWith("["))
		{
			return null;
		}
		return new DebuggingInstruction
		{
			text = text.Substring(1, text.Length - 2).Trim()
		};
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00027FE6 File Offset: 0x000261E6
	public override string ToString()
	{
		return string.Format("[" + base.GetType().Name + "] text {0}", this.text);
	}

	// Token: 0x040005CC RID: 1484
	public string text;
}
