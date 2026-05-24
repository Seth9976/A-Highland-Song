using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000215 RID: 533
	internal class ScriptingUtility
	{
		// Token: 0x06001756 RID: 5974 RVA: 0x000257B4 File Offset: 0x000239B4
		[RequiredByNativeCode]
		private static bool IsManagedCodeWorking()
		{
			ScriptingUtility.TestClass testClass = new ScriptingUtility.TestClass
			{
				value = 42
			};
			return testClass.value == 42;
		}

		// Token: 0x02000216 RID: 534
		private struct TestClass
		{
			// Token: 0x04000801 RID: 2049
			public int value;
		}
	}
}
