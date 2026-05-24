using System;

namespace UnityEngine
{
	// Token: 0x020001DD RID: 477
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	public sealed class TextAreaAttribute : PropertyAttribute
	{
		// Token: 0x060015D6 RID: 5590 RVA: 0x0002303E File Offset: 0x0002123E
		public TextAreaAttribute()
		{
			this.minLines = 3;
			this.maxLines = 3;
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00023056 File Offset: 0x00021256
		public TextAreaAttribute(int minLines, int maxLines)
		{
			this.minLines = minLines;
			this.maxLines = maxLines;
		}

		// Token: 0x040007B5 RID: 1973
		public readonly int minLines;

		// Token: 0x040007B6 RID: 1974
		public readonly int maxLines;
	}
}
