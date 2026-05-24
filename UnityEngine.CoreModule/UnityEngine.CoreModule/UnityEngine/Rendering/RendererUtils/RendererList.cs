using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering.RendererUtils
{
	// Token: 0x02000429 RID: 1065
	[NativeHeader("Runtime/Graphics/ScriptableRenderLoop/RendererList.h")]
	public struct RendererList
	{
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x0003EAC1 File Offset: 0x0003CCC1
		public bool isValid
		{
			get
			{
				return RendererList.get_isValid_Injected(ref this);
			}
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x0003EAC9 File Offset: 0x0003CCC9
		internal RendererList(UIntPtr ctx, uint indx)
		{
			this.context = ctx;
			this.index = indx;
			this.frame = 0U;
		}

		// Token: 0x0600251F RID: 9503
		[MethodImpl(4096)]
		private static extern bool get_isValid_Injected(ref RendererList _unity_self);

		// Token: 0x04000DCF RID: 3535
		internal UIntPtr context;

		// Token: 0x04000DD0 RID: 3536
		internal uint index;

		// Token: 0x04000DD1 RID: 3537
		internal uint frame;

		// Token: 0x04000DD2 RID: 3538
		public static readonly RendererList nullRendererList = new RendererList(UIntPtr.Zero, uint.MaxValue);
	}
}
