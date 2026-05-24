using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200024E RID: 590
	[VisibleToOtherModules]
	internal class SystemClock
	{
		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x000296E4 File Offset: 0x000278E4
		public static DateTime now
		{
			get
			{
				return DateTime.Now;
			}
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x000296FC File Offset: 0x000278FC
		public static long ToUnixTimeMilliseconds(DateTime date)
		{
			return Convert.ToInt64((date.ToUniversalTime() - SystemClock.s_Epoch).TotalMilliseconds);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0002972C File Offset: 0x0002792C
		public static long ToUnixTimeSeconds(DateTime date)
		{
			return Convert.ToInt64((date.ToUniversalTime() - SystemClock.s_Epoch).TotalSeconds);
		}

		// Token: 0x04000873 RID: 2163
		private static readonly DateTime s_Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 1);
	}
}
