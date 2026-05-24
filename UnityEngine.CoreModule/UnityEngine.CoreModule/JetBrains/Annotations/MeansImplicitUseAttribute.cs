using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000C2 RID: 194
	[AttributeUsage(18436)]
	public sealed class MeansImplicitUseAttribute : Attribute
	{
		// Token: 0x06000358 RID: 856 RVA: 0x00005D92 File Offset: 0x00003F92
		public MeansImplicitUseAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00005D9E File Offset: 0x00003F9E
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00005DAA File Offset: 0x00003FAA
		public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00005DB6 File Offset: 0x00003FB6
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00005DCE File Offset: 0x00003FCE
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00005DD6 File Offset: 0x00003FD6
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; }
	}
}
