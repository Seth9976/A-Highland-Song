using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002E0 RID: 736
	public class UxmlChildElementDescription
	{
		// Token: 0x06001841 RID: 6209 RVA: 0x00061474 File Offset: 0x0005F674
		public UxmlChildElementDescription(Type t)
		{
			bool flag = t == null;
			if (flag)
			{
				throw new ArgumentNullException("t");
			}
			this.elementName = t.Name;
			this.elementNamespace = t.Namespace;
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x000614B7 File Offset: 0x0005F6B7
		// (set) Token: 0x06001843 RID: 6211 RVA: 0x000614BF File Offset: 0x0005F6BF
		public string elementName { get; protected set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001844 RID: 6212 RVA: 0x000614C8 File Offset: 0x0005F6C8
		// (set) Token: 0x06001845 RID: 6213 RVA: 0x000614D0 File Offset: 0x0005F6D0
		public string elementNamespace { get; protected set; }
	}
}
