using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.U2D
{
	// Token: 0x02000453 RID: 1107
	[RequiredByNativeCode]
	[NativeHeader("Runtime/2D/Renderer/SpriteRendererGroup.h")]
	[StructLayout(0)]
	internal class SpriteRendererGroup
	{
		// Token: 0x060027A8 RID: 10152 RVA: 0x000414FC File Offset: 0x0003F6FC
		public static void AddRenderers(NativeArray<SpriteIntermediateRendererInfo> renderers)
		{
			SpriteRendererGroup.AddRenderers(renderers.GetUnsafeReadOnlyPtr<SpriteIntermediateRendererInfo>(), renderers.Length);
		}

		// Token: 0x060027A9 RID: 10153
		[MethodImpl(4096)]
		private unsafe static extern void AddRenderers(void* renderers, int count);

		// Token: 0x060027AA RID: 10154
		[MethodImpl(4096)]
		public static extern void Clear();
	}
}
