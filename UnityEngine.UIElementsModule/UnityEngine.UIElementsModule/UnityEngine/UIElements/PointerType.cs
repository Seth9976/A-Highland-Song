using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000212 RID: 530
	public static class PointerType
	{
		// Token: 0x06000FF3 RID: 4083 RVA: 0x0003EE5C File Offset: 0x0003D05C
		internal static string GetPointerType(int pointerId)
		{
			bool flag = pointerId == PointerId.mousePointerId;
			string text;
			if (flag)
			{
				text = PointerType.mouse;
			}
			else
			{
				text = PointerType.touch;
			}
			return text;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003EE88 File Offset: 0x0003D088
		internal static bool IsDirectManipulationDevice(string pointerType)
		{
			return pointerType == PointerType.touch || pointerType == PointerType.pen;
		}

		// Token: 0x04000701 RID: 1793
		public static readonly string mouse = "mouse";

		// Token: 0x04000702 RID: 1794
		public static readonly string touch = "touch";

		// Token: 0x04000703 RID: 1795
		public static readonly string pen = "pen";

		// Token: 0x04000704 RID: 1796
		public static readonly string unknown = "";
	}
}
