using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000C4 RID: 196
	internal struct RuleMatcher
	{
		// Token: 0x06000695 RID: 1685 RVA: 0x00018899 File Offset: 0x00016A99
		public RuleMatcher(StyleSheet sheet, StyleComplexSelector complexSelector, int styleSheetIndexInStack)
		{
			this.sheet = sheet;
			this.complexSelector = complexSelector;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000188AC File Offset: 0x00016AAC
		public override string ToString()
		{
			return this.complexSelector.ToString();
		}

		// Token: 0x04000297 RID: 663
		public StyleSheet sheet;

		// Token: 0x04000298 RID: 664
		public StyleComplexSelector complexSelector;
	}
}
