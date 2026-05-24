using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Scripting
{
	// Token: 0x020002D6 RID: 726
	[NativeHeader("Runtime/Scripting/GarbageCollector.h")]
	public static class GarbageCollector
	{
		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001DE8 RID: 7656 RVA: 0x00030A9C File Offset: 0x0002EC9C
		// (remove) Token: 0x06001DE9 RID: 7657 RVA: 0x00030AD0 File Offset: 0x0002ECD0
		[field: DebuggerBrowsable(0)]
		public static event Action<GarbageCollector.Mode> GCModeChanged;

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x00030B04 File Offset: 0x0002ED04
		// (set) Token: 0x06001DEB RID: 7659 RVA: 0x00030B1C File Offset: 0x0002ED1C
		public static GarbageCollector.Mode GCMode
		{
			get
			{
				return GarbageCollector.GetMode();
			}
			set
			{
				bool flag = value == GarbageCollector.GetMode();
				if (!flag)
				{
					GarbageCollector.SetMode(value);
					bool flag2 = GarbageCollector.GCModeChanged != null;
					if (flag2)
					{
						GarbageCollector.GCModeChanged.Invoke(value);
					}
				}
			}
		}

		// Token: 0x06001DEC RID: 7660
		[NativeThrows]
		[MethodImpl(4096)]
		private static extern void SetMode(GarbageCollector.Mode mode);

		// Token: 0x06001DED RID: 7661
		[MethodImpl(4096)]
		private static extern GarbageCollector.Mode GetMode();

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001DEE RID: 7662
		public static extern bool isIncremental
		{
			[NativeMethod("GetIncrementalEnabled")]
			[MethodImpl(4096)]
			get;
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001DEF RID: 7663
		// (set) Token: 0x06001DF0 RID: 7664
		public static extern ulong incrementalTimeSliceNanoseconds
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06001DF1 RID: 7665
		[NativeMethod("CollectIncrementalWrapper")]
		[NativeThrows]
		[MethodImpl(4096)]
		public static extern bool CollectIncremental(ulong nanoseconds = 0UL);

		// Token: 0x020002D7 RID: 727
		public enum Mode
		{
			// Token: 0x040009D0 RID: 2512
			Disabled,
			// Token: 0x040009D1 RID: 2513
			Enabled,
			// Token: 0x040009D2 RID: 2514
			Manual
		}
	}
}
