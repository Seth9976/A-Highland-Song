using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x020003E8 RID: 1000
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	[UsedByNativeCode]
	public struct BatchCullingContext
	{
		// Token: 0x060021CD RID: 8653 RVA: 0x00037CC8 File Offset: 0x00035EC8
		[Obsolete("For internal BatchRendererGroup use only")]
		public BatchCullingContext(NativeArray<Plane> inCullingPlanes, NativeArray<BatchVisibility> inOutBatchVisibility, NativeArray<int> outVisibleIndices, LODParameters inLodParameters)
		{
			this.cullingPlanes = inCullingPlanes;
			this.batchVisibility = inOutBatchVisibility;
			this.visibleIndices = outVisibleIndices;
			this.visibleIndicesY = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(null, 0, Allocator.Invalid);
			this.lodParameters = inLodParameters;
			this.cullingMatrix = Matrix4x4.identity;
			this.nearPlane = 0f;
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x00037D18 File Offset: 0x00035F18
		[Obsolete("For internal BatchRendererGroup use only")]
		public BatchCullingContext(NativeArray<Plane> inCullingPlanes, NativeArray<BatchVisibility> inOutBatchVisibility, NativeArray<int> outVisibleIndices, LODParameters inLodParameters, Matrix4x4 inCullingMatrix, float inNearPlane)
		{
			this.cullingPlanes = inCullingPlanes;
			this.batchVisibility = inOutBatchVisibility;
			this.visibleIndices = outVisibleIndices;
			this.visibleIndicesY = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(null, 0, Allocator.Invalid);
			this.lodParameters = inLodParameters;
			this.cullingMatrix = inCullingMatrix;
			this.nearPlane = inNearPlane;
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00037D57 File Offset: 0x00035F57
		internal BatchCullingContext(NativeArray<Plane> inCullingPlanes, NativeArray<BatchVisibility> inOutBatchVisibility, NativeArray<int> outVisibleIndices, NativeArray<int> outVisibleIndicesY, LODParameters inLodParameters, Matrix4x4 inCullingMatrix, float inNearPlane)
		{
			this.cullingPlanes = inCullingPlanes;
			this.batchVisibility = inOutBatchVisibility;
			this.visibleIndices = outVisibleIndices;
			this.visibleIndicesY = outVisibleIndicesY;
			this.lodParameters = inLodParameters;
			this.cullingMatrix = inCullingMatrix;
			this.nearPlane = inNearPlane;
		}

		// Token: 0x04000C50 RID: 3152
		public readonly NativeArray<Plane> cullingPlanes;

		// Token: 0x04000C51 RID: 3153
		public NativeArray<BatchVisibility> batchVisibility;

		// Token: 0x04000C52 RID: 3154
		public NativeArray<int> visibleIndices;

		// Token: 0x04000C53 RID: 3155
		public NativeArray<int> visibleIndicesY;

		// Token: 0x04000C54 RID: 3156
		public readonly LODParameters lodParameters;

		// Token: 0x04000C55 RID: 3157
		public readonly Matrix4x4 cullingMatrix;

		// Token: 0x04000C56 RID: 3158
		public readonly float nearPlane;
	}
}
