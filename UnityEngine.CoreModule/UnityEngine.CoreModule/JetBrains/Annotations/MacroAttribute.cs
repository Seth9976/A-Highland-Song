using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000CC RID: 204
	[AttributeUsage(2112, AllowMultiple = true)]
	public sealed class MacroAttribute : Attribute
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00005E29 File Offset: 0x00004029
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00005E31 File Offset: 0x00004031
		[CanBeNull]
		public string Expression { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00005E3A File Offset: 0x0000403A
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00005E42 File Offset: 0x00004042
		public int Editable { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00005E4B File Offset: 0x0000404B
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00005E53 File Offset: 0x00004053
		[CanBeNull]
		public string Target { get; set; }
	}
}
