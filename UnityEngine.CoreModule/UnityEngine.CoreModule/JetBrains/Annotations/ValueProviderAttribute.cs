using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000BA RID: 186
	[AttributeUsage(2432, AllowMultiple = true)]
	public sealed class ValueProviderAttribute : Attribute
	{
		// Token: 0x06000342 RID: 834 RVA: 0x00005CA3 File Offset: 0x00003EA3
		public ValueProviderAttribute([NotNull] string name)
		{
			this.Name = name;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00005CB4 File Offset: 0x00003EB4
		[NotNull]
		public string Name { get; }
	}
}
