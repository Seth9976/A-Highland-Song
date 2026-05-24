using System;

namespace UnityEngine
{
	// Token: 0x02000254 RID: 596
	public enum TouchScreenKeyboardType
	{
		// Token: 0x04000882 RID: 2178
		Default,
		// Token: 0x04000883 RID: 2179
		ASCIICapable,
		// Token: 0x04000884 RID: 2180
		NumbersAndPunctuation,
		// Token: 0x04000885 RID: 2181
		URL,
		// Token: 0x04000886 RID: 2182
		NumberPad,
		// Token: 0x04000887 RID: 2183
		PhonePad,
		// Token: 0x04000888 RID: 2184
		NamePhonePad,
		// Token: 0x04000889 RID: 2185
		EmailAddress,
		// Token: 0x0400088A RID: 2186
		[Obsolete("Wii U is no longer supported as of Unity 2018.1.")]
		NintendoNetworkAccount,
		// Token: 0x0400088B RID: 2187
		Social,
		// Token: 0x0400088C RID: 2188
		Search,
		// Token: 0x0400088D RID: 2189
		DecimalPad,
		// Token: 0x0400088E RID: 2190
		OneTimeCode
	}
}
