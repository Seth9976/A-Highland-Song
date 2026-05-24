using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000CD RID: 205
	[AttributeUsage(224)]
	public sealed class CollectionAccessAttribute : Attribute
	{
		// Token: 0x06000372 RID: 882 RVA: 0x00005E5C File Offset: 0x0000405C
		public CollectionAccessAttribute(CollectionAccessType collectionAccessType)
		{
			this.CollectionAccessType = collectionAccessType;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00005E6D File Offset: 0x0000406D
		public CollectionAccessType CollectionAccessType { get; }
	}
}
