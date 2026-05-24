using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000193 RID: 403
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Graphics/Mesh/SkinnedMeshRenderer.h")]
	public class SkinnedMeshRenderer : Renderer
	{
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000ECA RID: 3786
		// (set) Token: 0x06000ECB RID: 3787
		public extern SkinQuality quality
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000ECC RID: 3788
		// (set) Token: 0x06000ECD RID: 3789
		public extern bool updateWhenOffscreen
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000ECE RID: 3790
		// (set) Token: 0x06000ECF RID: 3791
		public extern bool forceMatrixRecalculationPerRender
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000ED0 RID: 3792
		// (set) Token: 0x06000ED1 RID: 3793
		public extern Transform rootBone
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000ED2 RID: 3794
		// (set) Token: 0x06000ED3 RID: 3795
		public extern Transform[] bones
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000ED4 RID: 3796
		// (set) Token: 0x06000ED5 RID: 3797
		[NativeProperty("Mesh")]
		public extern Mesh sharedMesh
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000ED6 RID: 3798
		// (set) Token: 0x06000ED7 RID: 3799
		[NativeProperty("SkinnedMeshMotionVectors")]
		public extern bool skinnedMotionVectors
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x06000ED8 RID: 3800
		[MethodImpl(4096)]
		public extern float GetBlendShapeWeight(int index);

		// Token: 0x06000ED9 RID: 3801
		[MethodImpl(4096)]
		public extern void SetBlendShapeWeight(int index, float value);

		// Token: 0x06000EDA RID: 3802 RVA: 0x00012D53 File Offset: 0x00010F53
		public void BakeMesh(Mesh mesh)
		{
			this.BakeMesh(mesh, false);
		}

		// Token: 0x06000EDB RID: 3803
		[MethodImpl(4096)]
		public extern void BakeMesh([NotNull("NullExceptionObject")] Mesh mesh, bool useScale);

		// Token: 0x06000EDC RID: 3804 RVA: 0x00012D60 File Offset: 0x00010F60
		public GraphicsBuffer GetVertexBuffer()
		{
			bool flag = this == null;
			if (flag)
			{
				throw new NullReferenceException();
			}
			return this.GetVertexBufferImpl();
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00012D8C File Offset: 0x00010F8C
		public GraphicsBuffer GetPreviousVertexBuffer()
		{
			bool flag = this == null;
			if (flag)
			{
				throw new NullReferenceException();
			}
			return this.GetPreviousVertexBufferImpl();
		}

		// Token: 0x06000EDE RID: 3806
		[FreeFunction(Name = "SkinnedMeshRendererScripting::GetVertexBufferPtr", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern GraphicsBuffer GetVertexBufferImpl();

		// Token: 0x06000EDF RID: 3807
		[FreeFunction(Name = "SkinnedMeshRendererScripting::GetPreviousVertexBufferPtr", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern GraphicsBuffer GetPreviousVertexBufferImpl();

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000EE0 RID: 3808
		// (set) Token: 0x06000EE1 RID: 3809
		public extern GraphicsBuffer.Target vertexBufferTarget
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}
	}
}
