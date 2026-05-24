using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x0200047D RID: 1149
	public sealed class RayTracingAccelerationStructure : IDisposable
	{
		// Token: 0x06002873 RID: 10355 RVA: 0x00042F84 File Offset: 0x00041184
		~RayTracingAccelerationStructure()
		{
			this.Dispose(false);
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x00042FB8 File Offset: 0x000411B8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x00042FCC File Offset: 0x000411CC
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				RayTracingAccelerationStructure.Destroy(this);
			}
			this.m_Ptr = IntPtr.Zero;
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x00042FF3 File Offset: 0x000411F3
		public RayTracingAccelerationStructure(RayTracingAccelerationStructure.RASSettings settings)
		{
			this.m_Ptr = RayTracingAccelerationStructure.Create(settings);
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x0004300C File Offset: 0x0004120C
		public RayTracingAccelerationStructure()
		{
			this.m_Ptr = RayTracingAccelerationStructure.Create(new RayTracingAccelerationStructure.RASSettings
			{
				rayTracingModeMask = RayTracingAccelerationStructure.RayTracingModeMask.Everything,
				managementMode = RayTracingAccelerationStructure.ManagementMode.Manual,
				layerMask = -1
			});
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x0004304E File Offset: 0x0004124E
		[FreeFunction("RayTracingAccelerationStructure_Bindings::Create")]
		private static IntPtr Create(RayTracingAccelerationStructure.RASSettings desc)
		{
			return RayTracingAccelerationStructure.Create_Injected(ref desc);
		}

		// Token: 0x06002879 RID: 10361
		[FreeFunction("RayTracingAccelerationStructure_Bindings::Destroy")]
		[MethodImpl(4096)]
		private static extern void Destroy(RayTracingAccelerationStructure accelStruct);

		// Token: 0x0600287A RID: 10362 RVA: 0x00043057 File Offset: 0x00041257
		public void Release()
		{
			this.Dispose();
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x00043061 File Offset: 0x00041261
		public void Build()
		{
			this.Build(Vector3.zero);
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x00043061 File Offset: 0x00041261
		[Obsolete("Method Update has been deprecated. Use Build instead (UnityUpgradable) -> Build()", true)]
		public void Update()
		{
			this.Build(Vector3.zero);
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x00043070 File Offset: 0x00041270
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::Build", HasExplicitThis = true)]
		public void Build(Vector3 relativeOrigin)
		{
			this.Build_Injected(ref relativeOrigin);
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x0004307A File Offset: 0x0004127A
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::Update", HasExplicitThis = true)]
		[Obsolete("Method Update has been deprecated. Use Build instead (UnityUpgradable) -> Build(*)", true)]
		public void Update(Vector3 relativeOrigin)
		{
			this.Update_Injected(ref relativeOrigin);
		}

		// Token: 0x0600287F RID: 10367
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstanceDeprecated", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void AddInstance([NotNull("ArgumentNullException")] Renderer targetRenderer, bool[] subMeshMask = null, bool[] subMeshTransparencyFlags = null, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, uint id = 4294967295U);

		// Token: 0x06002880 RID: 10368 RVA: 0x00043084 File Offset: 0x00041284
		public void AddInstance(Renderer targetRenderer, RayTracingSubMeshFlags[] subMeshFlags, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, uint id = 4294967295U)
		{
			this.AddInstanceSubMeshFlagsArray(targetRenderer, subMeshFlags, enableTriangleCulling, frontTriangleCounterClockwise, mask, id);
		}

		// Token: 0x06002881 RID: 10369
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::RemoveInstance", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void RemoveInstance([NotNull("ArgumentNullException")] Renderer targetRenderer);

		// Token: 0x06002882 RID: 10370 RVA: 0x00043098 File Offset: 0x00041298
		public void AddInstance(GraphicsBuffer aabbBuffer, uint numElements, Material material, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, bool reuseBounds = false, uint id = 4294967295U)
		{
			this.AddInstance_Procedural(aabbBuffer, numElements, material, Matrix4x4.identity, isCutOff, enableTriangleCulling, frontTriangleCounterClockwise, mask, reuseBounds, id);
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000430C4 File Offset: 0x000412C4
		public void AddInstance(GraphicsBuffer aabbBuffer, uint numElements, Material material, Matrix4x4 instanceTransform, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, bool reuseBounds = false, uint id = 4294967295U)
		{
			this.AddInstance_Procedural(aabbBuffer, numElements, material, instanceTransform, isCutOff, enableTriangleCulling, frontTriangleCounterClockwise, mask, reuseBounds, id);
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000430EC File Offset: 0x000412EC
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstance", HasExplicitThis = true)]
		private void AddInstance_Procedural([NotNull("ArgumentNullException")] GraphicsBuffer aabbBuffer, uint numElements, [NotNull("ArgumentNullException")] Material material, Matrix4x4 instanceTransform, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, bool reuseBounds = false, uint id = 4294967295U)
		{
			this.AddInstance_Procedural_Injected(aabbBuffer, numElements, material, ref instanceTransform, isCutOff, enableTriangleCulling, frontTriangleCounterClockwise, mask, reuseBounds, id);
		}

		// Token: 0x06002885 RID: 10373
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceTransform", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void UpdateInstanceTransform([NotNull("ArgumentNullException")] Renderer renderer);

		// Token: 0x06002886 RID: 10374
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceMask", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void UpdateInstanceMask([NotNull("ArgumentNullException")] Renderer renderer, uint mask);

		// Token: 0x06002887 RID: 10375
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceID", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern void UpdateInstanceID([NotNull("ArgumentNullException")] Renderer renderer, uint instanceID);

		// Token: 0x06002888 RID: 10376
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::GetSize", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern ulong GetSize();

		// Token: 0x06002889 RID: 10377
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::GetInstanceCount", HasExplicitThis = true)]
		[MethodImpl(4096)]
		public extern uint GetInstanceCount();

		// Token: 0x0600288A RID: 10378
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstanceSubMeshFlagsArray", HasExplicitThis = true)]
		[MethodImpl(4096)]
		private extern void AddInstanceSubMeshFlagsArray([NotNull("ArgumentNullException")] Renderer targetRenderer, RayTracingSubMeshFlags[] subMeshFlags, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, uint id = 4294967295U);

		// Token: 0x0600288B RID: 10379
		[MethodImpl(4096)]
		private static extern IntPtr Create_Injected(ref RayTracingAccelerationStructure.RASSettings desc);

		// Token: 0x0600288C RID: 10380
		[MethodImpl(4096)]
		private extern void Build_Injected(ref Vector3 relativeOrigin);

		// Token: 0x0600288D RID: 10381
		[MethodImpl(4096)]
		private extern void Update_Injected(ref Vector3 relativeOrigin);

		// Token: 0x0600288E RID: 10382
		[MethodImpl(4096)]
		private extern void AddInstance_Procedural_Injected(GraphicsBuffer aabbBuffer, uint numElements, Material material, ref Matrix4x4 instanceTransform, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, bool reuseBounds = false, uint id = 4294967295U);

		// Token: 0x04000F87 RID: 3975
		internal IntPtr m_Ptr;

		// Token: 0x0200047E RID: 1150
		[Flags]
		public enum RayTracingModeMask
		{
			// Token: 0x04000F89 RID: 3977
			Nothing = 0,
			// Token: 0x04000F8A RID: 3978
			Static = 2,
			// Token: 0x04000F8B RID: 3979
			DynamicTransform = 4,
			// Token: 0x04000F8C RID: 3980
			DynamicGeometry = 8,
			// Token: 0x04000F8D RID: 3981
			Everything = 14
		}

		// Token: 0x0200047F RID: 1151
		public enum ManagementMode
		{
			// Token: 0x04000F8F RID: 3983
			Manual,
			// Token: 0x04000F90 RID: 3984
			Automatic
		}

		// Token: 0x02000480 RID: 1152
		public struct RASSettings
		{
			// Token: 0x0600288F RID: 10383 RVA: 0x00043110 File Offset: 0x00041310
			public RASSettings(RayTracingAccelerationStructure.ManagementMode sceneManagementMode, RayTracingAccelerationStructure.RayTracingModeMask rayTracingModeMask, int layerMask)
			{
				this.managementMode = sceneManagementMode;
				this.rayTracingModeMask = rayTracingModeMask;
				this.layerMask = layerMask;
			}

			// Token: 0x04000F91 RID: 3985
			public RayTracingAccelerationStructure.ManagementMode managementMode;

			// Token: 0x04000F92 RID: 3986
			public RayTracingAccelerationStructure.RayTracingModeMask rayTracingModeMask;

			// Token: 0x04000F93 RID: 3987
			public int layerMask;
		}
	}
}
