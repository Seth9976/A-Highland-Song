using System;

namespace Ink.Runtime
{
	// Token: 0x0200003B RID: 59
	public class VariableReference : Object
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000367 RID: 871 RVA: 0x000134AF File Offset: 0x000116AF
		// (set) Token: 0x06000368 RID: 872 RVA: 0x000134B7 File Offset: 0x000116B7
		public string name { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000369 RID: 873 RVA: 0x000134C0 File Offset: 0x000116C0
		// (set) Token: 0x0600036A RID: 874 RVA: 0x000134C8 File Offset: 0x000116C8
		public Path pathForCount { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600036B RID: 875 RVA: 0x000134D4 File Offset: 0x000116D4
		public Container containerForCount
		{
			get
			{
				return base.ResolvePath(this.pathForCount).container;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000134F5 File Offset: 0x000116F5
		// (set) Token: 0x0600036D RID: 877 RVA: 0x0001350D File Offset: 0x0001170D
		public string pathStringForCount
		{
			get
			{
				if (this.pathForCount == null)
				{
					return null;
				}
				return base.CompactPathString(this.pathForCount);
			}
			set
			{
				if (value == null)
				{
					this.pathForCount = null;
					return;
				}
				this.pathForCount = new Path(value);
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00013526 File Offset: 0x00011726
		public VariableReference(string name)
		{
			this.name = name;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00013535 File Offset: 0x00011735
		public VariableReference()
		{
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00013540 File Offset: 0x00011740
		public override string ToString()
		{
			if (this.name != null)
			{
				return string.Format("var({0})", this.name);
			}
			string pathStringForCount = this.pathStringForCount;
			return string.Format("read_count({0})", pathStringForCount);
		}
	}
}
