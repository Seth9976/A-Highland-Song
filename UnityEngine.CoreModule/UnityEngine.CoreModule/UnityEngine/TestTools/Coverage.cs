using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.TestTools
{
	// Token: 0x0200048E RID: 1166
	[NativeType("Runtime/Scripting/ScriptingCoverage.h")]
	[NativeClass("ScriptingCoverage")]
	public static class Coverage
	{
		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06002934 RID: 10548
		// (set) Token: 0x06002935 RID: 10549
		public static extern bool enabled
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06002936 RID: 10550
		[FreeFunction("ScriptingCoverageGetCoverageForMethodInfoObject", ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern CoveredSequencePoint[] GetSequencePointsFor_Internal(MethodBase method);

		// Token: 0x06002937 RID: 10551
		[FreeFunction("ScriptingCoverageResetForMethodInfoObject", ThrowsException = true)]
		[MethodImpl(4096)]
		private static extern void ResetFor_Internal(MethodBase method);

		// Token: 0x06002938 RID: 10552 RVA: 0x00044318 File Offset: 0x00042518
		[FreeFunction("ScriptingCoverageGetStatsForMethodInfoObject", ThrowsException = true)]
		private static CoveredMethodStats GetStatsFor_Internal(MethodBase method)
		{
			CoveredMethodStats coveredMethodStats;
			Coverage.GetStatsFor_Internal_Injected(method, out coveredMethodStats);
			return coveredMethodStats;
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x00044330 File Offset: 0x00042530
		public static CoveredSequencePoint[] GetSequencePointsFor(MethodBase method)
		{
			bool flag = method == null;
			if (flag)
			{
				throw new ArgumentNullException("method");
			}
			return Coverage.GetSequencePointsFor_Internal(method);
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x0004435C File Offset: 0x0004255C
		public static CoveredMethodStats GetStatsFor(MethodBase method)
		{
			bool flag = method == null;
			if (flag)
			{
				throw new ArgumentNullException("method");
			}
			return Coverage.GetStatsFor_Internal(method);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x00044388 File Offset: 0x00042588
		public static CoveredMethodStats[] GetStatsFor(MethodBase[] methods)
		{
			bool flag = methods == null;
			if (flag)
			{
				throw new ArgumentNullException("methods");
			}
			CoveredMethodStats[] array = new CoveredMethodStats[methods.Length];
			for (int i = 0; i < methods.Length; i++)
			{
				array[i] = Coverage.GetStatsFor(methods[i]);
			}
			return array;
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x000443DC File Offset: 0x000425DC
		public static CoveredMethodStats[] GetStatsFor(Type type)
		{
			bool flag = type == null;
			if (flag)
			{
				throw new ArgumentNullException("type");
			}
			return Coverage.GetStatsFor(Enumerable.ToArray<MethodBase>(Enumerable.OfType<MethodBase>(type.GetMembers(62))));
		}

		// Token: 0x0600293D RID: 10557
		[FreeFunction("ScriptingCoverageGetStatsForAllCoveredMethodsFromScripting", ThrowsException = true)]
		[MethodImpl(4096)]
		public static extern CoveredMethodStats[] GetStatsForAllCoveredMethods();

		// Token: 0x0600293E RID: 10558 RVA: 0x00044418 File Offset: 0x00042618
		public static void ResetFor(MethodBase method)
		{
			bool flag = method == null;
			if (flag)
			{
				throw new ArgumentNullException("method");
			}
			Coverage.ResetFor_Internal(method);
		}

		// Token: 0x0600293F RID: 10559
		[FreeFunction("ScriptingCoverageResetAllFromScripting", ThrowsException = true)]
		[MethodImpl(4096)]
		public static extern void ResetAll();

		// Token: 0x06002940 RID: 10560
		[MethodImpl(4096)]
		private static extern void GetStatsFor_Internal_Injected(MethodBase method, out CoveredMethodStats ret);
	}
}
