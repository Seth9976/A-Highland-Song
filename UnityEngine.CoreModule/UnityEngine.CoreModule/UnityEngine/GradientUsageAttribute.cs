using System;

namespace UnityEngine
{
	// Token: 0x020001DF RID: 479
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	public sealed class GradientUsageAttribute : PropertyAttribute
	{
		// Token: 0x060015DB RID: 5595 RVA: 0x000231A4 File Offset: 0x000213A4
		public GradientUsageAttribute(bool hdr)
		{
			this.hdr = hdr;
			this.colorSpace = ColorSpace.Gamma;
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x000231CA File Offset: 0x000213CA
		public GradientUsageAttribute(bool hdr, ColorSpace colorSpace)
		{
			this.hdr = hdr;
			this.colorSpace = colorSpace;
		}

		// Token: 0x040007BD RID: 1981
		public readonly bool hdr = false;

		// Token: 0x040007BE RID: 1982
		public readonly ColorSpace colorSpace = ColorSpace.Gamma;
	}
}
