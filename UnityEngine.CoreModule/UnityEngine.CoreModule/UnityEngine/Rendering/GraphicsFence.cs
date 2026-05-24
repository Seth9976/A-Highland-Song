using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x020003DF RID: 991
	[UsedByNativeCode]
	[NativeHeader("Runtime/Graphics/GPUFence.h")]
	public struct GraphicsFence
	{
		// Token: 0x06001F90 RID: 8080 RVA: 0x00033A9C File Offset: 0x00031C9C
		internal static SynchronisationStageFlags TranslateSynchronizationStageToFlags(SynchronisationStage s)
		{
			return (s == SynchronisationStage.VertexProcessing) ? SynchronisationStageFlags.VertexProcessing : SynchronisationStageFlags.PixelProcessing;
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x00033AB8 File Offset: 0x00031CB8
		public bool passed
		{
			get
			{
				this.Validate();
				bool flag = !SystemInfo.supportsGraphicsFence || (this.m_FenceType == GraphicsFenceType.AsyncQueueSynchronisation && !SystemInfo.supportsAsyncCompute);
				if (flag)
				{
					throw new NotSupportedException("Cannot determine if this GraphicsFence has passed as this platform has not implemented GraphicsFences.");
				}
				bool flag2 = !this.IsFencePending();
				return flag2 || GraphicsFence.HasFencePassed_Internal(this.m_Ptr);
			}
		}

		// Token: 0x06001F92 RID: 8082
		[FreeFunction("GPUFenceInternals::HasFencePassed_Internal")]
		[MethodImpl(4096)]
		private static extern bool HasFencePassed_Internal(IntPtr fencePtr);

		// Token: 0x06001F93 RID: 8083 RVA: 0x00033B1C File Offset: 0x00031D1C
		internal void InitPostAllocation()
		{
			bool flag = this.m_Ptr == IntPtr.Zero;
			if (flag)
			{
				bool supportsGraphicsFence = SystemInfo.supportsGraphicsFence;
				if (supportsGraphicsFence)
				{
					throw new NullReferenceException("The internal fence ptr is null, this should not be possible for fences that have been correctly constructed using Graphics.CreateGraphicsFence() or CommandBuffer.CreateGraphicsFence()");
				}
				this.m_Version = this.GetPlatformNotSupportedVersion();
			}
			else
			{
				this.m_Version = GraphicsFence.GetVersionNumber(this.m_Ptr);
			}
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x00033B74 File Offset: 0x00031D74
		internal bool IsFencePending()
		{
			bool flag = this.m_Ptr == IntPtr.Zero;
			return !flag && this.m_Version == GraphicsFence.GetVersionNumber(this.m_Ptr);
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x00033BB4 File Offset: 0x00031DB4
		internal void Validate()
		{
			bool flag = this.m_Version == 0 || (SystemInfo.supportsGraphicsFence && this.m_Version == this.GetPlatformNotSupportedVersion());
			if (flag)
			{
				throw new InvalidOperationException("This GraphicsFence object has not been correctly constructed see Graphics.CreateGraphicsFence() or CommandBuffer.CreateGraphicsFence()");
			}
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x00033BF4 File Offset: 0x00031DF4
		private int GetPlatformNotSupportedVersion()
		{
			return -1;
		}

		// Token: 0x06001F97 RID: 8087
		[FreeFunction("GPUFenceInternals::GetVersionNumber")]
		[NativeThrows]
		[MethodImpl(4096)]
		private static extern int GetVersionNumber(IntPtr fencePtr);

		// Token: 0x04000C2A RID: 3114
		internal IntPtr m_Ptr;

		// Token: 0x04000C2B RID: 3115
		internal int m_Version;

		// Token: 0x04000C2C RID: 3116
		internal GraphicsFenceType m_FenceType;
	}
}
