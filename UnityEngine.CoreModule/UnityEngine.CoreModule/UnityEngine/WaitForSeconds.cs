using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200022C RID: 556
	[RequiredByNativeCode]
	[StructLayout(0)]
	public sealed class WaitForSeconds : YieldInstruction
	{
		// Token: 0x060017E7 RID: 6119 RVA: 0x00026D04 File Offset: 0x00024F04
		public WaitForSeconds(float seconds)
		{
			this.m_Seconds = seconds;
		}

		// Token: 0x04000831 RID: 2097
		internal float m_Seconds;
	}
}
