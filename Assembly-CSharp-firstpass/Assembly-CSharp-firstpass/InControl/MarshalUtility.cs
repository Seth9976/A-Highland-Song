using System;
using System.Runtime.InteropServices;

namespace InControl
{
	// Token: 0x0200006E RID: 110
	public static class MarshalUtility
	{
		// Token: 0x060004EB RID: 1259 RVA: 0x000130F3 File Offset: 0x000112F3
		public static void Copy(IntPtr source, uint[] destination, int length)
		{
			Utility.ArrayExpand<int>(ref MarshalUtility.buffer, length);
			Marshal.Copy(source, MarshalUtility.buffer, 0, length);
			Buffer.BlockCopy(MarshalUtility.buffer, 0, destination, 0, 4 * length);
		}

		// Token: 0x0400046A RID: 1130
		private static int[] buffer = new int[32];
	}
}
