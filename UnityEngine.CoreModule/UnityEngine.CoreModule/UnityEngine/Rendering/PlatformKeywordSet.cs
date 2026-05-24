using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000421 RID: 1057
	[UsedByNativeCode]
	public struct PlatformKeywordSet
	{
		// Token: 0x060024D9 RID: 9433 RVA: 0x0003E5F0 File Offset: 0x0003C7F0
		private ulong ComputeKeywordMask(BuiltinShaderDefine define)
		{
			return (ulong)(1L << (int)((define % (BuiltinShaderDefine)64) & BuiltinShaderDefine.SHADER_API_GLES30));
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x0003E60C File Offset: 0x0003C80C
		public bool IsEnabled(BuiltinShaderDefine define)
		{
			return (this.m_Bits & this.ComputeKeywordMask(define)) > 0UL;
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x0003E630 File Offset: 0x0003C830
		public void Enable(BuiltinShaderDefine define)
		{
			this.m_Bits |= this.ComputeKeywordMask(define);
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x0003E647 File Offset: 0x0003C847
		public void Disable(BuiltinShaderDefine define)
		{
			this.m_Bits &= ~this.ComputeKeywordMask(define);
		}

		// Token: 0x04000DA6 RID: 3494
		private const int k_SizeInBits = 64;

		// Token: 0x04000DA7 RID: 3495
		internal ulong m_Bits;
	}
}
