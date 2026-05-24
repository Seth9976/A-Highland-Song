using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000231 RID: 561
	public sealed class Security
	{
		// Token: 0x060017F2 RID: 6130 RVA: 0x00026E10 File Offset: 0x00025010
		[EditorBrowsable(1)]
		[Obsolete("This was an internal method which is no longer used", true)]
		public static Assembly LoadAndVerifyAssembly(byte[] assemblyData, string authorizationKey)
		{
			return null;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00026E24 File Offset: 0x00025024
		[EditorBrowsable(1)]
		[Obsolete("This was an internal method which is no longer used", true)]
		public static Assembly LoadAndVerifyAssembly(byte[] assemblyData)
		{
			return null;
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x00026E38 File Offset: 0x00025038
		[Obsolete("Security.PrefetchSocketPolicy is no longer supported, since the Unity Web Player is no longer supported by Unity.", true)]
		[ExcludeFromDocs]
		public static bool PrefetchSocketPolicy(string ip, int atPort)
		{
			int num = 3000;
			return Security.PrefetchSocketPolicy(ip, atPort, num);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00026E58 File Offset: 0x00025058
		[Obsolete("Security.PrefetchSocketPolicy is no longer supported, since the Unity Web Player is no longer supported by Unity.", true)]
		public static bool PrefetchSocketPolicy(string ip, int atPort, [DefaultValue("3000")] int timeout)
		{
			return false;
		}
	}
}
