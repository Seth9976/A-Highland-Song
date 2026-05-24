using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001F1 RID: 497
	[RequiredByNativeCode]
	[AttributeUsage(64, AllowMultiple = true)]
	public sealed class ContextMenu : Attribute
	{
		// Token: 0x06001658 RID: 5720 RVA: 0x00023D3B File Offset: 0x00021F3B
		public ContextMenu(string itemName)
			: this(itemName, false)
		{
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00023D47 File Offset: 0x00021F47
		public ContextMenu(string itemName, bool isValidateFunction)
			: this(itemName, isValidateFunction, 1000000)
		{
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00023D58 File Offset: 0x00021F58
		public ContextMenu(string itemName, bool isValidateFunction, int priority)
		{
			this.menuItem = itemName;
			this.validate = isValidateFunction;
			this.priority = priority;
		}

		// Token: 0x040007D3 RID: 2003
		public readonly string menuItem;

		// Token: 0x040007D4 RID: 2004
		public readonly bool validate;

		// Token: 0x040007D5 RID: 2005
		public readonly int priority;
	}
}
