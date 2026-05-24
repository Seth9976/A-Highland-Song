using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000061 RID: 97
	public class Text : Object
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00019969 File Offset: 0x00017B69
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x00019971 File Offset: 0x00017B71
		public string text { get; set; }

		// Token: 0x060004BF RID: 1215 RVA: 0x0001997A File Offset: 0x00017B7A
		public Text(string str)
		{
			this.text = str;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00019989 File Offset: 0x00017B89
		public override Object GenerateRuntimeObject()
		{
			return new StringValue(this.text);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00019996 File Offset: 0x00017B96
		public override string ToString()
		{
			return this.text;
		}
	}
}
