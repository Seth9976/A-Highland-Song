using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x0200012B RID: 299
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	public struct RenderBuffer
	{
		// Token: 0x06000863 RID: 2147 RVA: 0x0000C7AD File Offset: 0x0000A9AD
		[FreeFunction(Name = "RenderBufferScripting::SetLoadAction", HasExplicitThis = true)]
		internal void SetLoadAction(RenderBufferLoadAction action)
		{
			RenderBuffer.SetLoadAction_Injected(ref this, action);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0000C7B6 File Offset: 0x0000A9B6
		[FreeFunction(Name = "RenderBufferScripting::SetStoreAction", HasExplicitThis = true)]
		internal void SetStoreAction(RenderBufferStoreAction action)
		{
			RenderBuffer.SetStoreAction_Injected(ref this, action);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0000C7BF File Offset: 0x0000A9BF
		[FreeFunction(Name = "RenderBufferScripting::GetLoadAction", HasExplicitThis = true)]
		internal RenderBufferLoadAction GetLoadAction()
		{
			return RenderBuffer.GetLoadAction_Injected(ref this);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0000C7C7 File Offset: 0x0000A9C7
		[FreeFunction(Name = "RenderBufferScripting::GetStoreAction", HasExplicitThis = true)]
		internal RenderBufferStoreAction GetStoreAction()
		{
			return RenderBuffer.GetStoreAction_Injected(ref this);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0000C7CF File Offset: 0x0000A9CF
		[FreeFunction(Name = "RenderBufferScripting::GetNativeRenderBufferPtr", HasExplicitThis = true)]
		public IntPtr GetNativeRenderBufferPtr()
		{
			return RenderBuffer.GetNativeRenderBufferPtr_Injected(ref this);
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		// (set) Token: 0x06000869 RID: 2153 RVA: 0x0000C7F0 File Offset: 0x0000A9F0
		internal RenderBufferLoadAction loadAction
		{
			get
			{
				return this.GetLoadAction();
			}
			set
			{
				this.SetLoadAction(value);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x0000C7FC File Offset: 0x0000A9FC
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x0000C814 File Offset: 0x0000AA14
		internal RenderBufferStoreAction storeAction
		{
			get
			{
				return this.GetStoreAction();
			}
			set
			{
				this.SetStoreAction(value);
			}
		}

		// Token: 0x0600086C RID: 2156
		[MethodImpl(4096)]
		private static extern void SetLoadAction_Injected(ref RenderBuffer _unity_self, RenderBufferLoadAction action);

		// Token: 0x0600086D RID: 2157
		[MethodImpl(4096)]
		private static extern void SetStoreAction_Injected(ref RenderBuffer _unity_self, RenderBufferStoreAction action);

		// Token: 0x0600086E RID: 2158
		[MethodImpl(4096)]
		private static extern RenderBufferLoadAction GetLoadAction_Injected(ref RenderBuffer _unity_self);

		// Token: 0x0600086F RID: 2159
		[MethodImpl(4096)]
		private static extern RenderBufferStoreAction GetStoreAction_Injected(ref RenderBuffer _unity_self);

		// Token: 0x06000870 RID: 2160
		[MethodImpl(4096)]
		private static extern IntPtr GetNativeRenderBufferPtr_Injected(ref RenderBuffer _unity_self);

		// Token: 0x040003C2 RID: 962
		internal int m_RenderTextureInstanceID;

		// Token: 0x040003C3 RID: 963
		internal IntPtr m_BufferPtr;
	}
}
