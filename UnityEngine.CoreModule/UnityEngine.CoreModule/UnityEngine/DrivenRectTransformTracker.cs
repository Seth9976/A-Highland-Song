using System;

namespace UnityEngine
{
	// Token: 0x02000259 RID: 601
	public struct DrivenRectTransformTracker
	{
		// Token: 0x060019F6 RID: 6646 RVA: 0x00029FE4 File Offset: 0x000281E4
		internal static bool CanRecordModifications()
		{
			return true;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x00004557 File Offset: 0x00002757
		public void Add(Object driver, RectTransform rectTransform, DrivenTransformProperties drivenProperties)
		{
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x00029FF7 File Offset: 0x000281F7
		[Obsolete("revertValues parameter is ignored. Please use Clear() instead.")]
		public void Clear(bool revertValues)
		{
			this.Clear();
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00004557 File Offset: 0x00002757
		public void Clear()
		{
		}
	}
}
