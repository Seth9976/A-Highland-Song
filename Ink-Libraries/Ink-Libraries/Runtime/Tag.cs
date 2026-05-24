using System;

namespace Ink.Runtime
{
	// Token: 0x0200002F RID: 47
	public class Tag : Object
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00012D20 File Offset: 0x00010F20
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00012D28 File Offset: 0x00010F28
		public string text { get; private set; }

		// Token: 0x0600031B RID: 795 RVA: 0x00012D31 File Offset: 0x00010F31
		public Tag(string tagText)
		{
			this.text = tagText;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00012D40 File Offset: 0x00010F40
		public override string ToString()
		{
			return "# " + this.text;
		}
	}
}
