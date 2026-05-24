using System;
using JetBrains.Annotations;

namespace EasyButtons
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class ButtonAttribute : Attribute
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public ButtonAttribute()
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public ButtonAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002067 File Offset: 0x00000267
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000206F File Offset: 0x0000026F
		[PublicAPI]
		public ButtonMode Mode { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002078 File Offset: 0x00000278
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002080 File Offset: 0x00000280
		[PublicAPI]
		public ButtonSpacing Spacing { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002089 File Offset: 0x00000289
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002091 File Offset: 0x00000291
		[PublicAPI]
		public bool Expanded { get; set; }

		// Token: 0x04000009 RID: 9
		public readonly string Name;
	}
}
