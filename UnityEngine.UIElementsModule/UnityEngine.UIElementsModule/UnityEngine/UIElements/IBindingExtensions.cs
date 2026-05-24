using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000125 RID: 293
	public static class IBindingExtensions
	{
		// Token: 0x060009D8 RID: 2520 RVA: 0x00026940 File Offset: 0x00024B40
		public static bool IsBound(this IBindable control)
		{
			return ((control != null) ? control.binding : null) != null;
		}
	}
}
