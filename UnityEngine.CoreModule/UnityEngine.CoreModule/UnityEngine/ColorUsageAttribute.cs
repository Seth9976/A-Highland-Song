using System;

namespace UnityEngine
{
	// Token: 0x020001DE RID: 478
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	public sealed class ColorUsageAttribute : PropertyAttribute
	{
		// Token: 0x060015D8 RID: 5592 RVA: 0x00023070 File Offset: 0x00021270
		public ColorUsageAttribute(bool showAlpha)
		{
			this.showAlpha = showAlpha;
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x000230C8 File Offset: 0x000212C8
		public ColorUsageAttribute(bool showAlpha, bool hdr)
		{
			this.showAlpha = showAlpha;
			this.hdr = hdr;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00023128 File Offset: 0x00021328
		[Obsolete("Brightness and exposure parameters are no longer used for anything. Use ColorUsageAttribute(bool showAlpha, bool hdr)")]
		public ColorUsageAttribute(bool showAlpha, bool hdr, float minBrightness, float maxBrightness, float minExposureValue, float maxExposureValue)
		{
			this.showAlpha = showAlpha;
			this.hdr = hdr;
			this.minBrightness = minBrightness;
			this.maxBrightness = maxBrightness;
			this.minExposureValue = minExposureValue;
			this.maxExposureValue = maxExposureValue;
		}

		// Token: 0x040007B7 RID: 1975
		public readonly bool showAlpha = true;

		// Token: 0x040007B8 RID: 1976
		public readonly bool hdr = false;

		// Token: 0x040007B9 RID: 1977
		[Obsolete("This field is no longer used for anything.")]
		public readonly float minBrightness = 0f;

		// Token: 0x040007BA RID: 1978
		[Obsolete("This field is no longer used for anything.")]
		public readonly float maxBrightness = 8f;

		// Token: 0x040007BB RID: 1979
		[Obsolete("This field is no longer used for anything.")]
		public readonly float minExposureValue = 0.125f;

		// Token: 0x040007BC RID: 1980
		[Obsolete("This field is no longer used for anything.")]
		public readonly float maxExposureValue = 3f;
	}
}
