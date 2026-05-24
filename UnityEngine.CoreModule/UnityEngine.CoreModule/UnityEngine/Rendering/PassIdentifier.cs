using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000420 RID: 1056
	[UsedByNativeCode]
	[NativeHeader("Runtime/Shaders/PassIdentifier.h")]
	public readonly struct PassIdentifier : IEquatable<PassIdentifier>
	{
		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060024D2 RID: 9426 RVA: 0x0003E4F8 File Offset: 0x0003C6F8
		public uint SubshaderIndex
		{
			get
			{
				return this.m_SubShaderIndex;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060024D3 RID: 9427 RVA: 0x0003E510 File Offset: 0x0003C710
		public uint PassIndex
		{
			get
			{
				return this.m_PassIndex;
			}
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x0003E528 File Offset: 0x0003C728
		public override bool Equals(object o)
		{
			bool flag;
			if (o is PassIdentifier)
			{
				PassIdentifier passIdentifier = (PassIdentifier)o;
				flag = this.Equals(passIdentifier);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x0003E554 File Offset: 0x0003C754
		public bool Equals(PassIdentifier rhs)
		{
			return this.m_SubShaderIndex == rhs.m_SubShaderIndex && this.m_PassIndex == rhs.m_PassIndex;
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x0003E588 File Offset: 0x0003C788
		public static bool operator ==(PassIdentifier lhs, PassIdentifier rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x0003E5A4 File Offset: 0x0003C7A4
		public static bool operator !=(PassIdentifier lhs, PassIdentifier rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x0003E5C0 File Offset: 0x0003C7C0
		public override int GetHashCode()
		{
			return this.m_SubShaderIndex.GetHashCode() ^ this.m_PassIndex.GetHashCode();
		}

		// Token: 0x04000DA4 RID: 3492
		internal readonly uint m_SubShaderIndex;

		// Token: 0x04000DA5 RID: 3493
		internal readonly uint m_PassIndex;
	}
}
