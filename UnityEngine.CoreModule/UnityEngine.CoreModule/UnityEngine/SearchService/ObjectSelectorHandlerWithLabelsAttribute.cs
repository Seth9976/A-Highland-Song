using System;

namespace UnityEngine.SearchService
{
	// Token: 0x020002D3 RID: 723
	[AttributeUsage(256)]
	[Obsolete("ObjectSelectorHandlerWithLabelsAttribute has been deprecated. Use SearchContextAttribute instead.")]
	public class ObjectSelectorHandlerWithLabelsAttribute : Attribute
	{
		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x00030A43 File Offset: 0x0002EC43
		public string[] labels { get; }

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x00030A4B File Offset: 0x0002EC4B
		public bool matchAll { get; }

		// Token: 0x06001DE3 RID: 7651 RVA: 0x00030A53 File Offset: 0x0002EC53
		public ObjectSelectorHandlerWithLabelsAttribute(params string[] labels)
		{
			this.labels = labels;
			this.matchAll = true;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x00030A6B File Offset: 0x0002EC6B
		public ObjectSelectorHandlerWithLabelsAttribute(bool matchAll, params string[] labels)
		{
			this.labels = labels;
			this.matchAll = matchAll;
		}
	}
}
