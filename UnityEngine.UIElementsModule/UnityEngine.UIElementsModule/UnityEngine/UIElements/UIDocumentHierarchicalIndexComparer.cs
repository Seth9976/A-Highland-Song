using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200024E RID: 590
	internal class UIDocumentHierarchicalIndexComparer : IComparer<UIDocumentHierarchicalIndex>
	{
		// Token: 0x060011B1 RID: 4529 RVA: 0x00044668 File Offset: 0x00042868
		public int Compare(UIDocumentHierarchicalIndex x, UIDocumentHierarchicalIndex y)
		{
			return x.CompareTo(y);
		}
	}
}
