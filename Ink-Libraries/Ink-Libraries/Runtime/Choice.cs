using System;

namespace Ink.Runtime
{
	// Token: 0x02000013 RID: 19
	public class Choice : Object
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00007B9C File Offset: 0x00005D9C
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00007BA4 File Offset: 0x00005DA4
		public string text { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00007BAD File Offset: 0x00005DAD
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00007BBA File Offset: 0x00005DBA
		public string pathStringOnChoice
		{
			get
			{
				return this.targetPath.ToString();
			}
			set
			{
				this.targetPath = new Path(value);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00007BC8 File Offset: 0x00005DC8
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00007BD0 File Offset: 0x00005DD0
		public int index { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00007BD9 File Offset: 0x00005DD9
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00007BE1 File Offset: 0x00005DE1
		public CallStack.Thread threadAtGeneration { get; set; }

		// Token: 0x06000120 RID: 288 RVA: 0x00007BF4 File Offset: 0x00005DF4
		public Choice Clone()
		{
			Choice choice = new Choice();
			choice.text = this.text;
			choice.sourcePath = this.sourcePath;
			choice.index = this.index;
			choice.targetPath = this.targetPath;
			choice.originalThreadIndex = this.originalThreadIndex;
			choice.isInvisibleDefault = this.isInvisibleDefault;
			if (this.threadAtGeneration != null)
			{
				choice.threadAtGeneration = this.threadAtGeneration.Copy();
			}
			return choice;
		}

		// Token: 0x0400004C RID: 76
		public string sourcePath;

		// Token: 0x0400004E RID: 78
		public Path targetPath;

		// Token: 0x04000050 RID: 80
		public int originalThreadIndex;

		// Token: 0x04000051 RID: 81
		public bool isInvisibleDefault;
	}
}
