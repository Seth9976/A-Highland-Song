using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000220 RID: 544
	[UsedByNativeCode]
	[StructLayout(0)]
	public class TrackedReference
	{
		// Token: 0x0600177E RID: 6014 RVA: 0x00008CAF File Offset: 0x00006EAF
		protected TrackedReference()
		{
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00026030 File Offset: 0x00024230
		public static bool operator ==(TrackedReference x, TrackedReference y)
		{
			bool flag = y == null && x == null;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				bool flag3 = y == null;
				if (flag3)
				{
					flag2 = x.m_Ptr == IntPtr.Zero;
				}
				else
				{
					bool flag4 = x == null;
					if (flag4)
					{
						flag2 = y.m_Ptr == IntPtr.Zero;
					}
					else
					{
						flag2 = x.m_Ptr == y.m_Ptr;
					}
				}
			}
			return flag2;
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x000260A4 File Offset: 0x000242A4
		public static bool operator !=(TrackedReference x, TrackedReference y)
		{
			return !(x == y);
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x000260C0 File Offset: 0x000242C0
		public override bool Equals(object o)
		{
			return o as TrackedReference == this;
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x000260E0 File Offset: 0x000242E0
		public override int GetHashCode()
		{
			return (int)this.m_Ptr;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00026100 File Offset: 0x00024300
		public static implicit operator bool(TrackedReference exists)
		{
			return exists != null;
		}

		// Token: 0x04000810 RID: 2064
		internal IntPtr m_Ptr;
	}
}
