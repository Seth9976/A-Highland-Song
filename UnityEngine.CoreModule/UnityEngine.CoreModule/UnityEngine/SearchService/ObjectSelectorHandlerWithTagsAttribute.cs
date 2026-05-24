using System;

namespace UnityEngine.SearchService
{
	// Token: 0x020002D4 RID: 724
	[Obsolete("ObjectSelectorHandlerWithTagsAttribute has been deprecated. Use SearchContextAttribute instead.")]
	[AttributeUsage(256)]
	public class ObjectSelectorHandlerWithTagsAttribute : Attribute
	{
		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x00030A83 File Offset: 0x0002EC83
		public string[] tags { get; }

		// Token: 0x06001DE6 RID: 7654 RVA: 0x00030A8B File Offset: 0x0002EC8B
		public ObjectSelectorHandlerWithTagsAttribute(params string[] tags)
		{
			this.tags = tags;
		}
	}
}
