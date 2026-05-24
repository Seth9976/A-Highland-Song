using System;
using System.Collections.Generic;

// Token: 0x02000095 RID: 149
public class ContentInstruction : BaseContentInstruction
{
	// Token: 0x060004EF RID: 1263 RVA: 0x00027F67 File Offset: 0x00026167
	public static ContentInstruction TryParse(string rawContent, List<string> tags)
	{
		return new ContentInstruction
		{
			text = rawContent
		};
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00027F75 File Offset: 0x00026175
	public override string ToString()
	{
		return string.Format("[" + base.GetType().Name + "] text {0}", this.text);
	}

	// Token: 0x040005CB RID: 1483
	public string text;
}
