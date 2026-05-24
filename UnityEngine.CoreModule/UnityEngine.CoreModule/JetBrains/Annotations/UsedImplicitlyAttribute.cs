using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000C1 RID: 193
	[AttributeUsage(32767, Inherited = false)]
	public sealed class UsedImplicitlyAttribute : Attribute
	{
		// Token: 0x06000352 RID: 850 RVA: 0x00005D46 File Offset: 0x00003F46
		public UsedImplicitlyAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00005D52 File Offset: 0x00003F52
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00005D5E File Offset: 0x00003F5E
		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00005D6A File Offset: 0x00003F6A
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00005D82 File Offset: 0x00003F82
		public ImplicitUseKindFlags UseKindFlags { get; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00005D8A File Offset: 0x00003F8A
		public ImplicitUseTargetFlags TargetFlags { get; }
	}
}
