using System;
using System.Collections;
using System.Security;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001FF RID: 511
	[RequiredByNativeCode]
	internal class SetupCoroutine
	{
		// Token: 0x0600169E RID: 5790 RVA: 0x00024238 File Offset: 0x00022438
		[SecuritySafeCritical]
		[RequiredByNativeCode]
		public unsafe static void InvokeMoveNext(IEnumerator enumerator, IntPtr returnValueAddress)
		{
			bool flag = returnValueAddress == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Return value address cannot be 0.", "returnValueAddress");
			}
			*(byte*)(void*)returnValueAddress = (enumerator.MoveNext() ? 1 : 0);
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00024274 File Offset: 0x00022474
		[RequiredByNativeCode]
		public static object InvokeMember(object behaviour, string name, object variable)
		{
			object[] array = null;
			bool flag = variable != null;
			if (flag)
			{
				array = new object[] { variable };
			}
			return behaviour.GetType().InvokeMember(name, 308, null, behaviour, array, null, null, null);
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x000242B4 File Offset: 0x000224B4
		public static object InvokeStatic(Type klass, string name, object variable)
		{
			object[] array = null;
			bool flag = variable != null;
			if (flag)
			{
				array = new object[] { variable };
			}
			return klass.InvokeMember(name, 312, null, null, array, null, null, null);
		}
	}
}
