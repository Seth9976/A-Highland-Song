using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000B9 RID: 185
	[AttributeUsage(4320)]
	public sealed class StringFormatMethodAttribute : Attribute
	{
		// Token: 0x06000340 RID: 832 RVA: 0x00005C8A File Offset: 0x00003E8A
		public StringFormatMethodAttribute([NotNull] string formatParameterName)
		{
			this.FormatParameterName = formatParameterName;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00005C9B File Offset: 0x00003E9B
		[NotNull]
		public string FormatParameterName { get; }
	}
}
