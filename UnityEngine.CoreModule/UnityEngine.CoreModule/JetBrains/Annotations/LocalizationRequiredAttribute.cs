using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000BE RID: 190
	[AttributeUsage(32767)]
	public sealed class LocalizationRequiredAttribute : Attribute
	{
		// Token: 0x0600034C RID: 844 RVA: 0x00005D09 File Offset: 0x00003F09
		public LocalizationRequiredAttribute()
			: this(true)
		{
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00005D14 File Offset: 0x00003F14
		public LocalizationRequiredAttribute(bool required)
		{
			this.Required = required;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00005D25 File Offset: 0x00003F25
		public bool Required { get; }
	}
}
