using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x020003EA RID: 1002
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Math/Matrix4x4.h")]
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	[StructLayout(0)]
	public class BatchRendererGroup : IDisposable
	{
		// Token: 0x060021D0 RID: 8656 RVA: 0x00037D8F File Offset: 0x00035F8F
		public BatchRendererGroup(BatchRendererGroup.OnPerformCulling cullingCallback)
		{
			this.m_PerformCulling = cullingCallback;
			this.m_GroupHandle = BatchRendererGroup.Create(this);
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x00037DB7 File Offset: 0x00035FB7
		public void Dispose()
		{
			BatchRendererGroup.Destroy(this.m_GroupHandle);
			this.m_GroupHandle = IntPtr.Zero;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x00037DD4 File Offset: 0x00035FD4
		public int AddBatch(Mesh mesh, int subMeshIndex, Material material, int layer, ShadowCastingMode castShadows, bool receiveShadows, bool invertCulling, Bounds bounds, int instanceCount, MaterialPropertyBlock customProps, GameObject associatedSceneObject, ulong sceneCullingMask = 9223372036854775808UL, uint renderingLayerMask = 4294967295U)
		{
			return this.AddBatch_Injected(mesh, subMeshIndex, material, layer, castShadows, receiveShadows, invertCulling, ref bounds, instanceCount, customProps, associatedSceneObject, sceneCullingMask, renderingLayerMask);
		}

		// Token: 0x060021D3 RID: 8659
		[MethodImpl(4096)]
		public extern void SetBatchFlags(int batchIndex, ulong flags);

		// Token: 0x060021D4 RID: 8660 RVA: 0x00037DFE File Offset: 0x00035FFE
		public void SetBatchPropertyMetadata(int batchIndex, NativeArray<int> cbufferLengths, NativeArray<int> cbufferMetadata)
		{
			this.InternalSetBatchPropertyMetadata(batchIndex, (IntPtr)cbufferLengths.GetUnsafeReadOnlyPtr<int>(), cbufferLengths.Length, (IntPtr)cbufferMetadata.GetUnsafeReadOnlyPtr<int>(), cbufferMetadata.Length);
		}

		// Token: 0x060021D5 RID: 8661
		[MethodImpl(4096)]
		private extern void InternalSetBatchPropertyMetadata(int batchIndex, IntPtr cbufferLengths, int cbufferLengthsCount, IntPtr cbufferMetadata, int cbufferMetadataCount);

		// Token: 0x060021D6 RID: 8662
		[MethodImpl(4096)]
		public extern void SetInstancingData(int batchIndex, int instanceCount, MaterialPropertyBlock customProps);

		// Token: 0x060021D7 RID: 8663 RVA: 0x00037E30 File Offset: 0x00036030
		public unsafe NativeArray<Matrix4x4> GetBatchMatrices(int batchIndex)
		{
			int num = 0;
			void* batchMatrices = this.GetBatchMatrices(batchIndex, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Matrix4x4>(batchMatrices, num, Allocator.Invalid);
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x00037E58 File Offset: 0x00036058
		public unsafe NativeArray<int> GetBatchScalarArrayInt(int batchIndex, string propertyName)
		{
			int num = 0;
			void* batchScalarArray = this.GetBatchScalarArray(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(batchScalarArray, num, Allocator.Invalid);
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x00037E84 File Offset: 0x00036084
		public unsafe NativeArray<float> GetBatchScalarArray(int batchIndex, string propertyName)
		{
			int num = 0;
			void* batchScalarArray = this.GetBatchScalarArray(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<float>(batchScalarArray, num, Allocator.Invalid);
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x00037EB0 File Offset: 0x000360B0
		public unsafe NativeArray<int> GetBatchVectorArrayInt(int batchIndex, string propertyName)
		{
			int num = 0;
			void* batchVectorArray = this.GetBatchVectorArray(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(batchVectorArray, num, Allocator.Invalid);
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x00037EDC File Offset: 0x000360DC
		public unsafe NativeArray<Vector4> GetBatchVectorArray(int batchIndex, string propertyName)
		{
			int num = 0;
			void* batchVectorArray = this.GetBatchVectorArray(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector4>(batchVectorArray, num, Allocator.Invalid);
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x00037F08 File Offset: 0x00036108
		public unsafe NativeArray<Matrix4x4> GetBatchMatrixArray(int batchIndex, string propertyName)
		{
			int num = 0;
			void* batchMatrixArray = this.GetBatchMatrixArray(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Matrix4x4>(batchMatrixArray, num, Allocator.Invalid);
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x00037F34 File Offset: 0x00036134
		public unsafe NativeArray<int> GetBatchScalarArrayInt(int batchIndex, int propertyName)
		{
			int num = 0;
			void* batchScalarArray_Internal = this.GetBatchScalarArray_Internal(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(batchScalarArray_Internal, num, Allocator.Invalid);
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x00037F60 File Offset: 0x00036160
		public unsafe NativeArray<float> GetBatchScalarArray(int batchIndex, int propertyName)
		{
			int num = 0;
			void* batchScalarArray_Internal = this.GetBatchScalarArray_Internal(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<float>(batchScalarArray_Internal, num, Allocator.Invalid);
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x00037F8C File Offset: 0x0003618C
		public unsafe NativeArray<int> GetBatchVectorArrayInt(int batchIndex, int propertyName)
		{
			int num = 0;
			void* batchVectorArray_Internal = this.GetBatchVectorArray_Internal(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(batchVectorArray_Internal, num, Allocator.Invalid);
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x00037FB8 File Offset: 0x000361B8
		public unsafe NativeArray<Vector4> GetBatchVectorArray(int batchIndex, int propertyName)
		{
			int num = 0;
			void* batchVectorArray_Internal = this.GetBatchVectorArray_Internal(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector4>(batchVectorArray_Internal, num, Allocator.Invalid);
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x00037FE4 File Offset: 0x000361E4
		public unsafe NativeArray<Matrix4x4> GetBatchMatrixArray(int batchIndex, int propertyName)
		{
			int num = 0;
			void* batchMatrixArray_Internal = this.GetBatchMatrixArray_Internal(batchIndex, propertyName, out num);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Matrix4x4>(batchMatrixArray_Internal, num, Allocator.Invalid);
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x0003800D File Offset: 0x0003620D
		public void SetBatchBounds(int batchIndex, Bounds bounds)
		{
			this.SetBatchBounds_Injected(batchIndex, ref bounds);
		}

		// Token: 0x060021E3 RID: 8675
		[MethodImpl(4096)]
		public extern int GetNumBatches();

		// Token: 0x060021E4 RID: 8676
		[MethodImpl(4096)]
		public extern void RemoveBatch(int index);

		// Token: 0x060021E5 RID: 8677
		[MethodImpl(4096)]
		private unsafe extern void* GetBatchMatrices(int batchIndex, out int matrixCount);

		// Token: 0x060021E6 RID: 8678
		[MethodImpl(4096)]
		private unsafe extern void* GetBatchScalarArray(int batchIndex, string propertyName, out int elementCount);

		// Token: 0x060021E7 RID: 8679
		[MethodImpl(4096)]
		private unsafe extern void* GetBatchVectorArray(int batchIndex, string propertyName, out int elementCount);

		// Token: 0x060021E8 RID: 8680
		[MethodImpl(4096)]
		private unsafe extern void* GetBatchMatrixArray(int batchIndex, string propertyName, out int elementCount);

		// Token: 0x060021E9 RID: 8681
		[NativeName("GetBatchScalarArray")]
		[MethodImpl(4096)]
		private unsafe extern void* GetBatchScalarArray_Internal(int batchIndex, int propertyName, out int elementCount);

		// Token: 0x060021EA RID: 8682
		[NativeName("GetBatchVectorArray")]
		[MethodImpl(4096)]
		private unsafe extern void* GetBatchVectorArray_Internal(int batchIndex, int propertyName, out int elementCount);

		// Token: 0x060021EB RID: 8683
		[NativeName("GetBatchMatrixArray")]
		[MethodImpl(4096)]
		private unsafe extern void* GetBatchMatrixArray_Internal(int batchIndex, int propertyName, out int elementCount);

		// Token: 0x060021EC RID: 8684
		[MethodImpl(4096)]
		public extern void EnableVisibleIndicesYArray(bool enabled);

		// Token: 0x060021ED RID: 8685
		[MethodImpl(4096)]
		private static extern IntPtr Create(BatchRendererGroup group);

		// Token: 0x060021EE RID: 8686
		[MethodImpl(4096)]
		private static extern void Destroy(IntPtr groupHandle);

		// Token: 0x060021EF RID: 8687 RVA: 0x00038018 File Offset: 0x00036218
		[RequiredByNativeCode]
		private unsafe static void InvokeOnPerformCulling(BatchRendererGroup group, ref BatchRendererCullingOutput context, ref LODParameters lodParameters)
		{
			NativeArray<Plane> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Plane>((void*)context.cullingPlanes, context.cullingPlanesCount, Allocator.Invalid);
			NativeArray<BatchVisibility> nativeArray2 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<BatchVisibility>((void*)context.batchVisibility, context.batchVisibilityCount, Allocator.Invalid);
			NativeArray<int> nativeArray3 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>((void*)context.visibleIndices, context.visibleIndicesCount, Allocator.Invalid);
			NativeArray<int> nativeArray4 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>((void*)context.visibleIndicesY, context.visibleIndicesCount, Allocator.Invalid);
			try
			{
				context.cullingJobsFence = group.m_PerformCulling(group, new BatchCullingContext(nativeArray, nativeArray2, nativeArray3, nativeArray4, lodParameters, context.cullingMatrix, context.nearPlane));
			}
			finally
			{
				JobHandle.ScheduleBatchedJobs();
			}
		}

		// Token: 0x060021F0 RID: 8688
		[MethodImpl(4096)]
		private extern int AddBatch_Injected(Mesh mesh, int subMeshIndex, Material material, int layer, ShadowCastingMode castShadows, bool receiveShadows, bool invertCulling, ref Bounds bounds, int instanceCount, MaterialPropertyBlock customProps, GameObject associatedSceneObject, ulong sceneCullingMask = 9223372036854775808UL, uint renderingLayerMask = 4294967295U);

		// Token: 0x060021F1 RID: 8689
		[MethodImpl(4096)]
		private extern void SetBatchBounds_Injected(int batchIndex, ref Bounds bounds);

		// Token: 0x04000C61 RID: 3169
		private IntPtr m_GroupHandle = IntPtr.Zero;

		// Token: 0x04000C62 RID: 3170
		private BatchRendererGroup.OnPerformCulling m_PerformCulling;

		// Token: 0x020003EB RID: 1003
		// (Invoke) Token: 0x060021F3 RID: 8691
		public delegate JobHandle OnPerformCulling(BatchRendererGroup rendererGroup, BatchCullingContext cullingContext);
	}
}
